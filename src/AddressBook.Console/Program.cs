using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using AddressBook.Core.Repositories;
using AddressBook.Core.Services;

namespace AddressBook.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1) Hitta lösningsroten (mappen med AddressBookAssignment.sln)
            string solutionRoot = FindSolutionRoot();
            string contactsFile = Path.Combine(solutionRoot, "contacts.json");

            // 2) Välj fil via args eller default
            string filePath = args.Length > 0 ? args[0] : contactsFile;

            // 3) Konfigurera DI
            var services = new ServiceCollection();
            services.AddSingleton<IContactRepository>(_ => new JsonContactRepository(filePath));
            services.AddSingleton<IContactService, ContactService>();
            services.AddTransient<ConsoleApp>();

            var provider = services.BuildServiceProvider();

            // 4) Hämta din ConsoleApp från DI och kör den
            var app = provider.GetRequiredService<ConsoleApp>();
            app.Run();
        }

        /// <summary>
        /// Klättrar uppåt tills den hittar AddressBookAssignment.sln.
        /// </summary>
        static string FindSolutionRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !dir.GetFiles("AddressBookAssignment.sln").Any())
                dir = dir.Parent;
            if (dir == null)
                throw new InvalidOperationException("Kunde inte hitta lösningsroten.");
            return dir.FullName;
        }
    }
}
