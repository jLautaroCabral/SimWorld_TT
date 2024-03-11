using System.Collections;
using System.Collections.Generic;
using Cinemachine;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimWorld
{
	public interface ICameraManager : IDependencyInjectable, ILocalizableManager
	{
		public Camera CurrentRenderingCamera { get; }
		public CinemachineVirtualCamera CurrentRenderingVirtualCamera { get; }
		public void SetActiveSpectatorCamera();
		public void SetFollowCameraTarget(Transform target, Transform worldUpOverride);
	}

	public class CameraManager : MonoBehaviour, ICameraManager
	{
		[field: Header("Read only")]
		[field: SerializeField]
		public Camera CurrentRenderingCamera { get; private set; }
		[field: SerializeField]
		public CinemachineVirtualCamera CurrentRenderingVirtualCamera { get; private set; }

		private SpectatorCamera _spectatorCamera;

		[Header("Follow camera settings")]
		[SerializeField]
		private CinemachineBrain followCameraBrain;
		[SerializeField]
		private CinemachineVirtualCamera followVirtualCamera;


		public void InitializeManager()
		{
			DontDestroyOnLoad(this);
			SceneManager.sceneLoaded += this.OnLoadCallback;
			Debug.Log("Camera Manager initialization successfully");
			SetActiveSpectatorCamera();
		}

		private void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
		{
			SetActiveSpectatorCamera(); // By default we active an spectator camera when the scene is loaded
		}

		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.OnLoadCallback;
		}

		public void SetActiveSpectatorCamera()
		{
			GetSpectatorCameraInScene().SetActive(true);
			SetFollowCameraActive(false);
			CurrentRenderingCamera = _spectatorCamera.GetComponent<Camera>();
			CurrentRenderingVirtualCamera = null;
		}

		private void SetFollowCameraActive(bool active)
		{
			followCameraBrain.gameObject.SetActive(active);
			followVirtualCamera.gameObject.SetActive(active);
			if (active)
			{
				CurrentRenderingCamera = followCameraBrain.GetComponent<Camera>();
				CurrentRenderingVirtualCamera = followVirtualCamera;
			}
			else
			{
				followVirtualCamera.Follow = null;
				followVirtualCamera.LookAt = null;
			}
		}

		public void SetFollowCameraTarget(Transform target, Transform worldUpOverride)
		{
			if (!target || !worldUpOverride)
			{
				Debug.LogError("Follow target or worldUpOverride are null");
				return;
			}

			GetSpectatorCameraInScene().SetActive(false);
			SetFollowCameraActive(true);
			followCameraBrain.m_WorldUpOverride = worldUpOverride;
			followVirtualCamera.Follow = target;
			followVirtualCamera.LookAt = target;
		}

		private GameObject GetSpectatorCameraInScene()
		{
			_spectatorCamera = FindFirstObjectByType<SpectatorCamera>();
			if (_spectatorCamera is null)
			{
				Debug.LogWarning("No spectator camera found, making new spectator camera obj");
				var _spectatorCameraObj = new GameObject("[Default Spectator Camera]");
				_spectatorCamera = _spectatorCameraObj.AddComponent<SpectatorCamera>();
				CurrentRenderingCamera = _spectatorCamera.GetComponent<Camera>();
				_spectatorCameraObj.transform.position = Vector3.zero;
			}

			return _spectatorCamera.gameObject;
		}
	}
}
