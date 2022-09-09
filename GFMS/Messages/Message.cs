using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Messages
{
    public abstract class Message
    {
        public abstract (byte[], int) ToByteArray();

        protected static void WriteNumber(ushort num, ref byte[] arr, ref int idx)
        {
            var bytes = BitConverter.GetBytes(num);
            arr[idx++] = bytes[0];
            arr[idx++] = bytes[1];
        }

        protected static void WriteNumber(uint num, ref byte[] arr, ref int idx)
        {
            var bytes = BitConverter.GetBytes(num);
            arr[idx++] = bytes[0];
            arr[idx++] = bytes[1];
            arr[idx++] = bytes[2];
            arr[idx++] = bytes[3];
        }

        protected static void WriteNumber(ulong num, ref byte[] arr, ref int idx)
        {
            var bytes = BitConverter.GetBytes(num);
            arr[idx++] = bytes[0];
            arr[idx++] = bytes[1];
            arr[idx++] = bytes[2];
            arr[idx++] = bytes[3];
            arr[idx++] = bytes[4];
            arr[idx++] = bytes[5];
            arr[idx++] = bytes[6];
            arr[idx++] = bytes[7];
        }
    }

    public enum Mode
    {
        TELE = 0,
        TEST = 1,
        AUTO = 2,
    }

    public enum Alliance
    {
        RED = 0,
        BLUE = 1,
    }

    public enum TournamentLevel
    {
        TEST = 0,
        PRACTICE = 1,
        QUALS = 2,
        ELIMS = 3,
    }
}
