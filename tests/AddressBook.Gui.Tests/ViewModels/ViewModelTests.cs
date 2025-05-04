using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AddressBook.Core.Models;
using AddressBook.Core.Services;
using AddressBook.Core.Repositories;
using AddressBook.Gui.ViewModels;

namespace AddressBook.Gui.Tests.ViewModels
{
    /// <summary>
    /// Enkel fake-implementation av IContactService för att testa ViewModels.
    /// </summary>
    internal class FakeContactService : IContactService
    {
        private readonly List<Contact> _contacts;

        public FakeContactService(IEnumerable<Contact> initial)
        {
            _contacts = initial.ToList();
        }

        public IEnumerable<Contact> LoadAll() 
            => new List<Contact>(_contacts);

        public Contact? GetByEmail(string email) 
            => _contacts.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public void Add(Contact contact)
        {
            if (_contacts.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Duplicate email");
            _contacts.Add(contact);
        }

        public void Update(Contact contact)
        {
            var idx = _contacts.FindIndex(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase));
            if (idx < 0)
                throw new InvalidOperationException("Contact not found");
            _contacts[idx] = contact;
        }

        public bool RemoveByEmail(string email)
            => _contacts.RemoveAll(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) > 0;
    }

    public class MainViewModelTests
    {
        [Fact]
        public void Constructor_LoadsContacts()
        {
            var contact = new Contact { Email = "a@b.com", FirstName = "A", LastName = "B" };
            var fakeService = new FakeContactService(new[] { contact });

            var vm = new MainViewModel(fakeService);

            Assert.Single(vm.Contacts);
            Assert.Equal("a@b.com", vm.Contacts[0].Email);
        }

        [Fact]
        public void RefreshCommand_UpdatesContactsCollection()
        {
            var contact1 = new Contact { Email = "a@b.com" };
            var contact2 = new Contact { Email = "b@c.com" };
            var fakeService = new FakeContactService(new[] { contact1 });
            var vm = new MainViewModel(fakeService);

            // Ändra underliggande data och kör refresh
            fakeService.Add(contact2);
            vm.RefreshCommand.Execute(null);

            Assert.Equal(2, vm.Contacts.Count);
            Assert.Contains(vm.Contacts, c => c.Email == "b@c.com");
        }

        [Fact]
        public void RemoveCommand_RemovesSelectedContact()
        {
            var contact1 = new Contact { Email = "a@b.com" };
            var contact2 = new Contact { Email = "b@c.com" };
            var fakeService = new FakeContactService(new[] { contact1, contact2 });
            var vm = new MainViewModel(fakeService);

            vm.SelectedContact = vm.Contacts.First(c => c.Email == "a@b.com");
            vm.RemoveCommand.Execute(null);

            Assert.Single(vm.Contacts);
            Assert.DoesNotContain(vm.Contacts, c => c.Email == "a@b.com");
        }

        [Fact]
        public void RemoveCommand_CanExecute_OnlyWhenSelected()
        {
            var fakeService = new FakeContactService(Array.Empty<Contact>());
            var vm = new MainViewModel(fakeService);

            // Ingen vald
            Assert.False(vm.RemoveCommand.CanExecute(null));

            // Lägg till och välj
            var contact = new Contact { Email = "x@y.com" };
            fakeService.Add(contact);
            vm.RefreshCommand.Execute(null);
            vm.SelectedContact = vm.Contacts.First();

            Assert.True(vm.RemoveCommand.CanExecute(null));
        }
    }

    public class ContactDetailViewModelTests
    {
        [Fact]
        public void SaveCommand_CanExecute_OnlyWhenRequiredFieldsSet()
        {
            var contact = new Contact();
            var vm = new ContactDetailViewModel(contact, _ => { });

            // Bör vara inaktiverad initialt
            Assert.False(vm.SaveCommand.CanExecute(null));

            vm.FirstName = "A";
            vm.LastName = "B";
            vm.Email = "a@b.com";

            Assert.True(vm.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void SaveCommand_Execute_CallsCallback()
        {
            var contact = new Contact { FirstName = "A", LastName = "B", Email = "a@b.com" };
            Contact? saved = null;
            var vm = new ContactDetailViewModel(contact, c => saved = c);

            vm.SaveCommand.Execute(null);

            Assert.Equal(contact, saved);
        }
    }
}
