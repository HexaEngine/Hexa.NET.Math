namespace Hexa.NET.Mathematics
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER

    using System.Runtime.Intrinsics;

#endif

    /// <summary>
    /// Represents a 2D unsigned integer point in space.
    /// </summary>
    public struct UPoint2 : IEquatable<UPoint2>
    {
        /// <summary>
        /// The X component of the point.
        /// </summary>
        public uint X;

        /// <summary>
        /// The Y component of the point.
        /// </summary>
        public uint Y;

        internal const int Count = 2;

        /// <summary>
        /// Gets a <see cref="UPoint2"/> instance with all elements set to zero.
        /// </summary>
        public static readonly UPoint2 Zero = new(0);

        /// <summary>
        /// Gets a <see cref="UPoint2"/> instance with all elements set to one.
        /// </summary>
        public static readonly UPoint2 One = new(1);

        /// <summary>
        /// Gets the <see cref="UPoint2"/> instance representing the X-axis unit vector (1, 0).
        /// </summary>
        public static readonly UPoint2 UnitX = new(1, 0);

        /// <summary>
        /// Gets the <see cref="UPoint2"/> instance representing the Y-axis unit vector (0, 1).
        /// </summary>
        public static readonly UPoint2 UnitY = new(0, 1);

        /// <summary>
        /// Initializes a new <see cref="UPoint2"/> instance with the specified value for all elements.
        /// </summary>
        /// <param name="value">The value to set for all elements of the point.</param>
        public UPoint2(uint value)
        {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Initializes a new <see cref="UPoint2"/> instance with the specified X, and Y components.
        /// </summary>
        /// <param name="x">The X component of the point.</param>
        /// <param name="y">The Y component of the point.</param>
        public UPoint2(uint x, uint y)
        {
            X = x;
            Y = y;
        }

#if NET5_0_OR_GREATER

        public static UPoint2 Create(uint x, uint y) => Vector128.Create(x, y, 0, 0).AsUPoint2();

        public static UPoint2 Create(uint value) => Vector128.Create(value).AsUPoint2();

#endif

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of the valid range.</exception>
        public unsafe uint this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                }

                return ((uint*)Unsafe.AsPointer(ref this))[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                } ((uint*)Unsafe.AsPointer(ref this))[index] = value;
            }
        }

        /// <summary>
        /// Determines whether the current <see cref="UPoint2"/> instance is equal to another <see cref="UPoint2"/> instance element-wise.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the <see cref="UPoint2"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public override readonly bool Equals(object? obj)
        {
            return obj is UPoint2 point && Equals(point);
        }

        /// <summary>
        /// Determines whether the current <see cref="UPoint2"/> instance is equal to another <see cref="UPoint2"/> instance element-wise.
        /// </summary>
        /// <param name="other">The <see cref="UPoint2"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the <see cref="UPoint2"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(UPoint2 other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="UPoint2"/> instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Determines whether two <see cref="UPoint2"/> instances are equal element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint2"/> to compare.</param>
        /// <param name="right">The second <see cref="UPoint2"/> to compare.</param>
        /// <returns><c>true</c> if the <see cref="UPoint2"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public static bool operator ==(UPoint2 left, UPoint2 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="UPoint2"/> instances are not equal element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint2"/> to compare.</param>
        /// <param name="right">The second <see cref="UPoint2"/> to compare.</param>
        /// <returns><c>true</c> if the <see cref="UPoint2"/> instances are not equal element-wise; otherwise, <c>false</c>.</returns>
        public static bool operator !=(UPoint2 left, UPoint2 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Adds two <see cref="UPoint2"/> instances element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint2"/> to add.</param>
        /// <param name="right">The second <see cref="UPoint2"/> to add.</param>
        /// <returns>The element-wise sum of the two <see cref="UPoint2"/> instances.</returns>
        public static UPoint2 operator +(UPoint2 left, UPoint2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() + right.AsVector128Unsafe()).AsUPoint2();
#else
            return new UPoint2(left.X + right.X, left.Y + right.Y);
#endif
        }

        /// <summary>
        /// Subtracts the right <see cref="UPoint2"/> from the left <see cref="UPoint2"/> element-wise.
        /// </summary>
        /// <param name="left">The <see cref="UPoint2"/> to subtract from (minuend).</param>
        /// <param name="right">The <see cref="UPoint2"/> to subtract (subtrahend).</param>
        /// <returns>The element-wise difference between the left and right <see cref="UPoint2"/> instances.</returns>
        public static UPoint2 operator -(UPoint2 left, UPoint2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() - right.AsVector128Unsafe()).AsUPoint2();
