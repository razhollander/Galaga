using PathCreation;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    [CreateAssetMenu(fileName = "EnemyPaths", menuName = "Game/Enemies/EnemyPaths")]
    public class EnemyPathsData : ScriptableObject
    {
        public EnemyDataScriptableObject Enemy;
        public PathCreator EnterPath;
        public PathCreator ExitPath;
    }
}

