using System.Collections;
using UnityEngine;

namespace Entity
{
    public class EntityFX : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;

        private Material _originalMat;

        [Header("Flash FX")] 
        [SerializeField] private Material hitMat;

        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _originalMat = _spriteRenderer.material;
        }

        private IEnumerator FlashFX()
        {
            _spriteRenderer.material = hitMat;
            yield return new WaitForSeconds(.2f);
            _spriteRenderer.material = _originalMat;
        }

        private void RedColorBlink()
        {
            if(_spriteRenderer.color != Color.white)
                _spriteRenderer.color = Color.white;
            else _spriteRenderer.color = Color.red;
        }

        private void CancelRedBlink()
        {
            CancelInvoke();
            _spriteRenderer.color = Color.white;
        }
    }
}