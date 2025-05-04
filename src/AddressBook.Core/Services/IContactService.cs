using System.Collections.Generic;
using AddressBook.Core.Models;

namespace AddressBook.Core.Services
{
    /// <summary>
    /// Interface för kontakt­tjänst som hanterar inläsning, skrivning och CRUD-operationer.
    /// </summary>
    public interface IContactService
    {
        /// <summary>Laddar in alla kontakter från JSON-filen.</summary>
        IEnumerable<Contact> LoadAll();

        /// <summary>Sparar alla kontakter till JSON-filen.</summary>
        // void SaveAll(IEnumerable<Contact> contacts);

        /// <summary>Lägger till en ny kontakt.</summary>
        void Add(Contact contact);
        /// <summary>Uppdaterar en kontakt</summary>
        void Update(Contact contact);        // ← ny
        /// <summary>Hämtar en kontakt baserat på e-post, eller null om den inte finns.</summary>
        Contact? GetByEmail(string email);

        /// <summary>Tar bort en kontakt baserat på e-post. Returnerar true om borttagen, annars false.</summary>
        bool RemoveByEmail(string email);
    }
}
