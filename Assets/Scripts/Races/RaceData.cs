using TrainingBuddy.Managers;
using UnityEngine;

namespace TrainingBuddy.Races
{
	public class RaceData : MonoBehaviour
	{
		public void HostRace()
		{
			DatabaseManager.Instance.CreateLobby();
		}
		
		public void JoinRace(string lobbyId)
		{
			DatabaseManager.Instance.JoinLobby(lobbyId);
		}
		
		public void FindNearbyRaces()
		{
			DatabaseManager.Instance.FindNearbyLobbies();
		}
	}
}