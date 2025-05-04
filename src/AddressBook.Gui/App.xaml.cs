using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AddressBook.Core.Repositories;
using AddressBook.Core.Services;
using AddressBook.Gui.ViewModels;
using AddressBook.Gui.Views;

namespace AddressBook.Gui
{
    public partial class App : Application
    {
        private IHost _host = null!;

        protected override async void OnStartup(StartupEventArgs e)
        {
            // 1) Hitta lösningsroten och kontaktsfil
            string solutionRoot = FindSolutionRoot();
            string contactsFile = Path.Combine(solutionRoot, "contacts.json");

            // 2) Bygg host & DI
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IContactRepository>(_ => new JsonContactRepository(contactsFile));
                    services.AddSingleton<IContactService, ContactService>();

                    services.AddTransient<MainViewModel>();
                    services.AddTransient<ContactDetailView>();
                    services.AddTransient<MainWindow>();
                })
                .Build();

            await _host.StartAsync();

            // 3) Visa main window
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }

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
