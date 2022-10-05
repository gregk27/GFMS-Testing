using GFMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMSApp.ViewModels
{
    internal class UpcomingStationVM
    {
        public Station Station = new(Alliance.RED, 0);

        public Alliance Alliance
        {
            get => Station.Alliance;
            set => Station = new(value, Station.Number);
        }

        public byte Number
        {
            get => Station.Number;
            set => Station = new(Station.Alliance, value);
        }

        public ushort? Team { get; set; }

    }
}
