﻿#if NET5_0_OR_GREATER

namespace Hexa.NET.Mathematics
{
    using System;
    using System.Buffers;
    using System.Buffers.Binary;
    using System.Diagnostics;
    using System.IO;
    using System.Numerics;
    using System.Text;

    internal static class StreamExtensions
    {
#if !NET9_0_OR_GREATER

        public static int ReadExactly(this Stream stream, Span<byte> buffer)
        {
            return ReadAtLeastCore(stream, buffer, buffer.Length, throwOnEndOfStream: true);
        }

        public static int ReadExactly(this Stream stream, byte[] buffer, int offset, int count)
        {
            return ReadAtLeastCore(stream, buffer.AsSpan(offset, count), count, throwOnEndOfStream: true);
        }

        private static int ReadAtLeastCore(Stream stream, Span<byte> buffer, int minimumBytes, bool throwOnEndOfStream)
        {
            Debug.Assert(minimumBytes <= buffer.Length);

            int totalRead = 0;
            while (totalRead < minimumBytes)
            {
                int read = stream.Read(buffer[totalRead..]);
                if (read == 0)
                {
                    if (throwOnEndOfStream)
                    {
                        throw new EndOfStreamException();
                    }

                    return totalRead;
                }

                totalRead += read;
            }

            return totalRead;
        }

#endif

        public static int WriteString(this Span<byte> dest, string str, Encoder encoder)
        {
            BinaryPrimitives.WriteInt32LittleEndian(dest, encoder.GetByteCount(str, true));
            return encoder.GetBytes(str, dest[4..], true) + 4;
        }

        public static int ReadString(this ReadOnlySpan<byte> src, out string str, Decoder decoder)
        {
            int len = BinaryPrimitives.ReadInt32LittleEndian(src);
            ReadOnlySpan<byte> bytes = src.Slice(4, len);
            int charCount = decoder.GetCharCount(bytes, true);
            Span<char> chars = charCount < 2048 ? stackalloc char[charCount] : new char[charCount];
            decoder.GetChars(bytes, chars, true);
            str = new(chars);
            return len + 4;
        }

        public static int SizeOf(this string str, Encoder encoder)
        {
            return 4 + encoder.GetByteCount(str, true);
        }

        public static int WriteInt32(this Span<byte> dest, int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(dest, value);
            return 4;
        }

        public static int ReadInt32(this ReadOnlySpan<byte> src, out int value)
        {
            value = BinaryPrimitives.ReadInt32LittleEndian(src);
            return 4;
        }

        public static void WriteString(this Stream stream, string str, Encoding encoder, Endianness endianness)
        {
            var count = encoder.GetByteCount(str);
            var bytes = count + 4;
            Span<byte> dst = bytes < 2048 ? stackalloc byte[bytes] : new byte[bytes];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dst, count);
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(dst, count);
            }

