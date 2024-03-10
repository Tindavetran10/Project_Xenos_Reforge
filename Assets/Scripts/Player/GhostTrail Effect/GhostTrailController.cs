using UnityEngine;

namespace Player.GhostTrail_Effect
{
    public class GhostTrailController : MonoBehaviour
    {
        public GameObject ghostTrailPrefab;
        public float delay;
        private float delta;

        private PlayerStateMachine.Player _player;
        private SpriteRenderer _spriteRenderer;
        public float destroyTime;
        public Color color;
        public Material material;
        
        private void Start()
        {
            _player = GetComponent<PlayerStateMachine.Player>();
            _spriteRenderer = GetComponent<SpriteRenderer>(); 
        }

        private void Update()
        {
            if(delta > 0)
                delta -= Time.deltaTime;
            else
            {
                delta = delay;
                CreateGhostTrail();
            }
        }

        private void CreateGhostTrail()
        {
            // Create a new ghost trail 
            // Instantiate the ghostTrailPrefab and set the position and rotation to the player
            var playerTransform = transform;
            GameObject currentGhostTrail = Instantiate(ghostTrailPrefab, playerTransform.position, playerTransform.rotation);
            
            // Set the scale of the ghost trail to the player scale
            currentGhostTrail.transform.localScale = _player.transform.localScale;
            // Destroy the ghost trail after a certain amount of time
            Destroy(currentGhostTrail, destroyTime);
            
            // Set the sprite and color of the ghost trail
            _spriteRenderer = currentGhostTrail.GetComponent<SpriteRenderer>();
            
            // Set the sprite and color of the ghost trail
            _spriteRenderer.sprite = _player.SpriteRenderer.sprite;
            _spriteRenderer.color = color;
            
            // Set the material of the ghost trail
            if(material != null) _spriteRenderer.material = material;
        }
    }
}