using UnityEngine;

namespace Background.New
{
    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour
    {
        public float parallaxFactor;

        public void Move(float delta)
        {
            var newPos = transform.localPosition;
            newPos.x -= delta * parallaxFactor;

            transform.localPosition = newPos;
        }

    }
}
