using System;
using Fusion.Photon.Realtime;
using Fusion;
using UnityEngine;
using System.Threading.Tasks;

namespace SimWorld
{
	public struct SessionRequest
	{
		public string UserID;
		public GameMode GameMode;
		public string DisplayName;
		public string SessionName;
		public string ScenePath;
		public int MaxPlayers;
		public int ExtraPeers;
		public string CustomLobby;
		public string IPAddress;
		public ushort Port;
	}

	// Additional matchmaking layer in case we will use third party matchmakign tools or multiplay
	// TODO: Check how to improve this for Unity Multiplay support, check if we should inherit or not
	public class Matchmaking : MonoBehaviour//, INetworkRunnerCallbacks
	{
		private static NetworkManager NetworkManager => Locator.Resolve<NetworkManager>();

		// PUBLIC MEMBERS

		public bool IsJoiningToLobby;
		public bool IsConnectedToLobby;

		public event Action LobbyJoined;
		public event Action<string, string> LobbyJoinFailed;
		public event Action LobbyLeft;
		public event Action JoiningToLobby;

		// PRIVATE MEMBERS

		[SerializeField]
		private NetworkRunner _lobbyRunner;

		private string _currentRegion;
		private int _currentRegionIndex = 0;

		public string LobbyName => _lobbyName;

		[Header("Debug only, Editor Only")]
		public GameMode JoinToGameAs = GameMode.AutoHostOrClient;

		// TODO: Check if needed, maybe it is better to implement a radio button
		// Tuples bro
		public (string regionToken, string regionName)[] AvailableRegions { get; private set; } =
		new[]
		{
			("us", "USA East"),
			("usw", "USA West"),
			("kr", "South Korea"),
			("sa", "South America"),
			("jp", "Japan"),
			("eu", "Europe"),
			("asia", "Asia"),
		};

		public int RegionIndex { get; private set; } = 0;

		private string _lobbyName => $"DevelopmentLobby_{RegionIndex}";
		private string _sessionName => $"DevelopmentSession_{RegionIndex}";

		public void IncreaseRegionIndex()
		{
			RegionIndex++;
			if (RegionIndex > AvailableRegions.Length - 1)
				RegionIndex = 0;
		}

		public void DecreaseRegionIndex()
		{
			RegionIndex--;
			if (RegionIndex < 0)
				RegionIndex = AvailableRegions.Length - 1;
		}

		private void Start()
		{
			PhotonAppSettings.Instance.AppSettings.FixedRegion = AvailableRegions[RegionIndex].regionToken;
		}

		private void Update()
		{
			if (IsConnectedToLobby == true && _currentRegionIndex != RegionIndex)
			{
				// Region changed, let's rejoin lobby
				JoinLobby2(true);
			}
		}

		public void JoinSession()
		{
			var request = new SessionRequest
			{
				//UserID = Context.PlayerData.UserID,
#if UNITY_EDITOR
				GameMode = JoinToGameAs,
#else
				GameMode = GameMode.AutoHostOrClient,
#endif
				CustomLobby = _lobbyName,
				//SessionName = _sessionName,
			};

			//NetworkManager.Networking.StartGame(request);
		}

		public async Task JoinLobby2(bool force = false)
		{
			if (IsJoiningToLobby == true)
				return;

			if (IsConnectedToLobby == true && force == false)
				return;

			IsJoiningToLobby = true;

			await LeaveLobby();

			_currentRegion = AvailableRegions[RegionIndex].regionToken;
			PhotonAppSettings.Instance.AppSettings.FixedRegion = _currentRegion;

			var joinTask = _lobbyRunner.JoinSessionLobby(SessionLobby.Custom, _lobbyName);
			await joinTask;

			IsJoiningToLobby = false;
			IsConnectedToLobby = joinTask.Result.Ok;

			if (IsConnectedToLobby == true)
			{
				Debug.Log("Joining lobby success");
				LobbyJoined?.Invoke();
			}
			else
			{
				Debug.LogError($"Joining lobby failure, result's error message: {joinTask.Result.ErrorMessage}");
				LobbyJoinFailed?.Invoke(_currentRegion, joinTask.Result.ErrorMessage);
			}
		}

		public async Task LeaveLobby()
		{
			if (IsConnectedToLobby == true)
			{
				LobbyLeft?.Invoke();
			}

			IsConnectedToLobby = false;

			// HACK: Adding shutdown reason 'PhotonCloudTimeout' will prevent early return and cloud services
			// will be cleaned up (hulled) correctly. Without cleaned up services, rejoining lobby will always fail.
			// TODO: Can be removed after Fusion SDK fix
			await _lobbyRunner.Shutdown(false, ShutdownReason.PhotonCloudTimeout);
		}
	}

}
