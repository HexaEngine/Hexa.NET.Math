namespace Hexa.NET.Mathematics
{
    using System;
    using System.Buffers.Binary;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Numerics;
    using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER

    using System.Runtime.Intrinsics;

#endif

    /// <summary>
    /// Represents a 2D signed integer point in space.
    /// </summary>
    public struct Point2 : IEquatable<Point2>
    {
        /// <summary>
        /// The X component of the point.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y component of the point.
        /// </summary>
        public int Y;

        internal const int Count = 2;

        /// <summary>
        /// Gets a <see cref="Point2"/> instance with all elements set to zero.
        /// </summary>
        public static readonly Point2 Zero = new(0);

        /// <summary>
        /// Gets a <see cref="Point2"/> instance with all elements set to one.
        /// </summary>
        public static readonly Point2 One = new(1);

        /// <summary>
        /// Gets the <see cref="Point2"/> instance representing the X-axis unit vector (1, 0).
        /// </summary>
        public static readonly Point2 UnitX = new(1, 0);

        /// <summary>
        /// Gets the <see cref="Point2"/> instance representing the Y-axis unit vector (0, 1).
        /// </summary>
        public static readonly Point2 UnitY = new(0, 1);

        /// <summary>
        /// Initializes a new <see cref="Point2"/> instance with the specified value for all elements.
        /// </summary>
        /// <param name="value">The value to set for all elements of the point.</param>
        public Point2(int value)
        {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Initializes a new <see cref="Point2"/> instance with the specified X, and Y components.
        /// </summary>
        /// <param name="x">The X component of the point.</param>
        /// <param name="y">The Y component of the point.</param>
        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

#if NET5_0_OR_GREATER

        public static Point2 Create(int value) => Vector128.Create(value).AsPoint2();

        public static Point2 Create(int x, int y) => Vector128.Create(x, y, 0, 0).AsPoint2();

#endif

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of the valid range.</exception>
        public unsafe int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                }

                return ((int*)Unsafe.AsPointer(ref this))[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                } ((int*)Unsafe.AsPointer(ref this))[index] = value;
            }
        }

        /// <summary>
        /// Determines whether the current <see cref="Point2"/> instance is equal to another <see cref="Point2"/> instance element-wise.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the <see cref="Point2"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public override readonly bool Equals(object? obj)
        {
            return obj is Point2 point && Equals(point);
        }

        /// <summary>
        /// Determines whether the current <see cref="Point2"/> instance is equal to another <see cref="Point2"/> instance element-wise.
        /// </summary>
        /// <param name="other">The <see cref="Point2"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the <see cref="Point2"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Point2 other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="Point2"/> instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Determines whether two <see cref="Point2"/> instances are equal element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="Point2"/> to compare.</param>
        /// <param name="right">The second <see cref="Point2"/> to compare.</param>
        /// <returns><c>true</c> if the <see cref="Point2"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Point2 left, Point2 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="Point2"/> instances are not equal element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="Point2"/> to compare.</param>
        /// <param name="right">The second <see cref="Point2"/> to compare.</param>
        /// <returns><c>true</c> if the <see cref="Point2"/> instances are not equal element-wise; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Point2 left, Point2 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Adds two <see cref="Point2"/> instances element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="Point2"/> to add.</param>
        /// <param name="right">The second <see cref="Point2"/> to add.</param>
        /// <returns>The element-wise sum of the two <see cref="Point2"/> instances.</returns>
        public static Point2 operator +(Point2 left, Point2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() + right.AsVector128Unsafe()).AsPoint2();
#else
            return new Point2(left.X + right.X, left.Y + right.Y);
