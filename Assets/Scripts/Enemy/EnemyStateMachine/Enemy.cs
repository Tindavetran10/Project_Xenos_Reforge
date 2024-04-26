using System.Collections;
using Controller;
using Enemy.Data.EnemyDataScript;
using HitStop;
using Manager;
using Player.PlayerStats;
using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public class Enemy : Entity.Entity
    {
        #region Components
        [Header("Enemy Data")]
        public EnemyData enemyData;
        protected EnemyStateMachine StateMachine;
        #endregion

        #region Close Range Attack Properties
        private int _maxColliders = 50;
        #endregion
        
        #region Ranged Attack Properties
        [Header("Projectile")]
        [SerializeField] private float projectileSpeed;
        [SerializeField] private GameObject enemyProjectile;
        #endregion

        #region Counter and Stunned Mechanic
        private bool CanBeStunned { get; set; }
        [SerializeField] public GameObject counterImage;
        #endregion
        
        #region Knockback Components
        [Header("KnockBack Mechanics")] 
        [SerializeField] protected Vector2 knockBackDirection;
        [SerializeField] protected float knockBackDuration;
        #endregion
        
        #region Animation
        [HideInInspector] public bool isAnimationFinished;
        private Vector2 _velocityWorkspace;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        #endregion

        #region Attack Cooldown
        [Space][Header("Attack Cooldown Mechanics")] 
        [HideInInspector] public float lastTimeAttacked;
        public float attackCoolDown;
        #endregion
        
        public bool GetFrozen { get; private set; }
        
        protected HitStopController HitStopController;
        private Transform _player;
        private static readonly int EnemyDeathAnim = Animator.StringToHash("die");

        protected override void Awake() {
            base.Awake();
            StateMachine = new EnemyStateMachine();
        }
        
        protected override void Update() {
            base.Update();
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
            
            Anim.SetFloat(YVelocity, Movement.Rb.velocity.y);
            _player = PlayerManager.GetInstance().player.transform;
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            StateMachine.CurrentState.PhysicsUpdate();
        }
        
        public void BattleStateFlipControl()
        {
            if (Movement.CanSetVelocity)
            {
                // Determine if the player is to the left (-1) or right (1) of the enemy
                var playerDirectionRelativeToEnemy = _player.position.x > transform.position.x ? 1 : -1;

                // If the player is on the opposite side of the enemy's facing direction, flip the enemy
                if (playerDirectionRelativeToEnemy != Movement.FacingDirection) Movement.Flip();
            }
        }

        private void Freeze(bool getFrozen)
        {
            if (getFrozen)
            {
                Movement.CanSetVelocity = false;
                Anim.speed = 0;
            }
            else
            {
                Movement.CanSetVelocity = true;
                Anim.speed = 1;
            }
        }

        private IEnumerator FreezeCoroutine(float freezeDuration)
        {
            Freeze(true);
            yield return new WaitForSeconds(freezeDuration);
            Freeze(false);
        }
        
        public void FreezeMovementFor(float duration) => StartCoroutine(FreezeCoroutine(duration));
        
        #region Animator Function
        public void AttackTrigger()
        {
            // Create a Collider2D array with a size that you think will be enough for your use case
            var results = new Collider2D[_maxColliders];

            // Use OverlapCircleNonAlloc instead of OverlapCircleAll
            var numColliders = Physics2D.OverlapCircleNonAlloc(attackPosition.position, enemyData.hitBox.Length, results);

            // If the array is full, double the size of the array
            if (numColliders == results.Length)
                _maxColliders *= 2;
            // If the array is less than half full, reduce the size of the array
            else if (numColliders < _maxColliders / 2)
                _maxColliders = Mathf.Max(1, _maxColliders / 2);
            
            // Loop over the number of colliders found
            for (var i = 0; i < numColliders; i++) ProcessHit(results[i]);
        }

        private void ProcessHit(Component hit)
        {
            var playerComponent = hit.GetComponent<Player.PlayerStateMachine.Player>();
            if(playerComponent == null || playerComponent.Stats.IsInvincible) return;

            HitParticle(hit, enemyData.hitParticle);
            
            var target = hit.GetComponentInChildren<PlayerStats>();
            if(target!=null)
            {
                Stats.DoDamage(target);
                HitStopController.HitStop(enemyData.hitStopDuration);
            }
        }
        
        public void RangeAttackTrigger()
        {
            var newProjectile = ObjectPoolManager.SpawnObject(enemyProjectile, attackPosition.position, 
                Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            newProjectile.GetComponent<ProjectileController>().SetUpProjectile(projectileSpeed * Movement.FacingDirection, Stats);
        }
        
        public void FinishAttack()
        {
            isAnimationFinished = true;
            counterImage.SetActive(false);
        }
        #endregion
        
        #region CounterAttack Window
        private void SetCanBeStunned(bool state)
        {
            CanBeStunned = state;
            counterImage.SetActive(state);
        }
        public void OpenCounterAttackWindow() => SetCanBeStunned(true);
        public void CloseCounterAttackWindow() => SetCanBeStunned(false);

        public virtual bool TryCloseCounterAttackWindow()
        {
            if (CanBeStunned)
            {
                CloseCounterAttackWindow();
                return true;
            }
            return false;
        }
        #endregion
        
        #region KnockBack Function
        public void DamageImpact() => StartCoroutine(nameof(HitKnockBack));

        public IEnumerator HitKnockBack()
        {
            Movement.CanSetVelocity = false;
            
            //if the rb is static, then return
            if (Rb.bodyType == RigidbodyType2D.Static) yield break;
            CheckKnockBackDirection();
            
            yield return new WaitForSeconds(knockBackDuration);
            Movement.SetVelocityX(0f);
            Movement.CanSetVelocity = true;
        }

        private void CheckKnockBackDirection()
        {
            // Early return if the Rigidbody is static, as it cannot be moved.
            if (Rb.bodyType == RigidbodyType2D.Static) return;

            // Determine if the player is on the left (-1) or right (1) of the enemy
            var playerDirectionRelativeToEnemy = _player.position.x < transform.position.x ? -1 : 1;

            // Apply knockback in the opposite direction of where the player is relative to the enemy
            Rb.velocity = new Vector2(knockBackDirection.x * -playerDirectionRelativeToEnemy, knockBackDirection.y);

        }
        #endregion
        
        #region Die Function
        public override void Die()
        {
            base.Die();
            Anim.SetBool(EnemyDeathAnim, true);
            Invoke(nameof(DestroyEnemy), 1f);
        }
        private void DestroyEnemy() => Destroy(gameObject);
        #endregion
        
        #region Check Functions, Draw Gizmos
        public bool CheckPlayerInAgroRange() => 
            Physics2D.Raycast(attackPosition.position, transform.right, enemyData.agroDistance, 
                enemyData.whatIsPlayer);
        
        public bool CheckPlayerInCloseRangeAction() => 
            Physics2D.Raycast(attackPosition.position, transform.right, 
                enemyData.closeRangeActionDistance, enemyData.whatIsPlayer);

        public virtual void OnDrawGizmos() {}
        #endregion
        
        public virtual bool ChangeGetAttackedState() => Stats.IsAttacked;
        public virtual bool ChangeStunState() => Stats.IsStunned;
    }
}