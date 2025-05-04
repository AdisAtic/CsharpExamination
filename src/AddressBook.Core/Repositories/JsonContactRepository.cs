using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AddressBook.Core.Models;

namespace AddressBook.Core.Repositories
{
    /// <summary>
    /// En fil-baserad IContactRepository som persisterar i JSON.
    /// </summary>
    public class JsonContactRepository : IContactRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

        public JsonContactRepository(string filePath)
        {
            _filePath = filePath;
            _opts     = new JsonSerializerOptions { WriteIndented = true };

            // Om filen saknas **eller** är tom, (re)skapa den som en tom JSON-lista
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
            {
                Save(new List<Contact>());
            }
        }

        public IEnumerable<Contact> GetAll()
        {
            var json = File.ReadAllText(_filePath);

            // Om innehållet av någon anledning är blanksteg/nyrad, betrakta som tom lista
            if (string.IsNullOrWhiteSpace(json))
                return Enumerable.Empty<Contact>();

            return JsonSerializer.Deserialize<List<Contact>>(json, _opts)
                ?? Enumerable.Empty<Contact>();
        }

        public Contact? GetByEmail(string email)
            => GetAll()
               .FirstOrDefault(c => 
                   c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public void Add(Contact contact)
        {
            var all = GetAll().ToList();
            if (all.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Kontakt med e-post '{contact.Email}' finns redan.");
            all.Add(contact);
            Save(all);
        }

        public void Update(Contact contact)
        {
            var all = GetAll().ToList();
            var idx = all.FindIndex(c => 
                c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase));
            if (idx < 0)
                throw new InvalidOperationException($"Kontakt '{contact.Email}' finns inte.");
            all[idx] = contact;
            Save(all);
        }

        public bool Remove(string email)
        {
            var all = GetAll().ToList();
            var removed = all.RemoveAll(c => 
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) > 0;
            if (removed) Save(all);
            return removed;
        }

        private void Save(IEnumerable<Contact> contacts)
            => File.WriteAllText(_filePath, JsonSerializer.Serialize(contacts, _opts));
    }
}
