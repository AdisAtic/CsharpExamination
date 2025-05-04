using System;
using System.Collections.Generic;

namespace AddressBook.Core.Models
{
    /// <summary>
    /// Representerar en kontakt i adressboken.
    /// </summary>
    public class Contact
    {
        /// <summary>Förnamn på kontakten.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Efternamn på kontakten.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Telefonnummer, t.ex. +46701234567.</summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>E-postadress, används som unik nyckel.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Gatuadress, ort, postnummer osv.</summary>
        public string Address { get; set; } = string.Empty;
    }
}
