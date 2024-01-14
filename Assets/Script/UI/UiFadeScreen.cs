using System;
using UnityEngine;

namespace Script.UI
{
    public class UiFadeScreen : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void FadeOut() => _animator.SetTrigger("FadeOut");
        public void FadeIn() => _animator.SetTrigger("FadeIn");
    }
}
