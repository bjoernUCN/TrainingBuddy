using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class UIManager : Singleton<UIManager>
	{
		[SerializeField] private GameObject tutorialUI_01;
		[SerializeField] private GameObject tutorialUI_02;
		[SerializeField] private GameObject tutorialUI_03;
		[SerializeField] private GameObject homeUI;
		[SerializeField] private GameObject loginUI;
		[SerializeField] private GameObject registerUI;
		[SerializeField] private GameObject profileUI;
		[SerializeField] private GameObject raceUI;

		private void ClearScreen()
		{
			homeUI.SetActive(false);
			loginUI.SetActive(false);
			registerUI.SetActive(false);
			profileUI.SetActive(false);
			tutorialUI_01.SetActive(false);
			tutorialUI_02.SetActive(false);
			tutorialUI_03.SetActive(false);
			raceUI.SetActive(false);
		}

		public void TutorialScreen01()
		{
			ClearScreen();
			tutorialUI_01.SetActive(true);
		}
		
		public void TutorialScreen02()
		{
			ClearScreen();
			tutorialUI_02.SetActive(true);
		}
		
		public void TutorialScreen03()
		{
			ClearScreen();
			tutorialUI_03.SetActive(true);
		}
		
		public void LoginScreen()
		{
			ClearScreen();
			loginUI.SetActive(true);
		}
		public void RegisterScreen()
		{
			ClearScreen();
			registerUI.SetActive(true);
		}

		public void HomeScreen()
		{
			ClearScreen();
			homeUI.SetActive(true);
			
			if (DatabaseManager.Instance.Auth.CurrentUser == null)
			{
				LoginScreen();
				return;
			}
			
			//TODO - Fix this
			GameManager.Instance.UserData.StartStepCounter();
			GameManager.Instance.UserData.StartLocationUpdater();
		}

		public void RaceScreen()
		{
			ClearScreen();
			raceUI.SetActive(true);
		}
		
		public void ProfileScreen()
		{
			ClearScreen();
			profileUI.SetActive(true);

			GameManager.Instance.UserData.LoadUserData();
		}
		
		public void HighscoreScreen()
		{
			
		}
	}
}