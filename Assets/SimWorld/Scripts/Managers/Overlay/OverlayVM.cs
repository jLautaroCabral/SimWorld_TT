namespace SimWorld
{
	// TODO: Recheck the close logic
    public class OverlayVM : ViewModelBase
    {
		private static IOverlayManager OverlayManager => Locator.Resolve<IOverlayManager>();

		private bool _alreadyClosed;

		protected override void Awake()
		{
			// Avoid to call initialize here because the OverlayManager already does 
		}

		/// <summary>
		/// Close the overlay.
		/// You have to call base.OnClose() at the end of your function if you override this.
		/// </summary>
		public virtual void OnClose()
		{
			OverlayManager.HideOverlay(this);
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
