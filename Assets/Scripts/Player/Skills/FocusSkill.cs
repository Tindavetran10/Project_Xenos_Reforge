using System.Collections.Generic;
using System.Linq;
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
			if(Input.GetMouseButton(0))
			{
				bool mousePositionAdded = false;
				_mouseRecordTimer -= Time.deltaTime;

				// Record the world position of the mouse every x seconds
				if(_mouseRecordTimer <= 0.0f)
				{
					if (Camera.main != null)
					{
						MousePosition newMousePosition = new MousePosition
						{
							WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition),
							Time = Time.time
						};

						_mousePositions.Add(newMousePosition);
					}

					_mouseRecordTimer = mouseRecordInterval;
					mousePositionAdded = true;

					// Remove the first recorded point if we've recorded too many
					if(_mousePositions.Count > maxMousePositions) _mousePositions.RemoveAt(0);
				}

				// Forget any positions that are too old to care about
				if(_mousePositions.Count > 0 && (Time.time - _mousePositions[0].Time) > mouseRecordInterval * maxMousePositions)
					_mousePositions.RemoveAt(0);

				// Go through all our recorded positions and slice any sprites that intersect them
				if(mousePositionAdded)
				{
					for(int loop = 0; loop < _mousePositions.Count - 1; loop++)
					{
						SpriteSlicer2D.SliceAllSprites(_mousePositions[loop].WorldPosition, _mousePositions[^1].WorldPosition, true, ref _slicedSpriteInfo);

						if(_slicedSpriteInfo.Count > 0)
						{
							// Add some force in the direction of the swipe so that stuff topples over rather than just being
							// sliced but remaining stationary
							foreach (var t in _slicedSpriteInfo)
							{
								foreach (var t1 in t.ChildObjects)
								{
									Vector2 sliceDirection = _mousePositions[^1].WorldPosition - _mousePositions[loop].WorldPosition;
									sliceDirection.Normalize();
									t1.GetComponent<Rigidbody2D>().AddForce(sliceDirection * 500.0f);
								}
							}

							_mousePositions.Clear();
							break;
						}
					}
				}

				if(_trailRenderer)
				{
					if (Camera.main != null)
					{
						Vector3 trailPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						trailPosition.z = -9.0f;
						_trailRenderer.transform.position = trailPosition;
					}
				}
			}
			else _mousePositions.Clear();

			// Sliced sprites sharing the same layer as standard Unity sprites could increase the draw call count as
			// the engine will have to keep swapping between rendering SlicedSprites and Unity Sprites.To avoid this, 
			// move the newly sliced sprites either forward or back along the z-axis after they are created
			foreach (var info in _slicedSpriteInfo)
			{
				foreach (var childObject in info.ChildObjects)
				{
					Vector3 spritePosition = childObject.transform.position;
					spritePosition.z = -1.0f;
					childObject.transform.position = spritePosition;
				}
			}

	        if(fadeFragments)
	        {
		        // If we've chosen to fade out fragments once an object is destroyed, add a fade and destroy component
		        foreach (var childObject 
		                 in from info in _slicedSpriteInfo 
		                 from childObject in info.ChildObjects 
		                 where !childObject.GetComponent<Rigidbody2D>().isKinematic select childObject)
			        childObject.AddComponent<FadeAndDestroy>();
	        }

			_slicedSpriteInfo.Clear();
		}
    }
}