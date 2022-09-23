using GFMS;

namespace App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.Director.StationConnected += (object? src, ConnectedStation station) =>
            {
                if (station.TeamNumber.ToString() == TeamNumber.Text)
                {
                    StatusDSComms.Text = "Connected";
                }
            };

            Program.Director.StationConnected += (object? src, ConnectedStation station) =>
            {
                if (station.TeamNumber.ToString() == TeamNumber.Text)
                {
                    StatusDSComms.Text = "Disconnected";
                    StatusComms.Text = "...";
                    StatusEnabled.Text = "...";
                    StatusEStopped.Text = "...";
                    StatusMode.Text = "...";
                    StatusBatt.Text = "...";
                }
            };

            Program.Director.StationStateChanged += (object? src, ConnectedStation station) =>
            {
                if (station.TeamNumber.ToString() == TeamNumber.Text)
                {
                    StatusComms.Text = station.HasComms.ToString();
                    StatusEnabled.Text = station.IsEnabled.ToString();
                    StatusEStopped.Text = station.IsEstopped.ToString();
                    StatusMode.Text = station.RobotMode.ToString();
                    StatusBatt.Text = station.BatteryVoltage.ToString();
                }
            };


        }

        private void ApplyStation_Click(object sender, EventArgs e)
        {
            Program.Director.ClearStations();
            try
            {
                short teamNumber = short.Parse(TeamNumber.Text);
                Station? ds = null;
                switch (StationSelect.SelectedIndex)
                {
                    case 0:
                        ds = Station.RED_1;
                        break;
                    case 1:
                        ds = Station.RED_2;
                        break;
                    case 2:
                        ds = Station.RED_3;
                        break;
                    case 3:
                        ds = Station.BLUE_1;
                        break;
                    case 4:
                        ds = Station.BLUE_2;
                        break;
                    case 5:
                        ds = Station.BLUE_3;
                        break;
                }
                Director.StationMappings.Add(teamNumber, (Station)ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EnableButton_Click(object sender, EventArgs e)
        {
            Program.Director.SetEnabled(true);
        }

        private void DisableButton_Click(object sender, EventArgs e)
        {
            Program.Director.SetEnabled(false);
        }

        private void EStopButton_Click(object sender, EventArgs e)
        {
            Program.Director.EStop();
        }

        private void ModeSelect_Changed(object sender, EventArgs e)
        {
            Program.Director.SetEnabled(false);
            switch (ModeSelect.SelectedIndex)
            {
                case 0:
                    Program.Director.SetMode(Mode.TELE);
                    break;
                case 1:
                    Program.Director.SetMode(Mode.AUTO);
                    break;
                case 2:
                    Program.Director.SetMode(Mode.TEST);
                    break;
                default:
                    MessageBox.Show("Invalid mode selected");
                    break;
            }
        }

    }
}