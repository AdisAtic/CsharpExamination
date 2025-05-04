using System;
using System.IO;
using System.Linq;
using AddressBook.Core.Models;
using AddressBook.Core.Repositories;
using Xunit;

namespace AddressBook.Core.Tests.Repositories
{
    public class JsonContactRepositoryTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly JsonContactRepository _repo;

        public JsonContactRepositoryTests()
        {
            // Skapa temporär filväg
            _tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
            // Initiera repot (skapar filen om den inte finns)
            _repo = new JsonContactRepository(_tempFile);
        }

        [Fact]
        public void GetAll_InitiallyEmpty_ReturnsEmptyList()
        {
            var all = _repo.GetAll().ToList();
            Assert.Empty(all);
        }

        [Fact]
        public void Add_ThenGetAll_ReturnsAddedContact()
        {
            var c = new Contact
            {
                FirstName   = "Anna",
                LastName    = "Andersson",
                Email       = "anna@example.com",
                PhoneNumber = "0701234567",
                Address     = "Testgatan 1"
            };

            _repo.Add(c);
            var all = _repo.GetAll().ToList();

            Assert.Single(all);
            Assert.Equal("anna@example.com", all[0].Email);
        }

        [Fact]
        public void GetByEmail_ExistingAndNonExisting()
        {
            var c = new Contact { Email = "a@b.com", FirstName = "A", LastName="B" };
            _repo.Add(c);

            var found = _repo.GetByEmail("a@b.com");
            Assert.NotNull(found);
            Assert.Equal("A", found!.FirstName);

            var notFound = _repo.GetByEmail("nope@example.com");
            Assert.Null(notFound);
        }

        [Fact]
        public void Update_Existing_OverwritesData()
        {
            var c = new Contact { Email = "x@y.com", FirstName = "X", LastName="Y" };
            _repo.Add(c);

            c.FirstName = "Z";
            _repo.Update(c);

            var updated = _repo.GetByEmail("x@y.com");
            Assert.Equal("Z", updated!.FirstName);
        }

        [Fact]
        public void Remove_ExistingAndNonExisting()
        {
            var c1 = new Contact { Email = "rm@ex.com", FirstName = "R", LastName="M" };
            _repo.Add(c1);

            bool removedTrue  = _repo.Remove("rm@ex.com");
            bool removedFalse = _repo.Remove("rm@ex.com");

            Assert.True(removedTrue);
            Assert.False(removedFalse);
            Assert.Empty(_repo.GetAll());
        }

        [Fact]
        public void Add_DuplicateEmail_Throws()
        {
            var c = new Contact { Email = "dup@ex.com" };
            _repo.Add(c);
            var ex = Assert.Throws<InvalidOperationException>(() => _repo.Add(c));
            Assert.Contains("finns redan", ex.Message);
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}
