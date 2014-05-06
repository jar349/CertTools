using System;
using System.Text;

namespace certhex
{
    /// <summary>
    /// see: http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa/14333437#14333437
    /// </summary>
    public static class Hex
    {
        /// <summary>
        /// Will return a hex string WITH spaces
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            char first;
            char second;
            int b;

            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                first = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                second = (char)(55 + b + (((b - 10) >> 31) & -7));

                builder.Append(" ");
                builder.Append(first);
                builder.Append(second);
            }

            if (builder.Length > 0)
            {
                builder.Remove(0, 1);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Demands a hex string with no spaces.
        /// </summary>
        /// <param name="Hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string Hex)
        {
            byte[] Bytes = new byte[Hex.Length / 2];
            int[] HexValue = new int[] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 
                0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
            };

            for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
            {
                Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
                                  HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
            }

            return Bytes;
        }
    }
}
