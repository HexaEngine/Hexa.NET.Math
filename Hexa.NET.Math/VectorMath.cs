namespace Hexa.NET.Mathematics
{
#if NET5_0_OR_GREATER
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Intrinsics;
    using System.Runtime.Intrinsics.Arm;
    using System.Runtime.Intrinsics.X86;
    using System.Text;
    using System.Threading.Tasks;

    public class VectorMath
    {
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

        public static bool Equal(Vector256<double> vec1, Vector256<double> vec2)
        {
            Vector256<double> result = Avx.CompareEqual(vec1, vec2);
            Vector256<long> res = result.AsInt64();
            return res.GetElement(0) == -1 && res.GetElement(1) == -1 && res.GetElement(2) == -1 && res.GetElement(3) == -1;
        }

        public static bool NotEqual(Vector256<double> vec1, Vector256<double> vec2)
        {
            Vector256<double> result = Avx.CompareNotEqual(vec1, vec2);
            Vector256<long> res = result.AsInt64();
            return res.GetElement(0) == -1 && res.GetElement(1) == -1 && res.GetElement(2) == -1 && res.GetElement(3) == -1;
        }

#if NET8_0_OR_GREATER
        public static bool Equal(Vector512<double> vec1, Vector512<double> vec2)
        {
            Vector512<double> result = Avx512F.CompareEqual(vec1, vec2);
            Vector512<long> res = result.AsInt64();
            return res[0] == -1 && res[1] == -1 && res[2] == -1 && res[3] == -1 && res[4] == -1 && res[5] == -1 && res[6] == -1 && res[7] == -1;
        }

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
