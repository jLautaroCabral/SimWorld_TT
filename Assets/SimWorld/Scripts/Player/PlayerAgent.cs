using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimWorld
{
    public class PlayerAgent : MonoBehaviour
    {
		private static ICameraManager CameraManager => Locator.Resolve<ICameraManager>();

		private void Start()
		{
			CameraManager.SetFollowCameraTarget(transform, transform);
		}
	}
}
