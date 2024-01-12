using UnityEngine;

namespace Script.Player.Skills.SkillController
{
    public class CloneSkillController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private float colorTransparentSpeed;
        [SerializeField] private float cloneDuration;
        private float cloneTimer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            cloneTimer -= Time.deltaTime;

            if (cloneTimer < 0)
            {
                _spriteRenderer.color = new Color(1, 1, 1,
                    _spriteRenderer.color.a - Time.deltaTime * colorTransparentSpeed);
            }
        }

        public void SetupClone(Transform newTransform)
        {
            transform.position = newTransform.position;
            cloneTimer = cloneDuration;
        }
    
    }
}
