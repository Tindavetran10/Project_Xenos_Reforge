using Cinemachine;
using UnityEngine;

namespace Background.Old
{
    public class Parallax : MonoBehaviour
    {
        private Material _mat;
        private float _distance;

        [Range(0f, 0.5f)] public float speed = 0.2f;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Start() => _mat = GetComponent<Renderer>().material;
        
        private void Update()
        {
            if (Camera.main != null)
            {
                _distance += Time.deltaTime * speed;
                _mat.SetTextureOffset(MainTex, Vector2.right * _distance);
            }
        }
    }
}
