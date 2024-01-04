using UnityEngine;

namespace Script.Entity
{
    public class Entity : MonoBehaviour
    {
        public _Scripts.CoreSystem.Core Core { get; protected set; }
        public Animator Anim { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        
        protected virtual void Awake() {}

        protected virtual void Start()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}
    }
}