using TMPro;
using UnityEngine;

namespace SimWorld
{
    public class StandardDialogVM : DialogVM
    {
		[Header("Texts")]
		[SerializeField]
		private TMP_Text titleText;
		[SerializeField]
		private TMP_Text subtitleText;
		[SerializeField]
		private TMP_Text bodyText;
		[SerializeField]
		private TMP_Text smallBody;

		[Header("Buttons")]
		[SerializeField]
		private RectTransform cancelButton;
		[SerializeField]
		private RectTransform okButton;
		[SerializeField]
		private RectTransform closeButton;

		private DialogModel _dialogModel;

		public override void Initialize(params object[] viewModelArgs)
		{
			base.Initialize(viewModelArgs);
			if (viewModelArgs[0] is DialogModel dialogModel)
			{
				_dialogModel = dialogModel;
				titleText.text = _dialogModel.Title;
				subtitleText.text = _dialogModel.SubTitle;
				bodyText.text = _dialogModel.Body;
				smallBody.text = _dialogModel.SmallBody;

				okButton.gameObject.SetActive(_dialogModel.ShowOkButton);
				cancelButton.gameObject.SetActive(_dialogModel.ShowCancelButton);
				closeButton.gameObject.SetActive(_dialogModel.ShowCloseButton);
			}
			else
			{
				Debug.LogError("Standard dialog initialization error");
			}
		}

		public void OnOkPressed()
		{
			_dialogModel.OnOkButtonPressed?.Invoke();
			OnClose();
		}

		public void OnCancelPressed()
		{
			_dialogModel.OnCancelButtonPressed?.Invoke();
			OnClose();
		}

		public override void OnClose()
		{
			_dialogModel.OnCloseDialog?.Invoke();
			base.OnClose();
		}
	}
}
