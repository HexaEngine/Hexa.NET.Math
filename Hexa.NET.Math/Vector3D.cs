// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Converted to double by Juna Meinhold (c) 2025, MIT license.

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
    /// Represents a 3-dimensional double-precision vector.
    /// </summary>
    public struct Vector3D : IEquatable<Vector3D>
    {
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double Y;

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public double Z;

        internal const int Count = 3;

        /// <summary>
        /// Gets a <see cref="Vector3D"/> instance with all elements set to zero.
        /// </summary>
        public static readonly Vector3D Zero = new(0);

        /// <summary>
        /// Gets a <see cref="Vector3D"/> instance with all elements set to one.
        /// </summary>
        public static readonly Vector3D One = new(1);

        /// <summary>
        /// Gets the <see cref="Vector3D"/> instance representing the X-axis unit vector (1, 0, 0).
        /// </summary>
        public static readonly Vector3D UnitX = new(1, 0, 0);

        /// <summary>
        /// Gets the <see cref="Vector3D"/> instance representing the Y-axis unit vector (0, 1, 0).
        /// </summary>
        public static readonly Vector3D UnitY = new(0, 1, 0);

        /// <summary>
        /// Gets the <see cref="Vector3D"/> instance representing the Z-axis unit vector (0, 0, 1).
        /// </summary>
        public static readonly Vector3D UnitZ = new(0, 0, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> struct with the specified X, Y, and Z components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> struct with the specified <see cref="Vector2D"/> and Z component.
        /// </summary>
        /// <param name="vector">The 2D vector containing the X and Y components.</param>
        /// <param name="z">The Z component of the vector.</param>
        public Vector3D(Vector2D vector, double z)
        {
            X = vector.X;
            Y = vector.Y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3D"/> struct with the same value for all components.
        /// </summary>
        /// <param name="value">The value to set for all components of the vector.</param>
        public Vector3D(double value)
        {
            X = value;
            Y = value;
            Z = value;
        }

#if NET5_0_OR_GREATER

        internal static Vector3D Create(double value) => Vector256.Create(value).AsVector3D();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector3D Create(Vector2D value, double z) => value.AsVector256Unsafe().WithElement(2, z).AsVector3D();

        internal static Vector3D Create(double x, double y, double z) => Vector256.Create(x, y, z, 0).AsVector3D();

#else

        internal static Vector3D Create(double value) => new(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector3D Create(Vector2D value, double z) => new(value, z);

        internal static Vector3D Create(double x, double y, double z) => new(x, y, z);

#endif

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of the valid range.</exception>
        public unsafe double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                }

                return ((double*)Unsafe.AsPointer(ref this))[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                } ((double*)Unsafe.AsPointer(ref this))[index] = value;
            }
        }

        /// <summary>Returns a value that indicates whether each pair of elements in two specified vectors is equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3D left, Vector3D right)
        {
#if NET7_0_OR_GREATER
            return left.AsVector256() == right.AsVector256();
#else
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
#endif
        }

        /// <summary>Returns a value that indicates whether two specified vectors are not equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return !(left == right);
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256Unsafe() + right.AsVector256Unsafe()).AsVector3D();
#else
            return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
#endif
        }

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(Vector3D left, Vector3D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256Unsafe() * right.AsVector256Unsafe()).AsVector3D();
#else
            return new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
#endif
        }

        /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(Vector3D left, double right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256Unsafe() * right).AsVector3D();
#else
            return new(left.X * right, left.Y * right, left.Z * right);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(double left, Vector3D right) => right * left;

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator /(Vector3D left, Vector3D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256() / right.AsVector256()).AsVector3D();
#else
            return new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
#endif
        }

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator /(Vector3D left, double right)
        {
#if NET8_0_OR_GREATER
            return (left.AsVector256() / right).AsVector3D();
#else
            return left / new Vector3D(right);
#endif
        }

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="op_Subtraction" /> method defines the subtraction operation for <see cref="Vector3D" /> objects.</remarks>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256() - right.AsVector256()).AsVector3D();
#else
            return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
