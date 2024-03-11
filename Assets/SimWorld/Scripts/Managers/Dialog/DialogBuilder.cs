using System;

namespace SimWorld
{
    public class DialogBuilder
    {
		private DialogModel _dialogModel;
		private static DialogBuilder _builderInstance;

		private DialogBuilder()
		{
		}

		public static DialogBuilder StartBuilder()
		{
			_builderInstance = new DialogBuilder
			{
				_dialogModel = new DialogModel()
			};

			return _builderInstance;
		}

		public DialogModel GetBuiltDialog()
		{
			return _builderInstance._dialogModel;
		}

		public DialogBuilder AddTitle(string titleText)
		{
			_builderInstance._dialogModel.Title = titleText;
			return _builderInstance;
		}

		public DialogBuilder AddSubTitle(string subTitleText)
		{
			_builderInstance._dialogModel.SubTitle = subTitleText;
			return _builderInstance;
		}

		public DialogBuilder AddBody(string bodyText)
		{
			_builderInstance._dialogModel.Body = bodyText;
			return _builderInstance;
		}

		public DialogBuilder AddSmallBody(string smallBodyText)
		{
			_builderInstance._dialogModel.SmallBody = smallBodyText;
			return _builderInstance;
		}

		public DialogBuilder AddOkButton(Action buttonCallback = null)
		{
			_builderInstance._dialogModel.OnOkButtonPressed = buttonCallback;
			_builderInstance._dialogModel.ShowOkButton = true;
			return _builderInstance;
		}

		public DialogBuilder AddCancelButton(Action buttonCallback = null)
		{
			_builderInstance._dialogModel.OnCancelButtonPressed = buttonCallback;
			_builderInstance._dialogModel.ShowCancelButton = true;
			return _builderInstance;
		}

		public DialogBuilder AddOnCloseCallback(Action onCloseCallback)
		{
			_builderInstance._dialogModel.OnCloseDialog = onCloseCallback;
			return _builderInstance;
		}

		public DialogBuilder HideCloseButton()
		{
			_builderInstance._dialogModel.ShowCloseButton = true;
			return _builderInstance;
		}
	}
}
