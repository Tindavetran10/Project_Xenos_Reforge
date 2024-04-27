using CoreSystem;
using CoreSystem.CoreComponents;
using Enemy.EnemyStats;
using Player.Data;
using StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.SkillController
{
    public class CloneSkillController : MonoBehaviour
    {
        #region Core Component
        private Core Core { get; set; }

        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;
        #endregion

        #region AttackTrigger and Detect Enemy
        private Vector2 _offset;
        private CharacterStats _stats;
        [SerializeField] private Transform attackPosition;
        [SerializeField] private PlayerData playerData;

        private Transform _closetEnemy;
        #endregion
        
        #region Animation and SpriteRenderer
        private Animator _anim;
        private SpriteRenderer _spriteRenderer;

        private int _comboCounter;
        private static readonly int AttackCounter = Animator.StringToHash("AttackCounter");
        #endregion
        
        private const float GetColliderRadius = 25f;
        private Collider2D[] results = new Collider2D[5];
        
        [SerializeField] private float colorTransparentSpeed;
        private float _cloneTimer;
        
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _stats = GetComponentInChildren<CharacterStats>();
            Core = GetComponentInChildren<Core>();
        }

        private void Start() => _comboCounter = Random.Range(0,2);

        private void Update()
        {
            _cloneTimer -= Time.deltaTime;

            if (_cloneTimer < 0)
            {
                _spriteRenderer.color = new Color(1, 1, 1,
                    _spriteRenderer.color.a - Time.deltaTime * colorTransparentSpeed);
                
                if(_spriteRenderer.color.a <= 0)
                    Destroy(gameObject);
            }
        }

        public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack)
        {
            if(canAttack) _anim.SetInteger(AttackCounter, _comboCounter + 1);
            
            transform.position = newTransform.position;
            _cloneTimer = cloneDuration;
            
            FaceClosetObject();
        }

        private void AnimationTrigger() => _cloneTimer = -.1f;

        private void AnimationFinishTrigger() {}

        private void AttackTrigger()
        {
            var playerTransform = attackPosition.transform;
            var playerPosition = playerTransform.position;

            _offset.Set(playerPosition.x + playerData.hitBox[_comboCounter].center.x * Movement.FacingDirection,
                playerPosition.y + playerData.hitBox[_comboCounter].center.y);
            
            var numColliders = Physics2D.OverlapBoxNonAlloc(_offset, playerData.hitBox[_comboCounter].size, 
                0f, results, playerData.whatIsEnemy);
            
            var size = Physics2D.OverlapBoxNonAlloc(_offset, playerData.hitBox[_comboCounter].size, 0f, 
                results, playerData.whatIsEnemy);

            for (var i = 0; i < numColliders; i++)
            {
                var hit = results[i];
                if(hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                {
                    var targetStats = hit.GetComponentInChildren<EnemyStats>();
                    _stats.DoDamage(targetStats);
                }
            }
        }

        private void FaceClosetObject()
        {
            var numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, GetColliderRadius, results, 
                playerData.whatIsEnemy);
            
            // Step 1: Get all colliders within a circular area centered at the current object's position with a radius of 25 units.
            var collider2Ds = Physics2D.OverlapCircleAll(transform.position, GetColliderRadius);

            // Step 2: Initialize variables for tracking the closest enemy and its distance.
            var closetDistance = Mathf.Infinity;

            // Step 3: Iterate through all the colliders found in the specified area.
            for (var i = 0; i < numColliders; i++)
            {
                var hit = results[i];
                // Step 4: Check if the collider represents an enemy (by checking if it has a specific component).
                if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null)
                {
                    // Step 5: Calculate the distance from the current object to the enemy.
                    var distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                    // Step 6: If the current enemy is closer than the previously closest one, update the tracking variables.
                    if (distanceToEnemy < closetDistance)
                    {
                        closetDistance = distanceToEnemy;
                        _closetEnemy = hit.transform;
                    }
                }
            }

            // Step 7: If the closest enemy was found, rotate towards it.
            if (_closetEnemy != null)
            {
                // Step 8: Check if the enemy is to the left or right of the current object and rotate accordingly.
                if (transform.position.x > _closetEnemy.position.x)
                    transform.Rotate(0, 180, 0);
            }
        }
    }
}
