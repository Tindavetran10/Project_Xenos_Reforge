using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Move State")]
        public float movementVelocity;
        public float smoothInputSpeed;

        
        [Header("Jump State")]
        public float jumpVelocity;
        public int amountOfJumps;

        [Header("Wall Jump State")]
        public float wallJumpVelocity;
        public float wallJumpTime;
        public Vector2 wallJumpAngle = new(1, 2);

        [Header("In Air State")]
        public float coyoteTime;
        public float variableJumpHeightMultiplier;

        [Header("Wall Slide State")]
        public float wallSlideVelocity;

        [Header("Ledge Climb State")]
        public Vector2 startOffset;
        public Vector2 stopOffset;

        [Header("Dash State")]
        public float maxHoldTime;
        public float holdTimeScale;
        public float dashTime;
        public float dashVelocity;
        public float drag;
        public float dashEndYMultiplier;

        [Header("Crouch State")]
        public float crouchMovementVelocity;
        public float crouchColliderHeight;
        public float standColliderHeight;

        [Space]
        public LayerMask whatIsEnemy;
        
        [Header("Primary Attack State")] 
        public GameObject hitParticle;
        public float hitStopDuration;
        public float comboWindow;
        public int numberOfAttacks;

        public float[] attackVelocity;
        public Vector2[] direction;
        public Rect[] hitBox;
        
        [Header("Focus Sword State")]
        public Rect focusSwordHitBox;
        
        [Header("Counter Attack State")]
        public float counterAttackDuration;
        
    }
}

