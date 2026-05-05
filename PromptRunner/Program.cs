using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PromptRunner
{
    enum PromptService
    {
        MicrosoftCopilot,
        GoogleGemini,
    }

    struct Prompt
    {
        public PromptService Service;
        public string PromptText;
    }

    struct PromptEntry
    {
        public string Key;
        public Prompt Prompt;
    }

    internal class Program
    {
        private static readonly List<PromptEntry> Prompts = new List<PromptEntry>
        {
            new PromptEntry{Key = "PromptCard1",  Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Maak geheugenkaartjes om te leren voor mijn examen Natuurwetenschappen: Biodiversiteit. Gebruik deze pagina als bron: Biodiversiteit | Bioleren" }},
            new PromptEntry{Key = "PromptCard2",  Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Kun je me helpen de tekst op onderstaande pagina te herschrijven zodat deze duidelijker en beknopter wordt? Breng deze op niveau van een 13-jarige. https://zowerkthetlichaam.nl/39/longen-en-ademhaling/" }},
            new PromptEntry{Key = "PromptCard3",  Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Maak 30 flashcards met woordenschat voor een student in een klas Italiaans rond het thema familie. Geef dit weer in een tabel waarbij het Italiaanse woord in de eerste kolom staat." }},
            new PromptEntry{Key = "PromptCard4",  Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Maak een lijst van 10 creatieve team-buildingsactiviteiten die inzetten op samenwerking, vertrouwen en groepsgevoel voor mijn klas van 17-jarigen. Maak een tabel met de volgende kolommen: activiteit, geschatte tijdsduur, beschrijving en de nodige materialen." }},
            new PromptEntry{Key = "PromptCard5",  Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Schrijf Python code waardoor ik de eerste 50 getallen van de rij van Fibonacci terugkrijg." }},
            new PromptEntry{Key = "PromptCard6",  Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Stel een bericht voor sociale media op over de info-avond op onze school (Edu-Versity) binnenkort. Alle leerlingen en hun ouders die interesse hebben in onze school zijn welkom. Haal informatie uit onze visie om deze post specifiek voor onze school te maken: https://edu-versity.be/visie-op-coaching/" }},
            new PromptEntry{Key = "PromptCard7",  Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Ik ben leerkracht wiskunde en geef les aan het tweede middelbaar in Vlaanderen. Ik wil mijn leerlingen op een creatieve manier bevragen over het volume van kubus, balk en cilinder. Kan je mij 5 suggesties geven voor een niet-traditionele manier van toetsen? Zorg ervoor dat deze zaken getoetst worden: 06.10 De leerlingen berekenen oppervlakte en volume van ruimtefiguren: kubus, balk en cilinder." }},
            new PromptEntry{Key = "PromptCard8",  Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Geef kunst- en knutselideeën met instructies die een 4-jarige kleuter alleen kan uitvoeren waarbij enkel karton, plastic flessen, papier, plakband, behangerslijm en touw gebruikt mag worden." }},
            new PromptEntry{Key = "PromptCard9",  Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Kan je een PowerShell script schrijven om een dynamische mailgroep te maken met als filter Attribuut 1 en Attribuut 2? Ik wil de filter zelf zien zodat ik deze kan verifiëren." }},
            new PromptEntry{Key = "PromptCard10", Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Je bent een creatieve spelletjesontwikkelaar. Je ontwikkelt een escape game voor 12-jarigen. Voor de opdrachten heb ik dit ter beschikking: papier, schaar, lijm, cijferslot met 3 cijfers, kist, laptop met internetconnectie. Ik wil graag 5 opdrachten. Neem deze kennis/vaardigheden op in de opdrachten: https://wikikids.nl/Eerste_Wereldoorlog. Ontwikkel de escape game zo dat er één verhaallijn is en een mysterie bevat om op te lossen." }},
            new PromptEntry{Key = "PromptCard11", Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Schrijf een lesplan van 50 minuten over de industriële revolutie voor mijn lessen Europese geschiedenis in het derde middelbaar. Ik heb in een vervolgles nogmaals 50 minuten tijd. Mijn leerlingen hebben vroeger al kort kennisgemaakt met dit onderwerp, maar hebben nog geen diepgaande kennis. Voeg ideeën toe om het relevant te maken voor leerlingen van nu." }},
            new PromptEntry{Key = "PromptCard12", Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Schrijf wat grappige afwezigheidsberichten voor wanneer ik op vakantie ben van 22 december tot 3 januari. Neem ook stappen op voor het instellen hiervan in Outlook." }},
            new PromptEntry{Key = "PromptCard13", Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Orden de ongestructureerde data in onderstaande tekst in een tabel. Vraag nadien of de structuur goed zit of als er aanpassingen moeten komen. De 4 seizoenen: De aarde kent 4 seizoenen dubbelpunt lente, zomer, herfst en winter. Elk seizoen heeft zijn eigen kenmerken en weersomstandigheden. Lente: de lente begint in maart en eindigt in juni. In deze periode ontwaken de planten uit hun winterslaap. De temperaturen beginnen te stijgen en de dagen worden langer. Bloemen bloeien en dieren komen uit hun schuilplaatsen. Zomer: de zomer loopt van juni tot september. Dit seizoen staat bekend om zijn warme temperaturen en lange dagen. Mensen genieten van buitenactiviteiten zoals zwemmen, picknicken en kamperen. Planten groeien snel en de natuur is op zijn groenst. Herfst: de herfst begint in september en eindigt in december. De temperaturen beginnen te dalen en de dagen worden korter. Bladeren van bomen verkleuren en vallen af. Het is ook de tijd van het oogsten, waarbij veel fruit en groenten worden geplukt. Winter: de winter loopt van september tot maart. Dit seizoen wordt gekenmerkt door koude temperaturen en korte dagen. In veel gebieden valt er sneeuw en bevriezen oppervlakken. Mensen dragen warme kleding en blijven vaak binnen om zich te beschermen tegen de kou." }},
            new PromptEntry{Key = "PromptCard14", Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "We doen een rollenspel. Ik wil leren over het leven van Cleopatra. Jij neemt haar rol aan en ik stel je vragen. Zo voeren we een gesprek. Praat in de ik-vorm en spreek in de stijl waarin deze persoon waarschijnlijk zou spreken. Je antwoorden moeten feitelijk juist zijn. Begin het gesprek door mij te begroeten als deze persoon. Houd deze begroeting kort en formeel. Vraag daarna wat ik wil weten." }},
            new PromptEntry{Key = "PromptCard15", Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Maak een poster in tekenfilmstijl voor het schoolfeest met als thema circus. Zorg dat er veel verschillende circuselementen worden opgenomen zonder dat het te druk wordt. Gebruik enkel het woord ‘Circus’ op de poster." }},
            new PromptEntry{Key = "PromptCard16", Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Vat de inhoud van onderstaande webpagina samen in maximaal 10 bullets. https://www.microsoft.com/nl-be/education" }},
            new PromptEntry{Key = "PromptCard17", Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Kan je mij helpen om een stappenplan te maken over het gebruik van Microsoft Forms voor het maken van een quiz op punten met automatische correctie. Het doel is om leerkrachten stap voor stap uitleg te geven. Ik wil de stappen graag in bulletvorm en in een gebiedende wijs. Voeg ook links of video’s toe als extra hulplijn; bij voorkeur Nederlandstalig." }},
            new PromptEntry{Key = "PromptCard18", Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Lees de tekst op deze webpagina: https://nl.wikipedia.org/wiki/Romeinen_in_België. Maak 5 meerkeuzevragen met telkens 3 keuzemogelijkheden om uit te kiezen. Zorg dat elk antwoord realistisch overkomt. Zorg dat de vragen zo zijn opgesteld dat ze eenvoudig te begrijpen zijn door 14-jarigen." }},
            new PromptEntry{Key = "PromptCard19", Prompt = new Prompt{ Service = PromptService.GoogleGemini, PromptText = "Maak een standaardbericht op om te antwoorden op een mail van een ouder in verband met de ziekte van hun kind. Neem volgende zaken als bullet op: naam leerling, klas leerling, periode afwezigheid, reden afwezigheid, doktersbriefje aanwezig." }},
            new PromptEntry{Key = "PromptCard20", Prompt = new Prompt{ Service = PromptService.MicrosoftCopilot, PromptText = "Een lege, eenvoudige kleurboektekening van een schattige regenboog met een lachend gezicht en zachte, dikke wolken. De regenboog moet in zwart-wit zijn met strakke lijnen op een witte achtergrond. Er moet ruimte zijn voor het kleuren in kleurenboekstijl, gebruik geen arcering of andere vulling." }},
        };

        private static readonly string COPILOT_SERVICE_URL = "https://m365.cloud.microsoft/chat";
        private static readonly string GEMINI_SERVICE_URL = "https://gemini.google.com/app";

        static void Main(string[] args)
        {
            var promptKey = args[2];
            if (!Prompts.Any(p => p.Key.EndsWith(promptKey)))
            {
                Console.WriteLine("No suitable prompt found");
                return;
            }
            var promptEntry = Prompts.Where(x => x.Key.EndsWith(promptKey)).First();
            var prompt = promptEntry.Prompt;
            Console.WriteLine($"card {promptKey} - {prompt.PromptText}");

            try
            {
                var serviceUrl = "";
                switch (prompt.Service)
                {
                    case PromptService.MicrosoftCopilot:
                        serviceUrl = COPILOT_SERVICE_URL;
                        break;
                    case PromptService.GoogleGemini:
                        serviceUrl = GEMINI_SERVICE_URL;
                        break;
                }
                
                // OVERRIDING SERVICE URL FOR CUSTOMER!!!
                serviceUrl = COPILOT_SERVICE_URL;

                var browser = new EdgeWrapper();
                //var browser = new ChromeWrapper();
                browser.OpenUrl(serviceUrl);

                Thread.Sleep(5000);

                browser.RunPrompt(prompt.PromptText);

                Thread.Sleep(30000);

                browser.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception - {ex.Message}");
            }
        }
    }
}
