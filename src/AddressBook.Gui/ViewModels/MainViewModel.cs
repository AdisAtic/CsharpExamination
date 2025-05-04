using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AddressBook.Core.Models;
using AddressBook.Core.Services;
using AddressBook.Gui.Helpers;
using AddressBook.Gui.Views;

namespace AddressBook.Gui.ViewModels
{
    /// <summary>
    /// ViewModel för huvudfönstret: håller koll på kontakter och hanterar kommandon.
    /// </summary>
    public class MainViewModel
    {
        private readonly IContactService _contactService;

        public MainViewModel(IContactService contactService)
        {
            _contactService = contactService;

            // Ladda initiala kontakter
            Contacts = new ObservableCollection<ContactViewModel>(
                _contactService.LoadAll().Select(c => new ContactViewModel(c))
            );

            // Initiera kommandon
            AddCommand     = new RelayCommand(OnAdd);
            EditCommand    = new RelayCommand(OnEdit,    _ => SelectedContact != null);
            RemoveCommand  = new RelayCommand(OnRemove,  _ => SelectedContact != null);
            RefreshCommand = new RelayCommand(OnRefresh);
        }

        public ObservableCollection<ContactViewModel> Contacts { get; }

        private ContactViewModel? _selectedContact;
        public ContactViewModel? SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                // Uppdatera CanExecute för Edit/Remove
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }

        // Kommandon
        public ICommand AddCommand     { get; }
        public ICommand EditCommand    { get; }
        public ICommand RemoveCommand  { get; }
        public ICommand RefreshCommand { get; }

        private void OnAdd(object? _)
        {
            var model = new Contact();
            var vm = new ContactDetailViewModel(model, contact =>
            {
                _contactService.Add(contact);
                Contacts.Add(new ContactViewModel(contact));
            });

            var dialog = new ContactDetailView
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }

        private void OnEdit(object? _)
        {
            if (SelectedContact == null) return;

            var model = SelectedContact.GetModel();
            var vm = new ContactDetailViewModel(model, contact =>
            {
                // Anropa Update i stället för SaveAll
                _contactService.Update(contact);
                RefreshCommand.Execute(null);
            });

            var dialog = new ContactDetailView
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }
        private void OnRemove(object? _)
        {
            if (SelectedContact == null) return;

            _contactService.RemoveByEmail(SelectedContact.Email);
            Contacts.Remove(SelectedContact);
        }

        private void OnRefresh(object? _)
        {
            Contacts.Clear();
            foreach (var c in _contactService.LoadAll())
                Contacts.Add(new ContactViewModel(c));
        }
    }
}
