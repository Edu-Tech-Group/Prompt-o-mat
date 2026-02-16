using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PromptRunner
{
    internal interface IBrowserWrapper
    {
        void OpenUrl(string url);
        void Kill();
        void RunPrompt(string prompt);
    }

    internal class EdgeWrapper : IBrowserWrapper
    {
        private Process _process;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        public void OpenUrl(string url)
        {
            Process.Start("msedge.exe", url);
            Thread.Sleep(1000);
            _process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "msedge");

            if (_process == null)
            {
                Console.WriteLine("Cannot find process after starting");
            }
        }

        public void Kill()
        {
            var processes = Process.GetProcessesByName("msedge");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        public void RunPrompt(string prompt)
        {
            if (_process == null)
            {
                throw new Exception("Failed to find process, browser was likely not correctly initialized!");
            }

            SetForegroundWindow(_process.MainWindowHandle);
            SendKeys.SendWait($"{prompt}\n");
        }
    }

    internal class ChromeWrapper : IBrowserWrapper
    {
        private Process _process;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        public void OpenUrl(string url)
        {
            Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", $"--hide-crash-restore-bubble {url}");
            Thread.Sleep(1000);
            _process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "chrome");

            if (_process == null)
            {
                Console.WriteLine("Cannot find process after starting");
            }
        }

        public void Kill()
        {
            var processes = Process.GetProcessesByName("chrome");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        public void RunPrompt(string prompt)
        {
            if (_process == null)
            {
                throw new Exception("Failed to find process, browser was likely not correctly initialized!");
            }

            SetForegroundWindow(_process.MainWindowHandle);
            SendKeys.SendWait($"{prompt}\n");
        }
    }
}
