namespace Hexa.NET.Mathematics
{
    using System;
    using System.Runtime.CompilerServices;

    public static unsafe class HexHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HexToInt(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            throw new ArgumentException("Invalid hex character.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HexToInt(char a, char b)
        {
            return HexToInt(a) << 4 | HexToInt(b);
        }
    }
}