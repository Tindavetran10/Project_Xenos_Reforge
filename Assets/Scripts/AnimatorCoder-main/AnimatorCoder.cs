//Author: Small Hedge Games
//Date: 08/04/2024

using System;
using System.Collections;
using UnityEngine;

namespace AnimatorCoder_main
{
    public abstract class AnimatorCoder : MonoBehaviour
    {
        /// <summary> The baseline animation logic on a specific layer </summary>
        protected abstract void DefaultAnimation(int layer);
        private Animator _animator;
        private Animations[] _currentAnimation;
        private bool[] _layerLocked;
        private ParameterDisplay[] _parameters;
        private Coroutine _currentCoroutine = null;

        /// <summary> Sets up the Animator Brain </summary>
        public void Initialize()
        {
            AnimatorValues.Initialize();

            var animator = GetComponent<Animator>();
            _layerLocked = new bool[animator.layerCount];
            _currentAnimation = new Animations[animator.layerCount];
            this._animator = animator;

            for (int i = 0; i < animator.layerCount; ++i)
            {
                _layerLocked[i] = false;

                var hash = animator.GetCurrentAnimatorStateInfo(i).fullPathHash;
                for (var k = 0; k < AnimatorValues.Animations.Length; ++k)
                {
                    if (hash == AnimatorValues.Animations[i])
                    {
                        _currentAnimation[i] = (Animations)Enum.GetValues(typeof(Animations)).GetValue(k);
                        k = AnimatorValues.Animations.Length;
                    }
                }
            }

            var names = Enum.GetNames(typeof(Parameters));
            _parameters = new ParameterDisplay[names.Length];
            for (var i = 0; i < names.Length; ++i)
            {
                _parameters[i].name = names[i];
                _parameters[i].value = false;
            }
        }

        /// <summary> Returns the current animation that is playing </summary>
        public Animations GetCurrentAnimation(int layer)
        {
            try
            {
                return _currentAnimation[layer];
            }
            catch
            {
                LogError("Can't retrieve Current Animation. Fix: Initialize() in Start() and don't exceed number of animator layers");
                return Animations.RESET;
            }
        }

        /// <summary> Sets the whole layer to be locked or unlocked </summary>
        public void SetLocked(bool lockLayer, int layer)
        {
            try
            {
                _layerLocked[layer] = lockLayer;
            }
            catch
            {
                LogError("Can't retrieve Current Animation. Fix: Initialize() in Start() and don't exceed number of animator layers");
            }
        }

        public bool IsLocked(int layer)
        {
            try
            {
                return _layerLocked[layer];
            }
            catch
            {
                LogError("Can't retrieve Current Animation. Fix: Initialize() in Start() and don't exceed number of animator layers");
                return false;
            }
        }

        /// <summary> Sets an animator parameter </summary>
        public void SetBool(Parameters id, bool value)
        {
            try
            {
                _parameters[(int)id].value = value;
            }
            catch
            {
                LogError("Please Initialize() in Start()");
            }
        }

        /// <summary> Returns an animator parameter </summary>
        public bool GetBool(Parameters id)
        {
            try
            {
                return _parameters[(int)id].value;
            }
            catch
            {
                LogError("Please Initialize() in Start()");
                return false;
            }
        }

        /// <summary> Takes in the animation details and the animation layer, then attempts to play the animation </summary>
        public bool Play(AnimationData data, int layer = 0)
        {
            try
            {
                if (data.animation == Animations.RESET)
                {
                    DefaultAnimation(layer);
                    return false;
                }

                if (_layerLocked[layer] || _currentAnimation[layer] == data.animation) return false;

                if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
                _layerLocked[layer] = data.lockLayer;
                _currentAnimation[layer] = data.animation;

                _animator.CrossFade(AnimatorValues.GetHash(_currentAnimation[layer]), data.crossfade, layer);

                if (data.nextAnimation != null)
                {
                    _currentCoroutine = StartCoroutine(Wait());
                    IEnumerator Wait()
                    {
                        _animator.Update(0);
                        float delay = _animator.GetNextAnimatorStateInfo(layer).length;
                        if (data.crossfade == 0) delay = _animator.GetCurrentAnimatorStateInfo(layer).length;
                        yield return new WaitForSeconds(delay - data.nextAnimation.crossfade);
                        SetLocked(false, layer);
                        Play(data.nextAnimation, layer);
                    }
                }

                return true;
            }
            catch
            {
                LogError("Please Initialize() in Start()");
                return false;
            }
        }

        private void LogError(string message)
        {
            Debug.LogError("AnimatorCoder Error: " + message);
        }
    }

    /// <summary> Holds all data about an animation </summary>
    [Serializable]
    public class AnimationData
    {
        public Animations animation;
        /// <summary> Should the layer lock for this animation? </summary>
        public bool lockLayer;
        /// <summary> Should an animation play immediately after? </summary>
        public AnimationData nextAnimation;
        /// <summary> Should there be a transition time into this animation? </summary>
        public float crossfade = 0;

        /// <summary> Sets the animation data </summary>
        public AnimationData(Animations animation = Animations.RESET, bool lockLayer = false, AnimationData nextAnimation = null, float crossfade = 0)
        {
            this.animation = animation;
            this.lockLayer = lockLayer;
            this.nextAnimation = nextAnimation;
            this.crossfade = crossfade;
        }
    }

    /// <summary> Class the manages the hashes of animations and parameters </summary>
    public class AnimatorValues
    {
        /// <summary> Returns the animation hash array </summary>
        public static int[] Animations { get; private set; }

        private static bool _initialized = false;

        /// <summary> Initializes the animator state names </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            var names = Enum.GetNames(typeof(Animations));

            Animations = new int[names.Length];
            for (var i = 0; i < names.Length; i++)
                Animations[i] = Animator.StringToHash(names[i]);
        }

        /// <summary> Gets the animator hash value of an animation </summary>
        public static int GetHash(Animations animation) => Animations[(int)animation];
    }

    /// <summary> Allows the animation parameters to be shown in debug inspector </summary>
    [Serializable]
    public struct ParameterDisplay
    {
        [HideInInspector] public string name;
        public bool value;
    }
}
