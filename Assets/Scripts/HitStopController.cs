using UnityEngine;

public class HitStopController : MonoBehaviour
{
    public static HitStopController Instance;
    private float _hitStopTime;
    private float _hitStopDuration;
    private bool _isHitStopped;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (_isHitStopped)
        {
            _hitStopTime += Time.unscaledDeltaTime;
            if (_hitStopTime >= _hitStopDuration)
            {
                _isHitStopped = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void HitStop(float duration)
    {
        _hitStopTime = 0;
        _hitStopDuration = duration;
        _isHitStopped = true;
        Time.timeScale = 0f;
    }
}