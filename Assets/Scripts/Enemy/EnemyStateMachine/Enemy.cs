using System.Collections;
using Enemy.Data.EnemyDataScript;
using HitStop;
using Manager;
using Player.PlayerStats;
using Projectile;
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
        
        protected HitStopController HitStopController;
        private Transform _player;
        
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
            // Determine if the player is to the left (-1) or right (1) of the enemy
            var playerDirectionRelativeToEnemy = _player.position.x > transform.position.x ? 1 : -1;

            // If the player is on the opposite side of the enemy's facing direction, flip the enemy
            if (playerDirectionRelativeToEnemy != Movement.FacingDirection) Movement.Flip();
        }


        #region Animator Function
        public void AttackTrigger()
        {
            // Check if the player is in range
            var collider2Ds = Physics2D.OverlapCircleAll(attackPosition.position, enemyData.hitBox.Length);
            foreach (var hit in collider2Ds) ProcessHit(hit);
        }

        private void ProcessHit(Component hit)
        {
            var playerComponent = hit.GetComponent<Player.PlayerStateMachine.Player>();
            if(playerComponent == null) return;
            
            HitParticle(hit);
            
            var target = hit.GetComponentInChildren<PlayerStats>();
            if(target!=null)
            {
                Stats.DoDamage(target);
                HitStopController.HitStop(enemyData.hitStopDuration);
            }
        }

        private void HitParticle(Component hit)
        {
            // Instantiate Hit Particle
            var hitParticleInstance = Instantiate(enemyData.hitParticle, hit.transform.position, Quaternion.identity);
            
            // Destroy the hit particle prefab after 0.5f
            Destroy(hitParticleInstance, 0.5f);
        }

        public void RangeAttackTrigger()
        {
            var newProjectile = Instantiate(enemyProjectile, 
                attackPosition.position, Quaternion.identity);
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
        
        public virtual bool ChangeGetAttackedState() => Stats.IsAttacked;

        #region KnockBack Function
        public void DamageImpact() => StartCoroutine(nameof(HitKnockBack));

        public IEnumerator HitKnockBack()
        {
            Movement.CanSetVelocity = false;
            
            //if the rb is static then return
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
    }
}