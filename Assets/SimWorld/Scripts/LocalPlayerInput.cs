using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimWorld
{
	public struct SimWorldInputData
	{
		public Vector2 MoveDirection;
	}

	public class LocalPlayerInput : MonoBehaviour
    {
		/// <summary>
		/// Holds input for current frame render update.
		/// </summary>
		public SimWorldInputData RenderInput => _renderInput;

		private SimWorldInputData _renderInput;


		protected void Awake()
		{
			//_player = GetComponent<Player>();
		}

		private void Update()
		{
			ProcessStandaloneInput();
		}


		private void ProcessStandaloneInput()
		{
			Vector2 moveDirection = Vector2.zero;

			if (Input.GetKey(KeyCode.W) == true) { moveDirection += Vector2.up; }
			if (Input.GetKey(KeyCode.S) == true) { moveDirection += Vector2.down; }
			if (Input.GetKey(KeyCode.A) == true) { moveDirection += Vector2.left; }
			if (Input.GetKey(KeyCode.D) == true) { moveDirection += Vector2.right; }

			if (moveDirection != Vector2.zero)
			{
				moveDirection.Normalize();
			}

			_renderInput.MoveDirection = moveDirection;
		}
	}
}
