using GFMS;
using GFMSApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GFMSApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<StationVM> stations = new();
        ObservableCollection<UpcomingStationVM> nextMatch = new();

        public MainWindow()
        {
            InitializeComponent();

            stations.Add(new(new(2708, Station.RED_1)));
            stations.Add(new(new(2, Station.RED_2)));
            stations.Add(new(new(3, Station.RED_3)));

            stations.Add(new(new(2708, Station.BLUE_1)));
            stations.Add(new(new(5, Station.BLUE_2)));
            stations.Add(new(new(6, Station.BLUE_3)));

            ActiveTeamsList.ItemsSource = stations;

            NextMatchTeamList.ItemsSource = nextMatch;
        }

        private void AddStation(object sender, RoutedEventArgs e)
        {
            nextMatch.Add(new UpcomingStationVM((obj) => nextMatch.Remove(obj)));
        }

        private void LoadMatch(object sender, RoutedEventArgs e)
        {
            // TODO: This should interact with the director
            foreach(var station in nextMatch)
            {
                if (station.Team == null)
                    MessageBox.Show($"Team nuber for station {station.Station} not set");
            }

            // Clear previous match
            stations.Clear();
            // Load new stations
            foreach(var station in nextMatch)
            {
                DriveStation ds = new(station.Team ?? 0, station.Station);
                stations.Add(new(ds));
                station.Team = null;
            }
        }

        private void NextMatchTeamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(NextMatchTeamList.SelectedIndex >= 0)
                RemoveNextMatchTeamButton.IsEnabled = true;
            else
                RemoveNextMatchTeamButton.IsEnabled = false;
        }

        private void RemoveStation(object sender, RoutedEventArgs e)
        {
            // Do nothing if nothing is selected
            var selectedIDX = NextMatchTeamList.SelectedIndex;
            if (selectedIDX < 0) return;
            nextMatch.Remove((UpcomingStationVM)NextMatchTeamList.SelectedItem);
            
            // Keep something selected if possible
            if(selectedIDX >= nextMatch.Count) selectedIDX = nextMatch.Count - 1;
            if(selectedIDX >= 0) NextMatchTeamList.SelectedIndex = selectedIDX;
        }
    }
}
