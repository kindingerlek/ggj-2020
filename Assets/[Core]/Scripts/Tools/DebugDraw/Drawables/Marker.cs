using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class Marker : Drawable
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; }

        protected override void OnDraw()
        {
            Gizmos.color = Color;

            Gizmos.DrawLine(Position + (Vector3.down * Scale * .5f),Position + (Vector3.up * Scale *.5f));
            Gizmos.DrawLine(Position + (Vector3.left * Scale * .5f), Position + (Vector3.right * Scale * .5f));
            Gizmos.DrawLine(Position + (Vector3.forward * Scale * .5f), Position + (Vector3.back * Scale * .5f));
        }


    }
}
