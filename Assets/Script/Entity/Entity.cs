using System.Collections;
using Script.CoreSystem;
using Script.CoreSystem.CoreComponents;
using Script.Enemy.Intermediaries;
using UnityEngine;
using CharacterStats = Script.StatSystem.CharacterStats;

namespace Script.Entity
{
    public class Entity : MonoBehaviour
    {
        #region Base Components
        public Transform attackPosition;
        public Core Core { get; private set; }
        public Animator Anim { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public EntityFX FX { get; private set; }
        public CharacterStats Stats { get; private set; }

        protected CapsuleCollider2D MovementCollider2D { get; set; }

        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;
        #endregion

        #region Knockback Components
        [Header("KnockBack Mechanics")]
        [SerializeField] protected Vector2 knockBackDirection;
        [SerializeField] protected float knockBackDuration;
        #endregion
        
        public AnimationToStateMachine Atsm { get; protected set; }

        protected virtual void Awake() => Core = GetComponentInChildren<Core>();

        protected virtual void Start()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            Atsm = GetComponent<AnimationToStateMachine>();
            FX = GetComponent<EntityFX>();
            Stats = GetComponentInChildren<CharacterStats>();
            MovementCollider2D = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        public void DamageEffect()
        {
            FX.StartCoroutine("FlashFX");
            StartCoroutine(nameof(HitKnockBack));
            Debug.Log(gameObject.name + " was damaged");
        }

        protected IEnumerator HitKnockBack()
        {
            Movement.CanSetVelocity = false;
            Rb.velocity = new Vector2(knockBackDirection.x * - Movement.FacingDirection, knockBackDirection.y);
            yield return new WaitForSeconds(knockBackDuration);
            Movement.CanSetVelocity = true;
        }

        public virtual void Die() => Invoke(nameof(DestroyCharacter), 1f);
        private void DestroyCharacter() => Destroy(gameObject);
    }
}