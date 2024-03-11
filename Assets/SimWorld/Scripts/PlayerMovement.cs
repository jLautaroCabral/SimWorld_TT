using UnityEngine;

namespace SimWorld
{
	[RequireComponent(typeof(LocalPlayerInput), typeof(Rigidbody2D))]
	public class PlayerMovement : MonoBehaviour
	{
		public LocalPlayerInput Input { get; private set; }

		public float Speed = 4.0f;

		private Rigidbody2D _rigidbody;

		private void Awake()
		{
			Input = GetComponent<LocalPlayerInput>();
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		void FixedUpdate()
		{
			var move = Input.RenderInput.MoveDirection;

			//note: == and != for vector2 is overriden to take in account floating point imprecision.
			//if (move != Vector2.zero)
			//{
			//	SetLookDirectionFrom(move);
			//}
			//else
			//{
			//	//we aren't moving, look direction is based on the currently aimed toward point
			//	if (IsMouseOverGameWindow())
			//	{
			//		Vector3 posToMouse = m_CurrentWorldMousePos - transform.position;
			//		SetLookDirectionFrom(posToMouse);
			//	}
			//}

			var movement = move * Speed;
			//var speed = movement.sqrMagnitude;

			//m_Animator.SetFloat(m_DirXHash, m_CurrentLookDirection.x);
			//m_Animator.SetFloat(m_DirYHash, m_CurrentLookDirection.y);
			//m_Animator.SetFloat(m_SpeedHash, speed);

			_rigidbody.MovePosition(_rigidbody.position + movement * Time.deltaTime);
		}
	}
}
