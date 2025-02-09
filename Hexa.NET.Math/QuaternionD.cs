namespace Hexa.NET.Mathematics
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

#if NET5_0_OR_GREATER
    using System.Runtime.Intrinsics;
#endif

    /// <summary>Represents a vector that is used to encode three-dimensional physical rotations.</summary>
    /// <remarks>The <see cref="QuaternionD" /> structure is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where:
    /// <c>w = cos(theta/2)</c></remarks>
    public struct QuaternionD : IEquatable<QuaternionD>
    {
        /// <summary>The X value of the vector component of the quaternion.</summary>
        public double X;

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public double Y;

        /// <summary>The Z value of the vector component of the quaternion.</summary>
        public double Z;

        /// <summary>The rotation component of the quaternion.</summary>
        public double W;

        internal const int Count = 4;

        /// <summary>Constructs a quaternion from the specified components.</summary>
        /// <param name="x">The value to assign to the X component of the quaternion.</param>
        /// <param name="y">The value to assign to the Y component of the quaternion.</param>
        /// <param name="z">The value to assign to the Z component of the quaternion.</param>
        /// <param name="w">The value to assign to the W component of the quaternion.</param>
        public QuaternionD(double x, double y, double z, double w)
        {
#if NET5_0_OR_GREATER
            this = Create(x, y, z, w);
#else
            X = x;
            Y = y;
            Z = z;
            W = w;
#endif
        }

        /// <summary>Creates a quaternion from the specified vector and rotation parts.</summary>
        /// <param name="vectorPart">The vector part of the quaternion.</param>
        /// <param name="scalarPart">The rotation part of the quaternion.</param>
        public QuaternionD(Vector3D vectorPart, double scalarPart)
        {
#if NET5_0_OR_GREATER
            this = Create(vectorPart, scalarPart);
#else
            X = vectorPart.X;
            Y = vectorPart.Y;
            Z = vectorPart.Z;
            W = scalarPart;
#endif
        }

        /// <summary>Gets a quaternion that represents a zero.</summary>
        /// <value>A quaternion whose values are <c>(0, 0, 0, 0)</c>.</value>
        public static Quaternion Zero
        {
            get => default;
        }

        /// <summary>Gets a quaternion that represents no rotation.</summary>
        /// <value>A quaternion whose values are <c>(0, 0, 0, 1)</c>.</value>
        public static QuaternionD Identity
        {
            get => Create(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public unsafe double this[int index]
        {
#if NET5_0_OR_GREATER
            readonly get
            {
                return this.AsVector256().GetElement(index);
            }
#else
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                }

                return ((double*)Unsafe.AsPointer(ref this))[index];
            }
#endif
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
#if NET5_0_OR_GREATER
                this = this.AsVector256().WithElement(index, value).AsQuaternionD();
#else
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException($"Index must be smaller than {Count} and larger or equals to 0");
                } 
                ((double*)Unsafe.AsPointer(ref this))[index] = value;
#endif
            }
        }

        /// <summary>Gets a value that indicates whether the current instance is the identity quaternion.</summary>
        /// <value><see langword="true" /> if the current instance is the identity quaternion; otherwise, <see langword="false" />.</value>
        /// <altmember cref="Identity"/>
        public readonly bool IsIdentity => this == Identity;

        /// <summary>Adds each element in one quaternion with its corresponding element in a second quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion that contains the summed values of <paramref name="value1" /> and <paramref name="value2" />.</returns>
        /// <remarks>The <see cref="op_Addition" /> method defines the operation of the addition operator for <see cref="QuaternionD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(QuaternionD value1, QuaternionD value2)
        {
#if NET7_0_OR_GREATER
            return (value1.AsVector256() + value2.AsVector256()).AsQuaternionD();
#else
            return new(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z, value1.W + value2.W);
#endif
        }

        /// <summary>Divides one quaternion by a second quaternion.</summary>
        /// <param name="value1">The dividend.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The quaternion that results from dividing <paramref name="value1" /> by <paramref name="value2" />.</returns>
        /// <remarks>The <see cref="op_Division" /> method defines the division operation for <see cref="QuaternionD" /> objects.</remarks>
        public static QuaternionD operator /(QuaternionD value1, QuaternionD value2) => value1 * Inverse(value2);

        /// <summary>Returns a value that indicates whether two quaternions are equal.</summary>
        /// <param name="value1">The first quaternion to compare.</param>
        /// <param name="value2">The second quaternion to compare.</param>
        /// <returns><see langword="true" /> if the two quaternions are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two quaternions are equal if each of their corresponding components is equal.
        /// The <see cref="op_Equality" /> method defines the operation of the equality operator for <see cref="Quaternion" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(QuaternionD value1, QuaternionD value2)
        {
#if NET7_0_OR_GREATER
            return value1.AsVector256() == value2.AsVector256();
#else
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
#endif
        }

        /// <summary>Returns a value that indicates whether two quaternions are not equal.</summary>
        /// <param name="value1">The first quaternion to compare.</param>
        /// <param name="value2">The second quaternion to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="value1" /> and <paramref name="value2" /> are not equal; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(QuaternionD value1, QuaternionD value2) => !(value1 == value2);

        /// <summary>Returns the quaternion that results from multiplying two quaternions together.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The product quaternion.</returns>
        /// <remarks>The <see cref="Quaternion.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="Quaternion" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(QuaternionD value1, QuaternionD value2)
        {
#if NET9_0_OR_GREATER
            Vector256<double> left = value1.AsVector256();
            Vector256<double> right = value2.AsVector256();

            Vector256<double> result = right * left.GetElement(3);
            result = Vector256.MultiplyAddEstimate(Vector256.Shuffle(right, Vector256.Create(3, 2, 1, 0)) * left.GetElement(0), Vector256.Create(+1.0f, -1.0f, +1.0f, -1.0f), result);
            result = Vector256.MultiplyAddEstimate(Vector256.Shuffle(right, Vector256.Create(2, 3, 0, 1)) * left.GetElement(1), Vector256.Create(+1.0f, +1.0f, -1.0f, -1.0f), result);
            result = Vector256.MultiplyAddEstimate(Vector256.Shuffle(right, Vector256.Create(1, 0, 3, 2)) * left.GetElement(2), Vector256.Create(-1.0f, +1.0f, +1.0f, -1.0f), result);
            return result.AsQuaternionD();
#else
            return new QuaternionD(
                    +value1.W * value2.X + value1.X * value2.W + value1.Y * value2.Z - value1.Z * value2.Y,
                    +value1.W * value2.Y - value1.X * value2.Z + value1.Y * value2.W + value1.Z * value2.X,
                    +value1.W * value2.Z + value1.X * value2.Y - value1.Y * value2.X + value1.Z * value2.W,
                    +value1.W * value2.W - value1.X * value2.X - value1.Y * value2.Y - value1.Z * value2.Z
                );
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(QuaternionD value1, double value2)
        {
#if NET7_0_OR_GREATER
            return (value1.AsVector256() * value2).AsQuaternionD();
#else
            return new(value1.X * value2, value1.Y * value2, value1.Z * value2, value1.W * value2);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(QuaternionD value1, QuaternionD value2)
        {
#if NET7_0_OR_GREATER
            return (value1.AsVector256() - value2.AsVector256()).AsQuaternionD();
#else
            return new(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z, value1.W - value2.W);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(QuaternionD value)
        {
#if NET7_0_OR_GREATER
            return (-value.AsVector256()).AsQuaternionD();
#else
            return new(-value.X, -value.Y, -value.Z, -value.W);
#endif
        }

        /// <summary>Adds each element in one quaternion with its corresponding element in a second quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion that contains the summed values of <paramref name="value1" /> and <paramref name="value2" />.</returns>
        public static QuaternionD Add(QuaternionD value1, QuaternionD value2) => value1 + value2;


        /// <summary>Concatenates two quaternions.</summary>
        /// <param name="value1">The first quaternion rotation in the series.</param>
        /// <param name="value2">The second quaternion rotation in the series.</param>
        /// <returns>A new quaternion representing the concatenation of the <paramref name="value1" /> rotation followed by the <paramref name="value2" /> rotation.</returns>
        public static QuaternionD Concatenate(QuaternionD value1, QuaternionD value2) => value2 * value1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD Conjugate(QuaternionD value)
        {
#if NET7_0_OR_GREATER
            return (value.AsVector256() * Vector256.Create(-1.0f, -1.0f, -1.0f, 1.0f)).AsQuaternionD();
#else
            return new QuaternionD(-value.X, -value.Y, -value.Z, value.W);
#endif
        }

#if NET5_0_OR_GREATER

        internal static QuaternionD Create(double x, double y, double z, double w) => Vector256.Create(x, y, z, w).AsQuaternionD();

        internal static QuaternionD Create(Vector3D vectorPart, double scalarPart) => Vector4D.Create(vectorPart, scalarPart).AsQuaternionD();
#else

        internal static QuaternionD Create(double x, double y, double z, double w) => new(x, y, z, w);

        internal static QuaternionD Create(Vector3D vectorPart, double scalarPart) => new(vectorPart, scalarPart);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD CreateFromAxisAngle(Vector3D axis, double angle)
        {
            // This implementation is based on the DirectX Math Library XMQuaternionRotationNormal method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            (double s, double c) = MathUtil.SinCos(angle * 0.5f);
            return (Vector4D.Create(axis, 1) * Vector4D.Create(Vector3D.Create(s), c)).AsQuaternionD();
        }

        /// <summary>Creates a quaternion from the specified rotation matrix.</summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The newly created quaternion.</returns>
        public static QuaternionD CreateFromRotationMatrix(Matrix4x4D matrix)
        {
            double trace = matrix.M11 + matrix.M22 + matrix.M33;

            QuaternionD q = default;

            if (trace > 0.0f)
            {
                double s = Math.Sqrt(trace + 1.0f);
                q.W = s * 0.5f;
                s = 0.5f / s;
                q.X = (matrix.M23 - matrix.M32) * s;
                q.Y = (matrix.M31 - matrix.M13) * s;
                q.Z = (matrix.M12 - matrix.M21) * s;
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    double s = Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                    double invS = 0.5f / s;
                    q.X = 0.5f * s;
                    q.Y = (matrix.M12 + matrix.M21) * invS;
                    q.Z = (matrix.M13 + matrix.M31) * invS;
                    q.W = (matrix.M23 - matrix.M32) * invS;
                }
                else if (matrix.M22 > matrix.M33)
                {
                    double s = Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                    double invS = 0.5f / s;
                    q.X = (matrix.M21 + matrix.M12) * invS;
                    q.Y = 0.5f * s;
                    q.Z = (matrix.M32 + matrix.M23) * invS;
                    q.W = (matrix.M31 - matrix.M13) * invS;
                }
                else
                {
                    double s = Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                    double invS = 0.5f / s;
                    q.X = (matrix.M31 + matrix.M13) * invS;
                    q.Y = (matrix.M32 + matrix.M23) * invS;
                    q.Z = 0.5f * s;
                    q.W = (matrix.M12 - matrix.M21) * invS;
                }
            }

            return q;
        }


        /// <summary>Creates a new quaternion from the given yaw, pitch, and roll.</summary>
        /// <param name="yaw">The yaw angle, in radians, around the Y axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the X axis.</param>
        /// <param name="roll">The roll angle, in radians, around the Z axis.</param>
        /// <returns>The resulting quaternion.</returns>
        public static QuaternionD CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            (Vector3D sin, Vector3D cos) = Vector3D.SinCos(Vector3D.Create(roll, pitch, yaw) * 0.5f);

            (double sr, double cr) = (sin.X, cos.X);
            (double sp, double cp) = (sin.Y, cos.Y);
            (double sy, double cy) = (sin.Z, cos.Z);

            QuaternionD result;

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;

            return result;
        }

        /// <summary>Divides one quaternion by a second quaternion.</summary>
        /// <param name="value1">The dividend.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The quaternion that results from dividing <paramref name="value1" /> by <paramref name="value2" />.</returns>
        public static Quaternion Divide(Quaternion value1, Quaternion value2) => value1 / value2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(QuaternionD quaternion1, QuaternionD quaternion2)
        {
#if NET7_0_OR_GREATER
            return Vector256.Dot(quaternion1.AsVector256(), quaternion2.AsVector256());
#else
            return quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
#endif
        }

        /// <summary>Returns the inverse of a quaternion.</summary>
        /// <param name="value">The quaternion.</param>
        /// <returns>The inverted quaternion.</returns>
        public static QuaternionD Inverse(QuaternionD value)
        {
            // This implementation is based on the DirectX Math Library XMQuaternionInverse method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            const double Epsilon = 1e-10;



            //  -1   (       a              -v       )
            // q   = ( -------------   ------------- )
            //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )
#if NET8_0_OR_GREATER
            Vector256<double> lengthSquared = Vector256.Create(value.LengthSquared());
            return Vector256.AndNot(  (Conjugate(value).AsVector256() / lengthSquared),       Vector256.LessThanOrEqual(lengthSquared, Vector256.Create(Epsilon))   ).AsQuaternionD();
#else
            double lengthSquared = value.LengthSquared();
            if (lengthSquared <= Epsilon)
                return new QuaternionD(0, 0, 0, 0);

            value = Conjugate(value);

            double invLengthSq = 1.0 / lengthSquared;
            return new(value.X * invLengthSq, value.Y * invLengthSq, value.Z * invLengthSq, value.W * invLengthSq);
#endif
        }

        /// <summary>Performs a linear interpolation between two quaternions based on a value that specifies the weighting of the second quaternion.</summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="amount">The relative weight of <paramref name="quaternion2" /> in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static QuaternionD Lerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount)
        {
#if NET9_0_OR_GREATER
            Vector256<double> q2 = quaternion2.AsVector256();

            q2 = Vector256.ConditionalSelect(
                Vector256.GreaterThanOrEqual(Vector256.Create(Dot(quaternion1, quaternion2)), Vector256<double>.Zero),
                 q2,
                -q2
            );

            Vector256<double> result = Vector256.MultiplyAddEstimate(quaternion1.AsVector256(), Vector256.Create(1.0 - amount), q2 * amount);

            return Normalize(result.AsQuaternionD());
#else
            double dot = Dot(quaternion1, quaternion2);

            if (dot < 0.0)
            {
                quaternion2 = -quaternion2;
            }
            QuaternionD result = new(
                quaternion1.X * (1.0 - amount) + quaternion2.X * amount,
                quaternion1.Y * (1.0 - amount) + quaternion2.Y * amount,
                quaternion1.Z * (1.0 - amount) + quaternion2.Z * amount,
                quaternion1.W * (1.0 - amount) + quaternion2.W * amount
            );

            return Normalize(result);
#endif

        }

        /// <summary>Returns the quaternion that results from multiplying two quaternions together.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The product quaternion.</returns>
        public static QuaternionD Multiply(QuaternionD value1, QuaternionD value2) => value1 * value2;

        /// <summary>Returns the quaternion that results from scaling all the components of a specified quaternion by a scalar factor.</summary>
        /// <param name="value1">The source quaternion.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The scaled quaternion.</returns>
        public static QuaternionD Multiply(QuaternionD value1, double value2) => value1 * value2;

        /// <summary>Reverses the sign of each component of the quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>The negated quaternion.</returns>
        public static QuaternionD Negate(QuaternionD value) => -value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD Normalize(QuaternionD value)
        {
#if NET8_0_OR_GREATER
            return (value.AsVector256() / value.Length()).AsQuaternionD();
#else
            double length = value.Length();
            return new(value.X / length, value.Y / length, value.Z / length, value.W / length);
#endif
        }

        /// <summary>Interpolates between two quaternions, using spherical linear interpolation.</summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="amount">The relative weight of the second quaternion in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static QuaternionD Slerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount)
        {
            const double SlerpEpsilon = 1e-10f;

            double cosOmega = Dot(quaternion1, quaternion2);
            double sign = 1.0f;

            if (cosOmega < 0.0f)
            {
                cosOmega = -cosOmega;
                sign = -1.0f;
            }

            double s1, s2;

            if (cosOmega > (1.0f - SlerpEpsilon))
            {
                // Too close, do straight linear interpolation.
                s1 = 1.0f - amount;
                s2 = amount * sign;
            }
            else
            {
                double omega = Math.Acos(cosOmega);
                double invSinOmega = 1 / Math.Sin(omega);

                s1 = Math.Sin((1.0f - amount) * omega) * invSinOmega;
                s2 = Math.Sin(amount * omega) * invSinOmega * sign;
            }

            return (quaternion1 * s1) + (quaternion2 * s2);
        }

        /// <summary>Subtracts each element in a second quaternion from its corresponding element in a first quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion containing the values that result from subtracting each element in <paramref name="value2" /> from its corresponding element in <paramref name="value1" />.</returns>
        public static Quaternion Subtract(Quaternion value1, Quaternion value2) => value1 - value2;

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Quaternion" /> object and the corresponding components of each matrix are equal.</remarks>
        public override readonly bool Equals(object? obj) => (obj is QuaternionD other) && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another quaternion are equal.</summary>
        /// <param name="other">The other quaternion.</param>
        /// <returns><see langword="true" /> if the two quaternions are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two quaternions are equal if each of their corresponding components is equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(QuaternionD other)
        {
#if NET7_0_OR_GREATER
            return this.AsVector256().Equals(other.AsVector256());
#else
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
#endif
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        /// <summary>Calculates the length of the quaternion.</summary>
        /// <returns>The computed length of the quaternion.</returns>
        public readonly double Length() => Math.Sqrt(LengthSquared());

        /// <summary>Calculates the squared length of the quaternion.</summary>
        /// <returns>The length squared of the quaternion.</returns>
        public readonly double LengthSquared() => Dot(this, this);

        /// <summary>Returns a string that represents this quaternion.</summary>
        /// <returns>The string representation of this quaternion.</returns>
        /// <remarks>The numeric values in the returned string are formatted by using the conventions of the current culture. For example, for the en-US culture, the returned string might appear as <c>{X:1.1 Y:2.2 Z:3.3 W:4.4}</c>.</remarks>
        public override readonly string ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";

        public static implicit operator QuaternionD(Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        public static explicit operator Quaternion(QuaternionD quaternion) => new((float)quaternion.X, (float)quaternion.Y, (float)quaternion.Z, (float)quaternion.W);
    }
}
