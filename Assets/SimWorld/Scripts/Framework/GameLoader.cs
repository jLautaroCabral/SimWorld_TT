using UnityEngine;

namespace SimWorld
{
    public class GameLoader : MonoBehaviour
    {
		private static INavigationManager NavigationManager => Locator.Resolve<INavigationManager>();

		[SerializeField]
		private float autoLoginDelay = 3;

		private void Start()
		{
			Application.targetFrameRate = 60;
			Invoke(nameof(LoadMainMenu), 1f);
		}

		private void LoadMainMenu()
		{
			NavigationManager.NavigateToHome();
		}
	}
}
