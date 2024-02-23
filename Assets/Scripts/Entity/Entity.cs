using System.Collections;
using CoreSystem;
using CoreSystem.CoreComponents;
using UnityEngine;
using StatSystem_CharacterStats = StatSystem.CharacterStats;

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        #region Base Components
        public Transform attackPosition;
        public Core Core { get; protected set; }
        public Animator Anim { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public EntityFX FX { get; private set; }
        public StatSystem_CharacterStats Stats { get; private set; }

        protected CapsuleCollider2D MovementCollider2D { get; set; }

        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;
        #endregion

        

        public System.Action OnFlipped;

        protected virtual void Awake() => Core = GetComponentInChildren<Core>();

        protected virtual void Start()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            FX = GetComponent<EntityFX>();
            Stats = GetComponentInChildren<StatSystem_CharacterStats>();
            MovementCollider2D = GetComponent<CapsuleCollider2D>();
        }
        
        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        

        public virtual void Die(){}
    }
}