using Player.Data;
using UnityEngine;

namespace CoreSystem.CoreComponents
{
    // This class will take care of all functions that changing the velocity of different characters 
    public class Movement : CoreComponent
    {
        public Rigidbody2D Rb { get; private set; }
        public int FacingDirection { get; private set; }
        public bool CanSetVelocity { get; set; }
    
        public Vector2 CurrentVelocity { get; private set; }
        private Vector2 _workspace;
        
        [SerializeField] private PlayerData playerData;
    
        protected override void Awake()
        {
            base.Awake();
            // Get the Rigidbody2D from the Player game object
            Rb = GetComponentInParent<Rigidbody2D>();

            // Facing the player to the right by default
            FacingDirection = 1;
            // Allow to set a new velocity
            CanSetVelocity = true;
        }

        // Update the new velocity to the current velocity of player
        public override void LogicUpdate() => CurrentVelocity = Rb.velocity;
    
        #region Set Functions
        // Set velocity to zero to stop the character movement
        public void SetVelocityZero()
        {
            _workspace = Vector2.zero;        
            SetFinalVelocity();
        }

        //Set the velocity for Wall Jumping
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            // Change the direction x to the opposite, facing a wall after a character make a wall jump
            // Change the direction y to always go up
            _workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }

        // Set the velocity for Dash State
        public void SetVelocity(float velocity, Vector2 direction)
        {
            _workspace = direction * velocity;
            SetFinalVelocity();
        }

        // Set the velocity on the x axis for different states
        public void SetVelocityX(float velocity)
        {
            _workspace.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }
    
        private float _smoothInputVelocity;

        // Set the velocity for Move State
        public void SetVelocityXSmoothDamp(float targetVelocity)
        {
            // Calculate the velocity that will change gradually until reaching the maximum value
            // This will make the player's speed slowly change when there is an input
            // instead of running at the maximum speed immediately
            targetVelocity = Mathf.SmoothDamp(CurrentVelocity.x, targetVelocity, 
                ref _smoothInputVelocity, playerData.smoothInputSpeed);
            _workspace.Set(targetVelocity, CurrentVelocity.y);
            SetFinalVelocity();
        }

        // Set the velocity on the y axis for different states
        public void SetVelocityY(float velocity)
        {
            _workspace.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }

        // Making the final changes which will affect the current velocity of player
        // after some needed calculation
        private void SetFinalVelocity()
        {
            // End function if the rb is static
            if (Rb.bodyType == RigidbodyType2D.Static)
            {
                CanSetVelocity = false;
                return;
            }

            if (CanSetVelocity)
            {
                // Check if _workspace contains valid values
                if (!IsValidVector(_workspace))
                    // If _workspace is invalid, use a fallback velocity
                    _workspace = CalculateFallbackVelocity();

                // Assign the velocity to the Rigidbody2D
                Rb.velocity = _workspace;
                CurrentVelocity = _workspace;
            }
        }

        // Check if a vector contains valid values (no NaN)
        private static bool IsValidVector(Vector2 vector) => !float.IsNaN(vector.x) && !float.IsNaN(vector.y);

        // Calculate a fallback velocity when _workspace is invalid
        private static Vector2 CalculateFallbackVelocity() => Vector2.zero;


        // Flip the image of character when he want to turn left or right
        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection) 
                Flip();
        }

        public void Flip()
        {
            FacingDirection *= -1;
            Rb.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        #endregion
    }
}
