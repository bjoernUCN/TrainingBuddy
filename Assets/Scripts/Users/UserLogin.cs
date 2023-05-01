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
			StartCoroutine(LoginRoutine(emailLoginField.text, passwordLoginField.text));
		}
		
		public void TestLogin()
		{
			StartCoroutine(LoginRoutine("test@test.dk", "12341234"));
		}
		
		public void ClearLoginFields()
		{
		    emailLoginField.text = "";
		    passwordLoginField.text = "";
		}
		
		private IEnumerator LoginRoutine(string _email, string _password)
	    {
	        //Call the Firebase auth signin function passing the email and password
	        var LoginTask = FirebaseManager.Instance.Auth.SignInWithEmailAndPasswordAsync(_email, _password);
	        //Wait until the task completes
	        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
	    
	        if (LoginTask.Exception != null)
	        {
	            //If there are errors handle them
	            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
	            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
	            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
	    
	            string message = "Login Failed!";
	            switch (errorCode)
	            {
	                case AuthError.MissingEmail:
	                    message = "Missing Email";
	                    break;
	                case AuthError.MissingPassword:
	                    message = "Missing Password";
	                    break;
	                case AuthError.WrongPassword:
	                    message = "Wrong Password";
	                    break;
	                case AuthError.InvalidEmail:
	                    message = "Invalid Email";
	                    break;
	                case AuthError.UserNotFound:
	                    message = "Account does not exist";
	                    break;
	            }
	        }
	        else
	        {
		        //User is now logged in
		        //Now get the result
		        Debug.LogFormat("User signed in successfully: {0} ({1})", FirebaseManager.Instance.Auth.CurrentUser.DisplayName, FirebaseManager.Instance.Auth.CurrentUser.Email);

		        // StartCoroutine(LoadUserData());

		        yield return new WaitForSeconds(2);

		        // usernameField.text = _user.DisplayName;

		        UIManager.instance.ProfileScreen();
		        // warningLoginText.text = "";
	            // ClearLoginFields();
	            // ClearRegisterFields();
	        }
	    }
	}
}