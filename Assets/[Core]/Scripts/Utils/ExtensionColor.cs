using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Colors
{
    public static class ExtensionColor
    {
        public static int FindInList(this Color key, Color[] array, float precision)
        {
            Color[] color = array;
            Vector3[] channelAccuracy = new Vector3[array.Length];
            float[] accuracy = new float[array.Length];

            precision = Mathf.Clamp01(precision);

            int moreAccuratedID = 0;

            for (int i = 0; i < array.Length; i++)
            {
                accuracy[i] = 0;

                // Calculate accuracy for each channel

                channelAccuracy[i][0] = 1f - Mathf.Abs(key[0] - color[i][0]);
                channelAccuracy[i][1] = 1f - Mathf.Abs(key[1] - color[i][1]);
                channelAccuracy[i][2] = 1f - Mathf.Abs(key[2] - color[i][2]);

                accuracy[i] = (channelAccuracy[i][0] + channelAccuracy[i][1] + channelAccuracy[i][2]) / 3f;

                if (accuracy[i] > accuracy[moreAccuratedID] && accuracy[i] > precision)
                    moreAccuratedID = i;
            }

            return moreAccuratedID;
        }
    }
}
