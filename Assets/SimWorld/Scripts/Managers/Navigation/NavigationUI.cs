using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimWorld
{
    public class NavigationUI : MonoBehaviour
    {
		[SerializeField]
		private Animator transitionCanvasAnimator;
		[SerializeField]
		private GameObject loadingScreen;
		[SerializeField]
		private Slider slider;
		[SerializeField]
		private TMP_Text loadingText;

		private const string FadeInTrigger = "FadeIn", FadeOffTrigger = "FadeOff";

		private Action _onAnimationEndCallback;

		public void UpdateLoadingText(string text)
		{
			loadingText.text = text;
		}

		public float LoadingSliderValue
		{
			get => slider.value;
			set => slider.value = value;
		}

		public void ShowLoadingScreen(bool show)
		{
			loadingScreen.SetActive(show);
		}

		public void StartAnimationFadeIn(Action onAnimationEndCallback = null)
		{
			_onAnimationEndCallback = onAnimationEndCallback;
			transitionCanvasAnimator.SetTrigger(FadeInTrigger);
		}

		public void StartAnimationFadeOff(Action onAnimationEndCallback = null)
		{
			_onAnimationEndCallback = onAnimationEndCallback;
			transitionCanvasAnimator.SetTrigger(FadeOffTrigger);
		}

		// Called by an animation event
		private void OnTransitionFadeInEnd()
		{
			_onAnimationEndCallback?.Invoke();
		}

		// Called by an animation event
		private void OnTransitionFadeOffEnd()
		{
			_onAnimationEndCallback?.Invoke();
		}
	}
}
