using UnityEngine;

namespace SimWorld
{
	[RequireComponent(typeof(LocalPlayerInput))]
	public class PlayerAnimation : MonoBehaviour
	{
		[SerializeField]
		private Animator[] currentAnimators;
		private LocalPlayerInput _input;
		private const string idleParameter = "Idle";
		private const string walkParameter = "Walk";
		private string currentParameter;

		private void Awake()
		{
			_input = GetComponent<LocalPlayerInput>();
			//currentAnimators = GetComponentInChildren<Animator>();
		}
		private void Update()
		{
			if (_input.RenderInput.MoveDirection == Vector2.zero)
			{
				ToggleAnimation(idleParameter);
			}
			else
			{
				ToggleAnimation(walkParameter);
			}
			for (int i = 0; i < currentAnimators.Length; i++)
			{
				currentAnimators[i].SetFloat("X", _input.RenderInput.MoveDirection.x);
				currentAnimators[i].SetFloat("Y", _input.RenderInput.MoveDirection.y);
			}
			//currentAnimator.SetFloat("X", _input.RenderInput.MoveDirection.x);
			//currentAnimator.SetFloat("Y", _input.RenderInput.MoveDirection.y);
		}
		public void ToggleAnimation(string nextParameter)
		{
			if (currentParameter == nextParameter) return;
			for (int i = 0; i < currentAnimators.Length; i++)
			{
				currentAnimators[i].SetBool(currentParameter, false);
				currentAnimators[i].SetBool(nextParameter, true);
			}

			currentParameter = nextParameter;
		}
	}
}