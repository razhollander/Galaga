using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Game/Enemies/Enemy")]
    public class EnemyDataScriptableObject : ScriptableObject
    {
        public int Score = 20;
        public string EnemyAssetName = "EnemyBeeSpaceship";
    }
}