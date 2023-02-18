using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Levels/Level")]
public class LevelData : ScriptableObject
{
    public EnemiesWaveSequenceData[] EnemiesWaveSequenceData;
}