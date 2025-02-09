namespace Hexa.NET.Mathematics
{
#if NET5_0_OR_GREATER
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Intrinsics;
    using System.Runtime.Intrinsics.Arm;
    using System.Runtime.Intrinsics.X86;
    using System.Text;
    using System.Threading.Tasks;

    public class VectorMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> Negate(Vector256<double> vector)
        {
            return Avx.Xor(vector, Vector256.Create(-0.0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<double> Lerp(Vector128<double> vec1, Vector128<double> vec2, Vector128<double> t)
        {
            if (AdvSimd.Arm64.IsSupported)
            {
                Vector128<double> delta = AdvSimd.Arm64.Subtract(vec2, vec1);
                Vector128<double> scaled = AdvSimd.Arm64.Multiply(t, delta);
                return AdvSimd.Arm64.Add(vec1, scaled);
            }
            else
            {
                Vector128<double> delta = Sse2.Subtract(vec2, vec1);
                Vector128<double> scaled = Sse2.Multiply(t, delta);
                return Sse2.Add(vec1, scaled);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> Lerp(Vector256<double> vec1, Vector256<double> vec2, Vector256<double> t)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAdd(t, Avx.Subtract(vec2, vec1), vec1);
            }
            else
            {
                Vector256<double> delta = Avx.Subtract(vec2, vec1);
                Vector256<double> scaled = Avx.Multiply(t, delta);
                return Avx.Add(vec1, scaled);
            }
        }

#if NET8_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector512<double> Lerp(Vector512<double> vec1, Vector512<double> vec2, Vector512<double> t)
        {
            Vector512<double> delta = Avx512F.Subtract(vec2, vec1);
            return Avx512F.FusedMultiplyAdd(t, delta, vec1);
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equal(Vector128<double> vec1, Vector128<double> vec2)
        {
            if (AdvSimd.Arm64.IsSupported)
            {
                Vector128<double> result = AdvSimd.Arm64.CompareEqual(vec1, vec2);
                Vector128<long> res = result.AsInt64();
                return res.GetElement(0) == -1 && res.GetElement(1) == -1;
            }
            else
            {
                Vector128<double> result = Sse2.CompareEqual(vec1, vec2);
                Vector128<long> res = result.AsInt64();
                return res.GetElement(0) == -1 && res.GetElement(1) == -1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NotEqual(Vector128<double> vec1, Vector128<double> vec2)
        {
            if (AdvSimd.Arm64.IsSupported)
            {
                Vector128<double> result = AdvSimd.Arm64.CompareEqual(vec1, vec2);
                Vector128<long> res = result.AsInt64();
                return res.GetElement(0) != -1 && res.GetElement(1) != -1;
            }
            else
            {
                Vector128<double> result = Sse2.CompareNotEqual(vec1, vec2);
                Vector128<long> res = result.AsInt64();
                return res.GetElement(0) == -1 && res.GetElement(1) == -1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equal(Vector256<double> vec1, Vector256<double> vec2)
        {
            Vector256<double> result = Avx.CompareEqual(vec1, vec2);
            Vector256<long> res = result.AsInt64();
            return res.GetElement(0) == -1 && res.GetElement(1) == -1 && res.GetElement(2) == -1 && res.GetElement(3) == -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NotEqual(Vector256<double> vec1, Vector256<double> vec2)
        {
            Vector256<double> result = Avx.CompareNotEqual(vec1, vec2);
            Vector256<long> res = result.AsInt64();
            return res.GetElement(0) == -1 && res.GetElement(1) == -1 && res.GetElement(2) == -1 && res.GetElement(3) == -1;
        }

#if NET8_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equal(Vector512<double> vec1, Vector512<double> vec2)
        {
            Vector512<double> result = Avx512F.CompareEqual(vec1, vec2);
            Vector512<long> res = result.AsInt64();
            return res[0] == -1 && res[1] == -1 && res[2] == -1 && res[3] == -1 && res[4] == -1 && res[5] == -1 && res[6] == -1 && res[7] == -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NotEqual(Vector512<double> vec1, Vector512<double> vec2)
        {
            Vector512<double> result = Avx512F.CompareNotEqual(vec1, vec2);
            Vector512<long> res = result.AsInt64();
            return res[0] == -1 && res[1] == -1 && res[2] == -1 && res[3] == -1 && res[4] == -1 && res[5] == -1 && res[6] == -1 && res[7] == -1;
        }
#endif

    }
#endif
}
