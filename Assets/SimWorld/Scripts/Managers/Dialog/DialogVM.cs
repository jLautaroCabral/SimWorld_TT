using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimWorld
{
    public class DialogVM : ViewModelBase
    {
		private static IDialogManager DialogManager => Locator.Resolve<IDialogManager>();
		private bool _alreadyClosed;

		protected override void Awake()
		{
			// Avoid to call initialize here because the DialogManager already does 
		}

		/// <summary>
		/// Close the dialog.
		/// You have to call base.OnClose() at the end of your function if you override this.
		/// </summary>
		public virtual void OnClose()
		{
			DialogManager.HideDialog(this);
			_alreadyClosed = true;
		}

		private void OnDestroy()
		{
			if (!_alreadyClosed)
			{
				OnClose();
			}
		}
	}
}
