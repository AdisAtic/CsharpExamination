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

## Bygga och köra

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
