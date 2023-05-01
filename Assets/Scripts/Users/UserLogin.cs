using System;
using System.Collections;
using Firebase;
using Firebase.Auth;
using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TrainingBuddy.Users
{
	public class UserLogin : MonoBehaviour
	{
		[Header("Login")]
		public TMP_InputField emailLoginField;
		public TMP_InputField passwordLoginField;

		private Coroutine stepSnapshotRoutine;

		public void Login()
		{
			DatabaseManager.Instance.StartCoroutine(DatabaseManager.Instance.SignIn(emailLoginField.text, passwordLoginField.text));
		}
		
		public void TestLogin()
		{
			DatabaseManager.Instance.StartCoroutine(DatabaseManager.Instance.SignIn("test@test.dk", "12341234"));
		}
		
		public void ClearLoginFields()
		{
		    emailLoginField.text = "";
		    passwordLoginField.text = "";
		}
	}
}