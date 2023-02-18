using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "Game/Levels/Levels")]
public class LevelsData : ScriptableObject
{
    public LevelData[] LevelsByOrder;
}
