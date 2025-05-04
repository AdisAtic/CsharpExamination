using System;
using System.IO;
using System.Linq;
using AddressBook.Core.Models;
using AddressBook.Core.Repositories;
using AddressBook.Core.Services;
using Xunit;

namespace AddressBook.Console.Tests.Services
{
    public class ContactServiceTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly ContactService _service;

        public ContactServiceTests()
        {
            // Skapa en temporär fil och låt ContactService skapa JSON-filen själv
            _tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
            // Skapa JsonContactRepository och skicka in i ContactService
            var repo = new JsonContactRepository(_tempFile);
            _service = new ContactService(repo);
        }

        [Fact]
        public void Add_ThenLoadAll_ReturnsAddedContact()
        {
            var contact = new Contact
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PhoneNumber = "+46123456789",
                Address = "Testgatan 1, 111 11 Teststad"
            };

            _service.Add(contact);

            var all = _service.LoadAll().ToList();
            Assert.Single(all);
            Assert.Equal("test@example.com", all[0].Email);
        }

        [Fact]
        public void GetByEmail_ReturnsNullIfNotExists()
        {
            var result = _service.GetByEmail("nonexistent@example.com");
            Assert.Null(result);
        }

        [Fact]
        public void RemoveByEmail_WhenExists_ReturnsTrueAndRemoves()
        {
            var c = new Contact
            {
                FirstName = "Remove",
                LastName = "Me",
                Email = "remove@example.com",
                PhoneNumber = "000",
                Address = "Nowhere"
            };
            _service.Add(c);

            bool removed = _service.RemoveByEmail("remove@example.com");
            var allAfter = _service.LoadAll().ToList();

            Assert.True(removed);
            Assert.Empty(allAfter);
        }

        public void Dispose()
        {
            // Rensa upp tempfilen
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}
