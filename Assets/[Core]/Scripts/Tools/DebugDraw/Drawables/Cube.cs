using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class Cube : Drawable
    {
        public Vector3 Position { get; set;}
        public Vector3 Scale { get; set; }

        protected override void OnDraw()
        {
            Gizmos.DrawCube(Position, Scale);
        }

    }
}
