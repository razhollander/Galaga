using System;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    [CreateAssetMenu(fileName = "EnemiesWaveSequence", menuName = "Game/Enemies/EnemiesWaveSequence")]
    public class EnemiesWaveSequenceData : SerializedScriptableObject
    {
        [TableMatrix(SquareCells = true, DrawElementMethod = "DrawElement")]
        public EnemySequenceData[,] EnemiesGrid = new EnemySequenceData[5,5];
        public float SpaceBetweenColumns;
        public float SpaceBetweenRows;
        public float CellSize;

#if UNITY_EDITOR
        [Button]
        private void ResizeEnemiesGrid(int x, int y)
        {
            var newEnemiesGrid = new EnemySequenceData[x, y];
            var previousX = EnemiesGrid.GetLength(0);
            var previousY = EnemiesGrid.GetLength(1);
            
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (i < previousX && j < previousY)
                    {
                        newEnemiesGrid[i, j] = EnemiesGrid[i, j];
                    }
                    else
                    {
                        newEnemiesGrid[i, j] = new EnemySequenceData();
                    }
                }
            }
            
            EnemiesGrid = newEnemiesGrid;
        }

        public static EnemySequenceData DrawElement(Rect rect, EnemySequenceData enemySequenceData)
        {
            DrawLabels(rect);
            return GetAndDrawInputs(rect, enemySequenceData);
        }

        private static void DrawLabels(Rect rect)
        {
            var enemyPathsRect = new Rect(rect.x, rect.y, rect.width * 0.5f, rect.height / 3); 
            EditorGUI.LabelField(enemyPathsRect, "Enemy Paths:");
            var secondsBeforeEnterRect = new Rect(rect.x, rect.y+rect.height / 3, rect.width * 0.8f, rect.height / 3); 
            EditorGUI.LabelField(secondsBeforeEnterRect, "Seconds Before Enter:");
            var secondsInIdleRect = new Rect(rect.x, rect.y+rect.height * 2/ 3, rect.width * 0.8f, rect.height / 3); 
            EditorGUI.LabelField(secondsInIdleRect, "Seconds In Idle:");
        }
        
        private static EnemySequenceData GetAndDrawInputs(Rect rect, EnemySequenceData enemySequenceData)
        {
            var objRect = new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height / 3);
            var enemyPathsDataObject = EditorGUI.ObjectField(objRect, enemySequenceData.EnemyPathsData, typeof(EnemyPathsData));

            if (enemyPathsDataObject != null)
            {
                enemySequenceData.EnemyPathsData = (EnemyPathsData) enemyPathsDataObject;
            }
            
            var secondsBeforeEnterRect = new Rect(rect.x+rect.width*0.8f, rect.y+rect.height / 3, rect.width*0.2f, rect.height / 3);
            enemySequenceData.SecondsBeforeEnter = EditorGUI.FloatField(secondsBeforeEnterRect, enemySequenceData.SecondsBeforeEnter);
            var secondsInIdleRect = new Rect(rect.x+rect.width*0.8f, rect.y+rect.height * 2/ 3, rect.width*0.2f, rect.height / 3);
            enemySequenceData.SecondsInIdle = EditorGUI.FloatField(secondsInIdleRect, enemySequenceData.SecondsInIdle);
            return enemySequenceData;
        }
#endif
    }
    
    [Serializable]
    public class EnemySequenceData
    {
        public EnemyPathsData EnemyPathsData;
        public float SecondsBeforeEnter;
        public float SecondsInIdle;
    }
}