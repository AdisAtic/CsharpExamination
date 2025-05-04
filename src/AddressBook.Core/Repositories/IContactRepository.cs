using System.Collections.Generic;
using AddressBook.Core.Models;

namespace AddressBook.Core.Repositories
{
    /// <summary>
    /// Abstraktion för lagring och hämtning av kontakter.
    /// </summary>
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        Contact? GetByEmail(string email);
        void Add(Contact contact);
        void Update(Contact contact);
        bool Remove(string email);
    }
}
