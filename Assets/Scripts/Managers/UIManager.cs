using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

namespace TrainingBuddy.Managers
{
	public class UIManager : Singleton<UIManager>
	{
		public static UIManager instance;

		[SerializeField] private GameObject tutorialUI_01;
		[SerializeField] private GameObject tutorialUI_02;
		[SerializeField] private GameObject tutorialUI_03;
		[SerializeField] private GameObject loginUI;
		[SerializeField] private GameObject registerUI;
		[SerializeField] private GameObject profileUI;

		private new void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != null)
			{
				Debug.Log("Instance already exists, destroying object!");
				Destroy(this);
			}
		}

		private void ClearScreen()
		{
			loginUI.SetActive(false);
			registerUI.SetActive(false);
			profileUI.SetActive(false);
			tutorialUI_01.SetActive(false);
			tutorialUI_02.SetActive(false);
			tutorialUI_03.SetActive(false);
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

		public void ProfileScreen()
		{
			ClearScreen();
			profileUI.SetActive(true);
			if (DatabaseManager.Instance.Auth.CurrentUser == null)
			{
				LoginScreen();
				return;
			}

			if (GameManager.Instance.UserData.LocationUpdater != null)
			{
				StopCoroutine(GameManager.Instance.UserData.LocationUpdater);
				GameManager.Instance.UserData.LocationUpdater = null;
			}
			GameManager.Instance.UserData.LocationUpdater = StartCoroutine(GameManager.Instance.UserData.UpdateLocation());

			GameManager.Instance.UserData.LoadUserData();
		}
	}

	public enum UIScreen
	{
		RegisterScreen,
		LoginScreen,
		ProfileScreen,
	}
}