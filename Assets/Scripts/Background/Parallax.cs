using Cinemachine;
using UnityEngine;

namespace Background
{
    public class Parallax : MonoBehaviour {

        //Parallax Scroll Variables
        public CinemachineVirtualCamera cinemachineCam;//the camera
        public Transform subject;//the subject (usually the player character)


        //Instance variables
        private float _zPosition;
        private Vector2 _startPos;


        //Properties
        private float TwoAspect => cinemachineCam.m_Lens.Aspect * 2;
        private float TileWidth => TwoAspect > 3 ? TwoAspect : 3;
        private float ViewWidth => loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
        private Vector2 Travel => (Vector2)cinemachineCam.transform.position - _startPos; //2D distance travelled from our starting position
        private float DistanceFromSubject => transform.position.z - subject.position.z;
        private float ClippingPlane => cinemachineCam.transform.position.z + (DistanceFromSubject > 0 
            ? cinemachineCam.m_Lens.FarClipPlane : cinemachineCam.m_Lens.NearClipPlane);
        private float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;


        //User options
        public bool xAxis = true; //parallax on x?
        public bool yAxis = true; //parallax on y?
        public bool infiniteLoop; //are we looping?


        //Loop requirement
        public SpriteRenderer loopSpriteRenderer;


        // Start is called before the first frame update
        private void Awake()
        {
            var position = transform.position;
            _startPos = position;
            _zPosition = position.z;

            if (loopSpriteRenderer != null && infiniteLoop) {
                var sprite = loopSpriteRenderer.sprite;
                var spriteSizeX = sprite.rect.width / sprite.pixelsPerUnit;
                var spriteSizeY = sprite.rect.height / sprite.pixelsPerUnit;

                loopSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
                loopSpriteRenderer.size = new Vector2(spriteSizeX * TileWidth, spriteSizeY);
                transform.localScale = Vector3.one;
            }
        }


        // Update is called once per frame
        private void Update() {
            var newPos = _startPos + Travel * ParallaxFactor;
            transform.position = new Vector3(xAxis ? newPos.x : _startPos.x, yAxis ? newPos.y : _startPos.y, _zPosition);

            if (infiniteLoop) {
                Vector2 totalTravel = cinemachineCam.transform.position - transform.position;
                var boundsOffset = ViewWidth / 2 * (totalTravel.x > 0 ? 1 : -1);
                float screens = (int)((totalTravel.x + boundsOffset) / ViewWidth);
                transform.position += new Vector3(screens * ViewWidth, 0);
            }
        }
    }
}
