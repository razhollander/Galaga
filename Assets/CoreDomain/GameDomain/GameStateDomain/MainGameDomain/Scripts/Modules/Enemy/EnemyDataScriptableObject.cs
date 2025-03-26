using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Game/Enemies/Enemy")]
    public class EnemyDataScriptableObject : ScriptableObject
    {
        public int Score;
        public string EnemyAssetName;
    }
}