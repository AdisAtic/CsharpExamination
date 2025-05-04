using System;
using System.Collections.Generic;
using AddressBook.Core.Models;
using AddressBook.Core.Repositories;

namespace AddressBook.Core.Services
{
    /// <summary>
    /// Service‐layer som exponerar repository‐operationer via IContactService.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repo;

        public ContactService(IContactRepository repo) 
            => _repo = repo;

        public IEnumerable<Contact> LoadAll()   
            => _repo.GetAll();

        public Contact? GetByEmail(string e)    
            => _repo.GetByEmail(e);

        public void Add(Contact c)              
            => _repo.Add(c);

        public bool RemoveByEmail(string e)     
            => _repo.Remove(e);

        public void Update(Contact c)           
            => _repo.Update(c);

        public void SaveAll(IEnumerable<Contact> contacts)
            => throw new NotSupportedException("Use Add/Update/Remove via repository.");
    }
}
