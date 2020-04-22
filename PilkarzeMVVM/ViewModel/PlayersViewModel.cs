using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PilkarzeMVVM.ViewModel
{
    using Model;
    using BaseClass;
    class PlayersViewModel : ViewModelBase
    {
        private PlayersData players = new PlayersData();
        public PlayersViewModel()
        {
            PlayersList = new ObservableCollection<Player>(players.PlayersList);
            PlayersList.CollectionChanged += new NotifyCollectionChangedEventHandler(PlayerListChanged);
            Clear();
        }

        #region Properties
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                onPropertyChanged(nameof(Name));
            }
        }

        private string surname;
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
                onPropertyChanged(nameof(Surname));
            }
        }

        private uint age;
        public uint Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
                onPropertyChanged(nameof(Age));
            }
        }

        private uint weight;
        public uint Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                onPropertyChanged(nameof(Weight));
            }
        }

        public ObservableCollection<Player> PlayersList { get; set; }
        public Player Selected { get; set; }
        #endregion

        #region Commands
        private ICommand _add = null;
        public ICommand Add
        {
            get
            {
                if (_add == null)
                {
                    _add = new RelayCommand(
                        arg => { AddPlayer(); },
                        arg => (Name != null && Surname != null && Name.Trim() != "" && Surname.Trim() != "")
                     );
                }

                return _add;
            }
        }

        private ICommand _edit = null;
        public ICommand Edit
        {
            get
            {
                if (_edit == null)
                {
                    _edit = new RelayCommand(
                        arg => { EditSelected(); },
                        arg => (Selected != null)
                     );
                }

                return _edit;
            }
        }

        private ICommand _delete = null;
        public ICommand Delete
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new RelayCommand(
                        arg => { DeleteSelected(); },
                        arg => (Selected != null)
                     );
                }

                return _delete;
            }
        }

        private ICommand _playerSelectionChanged = null;
        public ICommand PlayerSelectionChanged
        {
            get
            {
                if (_playerSelectionChanged == null)
                {
                    _playerSelectionChanged = new RelayCommand(
                        arg =>
                        {
                            if (Selected != null)
                            {
                                Name = Selected.Name;
                                Surname = Selected.Surname;
                                Age = Selected.Age;
                                Weight = Selected.Weight;
                            }
                        },
                        arg => (true)
                     );
                }

                return _playerSelectionChanged;
            }
        }
        #endregion

        #region Methods
        private void AddPlayer()
        {
            Player temp = new Player(Name.Trim(), Surname.Trim(), Age, Weight);

            if (!PlayersList.Contains(temp))
            {
                PlayersList.Add(temp);
                Clear();
            }
            else
            {
                var dialog = MessageBox.Show($"{temp.ToString()} już jest na liście {Environment.NewLine} Czy wyczyścić formularz?", "Uwaga", MessageBoxButton.OKCancel);
                if (dialog == MessageBoxResult.OK)
                {
                    Clear();
                }
            }
        }
        private void EditSelected()
        {
            Player temp = new Player(Name.Trim(), Surname.Trim(), Age, Weight);

            if (!PlayersList.Contains(temp))
            {
                var dialogResult = MessageBox.Show($"Czy na pewno chcesz zmienić dane  {Environment.NewLine} {Selected}?", "Edycja", MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    PlayersList[PlayersList.IndexOf(Selected)] = temp;
                }
                Clear();
            }
            else
                MessageBox.Show($"{temp.ToString()} już jest na liście.", "Uwaga");
        }
        private void DeleteSelected()
        {
            var dialogResult = MessageBox.Show($"Czy na pewno chcesz usunąć {Environment.NewLine} {Selected}?", "Usuwanie", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                PlayersList.Remove(Selected);
            }
            Clear();
            Selected = null;
        }
        private void Clear()
        {
            Name = null;
            Surname = null;
            Age = 25;
            Weight = 75;
            Selected = null;
        }
        
        private void PlayerListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    players.PlayersList.AddRange(e.NewItems.OfType<Player>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    players.PlayersList.RemoveAll(player => e.OldItems.Contains(player));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    players.PlayersList.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    players.PlayersList[e.OldStartingIndex] = (Player)e.NewItems[0];
                    break;
                default:
                    break;
            }
            players.SaveData("players.txt");
        }

        #endregion
    }
}
