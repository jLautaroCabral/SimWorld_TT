using UnityEngine;

namespace SimWorld
{
	// The managers initialization should be the first thing when launching the game
	[DefaultExecutionOrder(-100)]
	public class ManagersInitializer : MonoBehaviour
    {
		[SerializeField]
		private NavigationManager navigationManagerImplementation;

		private void Awake()
		{
			InitManagers();
		}

		private void InitManagers()
		{
			INavigationManager navigationManager = navigationManagerImplementation;
			
			navigationManager.InitializeManager();
			
			Locator.Register<INavigationManager>(navigationManager);

			DontDestroyOnLoad(gameObject); // Just because we use this object as the managers parent
		}
	}
}
