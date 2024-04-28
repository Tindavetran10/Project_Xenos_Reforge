using CoreSystem;
using CoreSystem.CoreComponents;
using Manager;
using UnityEngine;
using StatSystem_CharacterStats = StatSystem.CharacterStats;

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        #region Base Components
        public Transform attackPosition;
        public Core Core { get; private set; }
        public Animator Anim { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public EntityFX FX { get; private set; }
        public StatSystem_CharacterStats Stats { get; private set; }

        protected CapsuleCollider2D MovementCollider2D { get; set; }

        public Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;
        #endregion
        
        public System.Action OnFlipped;

        protected virtual void Awake() => Core = GetComponentInChildren<Core>();

        protected virtual void Start()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            FX = GetComponent<EntityFX>();
            Stats = GetComponentInChildren<StatSystem_CharacterStats>();
            MovementCollider2D = GetComponent<CapsuleCollider2D>();
        }
        
        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        public static void HitParticle(Component hit, GameObject data)
        {
            // Instantiate the hit particle
            var hitParticleInstance = ObjectPoolManager.SpawnObject(data, 
                hit.transform.position + new Vector3(0f,0.15f,0f), Quaternion.identity, 
                ObjectPoolManager.PoolType.GameObject);
            
            // Destroy the hit particle after 0.5f
            Destroy(hitParticleInstance, 0.19f);
        }
        
        public virtual void SlowEntityBy(float slowPercentage, float slowDuration){}
        public virtual void ReturnDefaultSpeed() => Anim.speed = 1;
        
        public virtual void Die(){}
    }
}