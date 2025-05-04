# AddressBookAssignment

En C#-baserad adressbok som innehåller både en konsolapplikation och en WPF GUI-applikation. Projektet följer MVVM, Dependency Injection och Repository Pattern, och inkluderar enhetstester för Core-, Console- och GUI-delar.

## Funktionalitet

* **Console-app** (`AddressBook.Console`)

  * Lista, visa, lägga till och ta bort kontakter via meny i terminalen.

* **GUI-app (WPF)** (`AddressBook.Gui`)

  * Visa lista över kontakter.
  * Lägga till, redigera och ta bort kontakter via grafiskt gränssnitt.
  * MVVM-arkitektur med `MainViewModel` och `ContactDetailViewModel`.

* **Persistence** (`AddressBook.Core.Repositories`)

  * `IContactRepository` och `JsonContactRepository` för filbaserad JSON-persistens.
  * `ContactService` i `AddressBook.Core.Services` som exponerar CRUD via repository.

* **Tester**

  * **Core**: `JsonContactRepositoryTests` täcker repository-funktionalitet.
  * **Console**: enhetstester för `ContactService` och integrationstest av `ConsoleApp`.
  * **GUI**: enhetstester för `MainViewModel` och `ContactDetailViewModel`.

## Mappstruktur

```
AddressBookAssignment/
├── AddressBookAssignment.sln
├── contacts.json          # Delad datafil för båda apparna
├── src/
│   ├── AddressBook.Core/
│   │   ├── Models/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── AddressBook.Core.csproj
│   ├── AddressBook.Console/
│   │   ├── ConsoleApp.cs
│   │   ├── Program.cs
│   │   └── AddressBook.Console.csproj
│   └── AddressBook.Gui/
│       ├── Views/
│       ├── ViewModels/
│       ├── App.xaml
│       ├── App.xaml.cs
│       └── AddressBook.Gui.csproj
└── tests/
    ├── AddressBook.Core.Tests/
    ├── AddressBook.Console.Tests/
    └── AddressBook.Gui.Tests/
```

## Förutsättningar

* .NET 9 SDK (eller senare)
* Visual Studio Code eller Visual Studio 2022+
* Git (för att klona repot)
* C#-utökningen för Visual Studio Code (Om du kör VS Code)

## Installera beroenden

Innan du bygger projektet behöver du installera de NuGet-paket som används för DI, hosting och MVVM i respektive projekt:

```bash
# I Core-projektet behövs inga externa paket utöver .NET SDK

# I Console-appen (AddressBook.Console):
cd src/AddressBook.Console
dotnet add package Microsoft.Extensions.DependencyInjection

# I GUI-appen (AddressBook.Gui):
cd ../AddressBook.Gui
dotnet add package Microsoft.Extensions.Hosting
# (Eventuellt även DI om du inte redan har det via Hosting-paketet)
# dotnet add package Microsoft.Extensions.DependencyInjection

# I testprojekten: XUnit och dotnet test SDK är vanligtvis redan refererade när projektet skapades.
```

## Bygga och köraa

### Bygga lösningen

```bash
dotnet build
```

### Köra konsol-appen

```bash
cd src/AddressBook.Console
dotnet run
```

### Köra GUI-appen

```bash
cd src/AddressBook.Gui
dotnet run
```

## Köra tester

Från lösningsroten:

```bash
dotnet test
```

Det kör alla tester i `tests/`-mappen.

## Licence

MIT License © \[AdisAtic] 2025
