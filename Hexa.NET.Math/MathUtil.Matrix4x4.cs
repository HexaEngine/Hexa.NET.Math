namespace Hexa.NET.Mathematics
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    public static unsafe partial class MathUtil
    {
        /// <summary>
        /// Creates a rotation matrix from yaw, pitch, and roll angles.
        /// </summary>
        /// <param name="yaw">The yaw angle (in radians).</param>
        /// <param name="pitch">The pitch angle (in radians).</param>
        /// <param name="roll">The roll angle (in radians).</param>
        /// <returns>The rotation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            return RotationQuaternion(quaternion);
        }

        /// <summary>
        /// Creates a transformation matrix from position, rotation, and scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="rotation">The rotation vector (in radians).</param>
        /// <param name="scale">The scale vector.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransform(Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            return Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix4x4.CreateTranslation(pos);
        }

        /// <summary>
        /// Creates a transformation matrix from position, rotation, and uniform scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="rotation">The rotation vector (in radians).</param>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransform(Vector3 pos, Vector3 rotation, float scale)
        {
            return Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix4x4.CreateTranslation(pos);
        }

        /// <summary>
        /// Creates a 4x4 transformation matrix from a position, rotation, and uniform scale.
        /// </summary>
        /// <param name="position">The translation vector representing the position.</param>
        /// <param name="rotation">The quaternion representing the rotation.</param>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The 4x4 transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransform(Vector3 position, Quaternion rotation, float scale)
        {
            return Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        }

        /// <summary>
        /// Creates a 4x4 transformation matrix from a position, rotation, and non-uniform scale.
        /// </summary>
        /// <param name="position">The translation vector representing the position.</param>
        /// <param name="rotation">The quaternion representing the rotation.</param>
        /// <param name="scale">The vector representing non-uniform scaling factors along the x, y, and z axes.</param>
        /// <returns>The 4x4 transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            return Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        }

        /// <summary>
        /// Creates a transformation matrix from position and uniform scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransform(Vector3 pos, float scale)
        {
            return Matrix4x4.CreateTranslation(pos) * Matrix4x4.CreateScale(scale);
        }

        /// <summary>
        /// Creates a transformation matrix from position and non-uniform scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="scale">The non-uniform scale vector.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransform(Vector3 pos, Vector3 scale)
        {
            return Matrix4x4.CreateTranslation(pos) * Matrix4x4.CreateScale(scale);
        }

        /// <summary>
        /// Creates a left-handed spherical billboard that rotates around a specified object position.
        /// </summary>
        /// <param name="objectPosition">The position of the object around which the billboard will rotate.</param>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraForwardVector">The forward vector of the camera.</param>
        /// <returns>When the method completes, contains the created billboard matrix.</returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 BillboardLH(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector)
        {
            Vector3 crossed;
            Vector3 final;
            Vector3 difference = cameraPosition - objectPosition;

            float lengthSq = difference.LengthSquared();
            if (lengthSq == 0)
            {
                difference = -cameraForwardVector;
            }
            else
            {
                difference *= (float)(1.0 / Math.Sqrt(lengthSq));
            }

            crossed = Vector3.Cross(cameraUpVector, difference);
            crossed = Vector3.Normalize(crossed);
            final = Vector3.Cross(difference, crossed);

            Matrix4x4 result = new();
            result.M11 = crossed.X;
            result.M12 = crossed.Y;
            result.M13 = crossed.Z;
            result.M14 = 0.0f;
            result.M21 = final.X;
            result.M22 = final.Y;
            result.M23 = final.Z;
            result.M24 = 0.0f;
            result.M31 = difference.X;
            result.M32 = difference.Y;
            result.M33 = difference.Z;
            result.M34 = 0.0f;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1.0f;

            return result;
        }

        /// <summary>
        /// Creates a rotation matrix from the specified quaternion.
        /// </summary>
        /// <param name="rotation">The input quaternion representing the rotation.</param>
        /// <returns>The rotation matrix based on the provided quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 RotationQuaternion(Quaternion rotation)
        {
            float xx = rotation.X * rotation.X;
            float yy = rotation.Y * rotation.Y;
            float zz = rotation.Z * rotation.Z;
            float xy = rotation.X * rotation.Y;
            float zw = rotation.Z * rotation.W;
            float zx = rotation.Z * rotation.X;
            float yw = rotation.Y * rotation.W;
            float yz = rotation.Y * rotation.Z;
            float xw = rotation.X * rotation.W;

            Matrix4x4 result = Matrix4x4.Identity;
            result.M11 = 1.0f - 2.0f * (yy + zz);
            result.M12 = 2.0f * (xy + zw);
            result.M13 = 2.0f * (zx - yw);
            result.M21 = 2.0f * (xy - zw);
            result.M22 = 1.0f - 2.0f * (zz + xx);
            result.M23 = 2.0f * (yz + xw);
            result.M31 = 2.0f * (zx + yw);
            result.M32 = 2.0f * (yz - xw);
            result.M33 = 1.0f - 2.0f * (yy + xx);
            return result;
        }

        /// <summary>
        /// Creates a left-handed view matrix based on the specified camera position, target point, and up direction.
        /// </summary>
        /// <param name="eye">The position of the camera.</param>
        /// <param name="target">The target point that the camera is looking at.</param>
        /// <param name="up">The up direction of the camera.</param>
        /// <returns>The left-handed view matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 LookAtLH(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 zAxis = Vector3.Normalize(Vector3.Subtract(target, eye));
            Vector3 xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis));
            Vector3 yAxis = Vector3.Cross(zAxis, xAxis);

            Matrix4x4 result = Matrix4x4.Identity;
            result.M11 = xAxis.X; result.M21 = xAxis.Y; result.M31 = xAxis.Z;
            result.M12 = yAxis.X; result.M22 = yAxis.Y; result.M32 = yAxis.Z;
            result.M13 = zAxis.X; result.M23 = zAxis.Y; result.M33 = zAxis.Z;

            result.M41 = Vector3.Dot(xAxis, eye);
            result.M42 = Vector3.Dot(yAxis, eye);
            result.M43 = Vector3.Dot(zAxis, eye);

            result.M41 = -result.M41;
            result.M42 = -result.M42;
            result.M43 = -result.M43;
            return result;
        }

        /// <summary>
        /// Creates a left-handed perspective projection matrix based on the field of view, aspect ratio, and depth range.
        /// </summary>
        /// <param name="fov">The field of view in radians.</param>
        /// <param name="aspect">The aspect ratio of the view.</param>
        /// <param name="zNear">The minimum depth of the view frustum.</param>
        /// <param name="zFar">The maximum depth of the view frustum.</param>
        /// <returns>The perspective projection matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 PerspectiveFovLH(float fov, float aspect, float zNear, float zFar)
        {
            float yScale = (float)(1.0f / Math.Tan(fov * 0.5f));
            float q = zFar / (zFar - zNear);

            Matrix4x4 result = new();
            result.M11 = yScale / aspect;
            result.M22 = yScale;
            result.M33 = q;
            result.M34 = 1.0f;
            result.M43 = -q * zNear;
            return result;
        }

        /// <summary>
        /// Creates a left-handed orthographic projection matrix.
        /// </summary>
        /// <param name="width">The width of the view volume.</param>
        /// <param name="height">The height of the view volume.</param>
        /// <param name="zNear">The minimum depth of the view volume.</param>
        /// <param name="zFar">The maximum depth of the view volume.</param>
        /// <returns>The orthographic projection matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 OrthoLH(float width, float height, float zNear, float zFar)
        {
            float halfWidth = width * 0.5f;
            float halfHeight = height * 0.5f;

            return OrthoOffCenterLH(-halfWidth, halfWidth, -halfHeight, halfHeight, zNear, zFar);
        }

        /// <summary>
        /// Creates a left-handed, customized orthographic projection matrix.
        /// </summary>
        /// <param name="left">Minimum x-value of the viewing volume.</param>
        /// <param name="right">Maximum x-value of the viewing volume.</param>
        /// <param name="bottom">Minimum y-value of the viewing volume.</param>
        /// <param name="top">Maximum y-value of the viewing volume.</param>
        /// <param name="zNear">Minimum z-value of the viewing volume.</param>
        /// <param name="zFar">Maximum z-value of the viewing volume.</param>
        /// <returns>When the method completes, contains the created projection matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 OrthoOffCenterLH(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            float zRange = 1.0f / (zFar - zNear);

            Matrix4x4 result = Matrix4x4.Identity;
            result.M11 = 2.0f / (right - left);
            result.M22 = 2.0f / (top - bottom);
            result.M33 = zRange;
            result.M41 = (left + right) / (left - right);
            result.M42 = (top + bottom) / (bottom - top);
            result.M43 = -zNear * zRange;
            return result;
        }

        /// <summary>
        /// Gets the column of a <see cref="Matrix4x4"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4x4"/>.</param>
        /// <param name="index">The column index.</param>
        /// <returns>The column.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector4 GetColumn(this Matrix4x4 matrix, int index)
        {
            if (index < 0 || index > 3)
                throw new ArgumentOutOfRangeException(nameof(index), "Column index must be between 0 and 3.");

            // Unsafe pointer to the first element of the matrix
            float* m = (float*)&matrix;

            Vector4 column;
            column.X = m[index + 0 * 4];
            column.Y = m[index + 1 * 4];
            column.Z = m[index + 2 * 4];
            column.W = m[index + 3 * 4];

            return column;
        }

        /// <summary>
        /// Gets the row of a <see cref="Matrix4x4"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4x4"/>.</param>
        /// <param name="index">The row index.</param>
        /// <returns>The row.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector4 GetRow(this Matrix4x4 matrix, int index)
        {
            if (index < 0 || index > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Row index must be between 0 and 3.");
            }

            // Unsafe pointer to the first element of the matrix
            float* m = (float*)&matrix;

            Vector4 row;
            row.X = m[index * 4 + 0];
            row.Y = m[index * 4 + 1];
            row.Z = m[index * 4 + 2];
            row.W = m[index * 4 + 3];
            return row;
        }
    }
}
