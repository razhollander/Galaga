using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsSettings", menuName = "Game/Levels/LevelsSettings")]
public class LevelsScriptableObject : ScriptableObject
{
    public List<LevelScriptableObject> LevelsByOrder;
}
