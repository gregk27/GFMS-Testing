using GFMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GFMSApp.ViewModels
{
    internal class UpcomingStationVM : INotifyPropertyChanged
    {
        public Station Station = new(Alliance.RED, 0);

        public Alliance Alliance
        {
            get => Station.Alliance;
            set { Station = new(value, Station.Number); OnPropertyChanged("Alliance"); }
        }

        public byte Number
        {
            get => Station.Number;
            set { Station = new(Station.Alliance, value); OnPropertyChanged("Number"); }
        }

        private ushort? _team;
        public ushort? Team
        {
            get => _team;
            set { _team = value; OnPropertyChanged("Team"); }
        }
        public ICommand RemoveCommand { get; private set; }

        public UpcomingStationVM(Action<UpcomingStationVM> removeCallback)
        {
            RemoveCommand = new RelayCommand((obj) => true, (obj) => removeCallback(this));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
