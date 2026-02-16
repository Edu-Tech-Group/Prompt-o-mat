using Pcsc.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Windows.ApplicationModel;
using Windows.Devices.SmartCards;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CopilotSmartCards
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NotifyUser(string message)
        {
            // We can only modify the fields on the same thread as the one this page is running on.
            if (!this.Dispatcher.HasThreadAccess)
            {
                var _ = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { NotifyUser(message); });
                return;
            }

            Debug.WriteLine(message);
            Singleton.Log.Text = message;
        }

        static MainPage Singleton;

        SmartCardReader _smartCardReader;


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Singleton == null)
            {
                Singleton = this;
            }

            var deviceInfos = await SmartCardReaderUtils.GetAllSmartCardReaderInfos(SmartCardReaderKind.Any);

            if (deviceInfos == null)
            {
                NotifyUser("Failed to iterate all smart card readers. is the smart card reader plugged in?");
                return;
            }
            
            var deviceInfo = deviceInfos.Where(x => !x.Name.Contains("Windows Hello")).FirstOrDefault();

            if (deviceInfo == null)
            {
                NotifyUser("Couldn't find smart card reader. is it plugged in?");
                return;
            }

            if (!deviceInfo.IsEnabled)
            {
                NotifyUser("Smart card reader is not enabled!");
                return;
            }

            if (_smartCardReader == null)
            {
                _smartCardReader = await SmartCardReader.FromIdAsync(deviceInfo.Id);
                _smartCardReader.CardAdded += OnCardAdded;
            }

            Log.Text = "Scan a prompt to start!";
        }

        private async void OnCardAdded(SmartCardReader sender, CardAddedEventArgs args)
        {
            NotifyUser("Card added!");

            var card = args.SmartCard;

            var message = "";

            try
            {
                using (SmartCardConnection connection = await card.ConnectAsync())
                {
                    IccDetection cardIdentification = new IccDetection(card, connection);
                    await cardIdentification.DetectCardTypeAync();
                    message += "Connected to card\r\nPC/SC device class: " + cardIdentification.PcscDeviceClass.ToString() + "\r\n";
                    message += "Card name: " + cardIdentification.PcscCardName.ToString() + "\r\n";
                    message += "ATR: " + BitConverter.ToString(cardIdentification.Atr) + "\r\n";

                    if ((cardIdentification.PcscDeviceClass == Pcsc.Common.DeviceClass.StorageClass) &&
                        (cardIdentification.PcscCardName == Pcsc.CardName.MifareUltralightC
                        || cardIdentification.PcscCardName == Pcsc.CardName.MifareUltralight
                        || cardIdentification.PcscCardName == Pcsc.CardName.MifareUltralightEV1))
                    {
                        // Handle MIFARE Ultralight
                        MifareUltralight.AccessHandler mifareULAccess = new MifareUltralight.AccessHandler(connection);

                        byte[] allTheBytes = new byte[64];

                        // Each read should get us 16 bytes/4 blocks, so doing
                        // 4 reads will get us all 64 bytes/16 blocks on the card
                        for (byte i = 0; i < 4; i++)
                        {
                            byte[] response = await mifareULAccess.ReadAsync((byte)(4 * i));
                            message += "Block " + (4 * i).ToString() + " to Block " + (4 * i + 3).ToString() + " " + BitConverter.ToString(response) + "\r\n";

                            // we have 16 bytes per page
                            Buffer.BlockCopy(response, 0, allTheBytes, (16 * i), response.Length);
                        }

                        byte[] responseUid = await mifareULAccess.GetUidAsync();
                        message += "UID:  " + BitConverter.ToString(responseUid) + "\r\n";

                        message += "All the bytes: " + BitConverter.ToString(allTheBytes) + "\r\n";
                        NotifyUser(message);

                        var prompt = ParsePromptFromBytes(allTheBytes);

                        RunPrompt(prompt);
                    }
                    else
                    {
                        NotifyUser($"Unsupported card type: {cardIdentification.PcscCardName}");
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyUser("Error! " + ex.Message);
            }
        }

        private string ParsePromptFromBytes(byte[] bytes)
        {
            // Start reading at block 4
            using (BinaryReader reader = new BinaryReader(new MemoryStream(bytes)))
            {
                // Go to the start of the message
                reader.BaseStream.Seek(30, SeekOrigin.Begin);

                // Read bytes until we encounted the "end" byte (0xFE - 254).
                List<byte> allBytes = new List<byte>();
                do
                {
                    byte b = reader.ReadByte();
                    if (b == 0xFE)
                    {
                        break;
                    }
                    allBytes.Add(b);
                } while (reader.BaseStream.CanRead);

                // convert the bytes to a string;
                var prompt = Encoding.UTF8.GetString(allBytes.ToArray());
                NotifyUser(prompt);
                return prompt;
            }
        }

        private async void RunPrompt(string prompt)
        {
            try
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppWithArgumentsAsync(prompt);
            }
            catch (Exception ex)
            {
                NotifyUser($"Failed to run prompt in FullTrustProcessLauncher: {ex.Message}");
            }
        }
    }
}