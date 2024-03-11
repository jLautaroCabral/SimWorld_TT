using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimWorld
{
	public interface INavigationManager : IDependencyInjectable, ILocalizableManager
	{
		public SceneReference DefaultGameplayScene { get; }
		public SceneReference DefaultHomeScene { get; }
		public void NavigateToScene(string sceneNameIndex);
		public void NavigateToScene(int sceneBuildIndex);
		public void ReloadCurrentScene();
		public void NavigateToHome();
		public void UpdateLoadingText(string text);
		public void LoadingFadeIn();
		public void LoadingFadeOff();
	}

	public class NavigationManager : MonoBehaviour, INavigationManager
	{
		[field: SerializeField]
		public SceneReference DefaultGameplayScene { get; set; }
		[field: SerializeField]
		public SceneReference DefaultHomeScene { get; set; }

		[Header("UI references")]
		[SerializeField]
		private NavigationUI navigationUI;


		// There is a dummy loading time, because I want to show the loading screen for two seconds even if the device can load the scene pretty fast
		private int CalculatedDefaultDummyWaiting => (DefaultDummyWaitingSeconds * 1000) / DefaultDummyWaitingSteps; // Miliseconds
		private const int DefaultDummyWaitingSteps = 4;
		private const int DefaultDummyWaitingSeconds = 2;
		private const float MaxSliderValue = 1f;

		public void InitializeManager()
		{
			DontDestroyOnLoad(this.gameObject);
			navigationUI.ShowLoadingScreen(false);
			Debug.Log("Navigation Manager initialization successfully");
		}

		// TODO: Temporal
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

		public void ReloadCurrentScene()
		{
			NavigateToScene(SceneManager.GetActiveScene().name);
		}

		public void NavigateToHome()
		{
			NavigateToScene(DefaultHomeScene);
		}

		public void NavigateToScene(string sceneName)
		{
			NavigateToScene(SceneUtility.GetBuildIndexByScenePath(sceneName));
		}

		public void UpdateLoadingText(string text)
		{
			navigationUI.UpdateLoadingText(text);
		}

		public void NavigateToScene(int sceneBuildIndex)
		{
			navigationUI.StartAnimationFadeIn(() =>
			{
				StartLoadingScreenDisplay(sceneBuildIndex);
			}); // Callback on animation end 
		}

		/// <summary>
		///  Fade in the loading screen, it's just the animation
		/// </summary>
		public void LoadingFadeIn()
		{
			navigationUI.StartAnimationFadeIn(() =>
			{
				navigationUI.ShowLoadingScreen(true);
				navigationUI.LoadingSliderValue = 0;
				navigationUI.StartAnimationFadeOff();
			}); // Callback on animation end 
		}

		/// <summary>
		///  Fade off the loading screen, it's just the animation
		/// </summary>
		public void LoadingFadeOff()
		{
			navigationUI.StartAnimationFadeIn(() =>
			{
				navigationUI.ShowLoadingScreen(false);
				navigationUI.StartAnimationFadeOff();
			});
		}

		private void StartLoadingScreenDisplay(int sceneBuildIndex)
		{
			navigationUI.ShowLoadingScreen(true);
			navigationUI.LoadingSliderValue = 0;
			navigationUI.StartAnimationFadeOff(() => _ = LoadSceneAsync(sceneBuildIndex));
		}

		private async Task LoadSceneAsync(int sceneBuildIndex)
		{
			Debug.Log($"Starting Async Loading");
			await Task.Delay(CalculatedDefaultDummyWaiting); // Wait a little before start the loading
			navigationUI.LoadingSliderValue = MaxSliderValue * 0.1f; // let's force a 10% load on init

			await Task.Delay(CalculatedDefaultDummyWaiting); // Wait a little before start to load the next scene
			Debug.Log("Scene loading started");
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);
			float sliderValueBeforeOperation = navigationUI.LoadingSliderValue;

			while (!asyncOperation.isDone)
			{
				Debug.Log($"Async Scene Loading: {asyncOperation.progress}");

				navigationUI.LoadingSliderValue = sliderValueBeforeOperation + (((MaxSliderValue / 2) - sliderValueBeforeOperation) * asyncOperation.progress);

				if (asyncOperation.progress >= 0.9f)
				{
					Debug.Log($"Async Loading Completed");
					navigationUI.LoadingSliderValue = MaxSliderValue / 2; // Half reached
					break;
				}
				await Task.Yield();
			}

			await Task.Delay(CalculatedDefaultDummyWaiting); // Wait a little before start to initialize the next scene
			Debug.Log("Searching level initializer");

			navigationUI.LoadingSliderValue = MaxSliderValue;

			await Task.Delay(CalculatedDefaultDummyWaiting); // Wait a little before start the fade off

			navigationUI.StartAnimationFadeIn(() =>
			{
				navigationUI.ShowLoadingScreen(false);
				navigationUI.StartAnimationFadeOff();
			});
		}
	}
}
