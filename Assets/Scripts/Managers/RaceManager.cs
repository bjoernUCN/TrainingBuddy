using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class RaceManager : Singleton<RaceManager>
	{
		[SerializeField] private GameObject mainUI;
		[SerializeField] private GameObject hostUI;
		
		private void ClearScreen()
		{
			mainUI.SetActive(false);
			hostUI.SetActive(false);
		}
		
		public void MainScreen()
		{
			ClearScreen();
			mainUI.SetActive(true);
		}
		
		public void HostRaceScreen()
		{
			ClearScreen();
			hostUI.SetActive(true);
		}
	}
}