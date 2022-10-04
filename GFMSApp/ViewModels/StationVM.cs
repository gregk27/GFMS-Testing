using GFMS;
using System.Windows.Input;
using System.Windows.Media;

namespace GFMSApp.ViewModels
{
    public class StationVM
    {
        private DriveStation _station;
        public ushort TeamNumber { get => _station.TeamNumber; }

        public string StationName { get => _station.Station.ToString(); }

        public ICommand EnableCommand { get; private set; }
        public ICommand DisableCommand { get; private set; }

        /// <summary>
        /// Status colour code:
        /// - White: No connection
        /// - Black: DS Connected and E-Stopped
        /// - Red: DS Connected but no robot comms
        /// - Orange: Robot connected but disabled
        /// - Green: Robot connected and enabled
        /// </summary>
        public Brush StatusColour
        {
            get
            {
                if (_station.IsConnected)
                {
                    if (_station.IsEStopped)
                        return Brushes.Black;

                    if (_station.RobotComms)
                    {
                        if (_station.RobotEnabled)
                            return Brushes.Lime;
                        else
                            return Brushes.Orange;
                    }
                    else
                    {
                        return Brushes.Red;
                    }
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        public EnabledTextData EnabledText
        {
            get
            {
                if (_station.IsConnected)
                {
                    if (_station.IsEnabled)
                        return new("Enabled", Brushes.Green);
                    else if (_station.IsEStopped)
                        return new("E-Stopped", Brushes.DarkRed);
                    return new("Disabled", Brushes.Black);
                }
                else
                {
                    return new("-", Brushes.Black);
                }
            }
        }

        public EnableButtonData EnableButton
        {
            get
            {
                // If all comms are good and the robot is disabled (but not e-stopped) then the button should enable it
                if (_station.IsConnected && _station.RobotComms
                    && !_station.IsEnabled && !_station.IsEStopped)
                    return new EnableButtonData("Enable", EnableCommand);
                else
                    return new EnableButtonData("Disable", DisableCommand);
            }
        }

        public StationVM(DriveStation station)
        {
            _station = station;
            EnableCommand = new RelayCommand((obj) => true, (obj) => _station.SetEnabled(true));
            DisableCommand = new RelayCommand((obj) => true, (obj) => _station.SetEnabled(false));
        }

        public class EnabledTextData
        {
            public string Text { get; private set; }
            public Brush Colour { get; private set; }

            public EnabledTextData(string text, Brush colour)
            {
                Text = text;
                Colour = colour;
            }
        }

        public class EnableButtonData
        {
            public string Text { get; private set; }
            public ICommand Command { get; private set; }

            public EnableButtonData(string text, ICommand command)
            {
                Text = text;
                Command = command;
            }
        }
    }
}
