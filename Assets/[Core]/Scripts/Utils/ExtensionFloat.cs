using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Float
{
    public static class ExtensionFloat
    {
        public static float Map(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            try
            {
                return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return 0;
            }
        }
    }
}
