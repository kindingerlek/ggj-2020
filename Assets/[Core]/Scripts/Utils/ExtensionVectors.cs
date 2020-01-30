using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Vectors
{
    public static class ExtensionVector
    {
        public static float Area(this Vector2 v)
        {
            return v.x * v.y;
        }

        public static float Area(this Vector2Int v)
        {
            return v.x * v.y;
        }

        public static float Volume(this Vector3 v)
        {
            return v.x * v.y * v.z;
        }

        public static float Volume(this Vector3Int v)
        {
            return v.x * v.y * v.z;
        }

        public static Vector2 CartasianToPolar(this Vector2 value)
        {
            float x = value.x;
            float y = value.y;

            float sx = x * Mathf.Sqrt(1.0f - y * y * 0.5f);
            float sy = y * Mathf.Sqrt(1.0f - x * x * 0.5f);

            return new Vector2(sx, sy);
        }

        public static Vector3 CartasianToPolar(this Vector3 value)
        {
            float x = value.x;
            float y = value.y;
            float z = value.z;

            float sx = x * Mathf.Sqrt(1.0f - y * y * 0.5f - z * z * 0.5f + y * y * z * z / 3.0f);
            float sy = y * Mathf.Sqrt(1.0f - z * z * 0.5f - x * x * 0.5f + z * z * x * x / 3.0f);
            float sz = z * Mathf.Sqrt(1.0f - x * x * 0.5f - y * y * 0.5f + x * x * y * y / 3.0f);

            return new Vector3(sx, sy, sz);
        }

        //Returns the rotated Vector3 using a Quaterion
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion angle)
        {
            return angle * (point - pivot) + pivot;
        }
        //Returns the rotated Vector3 using Euler
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 euler)
        {
            return RotateAroundPivot(point, pivot, Quaternion.Euler(euler));
        }
        //Returns the rotated Vector3 using Euler
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 axis, float angle)
        {
            return RotateAroundPivot(point, pivot, Quaternion.Euler(axis * angle));
        }

        // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - unit vector in direction of line
        // pointOnLine - a point on the line (allowing us to define an actual line in space)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnLine(
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }
    }
}