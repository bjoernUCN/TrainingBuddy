using System.Collections;
using Firebase;
using Firebase.Auth;
using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;

namespace TrainingBuddy.Users
{
	public class UserRegister : MonoBehaviour
	{
		[Header("Register")]
		public TMP_InputField usernameRegisterField;
		public TMP_InputField emailRegisterField;
		public TMP_InputField passwordRegisterField;
		public TMP_InputField passwordRegisterVerifyField;
		
		public void Register()
		{
			StartCoroutine(RegisterRoutine(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
		}
		
		public void ClearRegisterFields()
		{
		    usernameRegisterField.text = "";
		    emailRegisterField.text = "";
		    passwordRegisterField.text = "";
		    passwordRegisterVerifyField.text = "";
		}
		
		private IEnumerator RegisterRoutine(string email, string password, string username)
	    {
	        if (username == "")
	        {
	            //If the username field is blank show a warning
	            // warningRegisterText.text = "Missing Username";
	        }
	        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
	        {
	            //If the password does not match show a warning
	            // warningRegisterText.text = "Password Does Not Match!";
	        }
	        else 
	        {
	            //Call the Firebase auth signin function passing the email and password
	            var RegisterTask = DatabaseManager.Instance.Auth.CreateUserWithEmailAndPasswordAsync(email, password);
	            //Wait until the task completes
	            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
	    
	            if (RegisterTask.Exception != null)
	            {
	                //If there are errors handle them
	                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
	                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
	                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
	    
	                string message = "Register Failed!";
	                switch (errorCode)
	                {
	                    case AuthError.MissingEmail:
	                        message = "Missing Email";
	                        break;
	                    case AuthError.MissingPassword:
	                        message = "Missing Password";
	                        break;
	                    case AuthError.WeakPassword:
	                        message = "Weak Password";
	                        break;
	                    case AuthError.EmailAlreadyInUse:
	                        message = "Email Already In Use";
	                        break;
	                }
	                // warningRegisterText.text = message;
	            }
	            else
	            {
	    
	                if (DatabaseManager.Instance.Auth.CurrentUser != null)
	                {
	                    //Create a user profile and set the username
	                    UserProfile profile = new UserProfile{DisplayName = username};
	    
	                    //Call the Firebase auth update user profile function passing the profile with the username
	                    var ProfileTask = DatabaseManager.Instance.Auth.CurrentUser.UpdateUserProfileAsync(profile);
	                    //Wait until the task completes
	                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
	    
	                    if (ProfileTask.Exception != null)
	                    {
	                        //If there are errors handle them
	                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
	                        // warningRegisterText.text = "Username Set Failed!";
	                    }
	                    else
	                    {
	                        //Username is now set
	                        //Now return to login screen
	                        
	                        DatabaseManager.Instance.WriteCurrentUserData("UserName", username);
	                        DatabaseManager.Instance.WriteCurrentUserData("SkillPoints", 0);
	                        DatabaseManager.Instance.WriteCurrentUserData("AccelerationPoints", 0);
	                        DatabaseManager.Instance.WriteCurrentUserData("SpeedPoints", 0);
	                        DatabaseManager.Instance.WriteCurrentUserData("ExperiencePoints", 0);
	                        DatabaseManager.Instance.WriteCurrentUserData("Level", 1);
	                        DatabaseManager.Instance.WriteCurrentUserData("Longitude", -1);
	                        DatabaseManager.Instance.WriteCurrentUserData("Latitude", -1);
	                        DatabaseManager.Instance.WriteCurrentUserData("StepSnapshot", -1);
	                        DatabaseManager.Instance.WriteCurrentUserData("StepCount", -1);
	                        DatabaseManager.Instance.WriteCurrentUserData("Lobby", "");
	                        
	                        UIManager.Instance.LoginScreen();
	                    }
	                }
	            }
	        }
	    }
	}
}