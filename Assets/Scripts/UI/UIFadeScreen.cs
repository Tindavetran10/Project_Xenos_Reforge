using UnityEngine;

namespace UI
{
    public class UIFadeScreen : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Out = Animator.StringToHash("FadeOut");
        private static readonly int In = Animator.StringToHash("FadeIn");

        private void Start() => _animator = GetComponent<Animator>();

        public void FadeOut() => _animator.SetTrigger(Out);
        public void FadeIn() => _animator.SetTrigger(In);
    }
}
