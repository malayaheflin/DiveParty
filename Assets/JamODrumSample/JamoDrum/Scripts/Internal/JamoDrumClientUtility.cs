using System;

namespace ETC.Platforms
{
    /// <summary>
    /// Utility class for Jam-o-Drum Client.
    /// </summary>
    internal class JamoDrumClientUtility
    {
        /// <summary>
        /// Revert int32 endianness.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static int RevertEndian(int value)
        {
            byte[] temp = BitConverter.GetBytes(value);
            Array.Reverse(temp);
            return BitConverter.ToInt32(temp, 0);
        }
    }
}
