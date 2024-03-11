using System.Collections;
using Fusion;
using UnityEngine;

namespace SimWorld
{
	[RequireComponent(typeof(Matchmaking), typeof(Networking))]
	public class NetworkManager : MonoBehaviour, ILocalizableManager, IDependencyInjectable
    {
		[field: SerializeField]
		public Matchmaking Matchmaking { get; private set; }

		[field: SerializeField]
		public Networking Networking { get; private set; }

		[field: Header("Other references")]
		[field: SerializeField]
		public NetworkRunner RunnerPrefab { get; set; }

		public NetworkRunner Runner { get; set; }
		public NetGame CurrentGame { get; set; }

		public PlayerRef LocalPlayerRef { get; set; }
		public PlayerRef ObservedPlayerRef { get; set; } // For now this one will be the same that LocalPlayer, will be useful later for spectator mode
		//public PlayerAgent ObservedAgent { get; set; } // For now this one will be the same that NetworkPlayer.ActiveAgent, will be useful later for spectator mode

		private void Awake()
		{
			Matchmaking = GetComponent<Matchmaking>();
			Networking = GetComponent<Networking>();
		}

		public void InitializeManager()
		{
			DontDestroyOnLoad(this);
			Debug.Log("Network Manager initialization successfully");
		}

		private IEnumerator StopGameCoroutine()
		{
			yield return new WaitForSeconds(3f);

			//CurrentGame.CurrentGameplayMode.StopGame();
		}
	}
}
