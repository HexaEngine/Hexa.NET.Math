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
    /// Represents a 3D unsigned integer point in space.
    /// </summary>
    public struct UPoint3 : IEquatable<UPoint3>
    {
        /// <summary>
        /// The X component of the point.
        /// </summary>
        public uint X;

        /// <summary>
        /// The Y component of the point.
        /// </summary>
        public uint Y;

        /// <summary>
        /// The Z component of the point.
        /// </summary>
        public uint Z;

        internal const int Count = 3;

        /// <summary>
        /// Gets a <see cref="UPoint3"/> instance with all elements set to zero.
        /// </summary>
        public static readonly UPoint3 Zero = new(0);

        /// <summary>
        /// Gets a <see cref="UPoint3"/> instance with all elements set to one.
        /// </summary>
        public static readonly UPoint3 One = new(1);

        /// <summary>
        /// Gets the <see cref="UPoint3"/> instance representing the X-axis unit vector (1, 0, 0).
        /// </summary>
        public static readonly UPoint3 UnitX = new(1, 0, 0);

        /// <summary>
        /// Gets the <see cref="UPoint3"/> instance representing the Y-axis unit vector (0, 1, 0).
        /// </summary>
        public static readonly UPoint3 UnitY = new(0, 1, 0);

        /// <summary>
        /// Gets the <see cref="UPoint3"/> instance representing the Z-axis unit vector (0, 0, 1).
        /// </summary>
        public static readonly UPoint3 UnitZ = new(0, 0, 1);

        /// <summary>
        /// Initializes a new <see cref="UPoint3"/> instance with the specified value for all elements.
        /// </summary>
        /// <param name="value">The value to set for all elements of the point.</param>
        public UPoint3(uint value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Initializes a new <see cref="UPoint3"/> instance with the specified X, Y, and Z components.
        /// </summary>
        /// <param name="x">The X component of the point.</param>
        /// <param name="y">The Y component of the point.</param>
        /// <param name="z">The Z component of the point.</param>
        public UPoint3(uint x, uint y, uint z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new <see cref="UPoint3"/> instance with the X and Y components from a UPoint2 and the specified Z component.
        /// </summary>
        /// <param name="point">The UPoint2 providing the X and Y components.</param>
        /// <param name="z">The Z component of the point.</param>
        public UPoint3(UPoint2 point, uint z)
        {
            X = point.X;
            Y = point.Y;
            Z = z;
        }

#if NET5_0_OR_GREATER

        public static UPoint3 Create(uint x, uint y, uint z) => Vector128.Create(x, y, z, 0).AsUPoint3();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint3 Create(UPoint2 value, uint z) => value.AsVector128Unsafe().WithElement(2, z).AsUPoint3();

        public static UPoint3 Create(uint value) => Vector128.Create(value).AsUPoint3();

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
        /// Determines whether the current <see cref="UPoint3"/> instance is equal to another <see cref="UPoint3"/> instance element-wise.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the <see cref="UPoint3"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public override readonly bool Equals(object? obj)
        {
            return obj is UPoint3 point && Equals(point);
        }

        /// <summary>
        /// Determines whether the current <see cref="UPoint3"/> instance is equal to another <see cref="UPoint3"/> instance element-wise.
        /// </summary>
        /// <param name="other">The <see cref="UPoint3"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the <see cref="UPoint3"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(UPoint3 other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="UPoint3"/> instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>
        /// Determines whether two <see cref="UPoint3"/> instances are equal element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint3"/> to compare.</param>
        /// <param name="right">The second <see cref="UPoint3"/> to compare.</param>
        /// <returns><c>true</c> if the <see cref="UPoint3"/> instances are equal element-wise; otherwise, <c>false</c>.</returns>
        public static bool operator ==(UPoint3 left, UPoint3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="UPoint3"/> instances are not equal element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint3"/> to compare.</param>
        /// <param name="right">The second <see cref="UPoint3"/> to compare.</param>
        /// <returns><c>true</c> if the <see cref="UPoint3"/> instances are not equal element-wise; otherwise, <c>false</c>.</returns>
        public static bool operator !=(UPoint3 left, UPoint3 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Adds two <see cref="UPoint3"/> instances element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint3"/> to add.</param>
        /// <param name="right">The second <see cref="UPoint3"/> to add.</param>
        /// <returns>The element-wise sum of the two <see cref="UPoint3"/> instances.</returns>
        public static UPoint3 operator +(UPoint3 left, UPoint3 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() + right.AsVector128Unsafe()).AsUPoint3();
#else
            return new UPoint3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
#endif
        }

        /// <summary>
        /// Subtracts the right <see cref="UPoint3"/> from the left <see cref="UPoint3"/> element-wise.
        /// </summary>
        /// <param name="left">The <see cref="UPoint3"/> to subtract from (minuend).</param>
        /// <param name="right">The <see cref="UPoint3"/> to subtract (subtrahend).</param>
        /// <returns>The element-wise difference between the left and right <see cref="UPoint3"/> instances.</returns>
        public static UPoint3 operator -(UPoint3 left, UPoint3 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() - right.AsVector128Unsafe()).AsUPoint3();
#else
            return new UPoint3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
#endif
        }

        /// <summary>
        /// Multiplies two <see cref="UPoint3"/> instances element-wise.
        /// </summary>
        /// <param name="left">The first <see cref="UPoint3"/> to multiply.</param>
        /// <param name="right">The second <see cref="UPoint3"/> to multiply.</param>
        /// <returns>The element-wise product of the two <see cref="UPoint3"/> instances.</returns>
        public static UPoint3 operator *(UPoint3 left, UPoint3 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() * right.AsVector128Unsafe()).AsUPoint3();
#else
            return new UPoint3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
#endif
        }

        /// <summary>
        /// Divides the left <see cref="UPoint3"/> by the right <see cref="UPoint3"/> element-wise.
        /// </summary>
        /// <param name="left">The <see cref="UPoint3"/> to divide (dividend).</param>
        /// <param name="right">The <see cref="UPoint3"/> to divide by (divisor).</param>
        /// <returns>The element-wise division of the left <see cref="UPoint3"/> by the right <see cref="UPoint3"/> instances.</returns>
        public static UPoint3 operator /(UPoint3 left, UPoint3 right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() / right.AsVector128Unsafe()).AsUPoint3();
#else
            return new UPoint3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
#endif
        }

        /// <summary>
        /// Multiplies each element of a <see cref="UPoint3"/> instance by a constant value.
        /// </summary>
        /// <param name="left">The <see cref="UPoint3"/> instance to multiply.</param>
        /// <param name="right">The constant value to multiply each element by.</param>
        /// <returns>A new <see cref="UPoint3"/> instance with each element multiplied by the constant value.</returns>
        public static UPoint3 operator *(UPoint3 left, uint right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector128Unsafe() * right).AsUPoint3();
#else
            return new UPoint3(left.X * right, left.Y * right, left.Z * right);
#endif
        }

        public static UPoint3 operator *(uint left, UPoint3 right) => right * left;

        /// <summary>
        /// Divides each element of a <see cref="UPoint3"/> instance by a constant value.
        /// </summary>
        /// <param name="left">The <see cref="UPoint3"/> instance to divide.</param>
        /// <param name="right">The constant value to divide each element by.</param>
        /// <returns>A new <see cref="UPoint3"/> instance with each element divided by the constant value.</returns>
        public static UPoint3 operator /(UPoint3 left, uint right)
        {
#if NET8_0_OR_GREATER
            return (left.AsVector128Unsafe() / right).AsUPoint3();
#else
            return new UPoint3(left.X / right, left.Y / right, left.Z / right);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint3 Clamp(UPoint3 value1, UPoint3 min, UPoint3 max)
        {
#if NET9_0_OR_GREATER
            return Vector128.Clamp(value1.AsVector128(), min.AsVector128(), max.AsVector128()).AsUPoint3();
#else
            return new(MathUtil.Clamp(value1.X, min.X, max.X), MathUtil.Clamp(value1.Y, min.Y, max.Y), MathUtil.Clamp(value1.Z, min.Z, max.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint Dot(UPoint3 x, UPoint3 y)
        {
#if NET7_0_OR_GREATER
            return Vector128.Dot(x.AsVector128(), y.AsVector128());
#else
            return (x.X * y.X) + (x.Y * y.Y) + (x.Z * y.Z);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint LengthSquared()
        {
            return Dot(this, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Length()
        {
            return MathF.Sqrt(LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(UPoint3 x, UPoint3 y)
        {
            return MathF.Sqrt((x - y).LengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint3 Min(UPoint3 value1, UPoint3 value2)
        {
#if NET7_0_OR_GREATER
            return Vector128.Min(value1.AsVector128(), value2.AsVector128()).AsUPoint3();
#else
            return new(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint3 Max(UPoint3 value1, UPoint3 value2)
        {
#if NET7_0_OR_GREATER
            return Vector128.Max(value1.AsVector128(), value2.AsVector128()).AsUPoint3();
#else
            return new(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z));
#endif
        }

        public static explicit operator UPoint3(Vector3 value) => new((uint)value.X, (uint)value.Y, (uint)value.Z);

        public static explicit operator UPoint3(Vector3D value) => new((uint)value.X, (uint)value.Y, (uint)value.Z);

        public static explicit operator UPoint3(Point3 value) => new((uint)value.X, (uint)value.Y, (uint)value.Z);

        public static implicit operator Vector3(UPoint3 value) => new(value.X, value.Y, value.Z);

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

            return $"<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Z.ToString(format, formatProvider)}>";
        }

#endif
    }
}