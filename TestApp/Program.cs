using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using Hexa.NET.Mathematics;
using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        var A = new Matrix4x4D(
           1.0, 2.0, 3.0, 4.0,
           5.0, 6.0, 7.0, 8.0,
           9.0, 10.0, 11.0, 12.0,
           13.0, 14.0, 15.0, 16.0);

        var B = new Matrix4x4D(
            16.0, 15.0, 14.0, 13.0,
            12.0, 11.0, 10.0, 9.0,
            8.0, 7.0, 6.0, 5.0,
            4.0, 3.0, 2.0, 1.0);

        var expected = new Matrix4x4D(
            80.0, 70.0, 60.0, 50.0,
            240.0, 214.0, 188.0, 162.0,
            400.0, 358.0, 316.0, 274.0,
            560.0, 502.0, 444.0, 386.0);


        var result = Multiply(A, B);

        var result1 = Mult(A, B);

        var result2 = SoftwareMultiply(A, B);

        if (result == expected)
        {

        }

        

        var aT = Transpose(A);
        var aT2 = TransposeSoftware(A);

        if (aT == aT2)
        {

        }
    }

    public unsafe static Matrix4x4D Transpose(Matrix4x4D matrix)
    {
        Vector256<double> row1 = Avx.LoadVector256(&matrix.M11);
        Vector256<double> row2 = Avx.LoadVector256(&matrix.M21);
        Vector256<double> row3 = Avx.LoadVector256(&matrix.M31);
        Vector256<double> row4 = Avx.LoadVector256(&matrix.M41);

        Vector256<double> t1 = Avx.UnpackLow(row1, row2);
        Vector256<double> t2 = Avx.UnpackHigh(row1, row2);
        Vector256<double> t3 = Avx.UnpackLow(row3, row4);
        Vector256<double> t4 = Avx.UnpackHigh(row3, row4);

        row1 = Avx.Permute2x128(t1, t3, 0x20); // M11, M21, M31, M41
        row2 = Avx.Permute2x128(t2, t4, 0x20); // M12, M22, M32, M42
        row3 = Avx.Permute2x128(t1, t3, 0x31); // M13, M23, M33, M43
        row4 = Avx.Permute2x128(t2, t4, 0x31); // M14, M24, M34, M44

        Avx.Store(&matrix.M11, row1);
        Avx.Store(&matrix.M21, row2);
        Avx.Store(&matrix.M31, row3);
        Avx.Store(&matrix.M41, row4);
        return matrix;
    }

    public static Matrix4x4D TransposeSoftware(Matrix4x4D matrix)
    {
        Matrix4x4D result;

        result.M11 = matrix.M11;
        result.M12 = matrix.M21;
        result.M13 = matrix.M31;
        result.M14 = matrix.M41;
        result.M21 = matrix.M12;
        result.M22 = matrix.M22;
        result.M23 = matrix.M32;
        result.M24 = matrix.M42;
        result.M31 = matrix.M13;
        result.M32 = matrix.M23;
        result.M33 = matrix.M33;
        result.M34 = matrix.M43;
        result.M41 = matrix.M14;
        result.M42 = matrix.M24;
        result.M43 = matrix.M34;
        result.M44 = matrix.M44;

        return result;
    }

    public unsafe static Matrix4x4D Multiply(Matrix4x4D value1, Matrix4x4D value2) 
    {
        Vector256<double> row1 = Avx.LoadVector256(&value2.M11);
        Vector256<double> row2 = Avx.LoadVector256(&value2.M21);
        Vector256<double> row3 = Avx.LoadVector256(&value2.M31);
        Vector256<double> row4 = Avx.LoadVector256(&value2.M41);

        // Process the first row
        Vector256<double> M11 = Avx.LoadVector256(&value1.M11);
        Vector256<double> r1 = Avx.Multiply(Avx2.Permute4x64(M11, 0b00_00_00_00), row1);
        r1 = Fma.MultiplyAdd(Avx2.Permute4x64(M11, 0b01_01_01_01), row2, r1);
        r1 = Fma.MultiplyAdd(Avx2.Permute4x64(M11, 0b10_10_10_10), row3, r1);
        r1 = Fma.MultiplyAdd(Avx2.Permute4x64(M11, 0b11_11_11_11), row4, r1);
        Avx.Store(&value1.M11, r1);

        // Process the second row
        Vector256<double> M21 = Avx.LoadVector256(&value1.M21);
        Vector256<double> r2 = Avx.Multiply(Avx2.Permute4x64(M21, 0b00_00_00_00), row1);
        r2 = Fma.MultiplyAdd(Avx2.Permute4x64(M21, 0b01_01_01_01), row2, r2);
        r2 = Fma.MultiplyAdd(Avx2.Permute4x64(M21, 0b10_10_10_10), row3, r2);
        r2 = Fma.MultiplyAdd(Avx2.Permute4x64(M21, 0b11_11_11_11), row4, r2);
        Avx.Store(&value1.M21, r2);

        // Process the third row
        Vector256<double> M31 = Avx.LoadVector256(&value1.M31);
        Vector256<double> r3 = Avx.Multiply(Avx2.Permute4x64(M31, 0b00_00_00_00), row1);
        r3 = Fma.MultiplyAdd(Avx2.Permute4x64(M31, 0b01_01_01_01), row2, r3);
        r3 = Fma.MultiplyAdd(Avx2.Permute4x64(M31, 0b10_10_10_10), row3, r3);
        r3 = Fma.MultiplyAdd(Avx2.Permute4x64(M31, 0b11_11_11_11), row4, r3);
        Avx.Store(&value1.M31, r3);

        // Process the fourth row
        Vector256<double> M41 = Avx.LoadVector256(&value1.M41);
        Vector256<double> r4 = Avx.Multiply(Avx2.Permute4x64(M41, 0b00_00_00_00), row1);
        r4 = Fma.MultiplyAdd(Avx2.Permute4x64(M41, 0b01_01_01_01), row2, r4);
        r4 = Fma.MultiplyAdd(Avx2.Permute4x64(M41, 0b10_10_10_10), row3, r4);
        r4 = Fma.MultiplyAdd(Avx2.Permute4x64(M41, 0b11_11_11_11), row4, r4);
        Avx.Store(&value1.M41, r4);

        return value1;
    } 

    public unsafe static Matrix4x4D Mult(Matrix4x4D value1, Matrix4x4D value2)
    {
        // Process the first row
        Vector256<double> M11 = Avx.LoadVector256(&value1.M11);

        Vector256<double> vX = Avx.Multiply(Avx2.Permute4x64(M11, 0b00_00_00_00), Avx.LoadVector256(&value2.M11));
        Vector256<double> vY = Avx.Multiply(Avx2.Permute4x64(M11, 0b01_01_01_01), Avx.LoadVector256(&value2.M21));
        Vector256<double> vZ = Avx.Multiply(Avx2.Permute4x64(M11, 0b10_10_10_10), Avx.LoadVector256(&value2.M31));
        Vector256<double> vW = Avx.Multiply(Avx2.Permute4x64(M11, 0b11_11_11_11), Avx.LoadVector256(&value2.M41));

        Avx.Store(&value1.M11, Avx.Add(Avx.Add(vX, vY), Avx.Add(vZ, vW)));

        // Process the second row
        Vector256<double> M21 = Avx.LoadVector256(&value1.M21);

        vX = Avx.Multiply(Avx2.Permute4x64(M21, 0b00_00_00_00), Avx.LoadVector256(&value2.M11));
        vY = Avx.Multiply(Avx2.Permute4x64(M21, 0b01_01_01_01), Avx.LoadVector256(&value2.M21));
        vZ = Avx.Multiply(Avx2.Permute4x64(M21, 0b10_10_10_10), Avx.LoadVector256(&value2.M31));
        vW = Avx.Multiply(Avx2.Permute4x64(M21, 0b11_11_11_11), Avx.LoadVector256(&value2.M41));

        Avx.Store(&value1.M21, Avx.Add(Avx.Add(vX, vY), Avx.Add(vZ, vW)));

        // Process the third row
        Vector256<double> M31 = Avx.LoadVector256(&value1.M31);

        vX = Avx.Multiply(Avx2.Permute4x64(M31, 0b00_00_00_00), Avx.LoadVector256(&value2.M11));
        vY = Avx.Multiply(Avx2.Permute4x64(M31, 0b01_01_01_01), Avx.LoadVector256(&value2.M21));
        vZ = Avx.Multiply(Avx2.Permute4x64(M31, 0b10_10_10_10), Avx.LoadVector256(&value2.M31));
        vW = Avx.Multiply(Avx2.Permute4x64(M31, 0b11_11_11_11), Avx.LoadVector256(&value2.M41));

        Avx.Store(&value1.M31, Avx.Add(Avx.Add(vX, vY), Avx.Add(vZ, vW)));

        // Process the fourth row
        Vector256<double> M41 = Avx.LoadVector256(&value1.M41);

        vX = Avx.Multiply(Avx2.Permute4x64(M41, 0b00_00_00_00), Avx.LoadVector256(&value2.M11));
        vY = Avx.Multiply(Avx2.Permute4x64(M41, 0b01_01_01_01), Avx.LoadVector256(&value2.M21));
        vZ = Avx.Multiply(Avx2.Permute4x64(M41, 0b10_10_10_10), Avx.LoadVector256(&value2.M31));
        vW = Avx.Multiply(Avx2.Permute4x64(M41, 0b11_11_11_11), Avx.LoadVector256(&value2.M41));

        Avx.Store(&value1.M41, Avx.Add(Avx.Add(vX, vY), Avx.Add(vZ, vW)));

        return value1;
    }

    public static Matrix4x4D SoftwareMultiply(Matrix4x4D value1, Matrix4x4D value2)
    {
        Matrix4x4D m;

        // First row
        m.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
        m.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
        m.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
        m.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;

        // Second row
        m.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
        m.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
        m.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
        m.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;

        // Third row
        m.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
        m.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
        m.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
        m.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;

        // Fourth row
        m.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
        m.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
        m.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
        m.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;
        return m;
    }
}
