using System.ComponentModel;
using System.Runtime.CompilerServices;
using AddressBook.Core.Models;

namespace AddressBook.Gui.ViewModels
{
    /// <summary>
    /// Wrappar en <see cref="Contact"/> för binding i UI.
    /// </summary>
    public class ContactViewModel : INotifyPropertyChanged
    {
        private readonly Contact _model;

        public ContactViewModel(Contact model)
        {
            _model = model;
        }

        public string FirstName
        {
            get => _model.FirstName;
            set
            {
                if (_model.FirstName != value)
                {
                    _model.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => _model.LastName;
            set
            {
                if (_model.LastName != value)
                {
                    _model.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _model.Email;
            set
            {
                if (_model.Email != value)
                {
                    _model.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _model.PhoneNumber;
            set
            {
                if (_model.PhoneNumber != value)
                {
                    _model.PhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _model.Address;
            set
            {
                if (_model.Address != value)
                {
                    _model.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public Contact GetModel() => _model;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
