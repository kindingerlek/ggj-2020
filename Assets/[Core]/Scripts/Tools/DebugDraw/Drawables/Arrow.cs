using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools.DebugDraw.Drawables
{
    public class Arrow : Drawable
    {
        public enum ArrowHeadType
        {
            ProportionalToLenght,
            AbsoluteSize
        }

        public ArrowHeadType HeadType { get; set; } = ArrowHeadType.ProportionalToLenght;
        public float ArrowHeadAngle { get; set; } = 20.0f;
        public float ArrowHeadLength { get; set; } = 0.25f;
        public Vector3 StartPosition { get; set; }
        public Vector3 Direction { get; set; }

        protected override void OnDraw()
        {
            Gizmos.DrawLine(StartPosition, StartPosition + Direction);

            Vector3 right = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * new Vector3(0, 0, 1);

            var arrowHeadSize =
                HeadType == ArrowHeadType.AbsoluteSize ?
                    ArrowHeadLength : Direction.magnitude * ArrowHeadLength;


            Gizmos.DrawRay(StartPosition + Direction, right * arrowHeadSize);
            Gizmos.DrawRay(StartPosition + Direction, left * arrowHeadSize);
        }
    }
}
