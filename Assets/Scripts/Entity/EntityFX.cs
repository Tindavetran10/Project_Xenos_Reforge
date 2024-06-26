using System.Collections;
using UnityEngine;

namespace Entity
{
    public class EntityFX : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        [Header("Flash FX")] 
        [SerializeField] private float flashDuration;
        [SerializeField] private Material hitMat;
        private Material _originalMat;
        
        [Header("Aliment FX")]
        [SerializeField] private Color[] frozenColor;
        [SerializeField] private Color[] igniteColor;
        [SerializeField] private Color[] shockColor;
        
        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _originalMat = _spriteRenderer.material;
        }

        private IEnumerator FlashFX()
        {
            _spriteRenderer.material = hitMat;
            
            var currentColor = _spriteRenderer.color;
            _spriteRenderer.color = Color.white;
            
            yield return new WaitForSeconds(flashDuration);
            
            _spriteRenderer.color = currentColor;
            _spriteRenderer.material = _originalMat;
        }


        private void CancelColorChange()
        {
            CancelInvoke();
            _spriteRenderer.color = Color.white;
        }
        
        public void IgniteFXFor(float duration)
        {
            //igniteFx.Play();
            InvokeRepeating(nameof(IgniteColorFX), 0, .3f);
            Invoke(nameof(CancelColorChange), duration);
        }
        
        public void FrozenFXFor(float duration)
        {
            //frozenFx.Play();
            InvokeRepeating(nameof(ChillColorFX), 0, .3f);
            Invoke(nameof(CancelColorChange), duration);
        }
        
        public void ShockFXFor(float duration)
        {
            //shockFx.Play();
            InvokeRepeating(nameof(ShockColorFX), 0, .3f);
            Invoke(nameof(CancelColorChange), duration);
        }
        
        private void RedColorBlink() => _spriteRenderer.color = _spriteRenderer.color != Color.white ? Color.white : Color.red;
        
        private void IgniteColorFX() => _spriteRenderer.color = _spriteRenderer.color != igniteColor[0] ? igniteColor[0] : igniteColor[1];
        private void ChillColorFX() => _spriteRenderer.color = _spriteRenderer.color != frozenColor[0] ? frozenColor[0] : frozenColor[1];
        private void ShockColorFX() => _spriteRenderer.color = _spriteRenderer.color != shockColor[0] ? shockColor[0] : shockColor[1];
    }
}