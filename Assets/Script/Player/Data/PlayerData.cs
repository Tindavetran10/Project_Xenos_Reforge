using UnityEngine;

namespace Script.Player.Data
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
        public Vector2 wallJumpAngle = new Vector2(1, 2);

        [Header("In Air State")]
        public float coyoteTime;
        public float variableJumpHeightMultiplier;

        [Header("Wall Slide State")]
        public float wallSlideVelocity;

        [Header("Wall Climb State")]
        public float wallClimbVelocity;

        [Header("Ledge Climb State")]
        public Vector2 startOffset;
        public Vector2 stopOffset;

        [Header("Dash State")]
        public float dashCooldown;
        public float maxHoldTime;
        public float holdTimeScale;
        public float dashTime;
        public float dashVelocity;
        public float drag;
        public float dashEndYMultiplier;
        public float distBetweenAfterImages;

        [Header("Crouch State")]
        public float crouchMovementVelocity;
        public float crouchColliderHeight;
        public float standColliderHeight;
    }
}