#endif
        }

        /// <summary>Negates the specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Point2" /> objects.</remarks>
        public static Point2 operator -(Point2 value)
        {
#if NET7_0_OR_GREATER
            return (-value.AsVector128Unsafe()).AsPoint2();
#else
            return new Point2(-value.X, -value.Y);
#endif
        }

        /// <summary>
        /// Subtracts the right <see cref="Point2"/> from the left <see cref="Point2"/> element-wise.
        /// </summary>
        /// <param name="left">The <see cref="Point2"/> to subtract from (minuend).</param>
        /// <param name="right">The <see cref="Point2"/> to subtract (subtrahend).</param>
        /// <returns>The element-wise difference between the left and right <see cref="Point2"/> instances.</returns>
        public static Point2 operator -(Point2 left, Point2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() - right.AsVector128Unsafe()).AsPoint2();
#else
            return new Point2(left.X - right.X, left.Y - right.Y);
#endif
        }

        /// <summary>
        /// Multiplies two <see cref="Point2"/> instances element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="Point2"/> to multiply.</param>
        /// <param name="right">The second <see cref="Point2"/> to multiply.</param>
        /// <returns>The element-wise product of the two <see cref="Point2"/> instances.</returns>
        public static Point2 operator *(Point2 left, Point2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() * right.AsVector128Unsafe()).AsPoint2();
#else
            return new Point2(left.X * right.X, left.Y * right.Y);
#endif
        }

        /// <summary>
        /// Divides the left <see cref="Point2"/> by the right <see cref="Point2"/> element-wise.
        /// </summary>
        /// <param name="left">The <see cref="Point2"/> to divide (dividend).</param>
        /// <param name="right">The <see cref="Point2"/> to divide by (divisor).</param>
        /// <returns>The element-wise division of the left <see cref="Point2"/> by the right <see cref="Point2"/> instances.</returns>
        public static Point2 operator /(Point2 left, Point2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() / right.AsVector128Unsafe()).AsPoint2();
#else
            return new Point2(left.X / right.X, left.Y / right.Y);
#endif
        }

        /// <summary>
        /// Multiplies each element of a <see cref="Point2"/> instance by a constant value.
        /// </summary>
        /// <param name="left">The <see cref="Point2"/> instance to multiply.</param>
        /// <param name="right">The constant value to multiply each element by.</param>
        /// <returns>A new <see cref="Point2"/> instance with each element multiplied by the constant value.</returns>
        public static Point2 operator *(Point2 left, int right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() * right).AsPoint2();
#else
            return new Point2(left.X * right, left.Y * right);
