// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Converted to double by Juna Meinhold (c) 2025, MIT license.

namespace Hexa.NET.Mathematics
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER
    using System.Runtime.Intrinsics;
#endif

    /// <summary>Represents a plane in three-dimensional space.</summary>
    public unsafe struct PlaneD : IEquatable<PlaneD>
    {
        /// <summary>The normal vector of the plane.</summary>
        public Vector3D Normal;

        /// <summary>The distance of the plane along its normal from the origin.</summary>
        public double D;

        /// <summary>Creates a <see cref="PlaneD" /> object from the X, Y, and Z components of its normal, and its distance from the origin on that normal.</summary>
        /// <param name="x">The X component of the normal.</param>
        /// <param name="y">The Y component of the normal.</param>
        /// <param name="z">The Z component of the normal.</param>
        /// <param name="d">The distance of the plane along its normal from the origin.</param>
        public PlaneD(double x, double y, double z, double d)
        {
#if NET7_0_OR_GREATER
            this = Create(x, y, z, d);
#else
            Normal = new(x, y, z);
            D = d;
#endif
        }

        /// <summary>Creates a <see cref="PlaneD" /> object from a specified normal and the distance along the normal from the origin.</summary>
        /// <param name="normal">The plane's normal vector.</param>
        /// <param name="d">The plane's distance from the origin along its normal vector.</param>
        public PlaneD(Vector3D normal, double d)
        {
#if NET7_0_OR_GREATER
            this = Create(normal, d);
#else   
            Normal = normal;
            D = d;
#endif
        }

        /// <summary>Creates a <see cref="PlaneD" /> object from a specified four-dimensional vector.</summary>
        /// <param name="value">A vector whose first three elements describe the normal vector, and whose <see cref="Vector4D.W" /> defines the distance along that normal from the origin.</param>
        public PlaneD(Vector4D value)
        {
            this = value.AsPlaneD();
        }

#if NET5_0_OR_GREATER
        internal static PlaneD Create(double x, double y, double z, double d) => Vector256.Create(x, y, z, d).AsPlaneD();

        internal static PlaneD Create(Vector3D normal, double d) => Vector4D.Create(normal, d).AsPlaneD();

#else

        internal static PlaneD Create(double x, double y, double z, double d) => new(x, y, z, d);

        internal static PlaneD Create(Vector3D normal, double d) => new(normal, d);

#endif 

        /// <summary>Creates a <see cref="PlaneD" /> object that contains three specified points.</summary>
        /// <param name="point1">The first point defining the plane.</param>
        /// <param name="point2">The second point defining the plane.</param>
        /// <param name="point3">The third point defining the plane.</param>
        /// <returns>The plane containing the three points.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD CreateFromVertices(Vector3D point1, Vector3D point2, Vector3D point3)
        {
            // This implementation is based on the DirectX Math Library XMPlaneFromPoints method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            Vector3D normal = Vector3D.Normalize(Vector3D.Cross(point2 - point1, point3 - point1));

            return Create(
                normal,
                -Vector3D.Dot(normal, point1)
            );
        }

        /// <summary>Calculates the dot product of a plane and a 4-dimensional vector.</summary>
        /// <param name="plane">The plane.</param>
        /// <param name="value">The four-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(PlaneD plane, Vector4D value)
        {
#if NET7_0_OR_GREATER
            return Vector256.Dot(plane.AsVector256(), value.AsVector256());
#else
            var value1 = plane.AsVector4D();
            return value1.X * value.X + value1.Y * value.Y + value1.Z * value.Z + value1.W * value.W;
#endif
        }

        /// <summary>Returns the dot product of a specified three-dimensional vector and the normal vector of this plane plus the distance (<see cref="D" />) value of the plane.</summary>
        /// <param name="plane">The plane.</param>
        /// <param name="value">The 3-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        public static double DotCoordinate(PlaneD plane, Vector3D value)
        {
            // This implementation is based on the DirectX Math Library XMPlaneDotCoord method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            return Dot(plane, Vector4D.Create(value, 1.0f));
        }

        /// <summary>Returns the dot product of a specified three-dimensional vector and the <see cref="Normal" /> vector of this plane.</summary>
        /// <param name="plane">The plane.</param>
        /// <param name="value">The three-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        public static double DotNormal(PlaneD plane, Vector3D value)
        {
            // This implementation is based on the DirectX Math Library XMPlaneDotNormal method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            return Vector3D.Dot(plane.Normal, value);
        }

        /// <summary>Creates a new <see cref="PlaneD" /> object whose normal vector is the source plane's normal vector normalized.</summary>
        /// <param name="value">The source plane.</param>
        /// <returns>The normalized plane.</returns>
        public static PlaneD Normalize(PlaneD value)
        {
            // This implementation is based on the DirectX Math Library XMPlaneNormalize method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

#if NET7_0_OR_GREATER
            Vector256<double> lengthSquared = Vector256.Create(value.Normal.LengthSquared());

            return Vector256.AndNot((value.AsVector256() / Vector256.Sqrt(lengthSquared)), Vector256.Equals(lengthSquared, Vector256.Create(double.PositiveInfinity))).AsPlaneD();
#else
            double lengthSquared = value.Normal.LengthSquared();
            if (lengthSquared > 0)
            {
                double length = Math.Sqrt(lengthSquared);
                return new PlaneD(value.Normal / length, value.D);
            }
            return value;
#endif
        }


        /// <summary>Transforms a normalized plane by a 4x4 matrix.</summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix to apply to <paramref name="plane" />.</param>
        /// <returns>The transformed plane.</returns>
        /// <remarks><paramref name="plane" /> must already be normalized so that its <see cref="Normal" /> vector is of unit length before this method is called.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD Transform(PlaneD plane, Matrix4x4D matrix)
        {
            Matrix4x4D.Impl.Invert(matrix.AsImpl(), out Matrix4x4D.Impl inverseMatrix);
            return Vector4D.Transform(plane.AsVector4D(), Matrix4x4D.Impl.Transpose(inverseMatrix)).AsPlaneD();
        }

        /// <summary>Transforms a normalized plane by a Quaternion rotation.</summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply to the plane.</param>
        /// <returns>A new plane that results from applying the Quaternion rotation.</returns>
        /// <remarks><paramref name="plane" /> must already be normalized so that its <see cref="Normal" /> vector is of unit length before this method is called.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD Transform(PlaneD plane, QuaternionD rotation) => Vector4D.Transform(plane.AsVector4D(), rotation).AsPlaneD();

        /// <summary>Returns a value that indicates whether two planes are equal.</summary>
        /// <param name="value1">The first plane to compare.</param>
        /// <param name="value2">The second plane to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="value1" /> and <paramref name="value2" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="PlaneD" /> objects are equal if their <see cref="Normal" /> and <see cref="D" /> fields are equal.
        /// The <see cref="op_Equality" /> method defines the operation of the equality operator for <see cref="PlaneD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PlaneD value1, PlaneD value2)
        {
#if NET7_0_OR_GREATER
            return value1.AsVector256() == value2.AsVector256();
#else
            return value1.AsVector4D() == value2.AsVector4D();
#endif
        }

        /// <summary>Returns a value that indicates whether two planes are not equal.</summary>
        /// <param name="value1">The first plane to compare.</param>
        /// <param name="value2">The second plane to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="value1" /> and <paramref name="value2" /> are not equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>The <see cref="op_Inequality" /> method defines the operation of the inequality operator for <see cref="PlaneD" /> objects.</remarks>
        public static bool operator !=(PlaneD value1, PlaneD value2) => !(value1 == value2);

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="PlaneD" /> object and their <see cref="Normal" /> and <see cref="D" /> fields are equal.</remarks>
        public override readonly bool Equals(object? obj) => (obj is PlaneD other) && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another plane object are equal.</summary>
        /// <param name="other">The other plane.</param>
        /// <returns><see langword="true" /> if the two planes are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Plane" /> objects are equal if their <see cref="Normal" /> and <see cref="D" /> fields are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PlaneD other)
        {
#if NET7_0_OR_GREATER
            return this.AsVector256().Equals(other.AsVector256());
#else
            return this.AsVector4D().Equals(other.AsVector4D());
#endif
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(Normal, D);

        /// <summary>Returns the string representation of this plane object.</summary>
        /// <returns>A string that represents this <see cref="Plane" /> object.</returns>
        /// <remarks>The string representation of a <see cref="Plane" /> object use the formatting conventions of the current culture to format the numeric values in the returned string. For example, a <see cref="Plane" /> object whose string representation is formatted by using the conventions of the en-US culture might appear as <c>{Normal:&lt;1.1, 2.2, 3.3&gt; D:4.4}</c>.</remarks>
        public override readonly string ToString() => $"{{Normal:{Normal} D:{D}}}";
    }
}
