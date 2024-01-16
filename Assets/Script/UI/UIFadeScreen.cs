using UnityEngine;

namespace Script.UI
{
    public class UIFadeScreen : MonoBehaviour
    {
        private Animator _animator;

        private void Start() => _animator = GetComponent<Animator>();

        public void FadeOut() => _animator.SetTrigger("FadeOut");
        public void FadeIn() => _animator.SetTrigger("FadeIn");
    }
}
