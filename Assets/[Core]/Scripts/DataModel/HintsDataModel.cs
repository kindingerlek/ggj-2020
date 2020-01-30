using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/GameHints")]
public class HintsDataModel : SingletonScriptableObject<HintsDataModel>
{
    [Multiline]
    public List<string> hintList;
}
