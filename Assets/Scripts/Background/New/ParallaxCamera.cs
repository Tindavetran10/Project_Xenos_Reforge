using UnityEngine;

namespace Background.New
{
    [ExecuteInEditMode]
    public class ParallaxCamera : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float deltaMovement);
        public ParallaxCameraDelegate OnCameraTranslate;

        private float _oldPosition;

        private void Start() => _oldPosition = transform.position.x;

        private void Update()
        {
            if (Mathf.Approximately(transform.position.x, _oldPosition))
            {
                if (OnCameraTranslate != null)
                {
                    var delta = _oldPosition - transform.position.x;
                    OnCameraTranslate(delta);
                }

                _oldPosition = transform.position.x;
            }
        }
    }
}
