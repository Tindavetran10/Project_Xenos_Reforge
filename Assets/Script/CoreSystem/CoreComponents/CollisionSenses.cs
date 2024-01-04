using _Scripts.Generics;
using UnityEngine;

namespace _Scripts.CoreSystem.CoreComponents
{
    public class CollisionSenses : CoreComponent
    {
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
        private Movement _movement;

		#region Check Transforms

		public Transform GroundCheck {
			get => GenericNotImplementedError<Transform>.TryGet(groundCheck, Core.transform.parent.name);
			private set => groundCheck = value;
		}
		public Transform WallCheck {
			get => GenericNotImplementedError<Transform>.TryGet(wallCheck, Core.transform.parent.name);
			private set => wallCheck = value;
		}
		public Transform LedgeCheckHorizontal {
			get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, Core.transform.parent.name);
			private set => ledgeCheckHorizontal = value;
		}
		public Transform LedgeCheckVertical {
			get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, Core.transform.parent.name);
			private set => ledgeCheckVertical = value;
		}
		public Transform CeilingCheck {
			get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, Core.transform.parent.name);
			private set => ceilingCheck = value;
		}
		public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
		public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
		public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }


		[SerializeField] private Transform groundCheck;
		[SerializeField] private Transform wallCheck;
		[SerializeField] private Transform ledgeCheckHorizontal;
		[SerializeField] private Transform ledgeCheckVertical;
		[SerializeField] private Transform ceilingCheck;

		[SerializeField] private float groundCheckRadius;
		[SerializeField] private float wallCheckDistance;

		[SerializeField] private LayerMask whatIsGround;

		#endregion
		
		#region Physic2D
		public bool Ceiling => Physics2D.OverlapCircle(CeilingCheck.position, GroundCheckRadius, WhatIsGround);
		public bool Ground => Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
		public bool WallFront => Physics2D.Raycast(WallCheck.position, 
            Vector2.right * Movement.FacingDirection, WallCheckDistance, WhatIsGround);
		public bool WallBack => Physics2D.Raycast(WallCheck.position, 
            Vector2.right * -Movement.FacingDirection, WallCheckDistance, WhatIsGround);

		public bool LedgeHorizontal =>
			Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection,
				wallCheckDistance, whatIsGround);
		
		public bool LedgeVertical => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);

		#endregion


		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(ceilingCheck.position, groundCheckRadius);
			Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
			Gizmos.DrawWireSphere(wallCheck.position, groundCheckRadius);
			Gizmos.DrawWireSphere(ledgeCheckHorizontal.position, groundCheckRadius);
			Gizmos.DrawWireSphere(ledgeCheckVertical.position, groundCheckRadius);
		}
    }
}
