using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace SimWorld
{
    public class NetGame : NetworkBehaviour
    {
		/*
		private static NetworkManager NetworkManager => Locator.Resolve<NetworkManager>();

		[Networked, HideInInspector, Capacity(byte.MaxValue)]
		public NetworkArray<NetworkPlayer> Players => default;
		public int ActivePlayerCount => GetActivePlayerCount();
		public GameplayMode CurrentGameplayMode { get; private set; }

		// PRIVATE MEMBERS
		[SerializeField]
		private NetworkPlayer _playerPrefab;
		[SerializeField]
		private GameplayMode[] _modePrefabs;


		private PlayerRef _localPlayer; // TODO: check if it is finally neded, local player change handling
		private Dictionary<PlayerRef, NetworkPlayer> _pendingPlayers = new Dictionary<PlayerRef, NetworkPlayer>();
		private Dictionary<string, NetworkPlayer> _disconnectedPlayers = new Dictionary<string, NetworkPlayer>();
		private FusionCallbacksHandler _fusionCallbacks = new FusionCallbacksHandler();
		private bool _isActive;

		public override void Spawned()
		{
			base.Spawned();
			Debug.Log($"NETWORK GAME SPAWNED, PEER MODE: {Runner.Config.PeerMode.ToString()}");
		}

		// PUBLIC METHODS
		public void Initialize(EGameplayType gameplayType)
		{
			NetworkManager.CurrentGame = this;
			if (Object.HasStateAuthority)
			{
				var prefab = _modePrefabs.Find(t => t.Type == gameplayType);
				CurrentGameplayMode = Runner.Spawn(prefab);
				Debug.Log("GameplayMode spawned");
			}
			else
			{
				CurrentGameplayMode = FindObjectOfType<GameplayMode>();
			}

			_localPlayer = Runner.LocalPlayer;
			_fusionCallbacks.DisconnectedFromServer -= OnDisconnectedFromServer;
			_fusionCallbacks.DisconnectedFromServer += OnDisconnectedFromServer;
			Runner.RemoveCallbacks(_fusionCallbacks);
			Runner.AddCallbacks(_fusionCallbacks); ;
		}

		public void Activate()
		{
			_isActive = true;
			if (!Object.HasStateAuthority)
			{
				if (ApplicationSettings.IsStrippedBatch)
				{
					Runner.GetComponent<NetworkPhysicsSimulation3D>().enabled = false;
					Runner.LagCompensation.enabled = false;
				}
				return;
			}

			CurrentGameplayMode.Activate();

			foreach (var playerRef in Runner.ActivePlayers)
			{
				SpawnPlayer(playerRef);
			}
		}

		public NetworkPlayer GetPlayer(PlayerRef playerRef)
		{
			if (playerRef.IsValid == false)
			{
				Debug.LogWarning($"NetworkGame.cs:80 - playerRef is not valid. {playerRef}");
				return null;
			}
			if (Object == null)
			{
				Debug.LogWarning($"NetworkGame.cs:84 - Object is null. {playerRef}");
				return null;
			}

			return Players[playerRef];
		}

		private int GetActivePlayerCount()
		{
			int players = 0;

			foreach (NetworkPlayer player in Players)
			{
				if (player == null)
					continue;

				var statistics = player.Statistics;
				if (!statistics.IsValid)
					continue;

				if (!statistics.IsEliminated)
				{
					players++;
				}
			}

			return players;
		}

		// NetworkBehaviour INTERFACE

		public override void FixedUpdateNetwork()
		{

			if (Object.HasStateAuthority == false)
				return;

			if (_pendingPlayers.Count == 0)
				return;

			var playersToRemove = new List<PlayerRef>();

			foreach (var playerPair in _pendingPlayers)
			{
				var playerRef = playerPair.Key;
				var player = playerPair.Value;

				if (player.IsInitialized == false)
					continue;

				playersToRemove.Add(playerRef);

				if (_disconnectedPlayers.TryGetValue(player.UserID, out NetworkPlayer disconnectedPlayer) == true)
				{
					// Remove original player, this is returning disconnected player
					Runner.Despawn(player.Object);
					Debug.Log($"NetworkGame.cs:154 - player.Object despawned for {player.UserID}");

					_disconnectedPlayers.Remove(player.UserID);
					player = disconnectedPlayer;

					player.Object.AssignInputAuthority(playerRef);
				}

				Players.Set(playerRef, player);
#if UNITY_EDITOR
				player.gameObject.name = $"[Player {player.Nickname}]";
#endif

				CurrentGameplayMode.PlayerJoined(player);
			}

			for (int i = 0; i < playersToRemove.Count; i++)
			{
				_pendingPlayers.Remove(playersToRemove[i]);
			}
		}

		// IPlayerJoined/IPlayerLeft INTERFACES

		void IPlayerJoined.PlayerJoined(PlayerRef playerRef)
		{
			if (!Runner.IsServer)
				return;
			if (!_isActive)
				return;

			SpawnPlayer(playerRef);
		}

		void IPlayerLeft.PlayerLeft(PlayerRef playerRef)
		{
			if (!playerRef.IsValid)
				return;
			if (!Runner.IsServer)
				return;
			if (!_isActive)
				return;

			NetworkPlayer player = Players[playerRef];

			Players.Set(playerRef, null);

			if (player != null)
			{
				if (player.UserID != null)
				{
					if (CurrentGameplayMode.Type != EGameplayType.OpenWorld && CurrentGameplayMode.Type != EGameplayType.SocialHub)
					{
						_disconnectedPlayers[player.UserID] = player;
					}
					Debug.Log($"NetworkGame:206 - {player.UserID} disconnected");

					CurrentGameplayMode.PlayerLeft(player);

					player.Object.RemoveInputAuthority();

#if UNITY_EDITOR
					player.gameObject.name = $"{player.gameObject.name} (Disconnected)";
#endif
				}
				else
				{
					CurrentGameplayMode.PlayerLeft(player);

					// Player wasn't initilized properly, safe to despawn
					Runner.Despawn(player.Object);
					Debug.Log($"NetworkGame.cs:222 - player.Object despawned for {player.UserID}");
					//GenericEventBus<OnNetworkGamePlayerLeft>.Dispatch(new OnNetworkGamePlayerLeft() { playerNickname = player.Nickname.Value });
				}
			}
			else
			{
				Debug.Log($"NetworkGame:228 - Player {playerRef} is null");
			}
		}

		private void SpawnPlayer(PlayerRef playerRef)
		{
			if (Players[playerRef] != null || _pendingPlayers.ContainsKey(playerRef) == true)
			{
				Log.Error($"Player for {playerRef} is already spawned!");
				return;
			}

			NetworkPlayer player = Runner.Spawn(_playerPrefab, inputAuthority: playerRef);

			Runner.SetPlayerAlwaysInterested(playerRef, player.Object, true);
			_pendingPlayers[playerRef] = player;

			player.gameObject.name = $"[Player: {playerRef}]";
			Debug.Log($"NetworkGame.cs:248 - [Player: {playerRef}] spawned for {playerRef}");

			//if (Runner.IsServer) // This should be handled in a GameplayMode class
			//{
			//	player.RPC_SetTeam(ActivePlayerCount % 2 == 0 ? 0 : 1); // If active players is pair (0, 2, 4...), set the team to 1 // TODO: improve this
			//}
		}

		private void OnDisconnectedFromServer(Fusion.NetworkRunner runner)
		{
			if (runner != null)
			{
				//runner.Simulation.SetLocalPlayer(_localPlayer); // TODO: check if it is finally neded, local player change handling
			}
		}
		*/
	}
}
