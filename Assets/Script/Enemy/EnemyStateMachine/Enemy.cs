using Script.Enemy.Data;
using Script.Projectile;
using UnityEngine;

namespace Script.Enemy.EnemyStateMachine
{
    public class Enemy : Entity.Entity
    {
        #region Components
        public EnemyData enemyData;
        protected EnemyStateMachine StateMachine;
        #endregion
        
        #region Ranged Attack Properties
        [SerializeField] private GameObject enemyProjectile;
        [SerializeField] private float projectileSpeed;
        #endregion
        
        [HideInInspector] public bool canBeStunned;
        [SerializeField] public GameObject counterImage;
        
        private Vector2 _velocityWorkspace;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        
        protected override void Awake() {
            base.Awake();
            StateMachine = new EnemyStateMachine();
        }

        protected override void Update() {
            base.Update();
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
            
            Anim.SetFloat(YVelocity, Movement.Rb.velocity.y);
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            StateMachine.CurrentState.PhysicsUpdate();
        }
        
        public void SpecialAttackTrigger()
        {
            GameObject newProjectile = Instantiate(enemyProjectile, attackPosition.position, Quaternion.identity);
            newProjectile.GetComponent<ProjectileController>().SetUpProjectile(projectileSpeed, Stats);
        }

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