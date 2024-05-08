using UnityEngine;

namespace Background.Old
{
    public class ParallaxController : MonoBehaviour
    {
        private Transform _cam;
        private Vector3 _camStartPosition;
        private float _distance;

        private GameObject[] _backgrounds;
        private Material[] _materials;
        private float[] _backSpeed;

        private float _farthestBack;

        [Range(0.01f, 0.05f)] public float parallaxSpeed;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Start()
        {
            if (Camera.main != null) 
                _cam = Camera.main.transform;
            _camStartPosition = _cam.position;
            
            var backCount = transform.childCount;
            _materials = new Material[backCount];
            _backSpeed = new float[backCount];
            _backgrounds = new GameObject[backCount];

            for (var i = 0; i < backCount; i++)
            {
                _backgrounds[i] = transform.GetChild(i).gameObject;
                _materials[i] = _backgrounds[i].GetComponent<Renderer>().material;
            }
            
            BackSpeedCalculate(backCount);
        }
        
        private void BackSpeedCalculate(int backCount)
        {
            for (var i = 0; i < backCount; i++)
            {
                if(_backgrounds[i].transform.position.z - _cam.position.z > _farthestBack)
                    _farthestBack = _backgrounds[i].transform.position.z - _cam.position.z;
            }

            for (var i = 0; i < backCount; i++)
            {
                _backSpeed[i] = 1- (_backgrounds[i].transform.position.z - _cam.position.z) / _farthestBack;
            }
        }

        private void LateUpdate()
        {
            _distance = _cam.position.x - _camStartPosition.x;
            transform.position = new Vector3(_cam.position.x, transform.position.y, 0);
            
            for (var i = 0; i < _backgrounds.Length; i++)
            {
                var speed = _backSpeed[i] * parallaxSpeed;
                _materials[i].SetTextureOffset(MainTex, new Vector2(_distance, 0) * speed);
            }
        }
    }
}
