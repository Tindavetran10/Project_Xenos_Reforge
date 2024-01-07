using System.Collections;
using Script.CoreSystem;
using Script.CoreSystem.CoreComponents;
using Script.Enemy.Intermediaries;
using UnityEngine;

namespace Script.Entity
{
    public class Entity : MonoBehaviour
    {
        #region Base Components
        public Transform attackPosition;
        public Core Core { get; protected set; }
        public Animator Anim { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        private EntityFX FX { get; set; }
        
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;

        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses 
            : Core.GetCoreComponent(ref _collisionSenses);
        private CollisionSenses _collisionSenses;
        #endregion

        #region Knockback Components
        [SerializeField] protected Vector2 knockBackDirection;
        [SerializeField] protected float knockBackDuration;
        private bool _isKnocked;
        #endregion
        
        public AnimationToStateMachine Atsm { get; private set; }
        
        protected virtual void Awake() {}

        protected virtual void Start()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            Atsm = GetComponent<AnimationToStateMachine>();
            FX = GetComponent<EntityFX>();
        }

        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        public virtual void Damage()
        {
            FX.StartCoroutine("FlashFX");
            StartCoroutine(nameof(HitKnockBack));
            Debug.Log(gameObject.name + " was damaged");
        }

        protected virtual IEnumerator HitKnockBack()
        {
            _isKnocked = true;
            Movement.CanSetVelocity = false;
            Rb.velocity = new Vector2(knockBackDirection.x * -Movement.FacingDirection, knockBackDirection.y);
            yield return new WaitForSeconds(knockBackDuration);
            Movement.CanSetVelocity = true;
            _isKnocked = false;
        }
    }
}