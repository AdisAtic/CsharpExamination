using System;
using AddressBook.Core.Models;
using AddressBook.Core.Services;

namespace AddressBook.App
{
    /// <summary>
    /// Ansvarar för konsol‐menyn och interaktion med användaren.
    /// </summary>
    public class ConsoleApp
    {
        private readonly IContactService _contactService;

        public ConsoleApp(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Kör huvudloopen med CRUD‐meny.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("1) Lista kontakter");
                Console.WriteLine("2) Visa kontakt");
                Console.WriteLine("3) Lägg till kontakt");
                Console.WriteLine("4) Ta bort kontakt");
                Console.WriteLine("5) Avsluta");
                Console.Write("Välj: ");
                var cmd = Console.ReadLine();
                Console.WriteLine();

                switch (cmd)
                {
                    case "1":
                        foreach (var c in _contactService.LoadAll())
                            Console.WriteLine($"{c.FirstName} {c.LastName} ({c.Email})");
                        break;

                    case "2":
                        Console.Write("Ange e-post: ");
                        var email = Console.ReadLine()!;
                        var contact = _contactService.GetByEmail(email);
                        if (contact == null) Console.WriteLine("Hittades ej.");
                        else
                            Console.WriteLine(
                                $"Namn: {contact.FirstName} {contact.LastName}\n" +
                                $"Telefon: {contact.PhoneNumber}\n" +
                                $"E-post: {contact.Email}\n" +
                                $"Adress: {contact.Address}");
                        break;

                    case "3":
                        var nc = new Contact();
                        Console.Write("Förnamn: "); nc.FirstName = Console.ReadLine()!;
                        Console.Write("Efternamn: "); nc.LastName = Console.ReadLine()!;
                        Console.Write("Telefon: "); nc.PhoneNumber = Console.ReadLine()!;
                        Console.Write("E-post: "); nc.Email = Console.ReadLine()!;
                        Console.Write("Adress: "); nc.Address = Console.ReadLine()!;
                        try
                        {
                            _contactService.Add(nc);
                            Console.WriteLine("Kontakt tillagd!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Fel: {ex.Message}");
                        }
                        break;

                    case "4":
                        Console.Write("Ange e-post att ta bort: ");
                        var rem = Console.ReadLine()!;
                        if (_contactService.RemoveByEmail(rem))
                            Console.WriteLine("Kontakt borttagen.");
                        else
                            Console.WriteLine("Hittades ej.");
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
