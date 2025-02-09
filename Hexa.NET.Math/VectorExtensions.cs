namespace Hexa.NET.Mathematics
{
    using System.Numerics;
    using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER

    using System.Runtime.Intrinsics;

#endif

    public static unsafe class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Normalize(this Vector2 v)
        {
            return MathUtil.Normalize(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Normalize(this Vector3 v)
        {
            return MathUtil.Normalize(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Normalize(this Vector4 v)
        {
            return MathUtil.Normalize(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D Normalize(this Vector2D v)
        {
            return MathUtil.Normalize(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Normalize(this Vector3D v)
        {
            return MathUtil.Normalize(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Normalize(this Vector4D v)
        {
            return MathUtil.Normalize(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD AsQuaternionD(this Vector4D value)
        {
            return *(QuaternionD*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D AsVector4D(this QuaternionD value)
        {
            return *(Vector4D*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD AsPlaneD(this Vector4D value)
        {
            return *(PlaneD*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D AsVector4D(this PlaneD value)
        {
            return *(Vector4D*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D AsVector3D(this Vector2D value)
        {
            return Vector3D.Create(value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D AsVector4D(this Vector2D value)
        {
            return Vector4D.Create(value, 0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D AsVector2D(this Vector3D value)
        {
            return *(Vector2D*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D AsVector4D(this Vector3D value)
        {
            return Vector4D.Create(value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D AsVector2D(this Vector4D value)
        {
            return *(Vector2D*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D AsVector3D(this Vector4D value)
        {
            return *(Vector3D*)&value;
        }


#if NET5_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256Unsafe(this Vector2D value)
        {
            Unsafe.SkipInit(out Vector256<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256Unsafe(this Vector3D value)
        {
            Unsafe.SkipInit(out Vector256<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256(this Vector2D value) => Vector4D.Create(value, 0, 0).AsVector256();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256(this Vector3D value) => Vector4D.Create(value, 0).AsVector256();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256(this Vector4D value)
        {
            return *(Vector256<double>*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D AsVector2D(this Vector256<double> value)
        {
            ref byte address = ref Unsafe.As<Vector256<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector2D>(ref address);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D AsVector3D(this Vector256<double> value)
        {
            ref byte address = ref Unsafe.As<Vector256<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector3D>(ref address);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D AsVector4D(this Vector256<double> value)
        {
            return *(Vector4D*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<int> AsVector128Unsafe(this Point2 value)
        {
            Unsafe.SkipInit(out Vector128<int> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<int>, byte>(ref result), value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<int> AsVector128Unsafe(this Point3 value)
        {
            Unsafe.SkipInit(out Vector128<int> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<int>, byte>(ref result), value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<int> AsVector128(this Point2 value) => Point4.Create(value, 0, 0).AsVector128();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<int> AsVector128(this Point3 value) => Point4.Create(value, 0).AsVector128();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<int> AsVector128(this Point4 value)
        {
            return *(Vector128<int>*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 AsPoint2(this Vector128<int> value)
        {
            ref byte address = ref Unsafe.As<Vector128<int>, byte>(ref value);
            return Unsafe.ReadUnaligned<Point2>(ref address);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 AsPoint3(this Vector128<int> value)
        {
            ref byte address = ref Unsafe.As<Vector128<int>, byte>(ref value);
            return Unsafe.ReadUnaligned<Point3>(ref address);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 AsPoint4(this Vector128<int> value)
        {
            return *(Point4*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<uint> AsVector128Unsafe(this UPoint2 value)
        {
            Unsafe.SkipInit(out Vector128<uint> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<uint>, byte>(ref result), value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<uint> AsVector128Unsafe(this UPoint3 value)
        {
            Unsafe.SkipInit(out Vector128<uint> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<uint>, byte>(ref result), value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<uint> AsVector128(this UPoint2 value) => UPoint4.Create(value, 0, 0).AsVector128();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<uint> AsVector128(this UPoint3 value) => UPoint4.Create(value, 0).AsVector128();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector128<uint> AsVector128(this UPoint4 value)
        {
            return *(Vector128<uint>*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint2 AsUPoint2(this Vector128<uint> value)
        {
            ref byte address = ref Unsafe.As<Vector128<uint>, byte>(ref value);
            return Unsafe.ReadUnaligned<UPoint2>(ref address);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint3 AsUPoint3(this Vector128<uint> value)
        {
            ref byte address = ref Unsafe.As<Vector128<uint>, byte>(ref value);
            return Unsafe.ReadUnaligned<UPoint3>(ref address);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UPoint4 AsUPoint4(this Vector128<uint> value)
        {
            return *(UPoint4*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256(this QuaternionD value)
        {
            return *(Vector256<double>*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD AsQuaternionD(this Vector256<double> value)
        {
            return *(QuaternionD*)&value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> AsVector256(this PlaneD value)
        {
            return *(Vector256<double>*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD AsPlaneD(this Vector256<double> value)
        {
            return *(PlaneD*)&value;
        }

#endif
    }
}