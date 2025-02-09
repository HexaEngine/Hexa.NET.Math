// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Converted to double by Juna Meinhold (c) 2025, MIT license.

namespace Hexa.NET.Mathematics
{
    using System;
    using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER
    using System.Runtime.Intrinsics.Arm;
    using System.Runtime.Intrinsics.X86;
    using System.Runtime.Intrinsics;
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
#endif

    public unsafe partial struct Matrix4x4D
    {
        internal const uint RowCount = 4;
        internal const uint ColumnCount = 4;
#if NET7_0_OR_GREATER
        [UnscopedRef]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref Impl AsImpl() => ref Unsafe.As<Matrix4x4D, Impl>(ref this);

#if NET7_0_OR_GREATER
        [UnscopedRef]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal readonly ref readonly Impl AsROImpl() => ref Unsafe.As<Matrix4x4D, Impl>(ref Unsafe.AsRef(in this));

        internal struct Impl : IEquatable<Impl>
        {
#if NET7_0_OR_GREATER
            [UnscopedRef]
#endif
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Matrix4x4D AsM4x4() => ref Unsafe.As<Impl, Matrix4x4D>(ref this);

            private const double BillboardEpsilon = 1e-8;
            private const double BillboardMinAngle = 1.0 - (0.1 * (Math.PI / 180.0)); // 0.1 degrees
            private const double DecomposeEpsilon = 1e-8;

            public Vector4D X;
            public Vector4D Y;
            public Vector4D Z;
            public Vector4D W;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(double m11, double m12, double m13, double m14,
                             double m21, double m22, double m23, double m24,
                             double m31, double m32, double m33, double m34,
                             double m41, double m42, double m43, double m44)
            {
                X = Vector4D.Create(m11, m12, m13, m14);
                Y = Vector4D.Create(m21, m22, m23, m24);
                Z = Vector4D.Create(m31, m32, m33, m34);
                W = Vector4D.Create(m41, m42, m43, m44);
            }

            /*
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public void Init(in Matrix3x2D.Impl value)
                        {
                            X = Vector4D.Create(value.X, 0, 0);
                            Y = Vector4D.Create(value.Y, 0, 0);
                            Z = Vector4D.UnitZ;
                            W = Vector4D.Create(value.Z, 0, 1);
                        }
            */

            public static Impl Identity
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    Impl result;

                    result.X = Vector4D.UnitX;
                    result.Y = Vector4D.UnitY;
                    result.Z = Vector4D.UnitZ;
                    result.W = Vector4D.UnitW;

                    return result;
                }
            }

            public double this[int row, int column]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                readonly get
                {
                    if ((uint)row >= RowCount)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    return Unsafe.Add(ref Unsafe.AsRef(in X), row)[column];
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set
                {
                    if ((uint)row >= RowCount)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    Unsafe.Add(ref X, row)[column] = value;
                }
            }

