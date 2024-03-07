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
        [HideInInspector] public bool canBeStunned;
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
        [HideInInspector] public float lastTimeAttacked;
        public float attackCoolDown;
        #endregion

        protected HitStopController _hitStopController;
        
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
            _player = PlayerManager.Instance.player.transform;
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            StateMachine.CurrentState.PhysicsUpdate();
        }
        
        public void BattleStateFlipControl()
        {
            if (_player.position.x > transform.position.x && Movement.FacingDirection == -1)
                Movement.Flip();
            else if (_player.position.x < transform.position.x && Movement.FacingDirection == 1)
                Movement.Flip();
        }

        #region Animator Function
        public void AttackTrigger()
        {
            var collider2Ds = Physics2D.OverlapCircleAll(attackPosition.position, enemyData.hitBox.Length);

            foreach (var hit in collider2Ds)
            {
                if(hit.GetComponent<Player.PlayerStateMachine.Player>() != null)
                {
                    var target = hit.GetComponentInChildren<PlayerStats>();
                    _hitStopController.HitStop(enemyData.hitStopDuration);
                    Stats.DoDamage(target);
                }
            }
        }
        
        public void SpecialAttackTrigger()
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
        public void OpenCounterAttackWindow()
        {
            canBeStunned = true;
            counterImage.SetActive(true);
        }

        public void CloseCounterAttackWindow()
        {
            canBeStunned = false;
            counterImage.SetActive(false);
        }

        public virtual bool CanBeStunned()
        {
            if (canBeStunned)
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
            CheckKnockBackDirection();
            yield return new WaitForSeconds(knockBackDuration);
            Movement.SetVelocityX(0f);
            Movement.CanSetVelocity = true;
        }

        private void CheckKnockBackDirection()
        {
            // Check player and enemy position to determine knockback direction
            if(_player.position.x < transform.position.x && Movement.FacingDirection == 1)
                Rb.velocity = new Vector2(knockBackDirection.x  * Movement.FacingDirection, knockBackDirection.y);
            else if(_player.position.x < transform.position.x && Movement.FacingDirection == -1)
                Rb.velocity = new Vector2(knockBackDirection.x  * -Movement.FacingDirection, knockBackDirection.y);
            else if(_player.position.x > transform.position.x && Movement.FacingDirection == 1) 
                Rb.velocity = new Vector2(knockBackDirection.x  * -Movement.FacingDirection, knockBackDirection.y);
            else if (_player.position.x > transform.position.x && Movement.FacingDirection == -1)
                Rb.velocity = new Vector2(knockBackDirection.x  * Movement.FacingDirection, knockBackDirection.y);

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