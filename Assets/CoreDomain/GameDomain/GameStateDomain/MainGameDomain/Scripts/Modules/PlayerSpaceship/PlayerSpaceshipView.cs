using TMPro;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipView : MonoBehaviour
    {
        [SerializeField] public float Speed = 5f;
        [SerializeField] private SpriteRenderer PlayerSpriteRenderer;
        [SerializeField] private Transform _shootPositionTransform;
        [SerializeField] private TextMeshPro _playerNameText;

        public Vector3 ShootPosition => _shootPositionTransform.position;
        public Bounds SpriteBounds => PlayerSpriteRenderer.bounds;
        
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void SetPlayerName(string playerName)
        {
            _playerNameText.text = playerName;
        }

        public void MoveToXPosition(float xPosition)
        {
            var transformPosition = _transform.position;
            _transform.position = new Vector3(xPosition, transformPosition.y, transformPosition.z);
        }
    }
}