            public readonly bool IsIdentity
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return (X == Vector4D.UnitX)
                        && (Y == Vector4D.UnitY)
                        && (Z == Vector4D.UnitZ)
                        && (W == Vector4D.UnitW);
                }
            }

            public Vector3D Translation
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                readonly get => W.AsVector3D();

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set
                {
                    W = Vector4D.Create(value, W.W);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl operator +(in Impl left, in Impl right)
            {
                Impl result;

                result.X = left.X + right.X;
                result.Y = left.Y + right.Y;
                result.Z = left.Z + right.Z;
                result.W = left.W + right.W;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(in Impl left, in Impl right)
            {
                return (left.X == right.X)
                    && (left.Y == right.Y)
                    && (left.Z == right.Z)
                    && (left.W == right.W);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator !=(in Impl left, in Impl right)
            {
                return (left.X != right.X)
                    || (left.Y != right.Y)
                    || (left.Z != right.Z)
                    || (left.W != right.W);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl operator *(in Impl left, in Impl right)
            {
                Impl result;

                result.X = Vector4D.Transform(left.X, in right);
                result.Y = Vector4D.Transform(left.Y, in right);
                result.Z = Vector4D.Transform(left.Z, in right);
                result.W = Vector4D.Transform(left.W, in right);

                return result;
            }


            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl operator *(in Impl left, double right)
            {
                Impl result;

                result.X = left.X * right;
                result.Y = left.Y * right;
                result.Z = left.Z * right;
                result.W = left.W * right;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl operator -(in Impl left, in Impl right)
            {
                Impl result;

                result.X = left.X - right.X;
                result.Y = left.Y - right.Y;
                result.Z = left.Z - right.Z;
                result.W = left.W - right.W;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl operator -(in Impl value)
            {
                Impl result;

                result.X = -value.X;
                result.Y = -value.Y;
                result.Z = -value.Z;
                result.W = -value.W;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateBillboard(in Vector3D objectPosition, in Vector3D cameraPosition, in Vector3D cameraUpVector, in Vector3D cameraForwardVector)
            {
                Vector3D axisZ = objectPosition - cameraPosition;

                if (axisZ.LengthSquared() < BillboardEpsilon)
                {
                    axisZ = -cameraForwardVector;
                }
                else
                {
                    axisZ = Vector3D.Normalize(axisZ);
                }

                Vector3D axisX = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, axisZ));
                Vector3D axisY = Vector3D.Cross(axisZ, axisX);

                Impl result;

                result.X = axisX.AsVector4D();
                result.Y = axisY.AsVector4D();
                result.Z = axisZ.AsVector4D();
                result.W = Vector4D.Create(objectPosition, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateConstrainedBillboard(in Vector3D objectPosition, in Vector3D cameraPosition, in Vector3D rotateAxis, in Vector3D cameraForwardVector, in Vector3D objectForwardVector)
            {
                // Treat the case when object and camera positions are too close.
                Vector3D faceDir = objectPosition - cameraPosition;

                if (faceDir.LengthSquared() < BillboardEpsilon)
                {
                    faceDir = -cameraForwardVector;
                }
                else
                {
                    faceDir = Vector3D.Normalize(faceDir);
                }

                Vector3D axisY = rotateAxis;

                // Treat the case when angle between faceDir and rotateAxis is too close to 0.
                double dot = Vector3D.Dot(axisY, faceDir);

                if (Math.Abs(dot) > BillboardMinAngle)
                {
                    faceDir = objectForwardVector;

                    // Make sure passed values are useful for compute.
                    dot = Vector3D.Dot(axisY, faceDir);

                    if (Math.Abs(dot) > BillboardMinAngle)
                    {
                        faceDir = (Math.Abs(axisY.Z) > BillboardMinAngle) ? Vector3D.UnitX : Vector3D.Create(0, 0, -1);
                    }
                }

                Vector3D axisX = Vector3D.Normalize(Vector3D.Cross(axisY, faceDir));
                Vector3D axisZ = Vector3D.Normalize(Vector3D.Cross(axisX, axisY));

                Impl result;

                result.X = axisX.AsVector4D();
                result.Y = axisY.AsVector4D();
                result.Z = axisZ.AsVector4D();
                result.W = Vector4D.Create(objectPosition, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateFromAxisAngle(in Vector3D axis, double angle)
            {
                QuaternionD q = QuaternionD.CreateFromAxisAngle(axis, angle);
                return CreateFromQuaternion(q);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateFromQuaternion(in QuaternionD quaternion)
            {
                double xx = quaternion.X * quaternion.X;
                double yy = quaternion.Y * quaternion.Y;
                double zz = quaternion.Z * quaternion.Z;

                double xy = quaternion.X * quaternion.Y;
                double wz = quaternion.Z * quaternion.W;
                double xz = quaternion.Z * quaternion.X;
                double wy = quaternion.Y * quaternion.W;
                double yz = quaternion.Y * quaternion.Z;
                double wx = quaternion.X * quaternion.W;

                Impl result;

                result.X = Vector4D.Create(
                    1.0f - 2.0f * (yy + zz),
                    2.0f * (xy + wz),
                    2.0f * (xz - wy),
                    0
                );
                result.Y = Vector4D.Create(
                    2.0f * (xy - wz),
                    1.0f - 2.0f * (zz + xx),
                    2.0f * (yz + wx),
                    0
                );
                result.Z = Vector4D.Create(
                    2.0f * (xz + wy),
                    2.0f * (yz - wx),
                    1.0f - 2.0f * (yy + xx),
                    0
                );
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateFromYawPitchRoll(double yaw, double pitch, double roll)
            {
                QuaternionD q = QuaternionD.CreateFromYawPitchRoll(yaw, pitch, roll);
                return CreateFromQuaternion(q);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateLookTo(in Vector3D cameraPosition, in Vector3D cameraDirection, in Vector3D cameraUpVector)
            {
                // This implementation is based on the DirectX Math Library XMMatrixLookToRH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                return CreateLookToLeftHanded(cameraPosition, -cameraDirection, cameraUpVector);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateLookToLeftHanded(in Vector3D cameraPosition, in Vector3D cameraDirection, in Vector3D cameraUpVector)
            {
                // This implementation is based on the DirectX Math Library XMMatrixLookToLH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                Vector3D axisZ = Vector3D.Normalize(cameraDirection);
                Vector3D axisX = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, axisZ));
                Vector3D axisY = Vector3D.Cross(axisZ, axisX);
                Vector3D negativeCameraPosition = -cameraPosition;

                Impl result;

                result.X = Vector4D.Create(axisX, Vector3D.Dot(axisX, negativeCameraPosition));
                result.Y = Vector4D.Create(axisY, Vector3D.Dot(axisY, negativeCameraPosition));
                result.Z = Vector4D.Create(axisZ, Vector3D.Dot(axisZ, negativeCameraPosition));
                result.W = Vector4D.UnitW;

                return Transpose(result);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateOrthographic(double width, double height, double zNearPlane, double zFarPlane)
            {
                // This implementation is based on the DirectX Math Library XMMatrixOrthographicRH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                double range = 1.0f / (zNearPlane - zFarPlane);

                Impl result;

                result.X = Vector4D.Create(2.0f / width, 0, 0, 0);
                result.Y = Vector4D.Create(0, 2.0f / height, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, 0);
                result.W = Vector4D.Create(0, 0, range * zNearPlane, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateOrthographicLeftHanded(double width, double height, double zNearPlane, double zFarPlane)
            {
                // This implementation is based on the DirectX Math Library XMMatrixOrthographicLH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                double range = 1.0f / (zFarPlane - zNearPlane);

                Impl result;

                result.X = Vector4D.Create(2.0f / width, 0, 0, 0);
                result.Y = Vector4D.Create(0, 2.0f / height, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, 0);
                result.W = Vector4D.Create(0, 0, -range * zNearPlane, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
            {
                // This implementation is based on the DirectX Math Library XMMatrixOrthographicOffCenterRH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                double reciprocalWidth = 1.0f / (right - left);
                double reciprocalHeight = 1.0f / (top - bottom);
                double range = 1.0f / (zNearPlane - zFarPlane);

                Impl result;

                result.X = Vector4D.Create(reciprocalWidth + reciprocalWidth, 0, 0, 0);
                result.Y = Vector4D.Create(0, reciprocalHeight + reciprocalHeight, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, 0);
                result.W = Vector4D.Create(
                    -(left + right) * reciprocalWidth,
                    -(top + bottom) * reciprocalHeight,
                    range * zNearPlane,
                    1
                );

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateOrthographicOffCenterLeftHanded(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
            {
                // This implementation is based on the DirectX Math Library XMMatrixOrthographicOffCenterLH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                double reciprocalWidth = 1.0f / (right - left);
                double reciprocalHeight = 1.0f / (top - bottom);
                double range = 1.0f / (zFarPlane - zNearPlane);

                Impl result;

                result.X = Vector4D.Create(reciprocalWidth + reciprocalWidth, 0, 0, 0);
                result.Y = Vector4D.Create(0, reciprocalHeight + reciprocalHeight, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, 0);
                result.W = Vector4D.Create(
                    -(left + right) * reciprocalWidth,
                    -(top + bottom) * reciprocalHeight,
                    -range * zNearPlane,
                    1
                );

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreatePerspective(double width, double height, double nearPlaneDistance, double farPlaneDistance)
            {
                // This implementation is based on the DirectX Math Library XMMatrixPerspectiveRH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

                double dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
                double range = double.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

                Impl result;

                result.X = Vector4D.Create(dblNearPlaneDistance / width, 0, 0, 0);
                result.Y = Vector4D.Create(0, dblNearPlaneDistance / height, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, -1.0f);
                result.W = Vector4D.Create(0, 0, range * nearPlaneDistance, 0);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreatePerspectiveLeftHanded(double width, double height, double nearPlaneDistance, double farPlaneDistance)
            {
                // This implementation is based on the DirectX Math Library XMMatrixPerspectiveLH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

                double dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
                double range = double.IsPositiveInfinity(farPlaneDistance) ? 1.0f : farPlaneDistance / (farPlaneDistance - nearPlaneDistance);

                Impl result;

                result.X = Vector4D.Create(dblNearPlaneDistance / width, 0, 0, 0);
                result.Y = Vector4D.Create(0, dblNearPlaneDistance / height, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, 1.0f);
                result.W = Vector4D.Create(0, 0, -range * nearPlaneDistance, 0);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreatePerspectiveFieldOfView(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
            {
                // This implementation is based on the DirectX Math Library XMMatrixPerspectiveFovRH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(fieldOfView, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(fieldOfView, double.Pi);

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

                double height = 1.0f / Math.Tan(fieldOfView * 0.5f);
                double width = height / aspectRatio;
                double range = double.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

                Impl result;

                result.X = Vector4D.Create(width, 0, 0, 0);
                result.Y = Vector4D.Create(0, height, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, -1.0f);
                result.W = Vector4D.Create(0, 0, range * nearPlaneDistance, 0);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreatePerspectiveFieldOfViewLeftHanded(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
            {
                // This implementation is based on the DirectX Math Library XMMatrixPerspectiveFovLH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(fieldOfView, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(fieldOfView, double.Pi);

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

                double height = 1.0f / Math.Tan(fieldOfView * 0.5f);
                double width = height / aspectRatio;
                double range = double.IsPositiveInfinity(farPlaneDistance) ? 1.0f : farPlaneDistance / (farPlaneDistance - nearPlaneDistance);

                Impl result;

                result.X = Vector4D.Create(width, 0, 0, 0);
                result.Y = Vector4D.Create(0, height, 0, 0);
                result.Z = Vector4D.Create(0, 0, range, 1.0f);
                result.W = Vector4D.Create(0, 0, -range * nearPlaneDistance, 0);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance)
            {
                // This implementation is based on the DirectX Math Library XMMatrixPerspectiveOffCenterRH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

                double dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
                double reciprocalWidth = 1.0f / (right - left);
                double reciprocalHeight = 1.0f / (top - bottom);
                double range = double.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

                Impl result;

                result.X = Vector4D.Create(dblNearPlaneDistance * reciprocalWidth, 0, 0, 0);
                result.Y = Vector4D.Create(0, dblNearPlaneDistance * reciprocalHeight, 0, 0);
                result.Z = Vector4D.Create(
                    (left + right) * reciprocalWidth,
                    (top + bottom) * reciprocalHeight,
                    range,
                    -1.0f
                );
                result.W = Vector4D.Create(0, 0, range * nearPlaneDistance, 0);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreatePerspectiveOffCenterLeftHanded(double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance)
            {
                // This implementation is based on the DirectX Math Library XMMatrixPerspectiveOffCenterLH method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0f);
                //ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

                double dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
                double reciprocalWidth = 1.0f / (right - left);
                double reciprocalHeight = 1.0f / (top - bottom);
                double range = double.IsPositiveInfinity(farPlaneDistance) ? 1.0f : farPlaneDistance / (farPlaneDistance - nearPlaneDistance);

                Impl result;

                result.X = Vector4D.Create(dblNearPlaneDistance * reciprocalWidth, 0, 0, 0);
                result.Y = Vector4D.Create(0, dblNearPlaneDistance * reciprocalHeight, 0, 0);
                result.Z = Vector4D.Create(
                    -(left + right) * reciprocalWidth,
                    -(top + bottom) * reciprocalHeight,
                    range,
                    1.0f
                );
                result.W = Vector4D.Create(0, 0, -range * nearPlaneDistance, 0);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateReflection(in PlaneD value)
            {
                // This implementation is based on the DirectX Math Library XMMatrixReflect method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                Vector4D p = PlaneD.Normalize(value).AsVector4D();
                Vector4D s = p * Vector4D.Create(-2.0f, -2.0f, -2.0f, 0.0f);

                Impl result;

                result.X = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.X), s, Vector4D.UnitX);
                result.Y = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.Y), s, Vector4D.UnitY);
                result.Z = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.Z), s, Vector4D.UnitZ);
                result.W = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.W), s, Vector4D.UnitW);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateRotationX(double radians)
            {
                (double s, double c) = MathUtil.SinCos(radians);

                // [  1  0  0  0 ]
                // [  0  c  s  0 ]
                // [  0 -s  c  0 ]
                // [  0  0  0  1 ]

                Impl result;

                result.X = Vector4D.UnitX;
                result.Y = Vector4D.Create(0, c, s, 0);
                result.Z = Vector4D.Create(0, -s, c, 0);
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateRotationX(double radians, in Vector3D centerPoint)
            {
                (double s, double c) = MathUtil.SinCos(radians);

                double y = MathUtil.MultiplyAddEstimate(centerPoint.Y, 1 - c, +centerPoint.Z * s);
                double z = MathUtil.MultiplyAddEstimate(centerPoint.Z, 1 - c, -centerPoint.Y * s);

                // [  1  0  0  0 ]
                // [  0  c  s  0 ]
                // [  0 -s  c  0 ]
                // [  0  y  z  1 ]

                Impl result;

                result.X = Vector4D.UnitX;
                result.Y = Vector4D.Create(0, c, s, 0);
                result.Z = Vector4D.Create(0, -s, c, 0);
                result.W = Vector4D.Create(0, y, z, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateRotationY(double radians)
            {
                (double s, double c) = MathUtil.SinCos(radians);

                // [  c  0 -s  0 ]
                // [  0  1  0  0 ]
                // [  s  0  c  0 ]
                // [  0  0  0  1 ]

                Impl result;

                result.X = Vector4D.Create(c, 0, -s, 0);
                result.Y = Vector4D.UnitY;
                result.Z = Vector4D.Create(s, 0, c, 0);
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateRotationY(double radians, in Vector3D centerPoint)
            {
                (double s, double c) = MathUtil.SinCos(radians);

                double x = MathUtil.MultiplyAddEstimate(centerPoint.X, 1 - c, -centerPoint.Z * s);
                double z = MathUtil.MultiplyAddEstimate(centerPoint.Z, 1 - c, +centerPoint.X * s);

                // [  c  0 -s  0 ]
                // [  0  1  0  0 ]
                // [  s  0  c  0 ]
                // [  x  0  z  1 ]

                Impl result;

                result.X = Vector4D.Create(c, 0, -s, 0);
                result.Y = Vector4D.UnitY;
                result.Z = Vector4D.Create(s, 0, c, 0);
                result.W = Vector4D.Create(x, 0, z, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateRotationZ(double radians)
            {
                (double s, double c) = MathUtil.SinCos(radians);

                // [  c  s  0  0 ]
                // [ -s  c  0  0 ]
                // [  0  0  1  0 ]
                // [  0  0  0  1 ]

                Impl result;

                result.X = Vector4D.Create(c, s, 0, 0);
                result.Y = Vector4D.Create(-s, c, 0, 0);
                result.Z = Vector4D.UnitZ;
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateRotationZ(double radians, in Vector3D centerPoint)
            {
                (double s, double c) = MathUtil.SinCos(radians);

                double x = MathUtil.MultiplyAddEstimate(centerPoint.X, 1 - c, +centerPoint.Y * s);
                double y = MathUtil.MultiplyAddEstimate(centerPoint.Y, 1 - c, -centerPoint.X * s);

                // [  c  s  0  0 ]
                // [ -s  c  0  0 ]
                // [  0  0  1  0 ]
                // [  x  y  0  1 ]

                Impl result;

                result.X = Vector4D.Create(c, s, 0, 0);
                result.Y = Vector4D.Create(-s, c, 0, 0);
                result.Z = Vector4D.UnitZ;
                result.W = Vector4D.Create(x, y, 0, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateScale(double scaleX, double scaleY, double scaleZ)
            {
                Impl result;

                result.X = Vector4D.Create(scaleX, 0, 0, 0);
                result.Y = Vector4D.Create(0, scaleY, 0, 0);
                result.Z = Vector4D.Create(0, 0, scaleZ, 0);
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateScale(double scaleX, double scaleY, double scaleZ, in Vector3D centerPoint)
            {
                Impl result;

                result.X = Vector4D.Create(scaleX, 0, 0, 0);
                result.Y = Vector4D.Create(0, scaleY, 0, 0);
                result.Z = Vector4D.Create(0, 0, scaleZ, 0);
                result.W = Vector4D.Create(centerPoint * (Vector3D.One - Vector3D.Create(scaleX, scaleY, scaleZ)), 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateScale(in Vector3D scales)
            {
                Impl result;

                result.X = Vector4D.Create(scales.X, 0, 0, 0);
                result.Y = Vector4D.Create(0, scales.Y, 0, 0);
                result.Z = Vector4D.Create(0, 0, scales.Z, 0);
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateScale(in Vector3D scales, in Vector3D centerPoint)
            {
                Impl result;

                result.X = Vector4D.Create(scales.X, 0, 0, 0);
                result.Y = Vector4D.Create(0, scales.Y, 0, 0);
                result.Z = Vector4D.Create(0, 0, scales.Z, 0);
                result.W = Vector4D.Create(centerPoint * (Vector3D.One - scales), 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateScale(double scale)
            {
                Impl result;

                result.X = Vector4D.Create(scale, 0, 0, 0);
                result.Y = Vector4D.Create(0, scale, 0, 0);
                result.Z = Vector4D.Create(0, 0, scale, 0);
                result.W = Vector4D.UnitW;

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateScale(double scale, in Vector3D centerPoint)
            {
                Impl result;

                result.X = Vector4D.Create(scale, 0, 0, 0);
                result.Y = Vector4D.Create(0, scale, 0, 0);
                result.Z = Vector4D.Create(0, 0, scale, 0);
                result.W = Vector4D.Create(centerPoint * (Vector3D.One - Vector3D.Create(scale)), 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateShadow(in Vector3D lightDirection, in PlaneD plane)
            {
                Vector4D p = PlaneD.Normalize(plane).AsVector4D();
                Vector4D l = lightDirection.AsVector4D();
                double dot = Vector4D.Dot(p, l);

                p = -p;

                Impl result;

                result.X = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.X), Vector4D.Create(dot, 0, 0, 0));
                result.Y = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.Y), Vector4D.Create(0, dot, 0, 0));
                result.Z = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.Z), Vector4D.Create(0, 0, dot, 0));
                result.W = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.W), Vector4D.Create(0, 0, 0, dot));

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateTranslation(in Vector3D position)
            {
                Impl result;

                result.X = Vector4D.UnitX;
                result.Y = Vector4D.UnitY;
                result.Z = Vector4D.UnitZ;
                result.W = Vector4D.Create(position, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateTranslation(double positionX, double positionY, double positionZ)
            {
                Impl result;

                result.X = Vector4D.UnitX;
                result.Y = Vector4D.UnitY;
                result.Z = Vector4D.UnitZ;
                result.W = Vector4D.Create(positionX, positionY, positionZ, 1);

                return result;
            }


            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateViewport(double x, double y, double width, double height, double minDepth, double maxDepth)
            {
                Impl result;

                // 4x SIMD fields to get a lot better codegen
                result.W = Vector4D.Create(width, height, 0f, 0f);
                result.W *= Vector4D.Create(0.5f, 0.5f, 0f, 0f);

                result.X = Vector4D.Create(result.W.X, 0f, 0f, 0f);
                result.Y = Vector4D.Create(0f, -result.W.Y, 0f, 0f);
                result.Z = Vector4D.Create(0f, 0f, minDepth - maxDepth, 0f);
                result.W += Vector4D.Create(x, y, minDepth, 1f);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateViewportLeftHanded(double x, double y, double width, double height, double minDepth, double maxDepth)
            {
                Impl result;

                // 4x SIMD fields to get a lot better codegen
                result.W = Vector4D.Create(width, height, 0f, 0f);
                result.W *= Vector4D.Create(0.5f, 0.5f, 0f, 0f);

                result.X = Vector4D.Create(result.W.X, 0f, 0f, 0f);
                result.Y = Vector4D.Create(0f, -result.W.Y, 0f, 0f);
                result.Z = Vector4D.Create(0f, 0f, maxDepth - minDepth, 0f);
                result.W += Vector4D.Create(x, y, minDepth, 1f);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl CreateWorld(in Vector3D position, in Vector3D forward, in Vector3D up)
            {
                Vector3D axisZ = Vector3D.Normalize(-forward);
                Vector3D axisX = Vector3D.Normalize(Vector3D.Cross(up, axisZ));
                Vector3D axisY = Vector3D.Cross(axisZ, axisX);

                Impl result;

                result.X = axisX.AsVector4D();
                result.Y = axisY.AsVector4D();
                result.Z = axisZ.AsVector4D();
                result.W = Vector4D.Create(position, 1);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe bool Decompose(in Impl matrix, out Vector3D scale, out QuaternionD rotation, out Vector3D translation)
            {
                Impl matTemp = Identity;

                Vector3D* canonicalBasis = stackalloc Vector3D[3] {
                    Vector3D.UnitX,
                    Vector3D.UnitY,
                    Vector3D.UnitZ,
                };

                translation = matrix.W.AsVector3D();

                Vector3D** vectorBasis = stackalloc Vector3D*[3] {
                    (Vector3D*)&matTemp.X,
                    (Vector3D*)&matTemp.Y,
                    (Vector3D*)&matTemp.Z,
                };

                *(vectorBasis[0]) = matrix.X.AsVector3D();
                *(vectorBasis[1]) = matrix.Y.AsVector3D();
                *(vectorBasis[2]) = matrix.Z.AsVector3D();

                double* scales = stackalloc double[3] {
                    vectorBasis[0]->Length(),
                    vectorBasis[1]->Length(),
                    vectorBasis[2]->Length(),
                };

                uint a, b, c;

                #region Ranking
                double x = scales[0];
                double y = scales[1];
                double z = scales[2];

                if (x < y)
                {
                    if (y < z)
                    {
                        a = 2;
                        b = 1;
                        c = 0;
                    }
                    else
                    {
                        a = 1;

                        if (x < z)
                        {
                            b = 2;
                            c = 0;
                        }
                        else
                        {
                            b = 0;
                            c = 2;
                        }
                    }
                }
                else
                {
                    if (x < z)
                    {
                        a = 2;
                        b = 0;
                        c = 1;
                    }
                    else
                    {
                        a = 0;

                        if (y < z)
                        {
                            b = 2;
                            c = 1;
                        }
                        else
                        {
                            b = 1;
                            c = 2;
                        }
                    }
                }
                #endregion

                if (scales[a] < DecomposeEpsilon)
                {
                    *(vectorBasis[a]) = canonicalBasis[a];
                }

                *vectorBasis[a] = Vector3D.Normalize(*vectorBasis[a]);

                if (scales[b] < DecomposeEpsilon)
                {
                    uint cc;
                    double fAbsX, fAbsY, fAbsZ;

                    fAbsX = Math.Abs(vectorBasis[a]->X);
                    fAbsY = Math.Abs(vectorBasis[a]->Y);
                    fAbsZ = Math.Abs(vectorBasis[a]->Z);

                    #region Ranking
                    if (fAbsX < fAbsY)
                    {
                        if (fAbsY < fAbsZ)
                        {
                            cc = 0;
                        }
                        else
                        {
                            if (fAbsX < fAbsZ)
                            {
                                cc = 0;
                            }
                            else
                            {
                                cc = 2;
                            }
                        }
                    }
                    else
                    {
                        if (fAbsX < fAbsZ)
                        {
                            cc = 1;
                        }
                        else
                        {
                            if (fAbsY < fAbsZ)
                            {
                                cc = 1;
                            }
                            else
                            {
                                cc = 2;
                            }
                        }
                    }
                    #endregion

                    *vectorBasis[b] = Vector3D.Cross(*vectorBasis[a], canonicalBasis[cc]);
                }

                *vectorBasis[b] = Vector3D.Normalize(*vectorBasis[b]);

                if (scales[c] < DecomposeEpsilon)
                {
                    *vectorBasis[c] = Vector3D.Cross(*vectorBasis[a], *vectorBasis[b]);
                }

                *vectorBasis[c] = Vector3D.Normalize(*vectorBasis[c]);

                double det = matTemp.GetDeterminant();

                // use Kramer's rule to check for handedness of coordinate system
                if (det < 0.0f)
                {
                    // switch coordinate system by negating the scale and inverting the basis vector on the x-axis
                    scales[a] = -scales[a];
                    *vectorBasis[a] = -(*vectorBasis[a]);

                    det = -det;
                }

                det -= 1.0f;
                det *= det;

                bool result;

                if (DecomposeEpsilon < det)
                {
                    // Non-SRT matrix encountered
                    rotation = QuaternionD.Identity;
                    result = false;
                }
                else
                {
                    // generate the quaternion from the matrix
                    rotation = QuaternionD.CreateFromRotationMatrix(matTemp.AsM4x4());
                    result = true;
                }

                scale = Unsafe.ReadUnaligned<Vector3D>(scales);
                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Invert(in Impl matrix, out Impl result)
            {
                // This implementation is based on the DirectX Math Library XMMatrixInverse method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

#if NET5_0_OR_GREATER
                if (Avx.IsSupported)
                {
                    if (Fma.IsSupported)
                    {
                        return AvxFmaImpl(matrix, out result);
                    }
                    return AvxImpl(matrix, out result);
                }
#endif

                return SoftwareFallback(in matrix, out result);


#if NET5_0_OR_GREATER
                static unsafe bool AvxImpl(in Impl matrix, out Impl result)
                {
                    if (!Avx.IsSupported)
                    {
                        // Redundant test so we won't prejit remainder of this method on platforms without SSE.
                        throw new PlatformNotSupportedException();
                    }

                    // Load the matrix values into rows
                    Vector256<double> row1 = matrix.X.AsVector256();
                    Vector256<double> row2 = matrix.Y.AsVector256();
                    Vector256<double> row3 = matrix.Z.AsVector256();
                    Vector256<double> row4 = matrix.W.AsVector256();

                    // Transpose the matrix
                    Vector256<double> vTemp1 = Avx.Shuffle(row1, row2, 0x44); //_MM_SHUFFLE(1, 0, 1, 0)
                    Vector256<double> vTemp3 = Avx.Shuffle(row1, row2, 0xEE); //_MM_SHUFFLE(3, 2, 3, 2)
                    Vector256<double> vTemp2 = Avx.Shuffle(row3, row4, 0x44); //_MM_SHUFFLE(1, 0, 1, 0)
                    Vector256<double> vTemp4 = Avx.Shuffle(row3, row4, 0xEE); //_MM_SHUFFLE(3, 2, 3, 2)

                    row1 = Avx.Shuffle(vTemp1, vTemp2, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)
                    row2 = Avx.Shuffle(vTemp1, vTemp2, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)
                    row3 = Avx.Shuffle(vTemp3, vTemp4, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)
                    row4 = Avx.Shuffle(vTemp3, vTemp4, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)

                    Vector256<double> V00 = Avx.Permute(row3, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    Vector256<double> V10 = Avx.Permute(row4, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    Vector256<double> V01 = Avx.Permute(row1, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    Vector256<double> V11 = Avx.Permute(row2, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    Vector256<double> V02 = Avx.Shuffle(row3, row1, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)
                    Vector256<double> V12 = Avx.Shuffle(row4, row2, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)

                    Vector256<double> D0 = Avx.Multiply(V00, V10);
                    Vector256<double> D1 = Avx.Multiply(V01, V11);
                    Vector256<double> D2 = Avx.Multiply(V02, V12);

                    V00 = Avx.Permute(row3, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    V10 = Avx.Permute(row4, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    V01 = Avx.Permute(row1, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    V11 = Avx.Permute(row2, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    V02 = Avx.Shuffle(row3, row1, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)
                    V12 = Avx.Shuffle(row4, row2, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)

                    // Note:  We use this expansion pattern instead of Fused Multiply Add
                    // in order to support older hardware
                    D0 = Avx.Subtract(D0, Avx.Multiply(V00, V10));
                    D1 = Avx.Subtract(D1, Avx.Multiply(V01, V11));
                    D2 = Avx.Subtract(D2, Avx.Multiply(V02, V12));

                    // V11 = D0Y,D0W,D2Y,D2Y
                    V11 = Avx.Shuffle(D0, D2, 0x5D);  //_MM_SHUFFLE(1, 1, 3, 1)
                    V00 = Avx.Permute(row2, 0x49);        //_MM_SHUFFLE(1, 0, 2, 1)
                    V10 = Avx.Shuffle(V11, D0, 0x32); //_MM_SHUFFLE(0, 3, 0, 2)
                    V01 = Avx.Permute(row1, 0x12);        //_MM_SHUFFLE(0, 1, 0, 2)
                    V11 = Avx.Shuffle(V11, D0, 0x99); //_MM_SHUFFLE(2, 1, 2, 1)

                    // V13 = D1Y,D1W,D2W,D2W
                    Vector256<double> V13 = Avx.Shuffle(D1, D2, 0xFD); //_MM_SHUFFLE(3, 3, 3, 1)
                    V02 = Avx.Permute(row4, 0x49);                        //_MM_SHUFFLE(1, 0, 2, 1)
                    V12 = Avx.Shuffle(V13, D1, 0x32);                 //_MM_SHUFFLE(0, 3, 0, 2)
                    Vector256<double> V03 = Avx.Permute(row3, 0x12);       //_MM_SHUFFLE(0, 1, 0, 2)
                    V13 = Avx.Shuffle(V13, D1, 0x99);                 //_MM_SHUFFLE(2, 1, 2, 1)

                    Vector256<double> C0 = Avx.Multiply(V00, V10);
                    Vector256<double> C2 = Avx.Multiply(V01, V11);
                    Vector256<double> C4 = Avx.Multiply(V02, V12);
                    Vector256<double> C6 = Avx.Multiply(V03, V13);

                    // V11 = D0X,D0Y,D2X,D2X
                    V11 = Avx.Shuffle(D0, D2, 0x4);    //_MM_SHUFFLE(0, 0, 1, 0)
                    V00 = Avx.Permute(row2, 0x9e);         //_MM_SHUFFLE(2, 1, 3, 2)
                    V10 = Avx.Shuffle(D0, V11, 0x93);  //_MM_SHUFFLE(2, 1, 0, 3)
                    V01 = Avx.Permute(row1, 0x7b);         //_MM_SHUFFLE(1, 3, 2, 3)
                    V11 = Avx.Shuffle(D0, V11, 0x26);  //_MM_SHUFFLE(0, 2, 1, 2)

                    // V13 = D1X,D1Y,D2Z,D2Z
                    V13 = Avx.Shuffle(D1, D2, 0xa4);   //_MM_SHUFFLE(2, 2, 1, 0)
                    V02 = Avx.Permute(row4, 0x9e);         //_MM_SHUFFLE(2, 1, 3, 2)
                    V12 = Avx.Shuffle(D1, V13, 0x93);  //_MM_SHUFFLE(2, 1, 0, 3)
                    V03 = Avx.Permute(row3, 0x7b);         //_MM_SHUFFLE(1, 3, 2, 3)
                    V13 = Avx.Shuffle(D1, V13, 0x26);  //_MM_SHUFFLE(0, 2, 1, 2)

                    C0 = Avx.Subtract(C0, Avx.Multiply(V00, V10));
                    C2 = Avx.Subtract(C2, Avx.Multiply(V01, V11));
                    C4 = Avx.Subtract(C4, Avx.Multiply(V02, V12));
                    C6 = Avx.Subtract(C6, Avx.Multiply(V03, V13));

                    V00 = Avx.Permute(row2, 0x33); //_MM_SHUFFLE(0, 3, 0, 3)

                    // V10 = D0Z,D0Z,D2X,D2Y
                    V10 = Avx.Shuffle(D0, D2, 0x4A); //_MM_SHUFFLE(1, 0, 2, 2)
                    V10 = Avx.Permute(V10, 0x2C);        //_MM_SHUFFLE(0, 2, 3, 0)
                    V01 = Avx.Permute(row1, 0x8D);       //_MM_SHUFFLE(2, 0, 3, 1)

                    // V11 = D0X,D0W,D2X,D2Y
                    V11 = Avx.Shuffle(D0, D2, 0x4C); //_MM_SHUFFLE(1, 0, 3, 0)
                    V11 = Avx.Permute(V11, 0x93);        //_MM_SHUFFLE(2, 1, 0, 3)
                    V02 = Avx.Permute(row4, 0x33);       //_MM_SHUFFLE(0, 3, 0, 3)

                    // V12 = D1Z,D1Z,D2Z,D2W
                    V12 = Avx.Shuffle(D1, D2, 0xEA); //_MM_SHUFFLE(3, 2, 2, 2)
                    V12 = Avx.Permute(V12, 0x2C);        //_MM_SHUFFLE(0, 2, 3, 0)
                    V03 = Avx.Permute(row3, 0x8D);       //_MM_SHUFFLE(2, 0, 3, 1)

                    // V13 = D1X,D1W,D2Z,D2W
                    V13 = Avx.Shuffle(D1, D2, 0xEC); //_MM_SHUFFLE(3, 2, 3, 0)
                    V13 = Avx.Permute(V13, 0x93);        //_MM_SHUFFLE(2, 1, 0, 3)

                    V00 = Avx.Multiply(V00, V10);
                    V01 = Avx.Multiply(V01, V11);
                    V02 = Avx.Multiply(V02, V12);
                    V03 = Avx.Multiply(V03, V13);

                    Vector256<double> C1 = Avx.Subtract(C0, V00);
                    C0 = Avx.Add(C0, V00);
                    Vector256<double> C3 = Avx.Add(C2, V01);
                    C2 = Avx.Subtract(C2, V01);
                    Vector256<double> C5 = Avx.Subtract(C4, V02);
                    C4 = Avx.Add(C4, V02);
                    Vector256<double> C7 = Avx.Add(C6, V03);
                    C6 = Avx.Subtract(C6, V03);

                    C0 = Avx.Shuffle(C0, C1, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C2 = Avx.Shuffle(C2, C3, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C4 = Avx.Shuffle(C4, C5, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C6 = Avx.Shuffle(C6, C7, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)

                    C0 = Avx.Permute(C0, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C2 = Avx.Permute(C2, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C4 = Avx.Permute(C4, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C6 = Avx.Permute(C6, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)

                    // Get the determinant
                    vTemp2 = row1;
                    double det = Vector4D.Dot(C0.AsVector4D(), vTemp2.AsVector4D());

                    // Check determinate is not zero
                    if (Math.Abs(det) < double.Epsilon)
                    {
                        Vector4D vNaN = Vector4D.Create(float.NaN);

                        result.X = vNaN;
                        result.Y = vNaN;
                        result.Z = vNaN;
                        result.W = vNaN;

                        return false;
                    }

                    // Create Vector256<double> copy of the determinant and invert them.

                    Vector256<double> ones = Vector256.Create(1.0);
                    Vector256<double> vTemp = Vector256.Create(det);
                    vTemp = Avx.Divide(ones, vTemp);

                    result.X = Avx.Multiply(C0, vTemp).AsVector4D();
                    result.Y = Avx.Multiply(C2, vTemp).AsVector4D();
                    result.Z = Avx.Multiply(C4, vTemp).AsVector4D();
                    result.W = Avx.Multiply(C6, vTemp).AsVector4D();

                    return true;
                }
                static unsafe bool AvxFmaImpl(in Impl matrix, out Impl result)
                {
                    if (!Avx.IsSupported || !Fma.IsSupported)
                    {
                        // Redundant test so we won't prejit remainder of this method on platforms without SSE.
                        throw new PlatformNotSupportedException();
                    }

                    // Load the matrix values into rows
                    Vector256<double> row1 = matrix.X.AsVector256();
                    Vector256<double> row2 = matrix.Y.AsVector256();
                    Vector256<double> row3 = matrix.Z.AsVector256();
                    Vector256<double> row4 = matrix.W.AsVector256();

                    // Transpose the matrix
                    Vector256<double> vTemp1 = Avx.Shuffle(row1, row2, 0x44); //_MM_SHUFFLE(1, 0, 1, 0)
                    Vector256<double> vTemp3 = Avx.Shuffle(row1, row2, 0xEE); //_MM_SHUFFLE(3, 2, 3, 2)
                    Vector256<double> vTemp2 = Avx.Shuffle(row3, row4, 0x44); //_MM_SHUFFLE(1, 0, 1, 0)
                    Vector256<double> vTemp4 = Avx.Shuffle(row3, row4, 0xEE); //_MM_SHUFFLE(3, 2, 3, 2)

                    row1 = Avx.Shuffle(vTemp1, vTemp2, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)
                    row2 = Avx.Shuffle(vTemp1, vTemp2, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)
                    row3 = Avx.Shuffle(vTemp3, vTemp4, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)
                    row4 = Avx.Shuffle(vTemp3, vTemp4, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)

                    Vector256<double> V00 = Avx.Permute(row3, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    Vector256<double> V10 = Avx.Permute(row4, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    Vector256<double> V01 = Avx.Permute(row1, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    Vector256<double> V11 = Avx.Permute(row2, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    Vector256<double> V02 = Avx.Shuffle(row3, row1, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)
                    Vector256<double> V12 = Avx.Shuffle(row4, row2, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)

                    Vector256<double> D0 = Avx.Multiply(V00, V10);
                    Vector256<double> D1 = Avx.Multiply(V01, V11);
                    Vector256<double> D2 = Avx.Multiply(V02, V12);

                    V00 = Avx.Permute(row3, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    V10 = Avx.Permute(row4, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    V01 = Avx.Permute(row1, 0xEE);           //_MM_SHUFFLE(3, 2, 3, 2)
                    V11 = Avx.Permute(row2, 0x50);           //_MM_SHUFFLE(1, 1, 0, 0)
                    V02 = Avx.Shuffle(row3, row1, 0xDD); //_MM_SHUFFLE(3, 1, 3, 1)
                    V12 = Avx.Shuffle(row4, row2, 0x88); //_MM_SHUFFLE(2, 0, 2, 0)

                    D0 = Fma.MultiplyAdd(VectorMath.Negate(V00), V10, D0);
                    D1 = Fma.MultiplyAdd(VectorMath.Negate(V01), V11, D1);
                    D2 = Fma.MultiplyAdd(VectorMath.Negate(V02), V12, D2);

                    // V11 = D0Y,D0W,D2Y,D2Y
                    V11 = Avx.Shuffle(D0, D2, 0x5D);  //_MM_SHUFFLE(1, 1, 3, 1)
                    V00 = Avx.Permute(row2, 0x49);        //_MM_SHUFFLE(1, 0, 2, 1)
                    V10 = Avx.Shuffle(V11, D0, 0x32); //_MM_SHUFFLE(0, 3, 0, 2)
                    V01 = Avx.Permute(row1, 0x12);        //_MM_SHUFFLE(0, 1, 0, 2)
                    V11 = Avx.Shuffle(V11, D0, 0x99); //_MM_SHUFFLE(2, 1, 2, 1)

                    // V13 = D1Y,D1W,D2W,D2W
                    Vector256<double> V13 = Avx.Shuffle(D1, D2, 0xFD); //_MM_SHUFFLE(3, 3, 3, 1)
                    V02 = Avx.Permute(row4, 0x49);                        //_MM_SHUFFLE(1, 0, 2, 1)
                    V12 = Avx.Shuffle(V13, D1, 0x32);                 //_MM_SHUFFLE(0, 3, 0, 2)
                    Vector256<double> V03 = Avx.Permute(row3, 0x12);       //_MM_SHUFFLE(0, 1, 0, 2)
                    V13 = Avx.Shuffle(V13, D1, 0x99);                 //_MM_SHUFFLE(2, 1, 2, 1)

                    Vector256<double> C0 = Avx.Multiply(V00, V10);
                    Vector256<double> C2 = Avx.Multiply(V01, V11);
                    Vector256<double> C4 = Avx.Multiply(V02, V12);
                    Vector256<double> C6 = Avx.Multiply(V03, V13);

                    // V11 = D0X,D0Y,D2X,D2X
                    V11 = Avx.Shuffle(D0, D2, 0x4);    //_MM_SHUFFLE(0, 0, 1, 0)
                    V00 = Avx.Permute(row2, 0x9e);         //_MM_SHUFFLE(2, 1, 3, 2)
                    V10 = Avx.Shuffle(D0, V11, 0x93);  //_MM_SHUFFLE(2, 1, 0, 3)
                    V01 = Avx.Permute(row1, 0x7b);         //_MM_SHUFFLE(1, 3, 2, 3)
                    V11 = Avx.Shuffle(D0, V11, 0x26);  //_MM_SHUFFLE(0, 2, 1, 2)

                    // V13 = D1X,D1Y,D2Z,D2Z
                    V13 = Avx.Shuffle(D1, D2, 0xa4);   //_MM_SHUFFLE(2, 2, 1, 0)
                    V02 = Avx.Permute(row4, 0x9e);         //_MM_SHUFFLE(2, 1, 3, 2)
                    V12 = Avx.Shuffle(D1, V13, 0x93);  //_MM_SHUFFLE(2, 1, 0, 3)
                    V03 = Avx.Permute(row3, 0x7b);         //_MM_SHUFFLE(1, 3, 2, 3)
                    V13 = Avx.Shuffle(D1, V13, 0x26);  //_MM_SHUFFLE(0, 2, 1, 2)

                    C0 = Fma.MultiplyAdd(VectorMath.Negate(V00), V10, C0);
                    C2 = Fma.MultiplyAdd(VectorMath.Negate(V01), V11, C2);
                    C4 = Fma.MultiplyAdd(VectorMath.Negate(V02), V12, C4);
                    C6 = Fma.MultiplyAdd(VectorMath.Negate(V03), V13, C6);

                    V00 = Avx.Permute(row2, 0x33); //_MM_SHUFFLE(0, 3, 0, 3)

                    // V10 = D0Z,D0Z,D2X,D2Y
                    V10 = Avx.Shuffle(D0, D2, 0x4A); //_MM_SHUFFLE(1, 0, 2, 2)
                    V10 = Avx.Permute(V10, 0x2C);        //_MM_SHUFFLE(0, 2, 3, 0)
                    V01 = Avx.Permute(row1, 0x8D);       //_MM_SHUFFLE(2, 0, 3, 1)

                    // V11 = D0X,D0W,D2X,D2Y
                    V11 = Avx.Shuffle(D0, D2, 0x4C); //_MM_SHUFFLE(1, 0, 3, 0)
                    V11 = Avx.Permute(V11, 0x93);        //_MM_SHUFFLE(2, 1, 0, 3)
                    V02 = Avx.Permute(row4, 0x33);       //_MM_SHUFFLE(0, 3, 0, 3)

                    // V12 = D1Z,D1Z,D2Z,D2W
                    V12 = Avx.Shuffle(D1, D2, 0xEA); //_MM_SHUFFLE(3, 2, 2, 2)
                    V12 = Avx.Permute(V12, 0x2C);        //_MM_SHUFFLE(0, 2, 3, 0)
                    V03 = Avx.Permute(row3, 0x8D);       //_MM_SHUFFLE(2, 0, 3, 1)

                    // V13 = D1X,D1W,D2Z,D2W
                    V13 = Avx.Shuffle(D1, D2, 0xEC); //_MM_SHUFFLE(3, 2, 3, 0)
                    V13 = Avx.Permute(V13, 0x93);        //_MM_SHUFFLE(2, 1, 0, 3)

                    V00 = Avx.Multiply(V00, V10);
                    V01 = Avx.Multiply(V01, V11);
                    V02 = Avx.Multiply(V02, V12);
                    V03 = Avx.Multiply(V03, V13);

                    Vector256<double> C1 = Avx.Subtract(C0, V00);
                    C0 = Avx.Add(C0, V00);
                    Vector256<double> C3 = Avx.Add(C2, V01);
                    C2 = Avx.Subtract(C2, V01);
                    Vector256<double> C5 = Avx.Subtract(C4, V02);
                    C4 = Avx.Add(C4, V02);
                    Vector256<double> C7 = Avx.Add(C6, V03);
                    C6 = Avx.Subtract(C6, V03);

                    C0 = Avx.Shuffle(C0, C1, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C2 = Avx.Shuffle(C2, C3, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C4 = Avx.Shuffle(C4, C5, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C6 = Avx.Shuffle(C6, C7, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)

                    C0 = Avx.Permute(C0, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C2 = Avx.Permute(C2, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C4 = Avx.Permute(C4, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)
                    C6 = Avx.Permute(C6, 0xD8); //_MM_SHUFFLE(3, 1, 2, 0)

                    // Get the determinant
                    vTemp2 = row1;
                    double det = Vector4D.Dot(C0.AsVector4D(), vTemp2.AsVector4D());

                    // Check determinate is not zero
                    if (Math.Abs(det) < double.Epsilon)
                    {
                        Vector4D vNaN = Vector4D.Create(float.NaN);

                        result.X = vNaN;
                        result.Y = vNaN;
                        result.Z = vNaN;
                        result.W = vNaN;

                        return false;
                    }

                    // Create Vector256<double> copy of the determinant and invert them.

                    Vector256<double> ones = Vector256.Create(1.0);
                    Vector256<double> vTemp = Vector256.Create(det);
                    vTemp = Avx.Divide(ones, vTemp);

                    result.X = Avx.Multiply(C0, vTemp).AsVector4D();
                    result.Y = Avx.Multiply(C2, vTemp).AsVector4D();
                    result.Z = Avx.Multiply(C4, vTemp).AsVector4D();
                    result.W = Avx.Multiply(C6, vTemp).AsVector4D();

                    return true;
                }
#endif

                static bool SoftwareFallback(in Impl matrix, out Impl result)
                {
                    //                                       -1
                    // If you have matrix M, inverse Matrix M   can compute
                    //
                    //     -1       1
                    //    M   = --------- A
                    //            det(M)
                    //
                    // A is adjugate (adjoint) of M, where,
                    //
                    //      T
                    // A = C
                    //
                    // C is Cofactor matrix of M, where,
                    //           i + j
                    // C   = (-1)      * det(M  )
                    //  ij                    ij
                    //
                    //     [ a b c d ]
                    // M = [ e f g h ]
                    //     [ i j k l ]
                    //     [ m n o p ]
                    //
                    // First Row
                    //           2 | f g h |
                    // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
                    //  11         | n o p |
                    //
                    //           3 | e g h |
                    // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
                    //  12         | m o p |
                    //
                    //           4 | e f h |
                    // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
                    //  13         | m n p |
                    //
                    //           5 | e f g |
                    // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
                    //  14         | m n o |
                    //
                    // Second Row
                    //           3 | b c d |
                    // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
                    //  21         | n o p |
                    //
                    //           4 | a c d |
                    // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
                    //  22         | m o p |
                    //
                    //           5 | a b d |
                    // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
                    //  23         | m n p |
                    //
                    //           6 | a b c |
                    // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
                    //  24         | m n o |
                    //
                    // Third Row
                    //           4 | b c d |
                    // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
                    //  31         | n o p |
                    //
                    //           5 | a c d |
                    // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
                    //  32         | m o p |
                    //
                    //           6 | a b d |
                    // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
                    //  33         | m n p |
                    //
                    //           7 | a b c |
                    // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
                    //  34         | m n o |
                    //
                    // Fourth Row
                    //           5 | b c d |
                    // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
                    //  41         | j k l |
                    //
                    //           6 | a c d |
                    // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
                    //  42         | i k l |
                    //
                    //           7 | a b d |
                    // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
                    //  43         | i j l |
                    //
                    //           8 | a b c |
                    // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
                    //  44         | i j k |
                    //
                    // Cost of operation
                    // 53 adds, 104 muls, and 1 div.

                    double a = matrix.X.X, b = matrix.X.Y, c = matrix.X.Z, d = matrix.X.W;
                    double e = matrix.Y.X, f = matrix.Y.Y, g = matrix.Y.Z, h = matrix.Y.W;
                    double i = matrix.Z.X, j = matrix.Z.Y, k = matrix.Z.Z, l = matrix.Z.W;
                    double m = matrix.W.X, n = matrix.W.Y, o = matrix.W.Z, p = matrix.W.W;

                    double kp_lo = k * p - l * o;
                    double jp_ln = j * p - l * n;
                    double jo_kn = j * o - k * n;
                    double ip_lm = i * p - l * m;
                    double io_km = i * o - k * m;
                    double in_jm = i * n - j * m;

                    double a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
                    double a12 = -(e * kp_lo - g * ip_lm + h * io_km);
                    double a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
                    double a14 = -(e * jo_kn - f * io_km + g * in_jm);

                    double det = a * a11 + b * a12 + c * a13 + d * a14;

                    if (Math.Abs(det) < double.Epsilon)
                    {
                        Vector4D vNaN = Vector4D.Create(double.NaN);

                        result.X = vNaN;
                        result.Y = vNaN;
                        result.Z = vNaN;
                        result.W = vNaN;

                        return false;
                    }

                    double invDet = 1.0f / det;

                    result.X.X = a11 * invDet;
                    result.Y.X = a12 * invDet;
                    result.Z.X = a13 * invDet;
                    result.W.X = a14 * invDet;

                    result.X.Y = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
                    result.Y.Y = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
                    result.Z.Y = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
                    result.W.Y = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

                    double gp_ho = g * p - h * o;
                    double fp_hn = f * p - h * n;
                    double fo_gn = f * o - g * n;
                    double ep_hm = e * p - h * m;
                    double eo_gm = e * o - g * m;
                    double en_fm = e * n - f * m;

                    result.X.Z = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
                    result.Y.Z = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
                    result.Z.Z = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
                    result.W.Z = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

                    double gl_hk = g * l - h * k;
                    double fl_hj = f * l - h * j;
                    double fk_gj = f * k - g * j;
                    double el_hi = e * l - h * i;
                    double ek_gi = e * k - g * i;
                    double ej_fi = e * j - f * i;

                    result.X.W = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
                    result.Y.W = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
                    result.Z.W = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
                    result.W.W = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

                    return true;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl Lerp(in Impl left, in Impl right, double amount)
            {
                Impl result;

                result.X = Vector4D.Lerp(left.X, right.X, amount);
                result.Y = Vector4D.Lerp(left.Y, right.Y, amount);
                result.Z = Vector4D.Lerp(left.Z, right.Z, amount);
                result.W = Vector4D.Lerp(left.W, right.W, amount);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl Transform(in Impl value, in QuaternionD rotation)
            {
                // Compute rotation matrix.
                double x2 = rotation.X + rotation.X;
                double y2 = rotation.Y + rotation.Y;
                double z2 = rotation.Z + rotation.Z;

                double wx2 = rotation.W * x2;
                double wy2 = rotation.W * y2;
                double wz2 = rotation.W * z2;

                double xx2 = rotation.X * x2;
                double xy2 = rotation.X * y2;
                double xz2 = rotation.X * z2;

                double yy2 = rotation.Y * y2;
                double yz2 = rotation.Y * z2;
                double zz2 = rotation.Z * z2;

                double q11 = 1.0f - yy2 - zz2;
                double q21 = xy2 - wz2;
                double q31 = xz2 + wy2;

                double q12 = xy2 + wz2;
                double q22 = 1.0f - xx2 - zz2;
                double q32 = yz2 - wx2;

                double q13 = xz2 - wy2;
                double q23 = yz2 + wx2;
                double q33 = 1.0f - xx2 - yy2;

                Impl result;

                result.X = Vector4D.Create(
                    value.X.X * q11 + value.X.Y * q21 + value.X.Z * q31,
                    value.X.X * q12 + value.X.Y * q22 + value.X.Z * q32,
                    value.X.X * q13 + value.X.Y * q23 + value.X.Z * q33,
                    value.X.W
                );
                result.Y = Vector4D.Create(
                    value.Y.X * q11 + value.Y.Y * q21 + value.Y.Z * q31,
                    value.Y.X * q12 + value.Y.Y * q22 + value.Y.Z * q32,
                    value.Y.X * q13 + value.Y.Y * q23 + value.Y.Z * q33,
                    value.Y.W
                );
                result.Z = Vector4D.Create(
                    value.Z.X * q11 + value.Z.Y * q21 + value.Z.Z * q31,
                    value.Z.X * q12 + value.Z.Y * q22 + value.Z.Z * q32,
                    value.Z.X * q13 + value.Z.Y * q23 + value.Z.Z * q33,
                    value.Z.W
                );
                result.W = Vector4D.Create(
                    value.W.X * q11 + value.W.Y * q21 + value.W.Z * q31,
                    value.W.X * q12 + value.W.Y * q22 + value.W.Z * q32,
                    value.W.X * q13 + value.W.Y * q23 + value.W.Z * q33,
                    value.W.W
                );

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Impl Transpose(in Impl matrix)
            {
                // This implementation is based on the DirectX Math Library XMMatrixTranspose method
                // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

                Impl result;

#if NET5_0_OR_GREATER
                if (AdvSimd.Arm64.IsSupported)
                {
                    // This implementation is based on the DirectX Math Library XMMatrixTranspose method
                    // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
                    Impl c = matrix;
                    Vector128<double> row1 = AdvSimd.LoadVector128((double*)&c.X);
                    Vector128<double> row2 = AdvSimd.LoadVector128((double*)&c.X + 2);
                    Vector128<double> row3 = AdvSimd.LoadVector128((double*)&c.Y);
                    Vector128<double> row4 = AdvSimd.LoadVector128((double*)&c.Y + 2);
                    Vector128<double> row5 = AdvSimd.LoadVector128((double*)&c.Z);
                    Vector128<double> row6 = AdvSimd.LoadVector128((double*)&c.Z + 2);
                    Vector128<double> row7 = AdvSimd.LoadVector128((double*)&c.W);
                    Vector128<double> row8 = AdvSimd.LoadVector128((double*)&c.W + 2);

                    Vector128<double> t1 = AdvSimd.Arm64.ZipLow(row1, row3);
                    Vector128<double> t2 = AdvSimd.Arm64.ZipHigh(row1, row3);
                    Vector128<double> t3 = AdvSimd.Arm64.ZipLow(row2, row4);
                    Vector128<double> t4 = AdvSimd.Arm64.ZipHigh(row2, row4);

                    Vector128<double> t5 = AdvSimd.Arm64.ZipLow(row5, row7);
                    Vector128<double> t6 = AdvSimd.Arm64.ZipHigh(row5, row7);
                    Vector128<double> t7 = AdvSimd.Arm64.ZipLow(row6, row8);
                    Vector128<double> t8 = AdvSimd.Arm64.ZipHigh(row6, row8);



                    AdvSimd.Store((double*)&result.X, AdvSimd.Arm64.ZipLow(t1, t5));
                    AdvSimd.Store((double*)&result.X + 2, AdvSimd.Arm64.ZipLow(t3, t7));
                    AdvSimd.Store((double*)&result.Y, AdvSimd.Arm64.ZipHigh(t1, t5));
                    AdvSimd.Store((double*)&result.Y + 2, AdvSimd.Arm64.ZipHigh(t3, t7));
                    AdvSimd.Store((double*)&result.Z, AdvSimd.Arm64.ZipLow(t2, t6));
                    AdvSimd.Store((double*)&result.Z + 2, AdvSimd.Arm64.ZipLow(t4, t8));
                    AdvSimd.Store((double*)&result.W, AdvSimd.Arm64.ZipHigh(t2, t6));
                    AdvSimd.Store((double*)&result.W + 2, AdvSimd.Arm64.ZipHigh(t4, t8));

                    return result;
                }
                else if (Avx.IsSupported)
                {
                    Vector256<double> row1 = matrix.X.AsVector256();
                    Vector256<double> row2 = matrix.Y.AsVector256();
                    Vector256<double> row3 = matrix.Z.AsVector256();
                    Vector256<double> row4 = matrix.W.AsVector256();

                    Vector256<double> t1 = Avx.UnpackLow(row1, row2);
                    Vector256<double> t2 = Avx.UnpackHigh(row1, row2);
                    Vector256<double> t3 = Avx.UnpackLow(row3, row4);
                    Vector256<double> t4 = Avx.UnpackHigh(row3, row4);

                    row1 = Avx.Permute2x128(t1, t3, 0x20); // M11, M21, M31, M41
                    row2 = Avx.Permute2x128(t2, t4, 0x20); // M12, M22, M32, M42
                    row3 = Avx.Permute2x128(t1, t3, 0x31); // M13, M23, M33, M43
                    row4 = Avx.Permute2x128(t2, t4, 0x31); // M14, M24, M34, M44

                    result.X = row1.AsVector4D();
                    result.Y = row2.AsVector4D();
                    result.Z = row3.AsVector4D();
                    result.W = row4.AsVector4D();

                    return result;
                }
                else if (Sse2.IsSupported)
                {
                    Impl c = matrix;
                    Vector128<double> row1 = Sse2.LoadVector128((double*)&c.X);
                    Vector128<double> row2 = Sse2.LoadVector128((double*)&c.X + 2);
                    Vector128<double> row3 = Sse2.LoadVector128((double*)&c.Y);
                    Vector128<double> row4 = Sse2.LoadVector128((double*)&c.Y + 2);
                    Vector128<double> row5 = Sse2.LoadVector128((double*)&c.Z);
                    Vector128<double> row6 = Sse2.LoadVector128((double*)&c.Z + 2);
                    Vector128<double> row7 = Sse2.LoadVector128((double*)&c.W);
                    Vector128<double> row8 = Sse2.LoadVector128((double*)&c.W + 2);

                    Vector128<double> t1 = Sse2.UnpackLow(row1, row3);
                    Vector128<double> t2 = Sse2.UnpackHigh(row1, row3);
                    Vector128<double> t3 = Sse2.UnpackLow(row2, row4);
                    Vector128<double> t4 = Sse2.UnpackHigh(row2, row4);

                    Vector128<double> t5 = Sse2.UnpackLow(row5, row7);
                    Vector128<double> t6 = Sse2.UnpackHigh(row5, row7);
                    Vector128<double> t7 = Sse2.UnpackLow(row6, row8);
                    Vector128<double> t8 = Sse2.UnpackHigh(row6, row8);

                    Sse2.Store((double*)&result.X, t1);
                    Sse2.Store((double*)&result.X + 2, t5);
                    Sse2.Store((double*)&result.Y, t2);
                    Sse2.Store((double*)&result.Y + 2, t6);
                    Sse2.Store((double*)&result.Z, t3);
                    Sse2.Store((double*)&result.Z + 2, t7);
                    Sse2.Store((double*)&result.W, t4);
                    Sse2.Store((double*)&result.W + 2, t8);

                    return result;
                }
#endif

                result.X = Vector4D.Create(matrix.X.X, matrix.Y.X, matrix.Z.X, matrix.W.X);
                result.Y = Vector4D.Create(matrix.X.Y, matrix.Y.Y, matrix.Z.Y, matrix.W.Y);
                result.Z = Vector4D.Create(matrix.X.Z, matrix.Y.Z, matrix.Z.Z, matrix.W.Z);
                result.W = Vector4D.Create(matrix.X.W, matrix.Y.W, matrix.Z.W, matrix.W.W);

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override readonly bool Equals(object? obj)
                => (obj is Matrix4x4D other) && Equals(in other.AsImpl());

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly bool Equals(in Impl other)
            {
                // This function needs to account for doubleing-point equality around NaN
                // and so must behave equivalently to the underlying double/double.Equals

                return X.Equals(other.X)
                    && Y.Equals(other.Y)
                    && Z.Equals(other.Z)
                    && W.Equals(other.W);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly double GetDeterminant()
            {
                // | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
                // | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
                // | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
                // | m n o p |
                //
                //   | f g h |
                // a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
                //   | n o p |
                //
                //   | e g h |
                // b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
                //   | m o p |
                //
                //   | e f h |
                // c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
                //   | m n p |
                //
                //   | e f g |
                // d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
                //   | m n o |
                //
                // Cost of operation
                // 17 adds and 28 muls.
                //
                // add: 6 + 8 + 3 = 17
                // mul: 12 + 16 = 28

                double a = X.X, b = X.Y, c = X.Z, d = X.W;
                double e = Y.X, f = Y.Y, g = Y.Z, h = Y.W;
                double i = Z.X, j = Z.Y, k = Z.Z, l = Z.W;
                double m = W.X, n = W.Y, o = W.Z, p = W.W;

                double kp_lo = k * p - l * o;
                double jp_ln = j * p - l * n;
                double jo_kn = j * o - k * n;
                double ip_lm = i * p - l * m;
                double io_km = i * o - k * m;
                double in_jm = i * n - j * m;

                return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
                       b * (e * kp_lo - g * ip_lm + h * io_km) +
                       c * (e * jp_ln - f * ip_lm + h * in_jm) -
                       d * (e * jo_kn - f * io_km + g * in_jm);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);

            readonly bool IEquatable<Impl>.Equals(Impl other) => Equals(in other);

            private static void ThrowPlatformNotSupportedException() => throw new PlatformNotSupportedException();
        }
    }
}