#endif
        }

        /// <summary>Multiplies the scalar value by the specified vector.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="Vector2.op_Multiply" /> method defines the multiplication operation for <see cref="Point2" /> objects.</remarks>
        public static Point2 operator *(int left, Point2 right) => right * left;

        /// <summary>
        /// Divides each element of a <see cref="Point2"/> instance by a constant value.
        /// </summary>
        /// <param name="left">The <see cref="Point2"/> instance to divide.</param>
        /// <param name="right">The constant value to divide each element by.</param>
        /// <returns>A new <see cref="Point2"/> instance with each element divided by the constant value.</returns>
        public static Point2 operator /(Point2 left, int right)
        {
#if NET8_0_OR_GREATER
            return (left.AsVector128Unsafe() / right).AsPoint2();
#else
            return new Point2(left.X / right, left.Y / right);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Abs(Point2 value)
        {
#if NET7_0_OR_GREATER
            return Vector128.Abs(value.AsVector128Unsafe()).AsPoint2();
#else
            return new(Math.Abs(value.X), Math.Abs(value.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Clamp(Point2 value1, Point2 min, Point2 max)
        {
#if NET9_0_OR_GREATER
            return Vector128.Clamp(value1.AsVector128Unsafe(), min.AsVector128Unsafe(), max.AsVector128Unsafe()).AsPoint2();
#else
            return new Point2(MathUtil.Clamp(value1.X, min.X, max.X), MathUtil.Clamp(value1.Y, min.Y, max.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 CopySign(Point2 value, Point2 sign)
        {
#if NET9_0_OR_GREATER
            return Vector128.CopySign(value.AsVector128Unsafe(), sign.AsVector128Unsafe()).AsPoint2();
#else
            return new(MathUtil.CopySign(value.X, sign.X), MathUtil.CopySign(value.Y, sign.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Dot(Point2 x, Point2 y)
        {
#if NET7_0_OR_GREATER
            return Vector128.Dot(x.AsVector128(), y.AsVector128());
#else
            return (x.X * y.X) + (x.Y * y.Y);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LengthSquared()
        {
            return Dot(this, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Length()
        {
            return MathF.Sqrt(LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Point2 x, Point2 y)
        {
            return MathF.Sqrt((x - y).LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Min(Point2 value1, Point2 value2)
        {
#if NET7_0_OR_GREATER
            return Vector128.Min(value1.AsVector128(), value2.AsVector128()).AsPoint2();
#else
            return new(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Max(Point2 value1, Point2 value2)
        {
#if NET7_0_OR_GREATER
            return Vector128.Max(value1.AsVector128(), value2.AsVector128()).AsPoint2();
#else
            return new(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y));
#endif
        }

        public static explicit operator Point2(Vector2 value) => new((int)value.X, (int)value.Y);

        public static implicit operator Point2(Vector2D value) => new((int)value.X, (int)value.Y);

        public static explicit operator Point2(UPoint2 value) => new((int)value.X, (int)value.Y);

        public static implicit operator Vector2(Point2 value) => new(value.X, value.Y);

#if NET8_0_OR_GREATER

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public override readonly string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return $"<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}>";
        }

#endif

#if NET5_0_OR_GREATER

        /// <summary>
        /// Reads a <see cref="Point2"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The source <see cref="Stream"/>.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns>The read <see cref="Point2"/>.</returns>
        public static Point2 Read(Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[8];
            stream.ReadExactly(src);
            Point2 point;
            if (endianness == Endianness.LittleEndian)
            {
                point.X = BinaryPrimitives.ReadInt32LittleEndian(src);
                point.Y = BinaryPrimitives.ReadInt32LittleEndian(src[4..]);
            }
            else
            {
                point.X = BinaryPrimitives.ReadInt32BigEndian(src);
                point.Y = BinaryPrimitives.ReadInt32BigEndian(src[4..]);
            }
            return point;
        }

        /// <summary>
        /// Writes a <see cref="Point2"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The destination <see cref="Stream"/>.</param>
        /// <param name="endianness">The endianness.</param>
        public readonly void Write(Stream stream, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dst, X);
                BinaryPrimitives.WriteInt32LittleEndian(dst[4..], Y);
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(dst, X);
                BinaryPrimitives.WriteInt32BigEndian(dst[4..], Y);
            }

            stream.Write(dst);
        }

        /// <summary>
        /// Reads a <see cref="Point2"/> from a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="src">The source <see cref="ReadOnlySpan{T}"/>.</param>
        /// <param name="endianness">The endianness.</param>
        /// <returns>The read <see cref="Point2"/>.</returns>
        public static Point2 Read(ReadOnlySpan<byte> src, Endianness endianness)
        {
            Point2 point;
            if (endianness == Endianness.LittleEndian)
            {
                point.X = BinaryPrimitives.ReadInt32LittleEndian(src);
                point.Y = BinaryPrimitives.ReadInt32LittleEndian(src[4..]);
            }
            else
            {
                point.X = BinaryPrimitives.ReadInt32BigEndian(src);
                point.Y = BinaryPrimitives.ReadInt32BigEndian(src[4..]);
            }
            return point;
        }

        /// <summary>
        /// Writes a <see cref="Point2"/> to a <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="dst">The destination <see cref="Span{T}"/>.</param>
        /// <param name="endianness">The endianness.</param>
        public readonly void Write(Span<byte> dst, Endianness endianness)
        {
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dst, X);
                BinaryPrimitives.WriteInt32LittleEndian(dst[4..], Y);
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(dst, X);
                BinaryPrimitives.WriteInt32BigEndian(dst[4..], Y);
            }
        }

#endif
    }
}