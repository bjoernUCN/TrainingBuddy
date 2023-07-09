using System.Threading.Tasks;
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
		
		public async void JoinRace(string lobbyId)
		{
			await DatabaseManager.Instance.JoinLobby(lobbyId);
			RaceManager.Instance.LobbyScreen(lobbyId);
		}
	}
}