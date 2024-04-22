using System.Collections.Generic;
using UnityEngine;

namespace Background.New
{
    [ExecuteInEditMode]
    public class ParallaxBackground : MonoBehaviour
    {
        public ParallaxCamera parallaxCamera;
        private readonly List<ParallaxLayer> _parallaxLayers = new();

        private void Start()
        {
            if (parallaxCamera == null)
                if (Camera.main != null)
                    parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

            if (parallaxCamera != null)
                parallaxCamera.OnCameraTranslate += Move;

            SetLayers();
        }

        private void SetLayers()
        {
            _parallaxLayers.Clear();

            for (var i = 0; i < transform.childCount; i++)
            {
                var layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

                if (layer != null)
                {
                    layer.name = "Layer-" + i;
                    _parallaxLayers.Add(layer);
                }
            }
        }

        private void Move(float delta)
        {
            foreach (var layer in _parallaxLayers) layer.Move(delta);
        }
    }
}