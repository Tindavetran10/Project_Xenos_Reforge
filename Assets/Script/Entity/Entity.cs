using Script.CoreSystem;
using Script.Enemy.Intermediaries;
using UnityEngine;

namespace Script.Entity
{
    public class Entity : MonoBehaviour
    {
        public Transform attackPosition;

        public Core Core { get; protected set; }
        public Animator Anim { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        
        public AnimationToStateMachine Atsm { get; private set; }
        
        protected virtual void Awake() {}

        protected virtual void Start()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
            Atsm = GetComponent<AnimationToStateMachine>();
        }

        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        public virtual void Damage()
        {
            Debug.Log(gameObject.name + " was damaged");
        }
    }
}