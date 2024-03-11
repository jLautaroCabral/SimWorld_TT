using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimWorld
{
	public interface IOverlayManager : IDependencyInjectable, ILocalizableManager
	{
		public void DisplayOverlay(OverlayVM overlayPrefab, params object[] initParams);
		public void HideOverlay(OverlayVM overlayInstance);
		public void HideAllStackedOverlays();
	}

	public class OverlayManager : MonoBehaviour, IOverlayManager
	{
		[SerializeField]
		private RectTransform overlaysParent;

		private Stack<(OverlayVM viewModelPrefab, object[] initParams)> _overlaysToDisplay;
		private OverlayVM _currentDisplayingOverlay;

		private List<OverlayVM> _instantiatedOverlays;

		private void OnDisable()
		{
			foreach (var overlay in _instantiatedOverlays)
			{
				Destroy(overlay.gameObject);
			}
		}

		public void InitializeManager()
		{
			_overlaysToDisplay = new Stack<(OverlayVM, object[])>();
			_instantiatedOverlays = new List<OverlayVM>();

			DontDestroyOnLoad(this);
			Debug.Log("Overlay Manager initialization successfully");
		}

		public void DisplayOverlay(OverlayVM overlayPrefab, params object[] initParams)
		{
			if (_overlaysToDisplay.Any(tuple => tuple.viewModelPrefab.GetType() == overlayPrefab.GetType()))
			{
				Debug.LogWarning("Overlay type duplication, aborting overlay spawn");
				return;
			}
			_overlaysToDisplay.Push((overlayPrefab, initParams));
			CheckOverlaysToDisplay();
		}

		public void HideOverlay(OverlayVM overlayInstance)
		{
			if (_overlaysToDisplay.Count == 0 || _instantiatedOverlays.Count == 0)
				return;

			if (_currentDisplayingOverlay == overlayInstance)
			{
				// At this line, this should do a pop of the exact value of _currentDisplayingOverlay
				_overlaysToDisplay.Pop();
				_instantiatedOverlays.Remove(overlayInstance);
				_currentDisplayingOverlay.gameObject.SetActive(false);
				Destroy(_currentDisplayingOverlay.gameObject);

				_currentDisplayingOverlay = null;


				Debug.Log("Hiding current overlay");

				CheckOverlaysToDisplay();
			}
		}

		public void HideAllStackedOverlays()
		{
			Debug.Log("Hiding all stacked overlays");
			foreach (var overlay in _instantiatedOverlays)
			{
				overlay.gameObject.SetActive(false);
				Destroy(overlay.gameObject);
			}

			_overlaysToDisplay.Clear();
			_instantiatedOverlays.Clear();

			// Do a last check just in case
			CheckOverlaysToDisplay();
		}

		private void CheckOverlaysToDisplay()
		{
			if (_overlaysToDisplay.Count == 0) return;

			var peekedOverlay = _overlaysToDisplay.Peek();

			// Search if we already instantiated it
			OverlayVM overlayInstanceToDisplay =
				_instantiatedOverlays.FirstOrDefault(item => item.GetType() == peekedOverlay.viewModelPrefab.GetType());

			// If not, let's instantiate it
			if (overlayInstanceToDisplay == null)
			{
				overlayInstanceToDisplay = Instantiate(peekedOverlay.viewModelPrefab, overlaysParent.transform);
				overlayInstanceToDisplay.Initialize(peekedOverlay.initParams);
				_instantiatedOverlays.Add(overlayInstanceToDisplay);
			}

			_currentDisplayingOverlay = overlayInstanceToDisplay;
		}
	}
}
