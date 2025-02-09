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
    /// Represents a 2-dimensional double-precision vector.
    /// </summary>
    public struct Vector2D : IEquatable<Vector2D>
    {
        /// <summary>
        /// The X-component of the vector.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y-component of the vector.
        /// </summary>
        public double Y;

        /// <summary>
        /// The number of components in the vector.
        /// </summary>
        internal const int Count = 2;

        /// <summary>
        /// Represents a vector with both components set to zero.
        /// </summary>
        public static readonly Vector2D Zero = new(0, 0);

        /// <summary>
        /// Represents a vector with both components set to one.
        /// </summary>
        public static readonly Vector2D One = new(1, 1);

        /// <summary>
        /// Represents a vector with the X component set to one and the Y component set to zero.
        /// </summary>
        public static readonly Vector2D UnitX = new(1, 0);

        /// <summary>
        /// Represents a vector with the Y component set to one and the X component set to zero.
        /// </summary>
        public static readonly Vector2D UnitY = new(0, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct with the specified X and Y components.
        /// </summary>
        /// <param name="x">The X-component of the vector.</param>
        /// <param name="y">The Y-component of the vector.</param>
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> struct with both components set to the same value.
        /// </summary>
        /// <param name="value">The value to set both components of the vector to.</param>
        public Vector2D(double value)
        {
            X = value;
        }

#if NET5_0_OR_GREATER

        internal static Vector2D Create(double value) => Vector256.Create(value).AsVector2D();

        internal static Vector2D Create(double x, double y) => Vector256.Create(x, y, 0, 0).AsVector2D();
#else

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2D Create(double value) => new(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2D Create(double x, double y) => new(x, y);

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
        public static bool operator ==(Vector2D left, Vector2D right)
        {
#if NET7_0_OR_GREATER
            return left.AsVector256() == right.AsVector256();
#else
            return left.X == right.X && left.Y == right.Y;
#endif
        }

        /// <summary>Returns a value that indicates whether two specified vectors are not equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2D left, Vector2D right)
        {
            return !(left == right);
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256Unsafe() + right.AsVector256Unsafe()).AsVector2D();
#else
            return new(left.X + right.X, left.Y + right.Y);
#endif
        }

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator *(Vector2D left, Vector2D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256Unsafe() * right.AsVector256Unsafe()).AsVector2D();
#else
            return new(left.X * right.X, left.Y * right.Y);
#endif
        }

        /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator *(Vector2D left, double right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256Unsafe() * right).AsVector2D();
#else
            return new(left.X * right, left.Y * right);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator *(double left, Vector2D right) => right * left;

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator /(Vector2D left, Vector2D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256() / right.AsVector256()).AsVector2D();
#else
            return new(left.X / right.X, left.Y / right.Y);
#endif
        }

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator /(Vector2D left, double right)
        {
#if NET8_0_OR_GREATER
            return (left.AsVector256() / right).AsVector2D();
#else
            return left / new Vector2D(right);
#endif
        }

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="op_Subtraction" /> method defines the subtraction operation for <see cref="Vector2D" /> objects.</remarks>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
#if NET7_0_OR_GREATER
            return (left.AsVector256() - right.AsVector256()).AsVector2D();
#else
            return new(left.X - right.X, left.Y - right.Y);
