using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Skills
{
    public class FocusSkill : Skill
    {
	    private List<SpriteSlicer2DSliceInfo> _slicedSpriteInfo = new();
        private TrailRenderer _trailRenderer;

        private Camera _mainCamera;
        
        #region Mouse Input

        private Vector3 mousePos;
        [HideInInspector] public Vector3 mouseWorldPos;
        
        private struct MousePosition
        {
            public Vector3 WorldPosition;
            public float Time;
        }

        public float mouseRecordInterval = 0.05f;
        public int maxMousePositions = 5;
        public float sliceForce = 2.0f;
        public LayerMask sliceLayer;
        public bool fadeFragments;

        private readonly List<MousePosition> _mousePositions = new();
        private float _mouseRecordTimer;
        
        [SerializeField] private float slicedSpriteZPosition = -1.0f;
        [SerializeField] private float trailZPosition = -9.0f;
        #endregion
        
        protected override void Start ()
        {
	        _trailRenderer = GetComponentInChildren<TrailRenderer>();
	        _mainCamera = Camera.main;
        }

        protected override void Update()
        {
	        base.Update();
	        if (Mouse.current.leftButton.IsPressed())
	        {
		        mousePos = Mouse.current.position.ReadValue();
	
		        mousePos.z = _mainCamera.nearClipPlane;
		        mouseWorldPos = _mainCamera.ScreenToWorldPoint(mousePos);
	        }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Slice ()
        {
			// Left mouse button - swipe to cut objects
			_mouseRecordTimer -= Time.deltaTime;

			// Record the world position of the mouse every x seconds
			var mousePositionAdded = RecordMousePosition(false);

			// Forget any positions that are too old to care about
			if (_mousePositions.Count > 0 && Time.time - _mousePositions[0].Time > mouseRecordInterval * maxMousePositions)
				_mousePositions.RemoveAt(0);

			// Go through all our recorded positions and slice any sprites that intersect them
			if (mousePositionAdded) SliceSpriteAndAddForce();
			
			MoveSpriteAlongZAxis();

			_slicedSpriteInfo.Clear();
		}

        private void SliceSpriteAndAddForce()
        {
	        for (var loop = 0; loop < _mousePositions.Count - 1; loop++)
	        {
		        SpriteSlicer2D.SliceAllSprites(_mousePositions[loop].WorldPosition, 
			        _mousePositions[^1].WorldPosition, true, 
			        ref _slicedSpriteInfo, sliceLayer);

		        if (_slicedSpriteInfo.Count > 0)
		        {
			        // Add some force in the direction of the swipe so that stuff topples over rather than just being
			        // sliced but remaining stationary
			        foreach (var info in _slicedSpriteInfo)
			        {
				        foreach (var child in info.ChildObjects)
				        {
					        Vector2 sliceDirection = _mousePositions[^1].WorldPosition - _mousePositions[loop].WorldPosition;
					        sliceDirection.Normalize();
					        child.GetComponent<Rigidbody2D>().AddForce(sliceDirection * sliceForce);
				        }
			        }

			        _mousePositions.Clear();
			        break;
		        }
	        }
        }

        private bool RecordMousePosition(bool mousePositionAdded)
        {
	        if (_mouseRecordTimer <= 0.0f)
	        {
		        if (_mainCamera != null)
		        {
			        var newMousePosition = new MousePosition
			        {
				        WorldPosition = mouseWorldPos,
				        Time = Time.time
			        };
			        _mousePositions.Add(newMousePosition);
		        }

		        _mouseRecordTimer = mouseRecordInterval;
		        mousePositionAdded = true;

		        // Remove the first recorded point if we've recorded too many
		        if (_mousePositions.Count > maxMousePositions)
			        _mousePositions.RemoveAt(0);
	        }

	        return mousePositionAdded;
        }
        
        private void MoveSpriteAlongZAxis()
        {
	        // Sliced sprites sharing the same layer as standard Unity sprites could increase the draw call count as
	        // the engine will have to keep swapping between rendering SlicedSprites and Unity Sprites.To avoid this, 
	        // move the newly sliced sprites either forward or back along the z-axis after they are created
	        foreach (var info in _slicedSpriteInfo)
	        {
		        foreach (var child in info.ChildObjects)
		        {
			        var spritePosition = child.transform.position;
			        spritePosition.z = slicedSpriteZPosition;
			        child.transform.position = spritePosition;
					
			        // Scale the fragments down to half their size
			        child.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		        }
	        }
        }
        
        public void Fragments()
        {
	        if(fadeFragments)
	        {
		        // Add a FadeAndDestroy script to each fragment so that they fade out and are destroyed after a few seconds
		        foreach (var child 
		                 in from info in _slicedSpriteInfo 
		                 from child in info.ChildObjects 
		                 where !child.GetComponent<Rigidbody2D>().isKinematic select child)
			        child.AddComponent<FadeAndDestroy>();
	        }
        }

        public void Trail()
        {
	        if(_trailRenderer)
	        {
		        if (_mainCamera != null)
		        {
			        var trailPosition = mouseWorldPos;
			        trailPosition.z = trailZPosition;
			        _trailRenderer.transform.position = trailPosition;
		        }
	        }
        }

        public void ClearMousePositions()
        {
	        _mousePositions.Clear();
	        _trailRenderer.Clear();
        }
    }
}