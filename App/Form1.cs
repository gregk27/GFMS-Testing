using GFMS;

namespace App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DriveStation? _station;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ApplyStation_Click(object sender, EventArgs e)
        {
            try
            {
                ushort teamNumber = ushort.Parse(TeamNumber.Text);
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
                _station = new DriveStation(teamNumber, (Station)ds);
                var match = new MatchConfig(new Match(TournamentLevel.TEST, 1), new DriveStation[] { _station });
                Director.SetMatch(match);
                StationChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StationChanged()
        {
            _station.OnConnect += (object? src, DriveStation station) =>
            {
                if (station.TeamNumber.ToString() == TeamNumber.Text)
                {
                    StatusDSComms.Text = "Connected";
                }
            };

            _station.OnDisconnect += (object? src, DriveStation station) =>
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

            _station.OnStateChanged += (object? src, DriveStation station) =>
            {
                if (station.TeamNumber.ToString() == TeamNumber.Text)
                {
                    StatusComms.Text = station.RobotComms.ToString();
                    StatusEnabled.Text = station.RobotEnabled.ToString();
                    StatusEStopped.Text = station.RobotEStopped.ToString();
                    StatusMode.Text = station.RobotMode.ToString();
                    StatusBatt.Text = station.BatteryVoltage.ToString();
                }
            };
        }

        private void EnableButton_Click(object sender, EventArgs e)
        {
            Director.SetEnabled(true);
        }

        private void DisableButton_Click(object sender, EventArgs e)
        {
            Director.SetEnabled(false);
        }

        private void EStopButton_Click(object sender, EventArgs e)
        {
            Director.EStop();
        }

        private void ModeSelect_Changed(object sender, EventArgs e)
        {
            Director.SetEnabled(false);
            switch (ModeSelect.SelectedIndex)
            {
                case 0:
                    Director.SetMode(Mode.TELE);
                    break;
                case 1:
                    Director.SetMode(Mode.AUTO);
                    break;
                case 2:
                    Director.SetMode(Mode.TEST);
                    break;
                default:
                    MessageBox.Show("Invalid mode selected");
                    break;
            }
        }

    }
}