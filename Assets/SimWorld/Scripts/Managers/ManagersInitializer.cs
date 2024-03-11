using UnityEngine;

namespace SimWorld
{
	// The managers initialization should be the first thing when launching the game
	[DefaultExecutionOrder(-100)]
	public class ManagersInitializer : MonoBehaviour
    {
		[SerializeField]
		private NavigationManager navigationManagerImplementation;
		[SerializeField]
		private CameraManager cameraManagerImplementation;

		private void Awake()
		{
			InitManagers();
		}

		private void InitManagers()
		{
			INavigationManager navigationManager = navigationManagerImplementation;
			ICameraManager cameraManager = cameraManagerImplementation;
			
			navigationManager.InitializeManager();
			cameraManager.InitializeManager();

			Locator.Register<INavigationManager>(navigationManager);
			Locator.Register<ICameraManager>(cameraManager);

			DontDestroyOnLoad(gameObject); // Just because we use this object as the managers parent
		}
	}
}
