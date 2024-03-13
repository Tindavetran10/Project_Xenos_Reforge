using UnityEngine;

namespace HitStop
{
    public class HitStopController : MonoBehaviour
    {
        public static HitStopController Instance;
        
        // The time that the hit stop started
        private float _hitStopTime;
        
        // The duration of the hit stop
        private float _hitStopDuration;
        
        // A flag to check if the hit stop is active
        private bool _isHitStopped;

        private void Awake() => Instance = this;

        private void Update()
        {
            // If the hit stop is active
            if (_isHitStopped)
            {
                // Increase the hit stop time
                // Time.unscaledDeltaTime is the time between the current frame and the last frame, unaffected by time scale
                _hitStopTime += Time.unscaledDeltaTime;
                
                // If the hit stop time is greater than or equal to the hit stop duration
                if (_hitStopTime >= _hitStopDuration)
                {
                    _isHitStopped = false;
                    
                    // Return to normal time scale
                    Time.timeScale = 1f;
                }
            }
        }

        
        public void HitStop(float duration)
        {
            // Reset the hit stop time
            _hitStopTime = 0;
            
            // Set the hit stop duration
            _hitStopDuration = duration;
            
            // Set the hit stop flag to true
            _isHitStopped = true;
            Time.timeScale = 0f;
        }
    }
}