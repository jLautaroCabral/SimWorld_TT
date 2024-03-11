using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimWorld
{
    public class MainMenuScreenVM : MonoBehaviour
    {
		private static INavigationManager NavigationManager => Locator.Resolve<INavigationManager>();

		[SerializeField]
		private SceneReference nextScene;

		public void OnPlayPressed()
		{
			NavigationManager.NavigateToScene(nextScene);
		}
    }
}
