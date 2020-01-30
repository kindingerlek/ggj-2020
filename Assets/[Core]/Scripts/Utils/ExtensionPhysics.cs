using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Physics
{
    public static class Physics
    {
        public static Vector3 SurfaceNormal(this ContactPoint contactpoint, Collider collider)
        {
            Vector3 point = contactpoint.point;
            Vector3 dir = -contactpoint.normal;

            point -= dir;
            RaycastHit hitInfo;

            if (collider.Raycast(new Ray(point, dir), out hitInfo, 2))
            {
                var normal = hitInfo.normal;

                return normal;
            }
            return Vector3.zero;

        }

        public static float Volume(this Mesh mesh)
        {
            float volume = 0f;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];

                Vector3 a = p1 - p2;
                Vector3 b = p1 - p3;
                Vector3 c = p1;

                float tetraVol = (Vector3.Dot(a, Vector3.Cross(b, c))) / 6f;

                volume += tetraVol;
            }

            return Mathf.Abs(volume);
        }

        public static float Volume(this MeshCollider meshCollider)
        {
            return meshCollider.sharedMesh.Volume();
        }

        public static float Volume(this BoxCollider boxCollider)
        {
            return boxCollider.size.x * boxCollider.size.y * boxCollider.size.z;
        }

        public static float Volume(this SphereCollider sphereCollider)
        {
            return (4f * Mathf.Pow(sphereCollider.radius, 3f) * Mathf.PI / 3f);
        }
    }
}
