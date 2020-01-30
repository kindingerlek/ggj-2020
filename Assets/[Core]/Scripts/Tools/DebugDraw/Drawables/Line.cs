using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class Line : Drawable
    {
        public Vector3 StartPoint { get; set; }
        public Vector3 EndPoint { get; set; }
        protected override void OnDraw()
        {
            Gizmos.color = Color;
            Gizmos.DrawLine(StartPoint, EndPoint);
        }
    }
}
