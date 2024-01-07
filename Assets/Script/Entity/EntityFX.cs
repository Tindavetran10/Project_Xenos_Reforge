using System;
using System.Collections;
using UnityEngine;

namespace Script.Entity
{
    public class EntityFX : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;

        private Material _originalMat;

        [Header("Flash FX")] 
        [SerializeField] private Material hitMat;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalMat = _spriteRenderer.material;
        }

        private IEnumerator FlashFX()
        {
            _spriteRenderer.material = hitMat;
            yield return new WaitForSeconds(.2f);
            _spriteRenderer.material = _originalMat;
        }
    }
}