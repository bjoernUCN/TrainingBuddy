using TrainingBuddy.Races;
using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class RaceManager : Singleton<RaceManager>
	{
		[SerializeField] private GameObject mainUI;
		[SerializeField] private GameObject hostUI;
		[SerializeField] private GameObject joinUI;
		[SerializeField] private RaceList RaceList;
		
		private GameObject raceListUI => RaceList.gameObject;
		
		private void ClearScreen()
		{
			mainUI.SetActive(false);
			hostUI.SetActive(false);
			joinUI.SetActive(false);
			raceListUI.SetActive(false);
		}
		
		public void MainScreen()
		{
			ClearScreen();
			mainUI.SetActive(true);
		}
		
		public void RaceListScreen()
		{
			ClearScreen();
			raceListUI.SetActive(true);

			GameManager.Instance.RaceData.FindNearbyRaces();
		}
		
		public void HostRaceScreen()
		{
			ClearScreen();
			hostUI.SetActive(true);
			
			GameManager.Instance.RaceData.HostRace();
		}
		
		public void JoinRaceScreen()
		{
			ClearScreen();
			joinUI.SetActive(true);
			
			GameManager.Instance.RaceData.JoinRace("j1KdMOVE83TeFevYOo2mnOTbZBZ2");
		}
	}
}