#endif
        }

        /// <summary>Negates the specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Vector3D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator -(Vector3D value)
        {
#if NET7_0_OR_GREATER
            return (-value.AsVector256()).AsVector3D();
#else
            return new(-value.X, -value.Y, -value.Z);
#endif
        }

        /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The absolute value vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Abs(Vector3D value)
        {
#if NET7_0_OR_GREATER
            return Vector256.Abs(value.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
#endif
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        public static Vector3D Add(Vector3D left, Vector3D right) => left + right;

        /// <summary>Restricts a vector between a minimum and a maximum value.</summary>
        /// <param name="value1">The vector to restrict.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Clamp(Vector3D value1, Vector3D min, Vector3D max)
        {
#if NET9_0_OR_GREATER
            return Vector256.Clamp(value1.AsVector256(), min.AsVector256(), max.AsVector256()).AsVector3D();
#else
            return Min(Max(value1, min), max);
#endif
        }

#if NET9_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D ClampNative(Vector3D value1, Vector3D min, Vector3D max) => Vector256.ClampNative(value1.AsVector256(), min.AsVector256(), max.AsVector256()).AsVector3D();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D CopySign(Vector3D value, Vector3D sign)
        {
#if NET9_0_OR_GREATER
            return Vector256.CopySign(value.AsVector256(), sign.AsVector256()).AsVector3D();
#else
            return new(MathUtil.CopySign(value.X, sign.X), MathUtil.CopySign(value.Y, sign.Y), MathUtil.CopySign(value.Z, sign.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Cos(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Cos(vector.AsVector256()).AsVector3D();
#else
            return new(Math.Cos(vector.X), Math.Cos(vector.Y), Math.Cos(vector.Z));
#endif
        }

        /// <summary>Computes the cross product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
#if NET9_0_OR_GREATER
            Vector256<double> v1 = vector1.AsVector256();
            Vector256<double> v2 = vector2.AsVector256();

            Vector256<double> temp = Vector256.Shuffle(v1, Vector256.Create(1, 2, 0, 3)) * Vector256.Shuffle(v2, Vector256.Create(2, 0, 1, 3));

            return Vector256.MultiplyAddEstimate(
                -Vector256.Shuffle(v1, Vector256.Create(2, 0, 1, 3)),
                 Vector256.Shuffle(v2, Vector256.Create(1, 2, 0, 3)),
                 temp
            ).AsVector3D();
#elif NET7_0_OR_GREATER
            Vector256<double> v1 = vector1.AsVector256();
            Vector256<double> v2 = vector2.AsVector256();

            Vector256<double> temp = Vector256.Shuffle(v1, Vector256.Create(1, 2, 0, 3)) * Vector256.Shuffle(v2, Vector256.Create(2, 0, 1, 3));

            return Vector256.Add(Vector256.Multiply(-Vector256.Shuffle(v1, Vector256.Create(2, 0, 1, 3)), Vector256.Shuffle(v2, Vector256.Create(1, 2, 0, 3))), temp).AsVector3D();
#else
            return new Vector3D(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D DegreesToRadians(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.DegreesToRadians(vector.AsVector256()).AsVector3D();
#else
            return vector * MathUtil.DegToRadFactor;
#endif
        }

        /// <summary>Computes the Euclidean distance between the two given points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector3D value1, Vector3D value2) => Math.Sqrt(DistanceSquared(value1, value2));

        /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector3D value1, Vector3D value2) => (value1 - value2).LengthSquared();

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Divide(Vector3D left, Vector3D right) => left / right;

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The vector that results from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Divide(Vector3D left, double divisor) => left / divisor;

        /// <summary>Returns the dot product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Dot(vector1.AsVector256(), vector2.AsVector256());
#else
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Exp(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Exp(vector.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Exp(vector.X), Math.Exp(vector.Y), Math.Exp(vector.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D FusedMultiplyAdd(Vector3D left, Vector3D right, Vector3D addend)
        {
#if NET9_0_OR_GREATER
            return Vector256.FusedMultiplyAdd(left.AsVector256(), right.AsVector256(), addend.AsVector256()).AsVector3D();
#else
            return new Vector3D(left.X * right.X + addend.X, left.Y * right.Y + addend.Y, left.Z * right.Z + addend.Z);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Hypot(Vector3D x, Vector3D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.Hypot(x.AsVector256(), y.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Sqrt(x.X * x.X + y.X * y.X), Math.Sqrt(x.Y * x.Y + y.Y * y.Y), Math.Sqrt(x.Z * x.Z + y.Z * y.Z));
#endif
        }

        /// <summary>Performs a linear interpolation between two vectors based on the given weighting.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">A value between 0 and 1 that indicates the weight of <paramref name="value2" />.</param>
        /// <returns>The interpolated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Lerp(Vector3D value1, Vector3D value2, double amount)
        {
#if NET9_0_OR_GREATER
            return Vector256.Lerp(value1.AsVector256(), value2.AsVector256(), Create(amount).AsVector256()).AsVector3D();
#else
            return value1 * (1.0f - amount) + value2 * amount;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Lerp(Vector3D value1, Vector3D value2, Vector3D amount)
        {
#if NET9_0_OR_GREATER
            return Vector256.Lerp(value1.AsVector256(), value2.AsVector256(), amount.AsVector256()).AsVector3D();
#else
            return new(MathUtil.Lerp(value1.X, value2.X, amount.X), MathUtil.Lerp(value1.Y, value2.Y, amount.Y), MathUtil.Lerp(value1.Z, value2.Z, amount.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Log(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Log(vector.AsVector256()).AsVector3D();
#else
            return new(Math.Log(vector.X), Math.Log(vector.Y), Math.Log(vector.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Log2(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Log2(vector.AsVector256()).AsVector3D();
#else
            return new(Math.Log(vector.X, 2), Math.Log(vector.Y, 2), Math.Log(vector.Z, 2));
#endif
        }

        /// <summary>Returns a vector whose elements are the maximum of each of the pairs of elements in two specified vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The maximized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Max(Vector3D value1, Vector3D value2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Max(value1.AsVector256(), value2.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxMagnitude(Vector3D value1, Vector3D value2)
        {
#if NET9_0_OR_GREATER
            return Vector256.MaxMagnitude(value1.AsVector256(), value2.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Abs(value1.X) > Math.Abs(value2.X) ? value1.X : value2.X, Math.Abs(value1.Y) > Math.Abs(value2.Y) ? value1.Y : value2.Y, Math.Abs(value1.Z) > Math.Abs(value2.Z) ? value1.Z : value2.Z);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxMagnitudeNumber(Vector3D value1, Vector3D value2)
        {
#if NET9_0_OR_GREATER
            return Vector256.MaxMagnitudeNumber(value1.AsVector256(), value2.AsVector256()).AsVector3D();
#else
            return new Vector3D(
                  double.IsNaN(value1.X) ? value2.X : double.IsNaN(value2.X) ? value1.X : Math.Abs(value1.X) > Math.Abs(value2.X) ? value1.X : value2.X,
                  double.IsNaN(value1.Y) ? value2.Y : double.IsNaN(value2.Y) ? value1.Y : Math.Abs(value1.Y) > Math.Abs(value2.Y) ? value1.Y : value2.Y,
                  double.IsNaN(value1.Z) ? value2.Z : double.IsNaN(value2.Z) ? value1.Z : Math.Abs(value1.Z) > Math.Abs(value2.Z) ? value1.Z : value2.Z
            );
#endif
        }

#if NET9_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxNative(Vector3D value1, Vector3D value2) => Vector256.MaxNative(value1.AsVector256(), value2.AsVector256()).AsVector3D();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxNumber(Vector3D value1, Vector3D value2)
        {
#if NET9_0_OR_GREATER
            return Vector256.MaxNumber(value1.AsVector256(), value2.AsVector256()).AsVector3D();
#else
            return new Vector3D(
                double.IsNaN(value1.X) ? value2.X : double.IsNaN(value2.X) ? value1.X : Math.Max(value1.X, value2.X),
                double.IsNaN(value1.Y) ? value2.Y : double.IsNaN(value2.Y) ? value1.Y : Math.Max(value1.Y, value2.Y),
                double.IsNaN(value1.Z) ? value2.Z : double.IsNaN(value2.Z) ? value1.Z : Math.Max(value1.Z, value2.Z)
            );
#endif
        }

        /// <summary>Returns a vector whose elements are the minimum of each of the pairs of elements in two specified vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The minimized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Min(Vector3D value1, Vector3D value2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Min(value1.AsVector256(), value2.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinMagnitude(Vector3D x, Vector3D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.MinMagnitude(x.AsVector256(), y.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Abs(x.X) < Math.Abs(y.X) ? x.X : y.X, Math.Abs(x.Y) < Math.Abs(y.Y) ? x.Y : y.Y, Math.Abs(x.Z) < Math.Abs(y.Z) ? x.Z : y.Z);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinMagnitudeNumber(Vector3D x, Vector3D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.MinMagnitudeNumber(x.AsVector256(), y.AsVector256()).AsVector3D();
#else
            return new Vector3D(
                double.IsNaN(x.X) ? y.X : double.IsNaN(y.X) ? x.X : Math.Abs(x.X) < Math.Abs(y.X) ? x.X : y.X,
                double.IsNaN(x.Y) ? y.Y : double.IsNaN(y.Y) ? x.Y : Math.Abs(x.Y) < Math.Abs(y.Y) ? x.Y : y.Y,
                double.IsNaN(x.Z) ? y.Z : double.IsNaN(y.Z) ? x.Z : Math.Abs(x.Z) < Math.Abs(y.Z) ? x.Z : y.Z
            );
#endif
        }

#if NET9_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinNative(Vector3D value1, Vector3D value2) => Vector256.MinNative(value1.AsVector256(), value2.AsVector256()).AsVector3D();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinNumber(Vector3D x, Vector3D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.MinNumber(x.AsVector256(), y.AsVector256()).AsVector3D();
#else
            return new Vector3D(
                double.IsNaN(x.X) ? y.X : double.IsNaN(y.X) ? x.X : Math.Min(x.X, y.X),
                double.IsNaN(x.Y) ? y.Y : double.IsNaN(y.Y) ? x.Y : Math.Min(x.Y, y.Y),
                double.IsNaN(x.Z) ? y.Z : double.IsNaN(y.Z) ? x.Z : Math.Min(x.Z, y.Z)
            );
#endif
        }

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Multiply(Vector3D left, Vector3D right)
        {
            return left * right;
        }

        /// <summary>Multiplies a vector by a specified scalar.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Multiply(Vector3D left, double right)
        {
            return left * right;
        }

        /// <summary>Multiplies a scalar value by a specified vector.</summary>
        /// <param name="left">The scaled value.</param>
        /// <param name="right">The vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Multiply(double left, Vector3D right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MultiplyAddEstimate(Vector3D left, Vector3D right, Vector3D addend)
        {
#if NET9_0_OR_GREATER
            return Vector256.MultiplyAddEstimate(left.AsVector256(), right.AsVector256(), addend.AsVector256()).AsVector3D();
#else
            return left * right + addend;
#endif
        }

        /// <summary>Negates a specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Negate(Vector3D value) => -value;

        /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Normalize(Vector3D vector) => vector / vector.Length();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D RadiansToDegrees(Vector3D radians)
        {
#if NET9_0_OR_GREATER
            return Vector256.RadiansToDegrees(radians.AsVector256()).AsVector3D();
#else
            return radians * MathUtil.RadToDefFactor;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Round(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Round(vector.AsVector256()).AsVector3D();
#else
            return new(Math.Round(vector.X), Math.Round(vector.Y), Math.Round(vector.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Round(Vector3D vector, MidpointRounding mode)
        {
#if NET9_0_OR_GREATER
            return Vector256.Round(vector.AsVector256(), mode).AsVector3D();
#else
            return new(Math.Round(vector.X, mode), Math.Round(vector.Y, mode), Math.Round(vector.Z, mode));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Sin(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Sin(vector.AsVector256()).AsVector3D();
#else
            return new(Math.Sin(vector.X), Math.Sin(vector.Y), Math.Sin(vector.Z));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector3D Sin, Vector3D Cos) SinCos(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            (Vector256<double> sin, Vector256<double> cos) = Vector256.SinCos(vector.AsVector256());
            return (sin.AsVector3D(), cos.AsVector3D());
#else
            return (Sin(vector), Cos(vector));
#endif
        }

        /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The square root vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D SquareRoot(Vector3D value)
        {
#if NET7_0_OR_GREATER
            return Vector256.Sqrt(value.AsVector256()).AsVector3D();
#else
            return new Vector3D(Math.Sqrt(value.X), Math.Sqrt(value.Y), Math.Sqrt(value.Z));
#endif
        }

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The difference vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Subtract(Vector3D left, Vector3D right) => left - right;

        /// <summary>Transforms a vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Transform(Vector3D position, Matrix4x4D matrix) => Vector4D.Transform(position, in matrix.AsImpl()).AsVector3D();

        /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Transform(Vector3D value, Quaternion rotation) => Vector4D.Transform(value, rotation).AsVector3D();

        /// <summary>Transforms a vector normal by the given 4x4 matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D TransformNormal(Vector3D normal, Matrix4x4D matrix) => TransformNormal(normal, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector3D TransformNormal(Vector3D normal, in Matrix4x4D.Impl matrix)
        {
            Vector4D result = matrix.X * normal.X;

            result = Vector4D.MultiplyAddEstimate(matrix.Y, Vector4D.Create(normal.Y), result);
            result = Vector4D.MultiplyAddEstimate(matrix.Z, Vector4D.Create(normal.Z), result);

            return result.AsVector3D();
        }

        public static Vector3D Truncate(Vector3D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Truncate(vector.AsVector256()).AsVector3D();
#else
            return new(Math.Truncate(vector.X), Math.Truncate(vector.Y), Math.Truncate(vector.Z));
#endif
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj)
        {
            return obj is Vector3D d && Equals(d);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Vector3D other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>Returns the length of this vector object.</summary>
        /// <returns>The vector's length.</returns>
        /// <altmember cref="LengthSquared"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double Length()
        {
            double lengthSquared = LengthSquared();
            return Math.Sqrt(lengthSquared);
        }

        /// <summary>Returns the length of the vector squared.</summary>
        /// <returns>The vector's length squared.</returns>
        /// <remarks>This operation offers better performance than a call to the <see cref="Length" /> method.</remarks>
        /// <altmember cref="Length"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double LengthSquared()
        {
            return Dot(this, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3D(Vector3 v) => new(v.X, v.Y, v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3D(Point3 v) => new(v.X, v.Y, v.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3D(UPoint3 v) => new(v.X, v.Y, v.Z);

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