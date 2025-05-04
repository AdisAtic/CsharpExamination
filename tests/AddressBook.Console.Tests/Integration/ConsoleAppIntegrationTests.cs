using System;
using System.IO;
using System.Text.Json;
using AddressBook.Core.Models;
using AddressBook.Core.Services;
using AddressBook.App;      // Namespace för din ConsoleApp
using AddressBook.Core.Repositories;
using Xunit;


namespace AddressBook.Console.Tests.Integration
{
    public class ConsoleAppIntegrationTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly StringWriter _output;
        private readonly StringReader _input;

        public ConsoleAppIntegrationTests()
        {
            // 1) Skapa en temporär JSON-fil med ett förifyllt kontakt.
            _tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
            var seed = new[]
            {
                new Contact { FirstName = "Seed", LastName = "User", Email = "seed@example.com", PhoneNumber = "123", Address = "Street 1" }
            };
            File.WriteAllText(_tempFile, JsonSerializer.Serialize(seed, new JsonSerializerOptions { WriteIndented = true }));

            // 2) Förbered in- och ut­ström
            _output = new StringWriter();
            System.Console.SetOut(_output);


            // vi kommer välja menyval 1 (lista) och därefter 5 (avsluta)
            _input = new StringReader("1\n5\n");
            System.Console.SetIn(_input);
        }

        [Fact]
        public void Run_ListThenExit_PrintsSeedContact()
        {
            // 3) Bygg ContactService och ConsoleApp
            var repo = new JsonContactRepository(_tempFile);
            var svc  = new ContactService(repo);
            var app = new ConsoleApp(svc);

            // 4) Kör loopen
            app.Run();

            // 5) Läs ut texten och verifiera att vår seed-kontakt listas
            var consoleOutput = _output.ToString();
            Assert.Contains("Seed User (seed@example.com)", consoleOutput);
        }

        public void Dispose()
        {
            // Rensa upp
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}
