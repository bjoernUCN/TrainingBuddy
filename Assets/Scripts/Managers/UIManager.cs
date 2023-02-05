using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class UIManager : Singleton<UIManager>
	{
		public static UIManager instance;

		[SerializeField] private GameObject loginUI;
		[SerializeField] private GameObject registerUI;
		[SerializeField] private GameObject ProfileUI;

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
			ProfileUI.SetActive(false);
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
			ProfileUI.SetActive(true);
			StartCoroutine(GameManager.Instance.UserData.LoadUserData());
		}
	}

	public enum UIScreen
	{
		RegisterScreen,
		LoginScreen,
		ProfileScreen,
	}
}