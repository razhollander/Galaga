using System;
using System.Collections;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship
{
    public class PlayerSpaceshipView : MonoBehaviour
    {
        private const float PLAYER_DIE_ANIMATION_TIME = 1.5f;
        private const float PLAYER_DIE_ANIMATION_STEPS = 9;
        
        [SerializeField] public SpriteRenderer PlayerSpriteRenderer;
        [SerializeField] private Transform shootPositionTransform;

        public Vector3 ShootPosition => shootPositionTransform.position;
        private Action _onDieAnimationEnd;

        private Action<Collider2D> _onCollision;
        private Coroutine _dieAnimationCoroutine;
        //private PlayerModel _model;
        private Transform _transform;
        
        private void Awake()
        {
            _transform = transform;
        }

        private void OnDestroy()
        {
            if (_dieAnimationCoroutine != null)
            {
                StopCoroutine(_dieAnimationCoroutine);
            }

            //RemoveListeners();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onCollision?.Invoke(other);
        }
        
        // public void Setup(PlayerModel model, Action<Collider2D> onCollision, Action onDieAnimationEnd)
        // {
        //     _model = model;
        //     _onCollision = onCollision;
        //     _onDieAnimationEnd = onDieAnimationEnd;
        //     AddListeners();
        // }
        
        // private void AddListeners()
        // {
        //     _model.PlayerLoseEvent += DoDieAnimation;
        // }
        //
        // private void RemoveListeners()
        // {
        //     _model.PlayerLoseEvent -= DoDieAnimation;
        // }

        private IEnumerator Rotate90Degrees()
        {
            var waitForSeconds = new WaitForSeconds(PLAYER_DIE_ANIMATION_TIME / PLAYER_DIE_ANIMATION_STEPS);
            var angle = 90 / PLAYER_DIE_ANIMATION_STEPS;

            for (var i = 0; i < PLAYER_DIE_ANIMATION_STEPS; i++)
            {
                _transform.Rotate(0, angle, 0);

                yield return waitForSeconds;
            }

            _onDieAnimationEnd?.Invoke();
        }
        
        private void DoDieAnimation()
        {
            _dieAnimationCoroutine = StartCoroutine(Rotate90Degrees());
        }
    }
}