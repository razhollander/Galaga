using System;
using UnityEngine;

namespace Features.MainGameScreen.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        #region --- Members ---

        public SpriteRenderer EnemySpriteRenderer;
        private Action<EnemyView> _onHit;

        #endregion


        #region --- Properties ---

        public int ColumnIndex { get; private set; }
        public int RowIndex { get; private set; }

        #endregion


        #region --- Public Methods ---

        public void HitByBullet()
        {
            _onHit(this);
        }

        public void Setup(int rowIndex, int columnIndex, Action<EnemyView> onHit)
        {
            _onHit = onHit;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        #endregion
    }
}