using GFMS;
using GFMSApp.ViewModels;
using System;
using System.Collections.Generic;
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

            List<StationVM> stations = new();
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
            nextMatch.Add(new UpcomingStationVM());
        }

        private void LoadMatch(object sender, RoutedEventArgs e)
        {
        }
    }
}
