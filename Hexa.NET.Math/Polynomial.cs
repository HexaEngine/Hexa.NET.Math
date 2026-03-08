namespace Hexa.NET.Mathematics
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represents a polynomial function.
    /// </summary>
    public unsafe struct Polynomial
    {
        private uint degree;
        private float* coefficients;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> struct with the specified degree.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        public Polynomial(uint degree)
        {
            this.degree = degree;
            if (degree == 0)
            {
                coefficients = null;
                return;
            }
            coefficients = (float*)Marshal.AllocHGlobal((nint)(degree * sizeof(float)));
            for (uint i = 0; i < degree; i++)
            {
                coefficients[i] = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> struct with the specified coefficients.
        /// </summary>
        /// <param name="coefficients">The coefficients of the polynomial.</param>
        public Polynomial(float[] coefficients)
        {
            degree = (uint)coefficients.Length;
            if (degree == 0)
            {
                this.coefficients = null;
                return;
            }
            this.coefficients = (float*)Marshal.AllocHGlobal((nint)(degree * sizeof(float)));
            for (uint i = 0; i < degree; i++)
            {
                this.coefficients[i] = coefficients[i];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> struct with the specified pointer to coefficients and degree.
        /// </summary>
        /// <param name="coefficients">The pointer to the coefficients of the polynomial.</param>
        /// <param name="degree">The degree of the polynomial.</param>
        public Polynomial(float* coefficients, uint degree)
        {
            this.degree = degree;
            if (degree == 0)
            {
                this.coefficients = null;
                return;
            }
            this.coefficients = (float*)Marshal.AllocHGlobal((nint)(degree * sizeof(float)));
            for (uint i = 0; i < degree; i++)
            {
                this.coefficients[i] = coefficients[i];
            }
        }

        /// <summary>
        /// Gets the degree of the polynomial.
        /// </summary>
        public readonly uint Degree => degree;

        /// <summary>
        /// Gets a pointer to the coefficients of the polynomial.
        /// </summary>
        public readonly float* Coefficients => coefficients;

        /// <summary>
        /// Gets or sets the coefficient at the specified index.
        /// </summary>
        /// <param name="index">The index of the coefficient to get or set.</param>
        /// <returns>The coefficient at the specified index.</returns>
        public float this[int index]
        {
            get
            {
                if (index < 0 || index >= degree)
                    throw new IndexOutOfRangeException();
                return coefficients[index];
            }
            set
            {
                if (index < 0 || index >= degree)
                    throw new IndexOutOfRangeException();
                coefficients[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the coefficient at the specified index.
        /// </summary>
        /// <param name="index">The index of the coefficient to get or set.</param>
        /// <returns>The coefficient at the specified index.</returns>
        public float this[uint index]
        {
            get
            {
                if (index >= degree)
                    throw new IndexOutOfRangeException();
                return coefficients[index];
            }
            set
            {
                if (index >= degree)
                    throw new IndexOutOfRangeException();
                coefficients[index] = value;
            }
        }

        /// <summary>
        /// Computes the value of the polynomial at the specified value of x.
        /// </summary>
        /// <param name="x">The value of x.</param>
        /// <returns>The value of the polynomial at the specified value of x.</returns>
        public readonly float Compute(float x)
        {
            float result = 0;
            float xPow = 1;
            for (uint i = 0; i < degree; i++)
            {
                result += coefficients[i] * xPow;
                xPow *= x;
            }
            return result;
        }

        /// <summary>
        /// Evaluates the polynomial at the specified array of points.
        /// </summary>
        /// <param name="points">The array of points at which to evaluate the polynomial.</param>
        /// <returns>An array of values representing the polynomial evaluated at the specified points.</returns>
        public readonly float[] Evaluate(float[] points)
        {
            float[] values = new float[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                values[i] = Compute(points[i]);
            }
            return values;
        }

        /// <summary>
        /// Evaluates the polynomial at the specified array of points and stores the result in the specified array of values.
        /// </summary>
        /// <param name="points">The array of points at which to evaluate the polynomial.</param>
        /// <param name="values">The array to store the evaluated values.</param>
        public readonly void Evaluate(float[] points, float[] values)
        {
            for (int i = 0; i < points.Length; i++)
            {
                values[i] = Compute(points[i]);
            }
        }

        /// <summary>
        /// Evaluates the polynomial at the specified pointer to points and count, and stores the result in the specified pointer to values.
        /// </summary>
        /// <param name="points">The pointer to the array of points at which to evaluate the polynomial.</param>
        /// <param name="count">The number of points to evaluate.</param>
        /// <param name="values">The pointer to the array to store the evaluated values.</param>
        public readonly void Evaluate(float* points, uint count, float* values)
        {
            for (uint i = 0; i < count; i++)
            {
                values[i] = Compute(points[i]);
            }
        }

        /// <summary>
        /// Removes trailing zero coefficients from the polynomial.
        /// </summary>
        public void Compact()
        {
            if (degree == 0 || coefficients == null)
                return;
            uint newDegree = degree;
            for (int i = (int)degree - 1; i >= 0; i--)
            {
                if (coefficients[i] == 0)
                    newDegree--;
                else
                    break;
            }
            if (newDegree == degree)
                return;
            float* newCoeffs = (float*)Marshal.AllocHGlobal((nint)(newDegree * sizeof(float)));
            for (uint i = 0; i < newDegree; i++)
                newCoeffs[i] = coefficients[i];
            Marshal.FreeHGlobal((nint)coefficients);
            coefficients = newCoeffs;
            degree = newDegree;
        }

        /// <summary>
        /// Gets the symmetry of the polynomial.
        /// </summary>
        /// <returns>The symmetry of the polynomial.</returns>
        public readonly PolynomialSymmetry GetSymmetry()
        {
            if (degree == 0 || coefficients == null)
                return PolynomialSymmetry.None;
            bool isEven = true, isOdd = true;
            for (uint i = 0; i < degree; i++)
            {
                if (i % 2 == 0)
                {
                    if (i != 0 && coefficients[i] != 0)
                        isOdd = false;
                }
                else
                {
                    if (coefficients[i] != 0)
                        isEven = false;
                }
            }
            if (isEven) return PolynomialSymmetry.Even;
            if (isOdd) return PolynomialSymmetry.Odd;
            return PolynomialSymmetry.None;
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Finds the roots of the polynomial.
        /// </summary>
        /// <param name="foundRoots">The number of roots found.</param>
        /// <returns>An array containing the roots of the polynomial.</returns>
        public readonly float[] FindRoots(out int foundRoots)
        {
            foundRoots = 0;
            if (degree == 0 || coefficients == null)
                return [];
            if (degree == 1)
            {
                float c = coefficients[0];
                if (c == 0)
                {
                    foundRoots = 1;
                    return [0];
                }
                return [];
            }
            if (degree == 2)
            {
                float c = coefficients[0];
                float b = coefficients[1];
                if (b == 0)
                {
                    if (c == 0)
                    {
                        foundRoots = 1;
                        return [0];
                    }
                    return [];
                }
                foundRoots = 1;
                return [(-c / b)];
            }
            if (degree == 3)
            {
                float c = coefficients[0];
                float b = coefficients[1];
                float a = coefficients[2];
                if (a == 0)
                {
                    if (b == 0)
                    {
                        if (c == 0)
                        {
                            foundRoots = 1;
                            return [0];
                        }
                        return [];
                    }
                    foundRoots = 1;
                    return [(-c / b)];
                }
                float discriminant = b * b - 4 * a * c;
                if (discriminant < 0)
                    return [];
                else if (discriminant == 0)
                {
                    foundRoots = 1;
                    return [(-b) / (2 * a)];
                }
                else
                {
                    foundRoots = 2;
                    float sqrtDiscriminant = MathF.Sqrt(discriminant);
                    float root1 = (-b + sqrtDiscriminant) / (2 * a);
                    float root2 = (-b - sqrtDiscriminant) / (2 * a);
                    return [root1, root2];
                }
            }
            const float tolerance = 1e-12f;
            Stack<Interval> walkStack = new();
            float[] roots = new float[degree];
            PolynomialSymmetry symmetry = GetSymmetry();
            float leadingCoefficient = coefficients[degree - 1];
            float step = 1.0f;
            float x = 1.0f;
            float xLast = 0.0f;
            if (symmetry == PolynomialSymmetry.None)
            {
                while (foundRoots < degree)
                {
                    float negX = -x;
                    float negXLast = -xLast;
                    Interval negInterval = new(negXLast, negX, default, default);
                    Interval interval = new(xLast, x, default, default);
                    if (SignChange(negInterval))
                        walkStack.Push(negInterval);
                    if (SignChange(interval))
                        walkStack.Push(interval);
                    FindRootsSubStep(ref foundRoots, walkStack, roots);
                    x += step;
                    xLast = x;
                    if (x >= float.MaxValue)
                        break;
                }
            }
            else
            {
                while (foundRoots < degree)
                {
                    Interval interval = new(xLast, x, default, default);
                    if (SignChange(interval, out _, out var y1))
                        walkStack.Push(interval);
                    FindRootsSubStepSym(ref foundRoots, walkStack, roots);
                    float y = leadingCoefficient * MathF.Pow(x, degree - 1);
                    if (y - y1 >= tolerance)
                        break;
                    x += step;
                    xLast = x;
                    if (x >= float.MaxValue)
                        break;
                }
            }
            return roots;
        }

        private readonly int FindRootsSubStep(ref int foundRoots, Stack<Interval> walkStack, float[] roots)
        {
            const float tolerance = 1e-12f;
            while (walkStack.TryPop(out var current))
            {
                current.Split(out var a, out var b);
                if (SignChange(a))
                {
                    if (a.Length <= tolerance)
                        roots[foundRoots++] = a.Midpoint;
                    else
                        walkStack.Push(a);
                }
                if (SignChange(b))
                {
                    if (b.Length <= tolerance)
                        roots[foundRoots++] = b.Midpoint;
                    else
                        walkStack.Push(b);
                }
            }
            return foundRoots;
        }

        private readonly int FindRootsSubStepSym(ref int foundRoots, Stack<Interval> walkStack, float[] roots)
        {
            const float tolerance = float.Epsilon;
            while (walkStack.TryPop(out var current))
            {
                current.Split(out var a, out var b);
                if (SignChange(a))
                {
                    if (a.Length <= tolerance)
                    {
                        roots[foundRoots++] = a.Midpoint;
                        roots[foundRoots++] = -a.Midpoint;
                    }
                    else
                        walkStack.Push(a);
                }
                if (SignChange(b))
                {
                    if (b.Length <= tolerance)
                    {
                        roots[foundRoots++] = b.Midpoint;
                        roots[foundRoots++] = -b.Midpoint;
                    }
                    else
                        walkStack.Push(b);
                }
            }
            return foundRoots;
        }
#endif

        /// <summary>
        /// Checks if there is a sign change in the polynomial function within the specified interval.
        /// </summary>
        /// <param name="interval">The interval to check for sign change.</param>
        /// <returns><c>true</c> if there is a sign change within the interval; otherwise, <c>false</c>.</returns>
        public readonly bool SignChange(Interval interval)
        {
            return SignChange(interval.Start, interval.End);
        }

        /// <summary>
        /// Checks if there is a sign change in the polynomial function within the specified interval.
        /// </summary>
        /// <param name="start">The start of the interval.</param>
        /// <param name="end">The end of the interval.</param>
        /// <returns><c>true</c> if there is a sign change within the interval; otherwise, <c>false</c>.</returns>
        public readonly bool SignChange(float start, float end)
        {
            float y0 = Compute(start);
            float y1 = Compute(end);
            return (y0 > 0 && y1 < 0) || (y0 < 0 && y1 > 0) || y0 == 0 || y1 == 0;
        }

        /// <summary>
        /// Checks if there is a sign change in the polynomial function within the specified interval.
        /// </summary>
        /// <param name="interval">The interval to check for sign change.</param>
        /// <param name="y0">The function value at the start of the interval.</param>
        /// <param name="y1">The function value at the end of the interval.</param>
        /// <returns><c>true</c> if there is a sign change within the interval; otherwise, <c>false</c>.</returns>
        public readonly bool SignChange(Interval interval, out float y0, out float y1)
        {
            return SignChange(interval.Start, interval.End, out y0, out y1);
        }

        /// <summary>
        /// Checks if there is a sign change in the polynomial function within the specified interval.
        /// </summary>
        /// <param name="start">The start of the interval.</param>
        /// <param name="end">The end of the interval.</param>
        /// <param name="y0">The function value at the start of the interval.</param>
        /// <param name="y1">The function value at the end of the interval.</param>
        /// <returns><c>true</c> if there is a sign change within the interval; otherwise, <c>false</c>.</returns>
        public readonly bool SignChange(float start, float end, out float y0, out float y1)
        {
            y0 = Compute(start);
            y1 = Compute(end);
            return (y0 > 0 && y1 < 0) || (y0 < 0 && y1 > 0) || y0 == 0 || y1 == 0;
        }

        /// <summary>
        /// Computes the derivative of the polynomial.
        /// </summary>
        /// <returns>The derivative of the polynomial.</returns>
        public readonly Polynomial Derivative()
        {
            if (degree <= 1 || coefficients == null)
                return new Polynomial(1);
            Polynomial polynomial = new(degree - 1);
            for (uint i = 1; i < degree; i++)
            {
                polynomial[i - 1] = coefficients[i] * i;
            }
            return polynomial;
        }

        /// <summary>
        /// Computes the indefinite integral of the polynomial.
        /// </summary>
        /// <returns>The indefinite integral of the polynomial.</returns>
        public readonly Polynomial Integrate()
        {
            Polynomial result = new(degree + 1);
            result[0] = 0;
            for (uint i = 1; i <= degree; i++)
            {
                result[i] = coefficients[i - 1] / i;
            }
            return result;
        }

        /// <summary>
        /// Scales the polynomial by the specified scalar value.
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>The scaled polynomial.</returns>
        public readonly Polynomial Scale(float scalar)
        {
            Polynomial result = new(degree);
            for (uint i = 0; i < degree; i++)
            {
                result[i] = coefficients[i] * scalar;
            }
            return result;
        }

        /// <summary>
        /// Adds two polynomials together.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>The sum of the two polynomials.</returns>
        public static Polynomial Add(Polynomial p1, Polynomial p2)
        {
            uint maxDegree = Math.Max(p1.Degree, p2.Degree);
            Polynomial result = new(maxDegree);
            for (uint i = 0; i < maxDegree; i++)
            {
                float coef1 = i < p1.Degree ? p1[i] : 0;
                float coef2 = i < p2.Degree ? p2[i] : 0;
                result[i] = coef1 + coef2;
            }
            return result;
        }

        /// <summary>
        /// Subtracts one polynomial from another.
        /// </summary>
        /// <param name="p1">The polynomial to subtract from.</param>
        /// <param name="p2">The polynomial to subtract.</param>
        /// <returns>The result of subtracting the second polynomial from the first.</returns>
        public static Polynomial Subtract(Polynomial p1, Polynomial p2)
        {
            uint maxDegree = Math.Max(p1.Degree, p2.Degree);
            Polynomial result = new(maxDegree);
            for (uint i = 0; i < maxDegree; i++)
            {
                float coef1 = i < p1.Degree ? p1[i] : 0;
                float coef2 = i < p2.Degree ? p2[i] : 0;
                result[i] = coef1 - coef2;
            }
            return result;
        }

        /// <summary>
        /// Multiplies two polynomials.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>The product of the two polynomials.</returns>
        public static Polynomial Multiply(Polynomial p1, Polynomial p2)
        {
            uint degree = p1.Degree + p2.Degree - 1;
            Polynomial result = new(degree);
            for (uint i = 0; i < p1.Degree; i++)
            {
                for (uint j = 0; j < p2.Degree; j++)
                {
                    result[i + j] += p1[i] * p2[j];
                }
            }
            return result;
        }

        /// <summary>
        /// Divides one polynomial by another and returns the quotient and remainder.
        /// </summary>
        /// <param name="dividend">The polynomial to be divided.</param>
        /// <param name="divisor">The polynomial by which to divide.</param>
        /// <param name="remainder">The remainder after division.</param>
        /// <returns>The quotient of the division.</returns>
        public static Polynomial Divide(Polynomial dividend, Polynomial divisor, out Polynomial remainder)
        {
            uint dividendDegree = dividend.Degree;
            uint divisorDegree = divisor.Degree;
            if (divisorDegree == 0 || (divisorDegree == 1 && divisor[0] == 0))
                throw new DivideByZeroException("Polynomial division by zero.");
            if (dividendDegree < divisorDegree)
            {
                remainder = dividend;
                return new Polynomial(1);
            }
            uint quotientDegree = dividendDegree - divisorDegree + 1;
            Polynomial quotient = new(quotientDegree);
            Polynomial rem = new(dividendDegree);
            for (uint i = 0; i < dividendDegree; i++)
                rem[i] = dividend[i];
            for (int i = (int)dividendDegree - 1; i >= (int)divisorDegree - 1; i--)
            {
                float coef = rem[i] / divisor[divisorDegree - 1];
                quotient[i - (int)divisorDegree + 1] = coef;
                for (int j = 0; j < divisorDegree; j++)
                    rem[i - j] -= coef * divisor[divisorDegree - 1 - (uint)j];
            }
            remainder = new(divisorDegree - 1);
            for (uint i = 0; i < divisorDegree - 1; i++)
                remainder[i] = rem[i];
            return quotient;
        }

        /// <summary>
        /// Releases the memory allocated for the polynomial.
        /// </summary>
        public void Release()
        {
            if (coefficients == null) return;
            Marshal.FreeHGlobal((nint)coefficients);
            coefficients = null;
            degree = 0;
        }
    }
}