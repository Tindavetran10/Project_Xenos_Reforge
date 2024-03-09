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

        public void CreateGhostTrail()
        {
            GameObject currentGhostTrail = Instantiate(ghostTrailPrefab, transform.position, transform.rotation);
            currentGhostTrail.transform.localScale = _player.transform.localScale;
            Destroy(currentGhostTrail, destroyTime);
            
            _spriteRenderer = currentGhostTrail.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _player.SpriteRenderer.sprite;
            _spriteRenderer.color = color;
            
            if(material != null) _spriteRenderer.material = material;
        }
    }
}