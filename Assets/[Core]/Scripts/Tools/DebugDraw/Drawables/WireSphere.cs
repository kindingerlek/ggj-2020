using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class WireSphere : Sphere
    {
        protected override void OnDraw()
        {
            Gizmos.DrawWireSphere(Position, Radius);
        }

    }
}