            encoder.GetBytes(str, dst[4..]);
            stream.Write(dst);
        }

        public static string ReadString(this Stream stream, Encoding decoder, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            stream.ReadExactly(buf);
            int len = 0;
            if (endianness == Endianness.LittleEndian)
            {
                len = BinaryPrimitives.ReadInt32LittleEndian(buf);
            }
            else
            {
                len = BinaryPrimitives.ReadInt32BigEndian(buf);
            }

            Span<byte> src = len < 2048 ? stackalloc byte[len] : new byte[len];
            stream.ReadExactly(src);

            int charCount = decoder.GetCharCount(src);
            Span<char> chars = charCount < 2048 ? stackalloc char[charCount] : new char[charCount];
            decoder.GetChars(src, chars);
            return new(chars);
        }

        public static void WriteInt(this Stream stream, int val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static int ReadInt(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            stream.ReadExactly(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadInt32LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadInt32BigEndian(buf);
            }
        }

        public static uint ReadUInt(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            stream.ReadExactly(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadUInt32LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadUInt32BigEndian(buf);
            }
        }

        public static void WriteUInt(this Stream stream, uint val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteUInt32BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static void WriteUInt64(this Stream stream, ulong val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteUInt64LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteUInt64BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static void WriteInt64(this Stream stream, long val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt64LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteInt64BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static long ReadInt64(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            stream.ReadExactly(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadInt64LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadInt64BigEndian(buf);
            }
        }

        public static ulong ReadUInt64(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            stream.ReadExactly(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadUInt64LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadUInt64BigEndian(buf);
            }
        }

        public static byte[] Read(this Stream stream, long length)
        {
            var buffer = new byte[length];
            stream.ReadExactly(buffer, 0, (int)length);
            return buffer;
        }

        public static bool Compare(this Stream stream, byte[] compare)
        {
#nullable disable
            bool pool = compare.Length > 2048;
            byte[] array = null;
            Span<byte> buffer = pool ? (Span<byte>)(array = ArrayPool<byte>.Shared.Rent(compare.Length)) : (stackalloc byte[compare.Length]);
            stream.ReadExactly(buffer);
            var result = buffer.SequenceEqual(compare);
            if (pool)
            {
                ArrayPool<byte>.Shared.Return(array);
            }

            return result;
#nullable enable
        }

        public static bool Compare(this Stream stream, ulong value, Endianness endianness)
        {
            return stream.ReadUInt64(endianness) == value;
        }

        public static bool Compare(this ReadOnlySpan<byte> src, ulong value)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(src) == value;
        }

        public static string ReadString(this ReadOnlySpan<byte> src, Encoding encoding, out int read)
        {
            var len = BinaryPrimitives.ReadInt32LittleEndian(src);
            read = len + 4;
            return encoding.GetString(src.Slice(4, len));
        }

        public static int WriteString(this Span<byte> dest, ReadOnlySpan<char> src, Encoding encoding)
        {
            var len = encoding.GetByteCount(src);
            BinaryPrimitives.WriteInt32LittleEndian(dest, len);
            encoding.GetBytes(src, dest[4..]);
            return 4 + len;
        }

        public static byte[] ReadBytes(this Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            stream.ReadExactly(bytes, 0, length);
            return bytes;
        }

        public static void WriteVector3(this Stream stream, Vector3 vector, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[12];
            if (endianness == Endianness.LittleEndian)
            {
                WriteSingleLittleEndian(dst, vector.X);
                WriteSingleLittleEndian(dst[4..], vector.Y);
                WriteSingleLittleEndian(dst[8..], vector.Z);
            }
            else
            {
                WriteSingleBigEndian(dst, vector.X);
                WriteSingleBigEndian(dst[4..], vector.Y);
                WriteSingleBigEndian(dst[8..], vector.Z);
            }

            stream.Write(dst);
        }

        public static Vector3 ReadVector3(this Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[12];
            stream.ReadExactly(src);
            Vector3 vector;
            if (endianness == Endianness.LittleEndian)
            {
                vector.X = ReadSingleLittleEndian(src);
                vector.Y = ReadSingleLittleEndian(src[4..]);
                vector.Z = ReadSingleLittleEndian(src[8..]);
            }
            else
            {
                vector.X = ReadSingleBigEndian(src);
                vector.Y = ReadSingleBigEndian(src[4..]);
                vector.Z = ReadSingleBigEndian(src[8..]);
            }

            return vector;
        }

        public static void WriteFloat(this Stream stream, float value, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[4];
            if (endianness == Endianness.LittleEndian)
            {
                WriteSingleLittleEndian(dst, value);
            }
            else
            {
                WriteSingleBigEndian(dst, value);
            }

            stream.Write(dst);
        }

        public static float ReadFloat(this Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[4];
            stream.ReadExactly(src);
            if (endianness == Endianness.LittleEndian)
            {
                return ReadSingleLittleEndian(src);
            }
            else
            {
                return ReadSingleBigEndian(src);
            }
        }

        public static unsafe float ReadSingleLittleEndian(ReadOnlySpan<byte> bytes)
        {
            uint bits = 0;
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 4; i++)
                {
                    bits |= (uint)bytes[i] << i * 8;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    bits |= (uint)bytes[3 - i] << i * 8;
                }
            }
            return *(float*)&bits;
        }

        public static unsafe float ReadSingleBigEndian(ReadOnlySpan<byte> bytes)
        {
            uint bits = 0;
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 4; i++)
                {
                    bits |= (uint)bytes[i] << i * 8;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    bits |= (uint)bytes[3 - i] << i * 8;
                }
            }
            return *(float*)&bits;
        }

        public static unsafe double ReadDoubleLittleEndian(ReadOnlySpan<byte> bytes)
        {
            ulong bits = 0;
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 8; i++)
                {
                    bits |= (uint)bytes[i] << i * 8;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    bits |= (uint)bytes[7 - i] << i * 8;
                }
            }
            return *(double*)&bits;
        }

        public static unsafe double ReadDoubleBigEndian(ReadOnlySpan<byte> bytes)
        {
            ulong bits = 0;
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 8; i++)
                {
                    bits |= (uint)bytes[i] << i * 8;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    bits |= (uint)bytes[7 - i] << i * 8;
                }
            }
            return *(double*)&bits;
        }

        public static unsafe void WriteSingleLittleEndian(Span<byte> bytes, float value)
        {
            uint bits = *(uint*)&value;
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = (byte)(bits >> i * 8);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = (byte)(bits >> (3 - i) * 8);
                }
            }
        }

        public static unsafe void WriteSingleBigEndian(Span<byte> bytes, float value)
        {
            uint bits = *(uint*)&value;
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = (byte)(bits >> i * 8);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = (byte)(bits >> (3 - i) * 8);
                }
            }
        }

        public static unsafe void WriteDoubleLittleEndian(Span<byte> bytes, double value)
        {
            ulong bits = *(ulong*)&value;
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = (byte)(bits >> i * 8);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = (byte)(bits >> (7 - i) * 8);
                }
            }
        }

        public static unsafe void WriteDoubleBigEndian(Span<byte> bytes, double value)
        {
            ulong bits = *(ulong*)&value;
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = (byte)(bits >> i * 8);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = (byte)(bits >> (7 - i) * 8);
                }
            }
        }
    }
}

#endif