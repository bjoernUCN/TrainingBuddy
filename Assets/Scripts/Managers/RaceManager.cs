using TrainingBuddy.Races;
using UnityEngine;
using UnityEngine.Serialization;

namespace TrainingBuddy.Managers
{
	public class RaceManager : Singleton<RaceManager>
	{
		[SerializeField] private GameObject mainUI;
		[SerializeField] private GameObject hostUI;
		[SerializeField] private GameObject lobbyUI;
		[SerializeField] private GameObject watchUI;
		[SerializeField] private RaceList RaceList;
		[SerializeField] private Lobby lobby;
		
		private GameObject raceListUI => RaceList.gameObject;
		
		private void ClearScreen()
		{
			mainUI.SetActive(false);
			hostUI.SetActive(false);
			lobbyUI.SetActive(false);
			watchUI.SetActive(false);
			raceListUI.SetActive(false);
		}
		
		public async void MainScreen()
		{
			var lobbyId = await DatabaseManager.Instance.IsInLobby();
			if (lobbyId != null)
			{
				LobbyScreen(lobbyId);
				return;
			}
			
			ClearScreen();
			mainUI.SetActive(true);
		}
		
		public async void RaceListScreen()
		{
			ClearScreen();
			raceListUI.SetActive(true);

			var lobbyList = await DatabaseManager.Instance.FindNearbyLobbies();
			RaceList.AddRaces(lobbyList);
		}
		
		public void HostRaceScreen()
		{
			ClearScreen();
			hostUI.SetActive(true);

			GameManager.Instance.RaceData.HostRace();
			LobbyScreen(DatabaseManager.Instance.Auth.CurrentUser.UserId);
		}
		
		public void LobbyScreen(string lobbyId)
		{
			ClearScreen();
			lobbyUI.SetActive(true);
			
			lobby.Initialize(lobbyId);
		}
		
		public void WatchRaceScreen()
		{
			ClearScreen();
			watchUI.SetActive(true);
		}
	}
}