#endif
        }

        /// <summary>Negates the specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Vector2D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator -(Vector2D value)
        {
#if NET7_0_OR_GREATER
            return (-value.AsVector256()).AsVector2D();
#else
            return new(-value.X, -value.Y);
#endif
        }

        /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The absolute value vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Abs(Vector2D value)
        {
#if NET7_0_OR_GREATER
            return Vector256.Abs(value.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Abs(value.X), Math.Abs(value.Y));
#endif
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        public static Vector2D Add(Vector2D left, Vector2D right) => left + right;

        /// <summary>Restricts a vector between a minimum and a maximum value.</summary>
        /// <param name="value1">The vector to restrict.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Clamp(Vector2D value1, Vector2D min, Vector2D max)
        {
#if NET9_0_OR_GREATER
            return Vector256.Clamp(value1.AsVector256(), min.AsVector256(), max.AsVector256()).AsVector2D();
#else
            return Min(Max(value1, min), max);
#endif
        }

#if NET9_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D ClampNative(Vector2D value1, Vector2D min, Vector2D max) => Vector256.ClampNative(value1.AsVector256(), min.AsVector256(), max.AsVector256()).AsVector2D();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D CopySign(Vector2D value, Vector2D sign)
        {
#if NET9_0_OR_GREATER
            return Vector256.CopySign(value.AsVector256(), sign.AsVector256()).AsVector2D();
#else
            return new(MathUtil.CopySign(value.X, sign.X), MathUtil.CopySign(value.Y, sign.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Cos(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Cos(vector.AsVector256()).AsVector2D();
#else
            return new(Math.Cos(vector.X), Math.Cos(vector.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D DegreesToRadians(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.DegreesToRadians(vector.AsVector256()).AsVector2D();
#else
            return vector * MathUtil.DegToRadFactor;
#endif
        }

        /// <summary>Computes the Euclidean distance between the two given points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector2D value1, Vector2D value2) => Math.Sqrt(DistanceSquared(value1, value2));

        /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector2D value1, Vector2D value2) => (value1 - value2).LengthSquared();

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Divide(Vector2D left, Vector2D right) => left / right;

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The vector that results from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Divide(Vector2D left, double divisor) => left / divisor;

        /// <summary>Returns the dot product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector2D vector1, Vector2D vector2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Dot(vector1.AsVector256(), vector2.AsVector256());
#else
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Exp(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Exp(vector.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Exp(vector.X), Math.Exp(vector.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D FusedMultiplyAdd(Vector2D left, Vector2D right, Vector2D addend)
        {
#if NET9_0_OR_GREATER
            return Vector256.FusedMultiplyAdd(left.AsVector256(), right.AsVector256(), addend.AsVector256()).AsVector2D();
#else
            return new Vector2D((left.X * right.X) + addend.X, (left.Y * right.Y) + addend.Y);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Hypot(Vector2D x, Vector2D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.Hypot(x.AsVector256(), y.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Sqrt((x.X * x.X) + (y.X * y.X)), Math.Sqrt((x.Y * x.Y) + (y.Y * y.Y)));
#endif
        }

        /// <summary>Performs a linear interpolation between two vectors based on the given weighting.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">A value between 0 and 1 that indicates the weight of <paramref name="value2" />.</param>
        /// <returns>The interpolated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Lerp(Vector2D value1, Vector2D value2, double amount)
        {
#if NET9_0_OR_GREATER
            return Vector256.Lerp(value1.AsVector256(), value2.AsVector256(), Create(amount).AsVector256()).AsVector2D();
#else
            return value1 * (1.0f - amount) + value2 * amount;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Lerp(Vector2D value1, Vector2D value2, Vector2D amount)
        {
#if NET9_0_OR_GREATER
            return Vector256.Lerp(value1.AsVector256(), value2.AsVector256(), amount.AsVector256()).AsVector2D();
#else
            return new(MathUtil.Lerp(value1.X, value2.X, amount.X), MathUtil.Lerp(value1.Y, value2.Y, amount.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Log(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Log(vector.AsVector256()).AsVector2D();
#else
            return new(Math.Log(vector.X), Math.Log(vector.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Log2(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Log2(vector.AsVector256()).AsVector2D();
#else
            return new(Math.Log(vector.X, 2), Math.Log(vector.Y, 2));
#endif
        }

        /// <summary>Returns a vector whose elements are the maximum of each of the pairs of elements in two specified vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The maximized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Max(Vector2D value1, Vector2D value2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Max(value1.AsVector256(), value2.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MaxMagnitude(Vector2D value1, Vector2D value2)
        {
#if NET9_0_OR_GREATER
            return Vector256.MaxMagnitude(value1.AsVector256(), value2.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Abs(value1.X) > Math.Abs(value2.X) ? value1.X : value2.X, Math.Abs(value1.Y) > Math.Abs(value2.Y) ? value1.Y : value2.Y);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MaxMagnitudeNumber(Vector2D value1, Vector2D value2)
        {
#if NET9_0_OR_GREATER
            return Vector256.MaxMagnitudeNumber(value1.AsVector256(), value2.AsVector256()).AsVector2D();
#else
            return new Vector2D(
                  double.IsNaN(value1.X) ? value2.X : double.IsNaN(value2.X) ? value1.X : Math.Abs(value1.X) > Math.Abs(value2.X) ? value1.X : value2.X,
                  double.IsNaN(value1.Y) ? value2.Y : double.IsNaN(value2.Y) ? value1.Y : Math.Abs(value1.Y) > Math.Abs(value2.Y) ? value1.Y : value2.Y
            );
#endif
        }

#if NET9_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MaxNative(Vector2D value1, Vector2D value2) => Vector256.MaxNative(value1.AsVector256(), value2.AsVector256()).AsVector2D();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MaxNumber(Vector2D value1, Vector2D value2)
        {
#if NET9_0_OR_GREATER
            return Vector256.MaxNumber(value1.AsVector256(), value2.AsVector256()).AsVector2D();
#else
            return new Vector2D(
                double.IsNaN(value1.X) ? value2.X : double.IsNaN(value2.X) ? value1.X : Math.Max(value1.X, value2.X),
                double.IsNaN(value1.Y) ? value2.Y : double.IsNaN(value2.Y) ? value1.Y : Math.Max(value1.Y, value2.Y)
            );
#endif
        }

        /// <summary>Returns a vector whose elements are the minimum of each of the pairs of elements in two specified vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The minimized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Min(Vector2D value1, Vector2D value2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Min(value1.AsVector256(), value2.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MinMagnitude(Vector2D x, Vector2D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.MinMagnitude(x.AsVector256(), y.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Abs(x.X) < Math.Abs(y.X) ? x.X : y.X, Math.Abs(x.Y) < Math.Abs(y.Y) ? x.Y : y.Y);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MinMagnitudeNumber(Vector2D x, Vector2D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.MinMagnitudeNumber(x.AsVector256(), y.AsVector256()).AsVector2D();
#else
            return new Vector2D(
                double.IsNaN(x.X) ? y.X : double.IsNaN(y.X) ? x.X : Math.Abs(x.X) < Math.Abs(y.X) ? x.X : y.X,
                double.IsNaN(x.Y) ? y.Y : double.IsNaN(y.Y) ? x.Y : Math.Abs(x.Y) < Math.Abs(y.Y) ? x.Y : y.Y
            );
#endif
        }

#if NET9_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MinNative(Vector2D value1, Vector2D value2) => Vector256.MinNative(value1.AsVector256(), value2.AsVector256()).AsVector2D();

#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MinNumber(Vector2D x, Vector2D y)
        {
#if NET9_0_OR_GREATER
            return Vector256.MinNumber(x.AsVector256(), y.AsVector256()).AsVector2D();
#else
            return new Vector2D(
                double.IsNaN(x.X) ? y.X : double.IsNaN(y.X) ? x.X : Math.Min(x.X, y.X),
                double.IsNaN(x.Y) ? y.Y : double.IsNaN(y.Y) ? x.Y : Math.Min(x.Y, y.Y)
            );
#endif
        }

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Multiply(Vector2D left, Vector2D right)
        {
            return left * right;
        }

        /// <summary>Multiplies a vector by a specified scalar.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Multiply(Vector2D left, double right)
        {
            return left * right;
        }

        /// <summary>Multiplies a scalar value by a specified vector.</summary>
        /// <param name="left">The scaled value.</param>
        /// <param name="right">The vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Multiply(double left, Vector2D right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D MultiplyAddEstimate(Vector2D left, Vector2D right, Vector2D addend)
        {
#if NET9_0_OR_GREATER
            return Vector256.MultiplyAddEstimate(left.AsVector256(), right.AsVector256(), addend.AsVector256()).AsVector2D();
#else
            return left * right + addend;
#endif
        }

        /// <summary>Negates a specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Negate(Vector2D value) => -value;

        /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Normalize(Vector2D vector) => vector / vector.Length();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D RadiansToDegrees(Vector2D radians)
        {
#if NET9_0_OR_GREATER
            return Vector256.RadiansToDegrees(radians.AsVector256()).AsVector2D();
#else
            return radians * MathUtil.RadToDefFactor;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Round(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Round(vector.AsVector256()).AsVector2D();
#else
            return new(Math.Round(vector.X), Math.Round(vector.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Round(Vector2D vector, MidpointRounding mode)
        {
#if NET9_0_OR_GREATER
            return Vector256.Round(vector.AsVector256(), mode).AsVector2D();
#else
            return new(Math.Round(vector.X, mode), Math.Round(vector.Y, mode));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Sin(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Sin(vector.AsVector256()).AsVector2D();
#else
            return new(Math.Sin(vector.X), Math.Sin(vector.Y));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector2D Sin, Vector2D Cos) SinCos(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            (Vector256<double> sin, Vector256<double> cos) = Vector256.SinCos(vector.AsVector256());
            return (sin.AsVector2D(), cos.AsVector2D());
#else
            return (Sin(vector), Cos(vector));
#endif
        }

        /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The square root vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D SquareRoot(Vector2D value)
        {
#if NET7_0_OR_GREATER
            return Vector256.Sqrt(value.AsVector256()).AsVector2D();
#else
            return new Vector2D(Math.Sqrt(value.X), Math.Sqrt(value.Y));
#endif
        }

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The difference vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Subtract(Vector2D left, Vector2D right) => left - right;
        /*
        /// <summary>Transforms a vector by a specified 3x2 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector2D Transform(Vector2D position, Matrix3x2D matrix) => Transform(position, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2D Transform(Vector2D position, in Matrix3x2D.Impl matrix)
        {
            Vector2D result = matrix.X * position.X;
            result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
            return result + matrix.Z;
        }
        */
        /// <summary>Transforms a vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Transform(Vector2D position, Matrix4x4D matrix) => Vector4D.Transform(position, in matrix.AsImpl()).AsVector2D();

        /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Transform(Vector2D value, Quaternion rotation) => Vector4D.Transform(value, rotation).AsVector2D();
        /*
        /// <summary>Transforms a vector normal by the given 3x2 matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector2D TransformNormal(Vector2D normal, Matrix3x2D matrix) => TransformNormal(normal, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2D TransformNormal(Vector2D normal, in Matrix3x2D.Impl matrix)
        {
            Vector2D result = matrix.X * normal.X;
            result = MultiplyAddEstimate(matrix.Y, Create(normal.Y), result);
            return result;
        }
        */
        /// <summary>Transforms a vector normal by the given 4x4 matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector2D TransformNormal(Vector2D normal, Matrix4x4D matrix) => TransformNormal(normal, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2D TransformNormal(Vector2D normal, in Matrix4x4D.Impl matrix)
        {
            Vector4D result = matrix.X * normal.X;
            result = Vector4D.MultiplyAddEstimate(matrix.Y, Vector4D.Create(normal.Y), result);
            return result.AsVector2D();
        }

        public static Vector2D Truncate(Vector2D vector)
        {
#if NET9_0_OR_GREATER
            return Vector256.Truncate(vector.AsVector256()).AsVector2D();
#else
            return new(Math.Truncate(vector.X), Math.Truncate(vector.Y));
#endif
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj)
        {
            return obj is Vector2D d && Equals(d);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Vector2D other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>Returns the length of this vector object.</summary>
        /// <returns>The vector's length.</returns>
        /// <altmember cref="LengthSquared"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>Returns the length of the vector squared.</summary>
        /// <returns>The vector's length squared.</returns>
        /// <remarks>This operation offers better performance than a call to the <see cref="Length" /> method.</remarks>
        /// <altmember cref="Length"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double LengthSquared()
        {
            return X * X + Y * Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2D(Vector2 v) => new(v.X, v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2D(Point2 v) => new(v.X, v.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2D(UPoint2 v) => new(v.X, v.Y);

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