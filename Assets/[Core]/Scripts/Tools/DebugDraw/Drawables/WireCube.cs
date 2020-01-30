using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class WireCube : Cube
    {
        protected override void OnDraw()
        {
            Gizmos.DrawWireCube(Position, Scale);
        }

    }
}
