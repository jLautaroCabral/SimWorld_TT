using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimWorld
{
	public class DialogModel
	{
		public string Title;
		public string SubTitle;
		public string Body;
		public string SmallBody;

		public bool ShowOkButton;
		public bool ShowCancelButton;
		public bool ShowCloseButton;

		public Action OnOkButtonPressed;
		public Action OnCancelButtonPressed;
		public Action OnCloseDialog;
	}

	public interface IDialogManager : IDependencyInjectable, ILocalizableManager
	{
		public void DisplayStandardDialog(DialogModel dialogModel);
		public void DisplayDialog(DialogVM dialogViewModel, params object[] initParams);
		public void HideDialog(DialogVM dialogInstance);
		public void HideAllQueuedDialogs();
		public void DisplayBasicMessage(string message);
		public void DisplayBasicError(string message);
	}

	public class DialogManager : MonoBehaviour, IDialogManager
	{
		[SerializeField]
		private RectTransform dialogsParent;
		[SerializeField]
		private RectTransform dialogBackground;

		[SerializeField]
		private StandardDialogVM standardDialogPrefab;

		private Queue<(DialogVM viewModelPrefab, object[] initParams)> _dialogsToDisplay;
		private DialogVM _currentDisplayingDialog;
		//private List<DialogViewModel> _instantiatedDialogs; // No need of this so far

		private void OnDestroy()
		{
			dialogBackground.gameObject.SetActive(false);
			HideAllQueuedDialogs();
		}

		public void InitializeManager()
		{
			_dialogsToDisplay = new Queue<(DialogVM viewModelPrefab, object[] initParams)>();
			DontDestroyOnLoad(this);
			Debug.Log("Dialog Manager initialization successfully");
		}

		public void DisplayStandardDialog(DialogModel dialogModel)
		{
			_dialogsToDisplay.Enqueue((standardDialogPrefab, new object[] { dialogModel }));
			CheckDialogsToDisplay();
		}

		public void DisplayDialog(DialogVM dialogViewModel, params object[] initParams)
		{
			_dialogsToDisplay.Enqueue((dialogViewModel, initParams));
			CheckDialogsToDisplay();
		}

		public void DisplayBasicMessage(string message)
		{
			DisplayBasicMessage(message, "Game Message");
		}

		public void DisplayBasicMessage(string message, string title)
		{
			var dialogModel = DialogBuilder.StartBuilder()
				.AddTitle(title)
				.AddBody(message)
				.AddOkButton()
				.GetBuiltDialog();

			DisplayStandardDialog(dialogModel);
		}

		public void DisplayBasicError(string message)
		{
			var dialogModel = DialogBuilder.StartBuilder()
				.AddTitle("Error")
				.AddBody(message)
				.AddOkButton()
				.GetBuiltDialog();

			DisplayStandardDialog(dialogModel);
		}

		public void HideDialog(DialogVM dialogInstance)
		{
			if (_currentDisplayingDialog == dialogInstance)
			{
				// At this line, this should do a dequeue of the exact value of _currentDisplayingOverlay
				if (_dialogsToDisplay.Count > 0) _dialogsToDisplay.Dequeue();

				_currentDisplayingDialog?.gameObject?.SetActive(false);
				Destroy(_currentDisplayingDialog?.gameObject);

				_currentDisplayingDialog = null;

				Debug.Log("Hiding current dialog");

				dialogBackground.gameObject.SetActive(false);
				CheckDialogsToDisplay();
			}
			else
			{
				Debug.Log("Weird error, not equal dialog");
			}
		}

		public void HideAllQueuedDialogs()
		{
			Debug.Log("Hiding all queued dialogs");
			_dialogsToDisplay.Clear();
			HideDialog(_currentDisplayingDialog);

			// Do a last check just in case
			CheckDialogsToDisplay();
		}

		private void CheckDialogsToDisplay()
		{
			if (_dialogsToDisplay.Count == 0) return;
			if (_currentDisplayingDialog != null) return;

			var dialogModelToDisplay = _dialogsToDisplay.Peek();
			DialogVM dialogInstance = Instantiate(dialogModelToDisplay.viewModelPrefab, dialogsParent.transform);

			dialogInstance.Initialize(dialogModelToDisplay.initParams);
			_currentDisplayingDialog = dialogInstance;

			dialogBackground.gameObject.SetActive(true);
		}
	}
}
