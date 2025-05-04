using System;
using System.Windows;
using System.Windows.Input;
using AddressBook.Core.Models;
using AddressBook.Gui.Helpers;

namespace AddressBook.Gui.ViewModels
{
    /// <summary>
    /// ViewModel för skapa/uppdatera en kontakt.
    /// </summary>
    public class ContactDetailViewModel : ContactViewModel
    {
        private readonly Action<Contact> _onSaved;

        public ContactDetailViewModel(Contact model, Action<Contact> onSaved)
            : base(model)
        {
            _onSaved = onSaved;
            SaveCommand = new RelayCommand(OnSave, _ => CanSave());
        }

        public ICommand SaveCommand { get; }

        private void OnSave(object? parameter)
        {
            // 1) Anropa callback så att service sparar/uppdaterar modellen
            _onSaved(GetModel());

            // 2) Kasta parametern till Window och sätt DialogResult=true så att fönstret stängs
            if (parameter is Window window)
            {
                window.DialogResult = true;
            }
        }

        private bool CanSave()
        {
            // Minsta validering: förnamn, efternamn och email måste finnas
            return !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(Email);
        }
    }
}
