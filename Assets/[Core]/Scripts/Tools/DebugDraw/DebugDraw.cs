using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.DebugDraw.Drawables;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Tools.DebugDraw
{
    public class DebugDraw : MonoBehaviour
    {
        private static List<Drawable> pool = new List<Drawable>();
        private static DebugDraw instance;

        public static Drawable Line(Vector3 startPoint, Vector3 endPoint, float lifeTime = 0)
        {
            var e = Get<Line>();
            e.StartPoint = startPoint;
            e.EndPoint = endPoint;
            e.LifeTime = lifeTime;
            return e;
        }

        public static Drawable Arrow(Vector3 startPoint, Vector3 direction, float lifeTime = 0)
        {
            var e = Get<Arrow>();
            e.StartPosition = startPoint;
            e.Direction = direction;
            e.LifeTime = lifeTime;
            return e;
        }
        public static void Flag() { }
        public static Drawable Marker(Vector3 Position, float scale = 1, float lifeTime = 0)
        {
            var e = Get<Marker>();
            e.Position = Position;
            e.Scale = scale;
            e.LifeTime = lifeTime;
            return e;
        }
        public static Drawable WireSphere(Vector3 position, float radius = 1, float lifeTime = 0)
        {
            var e = Get<WireSphere>();
            e.Position = position;
            e.Radius = radius;
            e.LifeTime = lifeTime;
            return e;
        }
        public static Drawable Sphere(Vector3 position, float radius = 1, float lifeTime = 0)
        {
            var e = Get<Sphere>();
            e.Position = position;
            e.Radius = radius;
            e.LifeTime = lifeTime;
            return e;
        }
        public static Drawable Cube(Vector3 position, Vector3 scale, float lifeTime = 0)
        {
            var e = Get<Cube>();
            e.Position = position;
            e.Scale = scale;
            e.LifeTime = lifeTime;
            return e;
        }
        public static Drawable WireCube(Vector3 position, Vector3 scale, float lifeTime = 0)
        {
            var e = Get<WireCube>();
            e.Position = position;
            e.Scale = scale;
            e.LifeTime = lifeTime;
            return e;
        }
        public static void Clear()
        {
            pool = new List<Drawable>();
        }

        public void Update()
        {
            foreach (var drawableElement in pool)
            {
                if (!drawableElement.Active)
                    continue;

                drawableElement.Update();
            }
        }

        static T Get<T>()
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<DebugDraw>() ??
                            new GameObject("Debug Draw Instance").AddComponent<DebugDraw>();
            }

            var tmp = pool.FirstOrDefault(x => (x.GetType() == typeof(T)) && !x.Active);
            if (tmp == null)
            {
                tmp = (Drawable)Activator.CreateInstance(typeof(T));
                pool.Add(tmp);
            }

            tmp.ResetState();

            return (T) Convert.ChangeType(tmp, typeof(T));
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            foreach (var drawableElement in pool)
            {
                if (!drawableElement.Active)
                    continue;

                drawableElement.Draw();
            }
        }
    }
}
