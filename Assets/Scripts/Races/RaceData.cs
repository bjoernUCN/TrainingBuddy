using TrainingBuddy.Managers;
using UnityEngine;

namespace TrainingBuddy.Races
{
	public class RaceData : MonoBehaviour
	{
		public void HostRace()
		{
			DatabaseManager.Instance.HostLobby();
		}
		
		public void JoinRace(string lobbyId)
		{
			DatabaseManager.Instance.JoinLobby(lobbyId);
			RaceManager.Instance.LobbyScreen(lobbyId);
		}
	}
}