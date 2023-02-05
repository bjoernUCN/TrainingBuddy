using System;
using TrainingBuddy.Managers;
using UnityEngine;

namespace TrainingBuddy.Navigation
{
	public class LoadScreen : MonoBehaviour
	{
		[SerializeField] private UIScreen _uiScreen;
		
		public void Load()
		{
			switch (_uiScreen)
			{
				case UIScreen.RegisterScreen:
					UIManager.instance.RegisterScreen();
					break;
				case UIScreen.LoginScreen:
					UIManager.instance.LoginScreen();
					break;
				case UIScreen.ProfileScreen:
					UIManager.instance.ProfileScreen();
					StartCoroutine(GameManager.Instance.UserData.LoadUserData());
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
