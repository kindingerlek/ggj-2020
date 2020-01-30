using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class Sphere : Drawable
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        protected override void OnDraw()
        {
            Gizmos.DrawWireSphere(Position, Radius);
        }

    }
}