#else
            return new UPoint2(left.X - right.X, left.Y - right.Y);
#endif
        }

        /// <summary>
        /// Multiplies two <see cref="UPoint2"/> instances element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint2"/> to multiply.</param>
        /// <param name="right">The second <see cref="UPoint2"/> to multiply.</param>
        /// <returns>The element-wise product of the two <see cref="UPoint2"/> instances.</returns>
        public static UPoint2 operator *(UPoint2 left, UPoint2 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() * right.AsVector128Unsafe()).AsUPoint2();
#else
            return new UPoint2(left.X * right.X, left.Y * right.Y);
#endif
        }

        /// <summary>
        /// Divides the left <see cref="UPoint2"/> by the right <see cref="UPoint2"/> element-wise.
        /// </summary>
        /// <param name="left">The <see cref="UPoint2"/> to divide (dividend).</param>
        /// <param name="right">The <see cref="UPoint2"/> to divide by (divisor).</param>
        /// <returns>The element-wise division of the left <see cref="UPoint2"/> by the right <see cref="UPoint2"/> instances.</returns>
        public static UPoint2 operator /(UPoint2 left, UPoint2 right)
        {
#if NET8_0_OR_GREATER
            return (left.AsVector128Unsafe() / right.AsVector128Unsafe()).AsUPoint2();
#else
            return new UPoint2(left.X / right.X, left.Y / right.Y);
#endif
        }

        /// <summary>
        /// Multiplies each element of a <see cref="UPoint2"/> instance by a constant value.
        /// </summary>
        /// <param name="left">The <see cref="UPoint2"/> instance to multiply.</param>
        /// <param name="right">The constant value to multiply each element by.</param>
        /// <returns>A new <see cref="UPoint2"/> instance with each element multiplied by the constant value.</returns>
        public static UPoint2 operator *(UPoint2 left, uint right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() * right).AsUPoint2();
#else
            return new UPoint2(left.X * right, left.Y * right);
#endif
        }

        public static UPoint2 operator *(uint left, UPoint2 right) => right * left;

        /// <summary>
        /// Divides each element of a <see cref="UPoint2"/> instance by a constant value.
        /// </summary>
        /// <param name="left">The <see cref="UPoint2"/> instance to divide.</param>
        /// <param name="right">The constant value to divide each element by.</param>
        /// <returns>A new <see cref="UPoint2"/> instance with each element divided by the constant value.</returns>
        public static UPoint2 operator /(UPoint2 left, uint right)
        {
#if NET8_0_OR_GREATER
            return (left.AsVector128Unsafe() / right).AsUPoint2();
#else
            return new UPoint2(left.X / right, left.Y / right);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint2 Clamp(UPoint2 value1, UPoint2 min, UPoint2 max)
        {
#if NET9_0_OR_GREATER
            return Vector128.Clamp(value1.AsVector128(), min.AsVector128(), max.AsVector128()).AsUPoint2();
#else
            return new(MathUtil.Clamp(value1.X, min.X, max.X), MathUtil.Clamp(value1.Y, min.Y, max.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint Dot(UPoint2 x, UPoint2 y)
        {
#if NET7_0_OR_GREATER
            return Vector128.Dot(x.AsVector128(), y.AsVector128());
#else
            return (x.X * y.X) + (x.Y * y.Y);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint LengthSquared()
        {
            return Dot(this, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Length(UPoint2 x)
        {
            return MathF.Sqrt(LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(UPoint2 x, UPoint2 y)
        {
            return MathF.Sqrt((x - y).LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint2 Min(UPoint2 value1, UPoint2 value2)
        {
#if NET7_0_OR_GREATER
            return Vector128.Min(value1.AsVector128(), value2.AsVector128()).AsUPoint2();
#else
            return new(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint2 Max(UPoint2 value1, UPoint2 value2)
        {
#if NET7_0_OR_GREATER
            return Vector128.Max(value1.AsVector128(), value2.AsVector128()).AsUPoint2();
#else
            return new(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y));
#endif
        }

        public static explicit operator UPoint2(Vector2 value) => new((uint)value.X, (uint)value.Y);

        public static explicit operator UPoint2(Vector2D value) => new((uint)value.X, (uint)value.Y);

        public static explicit operator UPoint2(Point2 value) => new((uint)value.X, (uint)value.Y);

        public static implicit operator Vector2(UPoint2 value) => new(value.X, value.Y);

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
    }
}