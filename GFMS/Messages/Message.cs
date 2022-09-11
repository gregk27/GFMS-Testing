using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Messages
{
    public abstract class Message
    {
        public Message() { }

        public abstract (byte[], int) ToByteArray();
        protected abstract void FromByteArray(byte[] data);

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

        protected static ushort ReadShort(byte[] arr, ref int idx)
        {
            return (ushort)(arr[idx++] << 8 + arr[idx]);
        }
        protected static uint ReadInt(byte[] arr, ref int idx)
        {
            return (ushort)(arr[idx++] << 16 + arr[idx++] << 8 + arr[idx]);
        }
        protected static ulong ReadLong(byte[] arr, ref int idx)
        {
            return (ushort)(arr[idx++] << 24 + arr[idx++] << 16 + arr[idx++] << 8 + arr[idx]);
        }

        protected static bool CheckBit(byte subject, int bit)
        {
            return (subject & 0b1 << bit) != 0;
        }

        protected static void SetBit(ref byte subject, int bit, bool val)
        {
            if (val)
                subject |= (byte)(0b1 << bit);
            else
                subject &= (byte)(0b0 << bit);
        }

        /// <summary>
        /// Create a new message of specified type from byte array
        /// </summary>
        /// <typeparam name="T">Type of message to create</typeparam>
        /// <param name="bytes">Byte array containing data</param>
        /// <returns>Created message</returns>
        public static T FromBytes<T>(byte[] bytes)
            where T : Message, new()
        {
            T message = new();
            message.FromByteArray(bytes);
            return message;
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
