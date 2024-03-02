using System.Collections.Generic;
using UnityEngine;

namespace Player.Skills
{
    public class FocusSkill : Skill
    {
	    private List<SpriteSlicer2DSliceInfo> _slicedSpriteInfo = new();
        private TrailRenderer _trailRenderer;

        private struct MousePosition
        {
            public Vector3 WorldPosition;
            public float Time;
        }

        public float mouseRecordInterval = 0.05f;
        public int maxMousePositions = 5;
        public bool fadeFragments;

        private readonly List<MousePosition> _mousePositions = new();
        private float _mouseRecordTimer;

        protected override void Start () => _trailRenderer = GetComponentInChildren<TrailRenderer>();

        public void Slice () 
		{
			// Left mouse button - hold and swipe to cut objects
			var mousePositionAdded = false;
			_mouseRecordTimer -= Time.deltaTime;

			// Record the world position of the mouse every x seconds
			if(_mouseRecordTimer <= 0.0f)
			{
				MousePosition newMousePosition = new MousePosition();
				newMousePosition.WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				newMousePosition.Time = Time.time;

				_mousePositions.Add(newMousePosition);
				_mouseRecordTimer = mouseRecordInterval;
				mousePositionAdded = true;

				// Remove the first recorded point if we've recorded too many
				if(_mousePositions.Count > maxMousePositions) 
					_mousePositions.RemoveAt(0);
			}

			// Forget any positions that are too old to care about
			if(_mousePositions.Count > 0 && (Time.time - _mousePositions[0].Time) > mouseRecordInterval * maxMousePositions)
				_mousePositions.RemoveAt(0);

			// Go through all our recorded positions and slice any sprites that intersect them
			if(mousePositionAdded)
			{
				for(int loop = 0; loop < _mousePositions.Count - 1; loop++)
				{
					SpriteSlicer2D.SliceAllSprites(_mousePositions[loop].WorldPosition, _mousePositions[_mousePositions.Count - 1].WorldPosition, true, ref _slicedSpriteInfo);

					if(_slicedSpriteInfo.Count > 0)
					{
						// Add some force in the direction of the swipe so that stuff topples over rather than just being
						// sliced but remaining stationary
						for(int spriteIndex = 0; spriteIndex < _slicedSpriteInfo.Count; spriteIndex++)
						{
							for(int childSprite = 0; childSprite < _slicedSpriteInfo[spriteIndex].ChildObjects.Count; childSprite++)
							{
								Vector2 sliceDirection = _mousePositions[_mousePositions.Count - 1].WorldPosition - _mousePositions[loop].WorldPosition;
								sliceDirection.Normalize();
								_slicedSpriteInfo[spriteIndex].ChildObjects[childSprite].GetComponent<Rigidbody2D>().AddForce(sliceDirection * 500.0f);
							}
						}

						_mousePositions.Clear();
						break;
					}
				}
			}

			if(_trailRenderer)
			{
				Vector3 trailPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				trailPosition.z = -9.0f;
				_trailRenderer.transform.position = trailPosition;
			}
			//_mousePositions.Clear();

			// Sliced sprites sharing the same layer as standard Unity sprites could increase the draw call count as
			// the engine will have to keep swapping between rendering SlicedSprites and Unity Sprites.To avoid this, 
			// move the newly sliced sprites either forward or back along the z-axis after they are created
			for(int spriteIndex = 0; spriteIndex < _slicedSpriteInfo.Count; spriteIndex++)
			{
				for(int childSprite = 0; childSprite < _slicedSpriteInfo[spriteIndex].ChildObjects.Count; childSprite++)
				{
					Vector3 spritePosition = _slicedSpriteInfo[spriteIndex].ChildObjects[childSprite].transform.position;
					spritePosition.z = -1.0f;
					_slicedSpriteInfo[spriteIndex].ChildObjects[childSprite].transform.position = spritePosition;
				}
			}

			if(fadeFragments)
			{
				// If we've chosen to fade out fragments once an object is destroyed, add a fade and destroy component
				for (int spriteIndex = 0; spriteIndex < _slicedSpriteInfo.Count; spriteIndex++)
				{
					for (int childSprite = 0; childSprite < _slicedSpriteInfo[spriteIndex].ChildObjects.Count; childSprite++)
					{
						if (!_slicedSpriteInfo[spriteIndex].ChildObjects[childSprite].GetComponent<Rigidbody2D>().isKinematic)
							_slicedSpriteInfo[spriteIndex].ChildObjects[childSprite].AddComponent<FadeAndDestroy>();                    
					}
				}
			}

			_slicedSpriteInfo.Clear();
		}
        
        public void ClearMousePositions()
        {
	        _mousePositions.Clear();
	        _trailRenderer.Clear();
        }
    }
}