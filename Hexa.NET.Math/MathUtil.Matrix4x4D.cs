namespace Hexa.NET.Mathematics
{
    using System;
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
        public static Matrix4x4D RotationYawPitchRoll(double yaw, double pitch, double roll)
        {
            QuaternionD quaternion = QuaternionD.CreateFromYawPitchRoll(yaw, pitch, roll);
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
        public static Matrix4x4D CreateTransform(Vector3D pos, Vector3D rotation, Vector3D scale)
        {
            return Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix4x4D.CreateTranslation(pos);
        }

        /// <summary>
        /// Creates a transformation matrix from position, rotation, and uniform scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="rotation">The rotation vector (in radians).</param>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4D CreateTransform(Vector3D pos, Vector3D rotation, double scale)
        {
            return Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix4x4D.CreateTranslation(pos);
        }

        /// <summary>
        /// Creates a 4x4 transformation matrix from a position, rotation, and uniform scale.
        /// </summary>
        /// <param name="position">The translation vector representing the position.</param>
        /// <param name="rotation">The quaternion representing the rotation.</param>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The 4x4 transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4D CreateTransform(Vector3D position, QuaternionD rotation, double scale)
        {
            return Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateFromQuaternion(rotation) * Matrix4x4D.CreateTranslation(position);
        }

        /// <summary>
        /// Creates a 4x4 transformation matrix from a position, rotation, and non-uniform scale.
        /// </summary>
        /// <param name="position">The translation vector representing the position.</param>
        /// <param name="rotation">The quaternion representing the rotation.</param>
        /// <param name="scale">The vector representing non-uniform scaling factors along the x, y, and z axes.</param>
        /// <returns>The 4x4 transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4D CreateTransform(Vector3D position, QuaternionD rotation, Vector3D scale)
        {
            return Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateFromQuaternion(rotation) * Matrix4x4D.CreateTranslation(position);
        }

        /// <summary>
        /// Creates a transformation matrix from position and uniform scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4D CreateTransform(Vector3D pos, double scale)
        {
            return Matrix4x4D.CreateTranslation(pos) * Matrix4x4D.CreateScale(scale);
        }

        /// <summary>
        /// Creates a transformation matrix from position and non-uniform scale.
        /// </summary>
        /// <param name="pos">The position vector.</param>
        /// <param name="scale">The non-uniform scale vector.</param>
        /// <returns>The transformation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4D CreateTransform(Vector3D pos, Vector3D scale)
        {
            return Matrix4x4D.CreateTranslation(pos) * Matrix4x4D.CreateScale(scale);
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
        public static Matrix4x4D BillboardLH(Vector3D objectPosition, Vector3D cameraPosition, Vector3D cameraUpVector, Vector3D cameraForwardVector)
        {
            Vector3D crossed;
            Vector3D final;
            Vector3D difference = cameraPosition - objectPosition;

            double lengthSq = difference.LengthSquared();
            if (lengthSq == 0)
            {
                difference = -cameraForwardVector;
            }
            else
            {
                difference *= (double)(1.0 / Math.Sqrt(lengthSq));
            }

            crossed = Vector3D.Cross(cameraUpVector, difference);
            crossed = Vector3D.Normalize(crossed);
            final = Vector3D.Cross(difference, crossed);

            Matrix4x4D result = new();
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
        public static Matrix4x4D RotationQuaternion(QuaternionD rotation)
        {
            double xx = rotation.X * rotation.X;
            double yy = rotation.Y * rotation.Y;
            double zz = rotation.Z * rotation.Z;
            double xy = rotation.X * rotation.Y;
            double zw = rotation.Z * rotation.W;
            double zx = rotation.Z * rotation.X;
            double yw = rotation.Y * rotation.W;
            double yz = rotation.Y * rotation.Z;
            double xw = rotation.X * rotation.W;

            Matrix4x4D result = Matrix4x4D.Identity;
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
        public static Matrix4x4D LookAtLH(Vector3D eye, Vector3D target, Vector3D up)
        {
            Vector3D zAxis = Vector3D.Normalize(Vector3D.Subtract(target, eye));
            Vector3D xAxis = Vector3D.Normalize(Vector3D.Cross(up, zAxis));
            Vector3D yAxis = Vector3D.Cross(zAxis, xAxis);

            Matrix4x4D result = Matrix4x4D.Identity;
            result.M11 = xAxis.X; result.M21 = xAxis.Y; result.M31 = xAxis.Z;
            result.M12 = yAxis.X; result.M22 = yAxis.Y; result.M32 = yAxis.Z;
            result.M13 = zAxis.X; result.M23 = zAxis.Y; result.M33 = zAxis.Z;

            result.M41 = Vector3D.Dot(xAxis, eye);
            result.M42 = Vector3D.Dot(yAxis, eye);
            result.M43 = Vector3D.Dot(zAxis, eye);

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
        public static Matrix4x4D PerspectiveFovLH(double fov, double aspect, double zNear, double zFar)
        {
            double yScale = (double)(1.0f / Math.Tan(fov * 0.5f));
            double q = zFar / (zFar - zNear);

            Matrix4x4D result = new();
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
        public static Matrix4x4D OrthoLH(double width, double height, double zNear, double zFar)
        {
            double halfWidth = width * 0.5f;
            double halfHeight = height * 0.5f;

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
        public static Matrix4x4D OrthoOffCenterLH(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            double zRange = 1.0f / (zFar - zNear);

            Matrix4x4D result = Matrix4x4D.Identity;
            result.M11 = 2.0f / (right - left);
            result.M22 = 2.0f / (top - bottom);
            result.M33 = zRange;
            result.M41 = (left + right) / (left - right);
            result.M42 = (top + bottom) / (bottom - top);
            result.M43 = -zNear * zRange;
            return result;
        }

        /// <summary>
        /// Gets the column of a <see cref="Matrix4x4D"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4x4D"/>.</param>
        /// <param name="index">The column index.</param>
        /// <returns>The column.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector4D GetColumn(this Matrix4x4D matrix, int index)
        {
            if (index < 0 || index > 3)
                throw new ArgumentOutOfRangeException(nameof(index), "Column index must be between 0 and 3.");

            // Unsafe pointer to the first element of the matrix
            double* m = (double*)&matrix;

            Vector4D column;
            column.X = m[index + 0 * 4];
            column.Y = m[index + 1 * 4];
            column.Z = m[index + 2 * 4];
            column.W = m[index + 3 * 4];

            return column;
        }

        /// <summary>
        /// Gets the row of a <see cref="Matrix4x4D"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4x4D"/>.</param>
        /// <param name="index">The row index.</param>
        /// <returns>The row.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector4D GetRow(this Matrix4x4D matrix, int index)
        {
            if (index < 0 || index > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Row index must be between 0 and 3.");
            }

            // Unsafe pointer to the first element of the matrix
            double* m = (double*)&matrix;

            Vector4D row;
            row.X = m[index * 4 + 0];
            row.Y = m[index * 4 + 1];
            row.Z = m[index * 4 + 2];
            row.W = m[index * 4 + 3];
            return row;
        }
    }
}
