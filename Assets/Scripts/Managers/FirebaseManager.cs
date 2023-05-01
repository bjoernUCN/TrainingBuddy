// using System.Collections;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
// using Firebase;
// using Firebase.Auth;
// using Firebase.Database;
//
// namespace TrainingBuddy.Managers
// {
// 	public class FirebaseManager : Singleton<FirebaseManager>
// 	{
// 		public FirebaseAuth Auth { get; set; }
//
// 		// Firebase database reference
// 		public DatabaseReference databaseReference { get; private set; }
//
// 		private DependencyStatus _dependencyStatus;
//
// 		private void Awake()
// 		{
// 			// Initialize Firebase
// 			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
// 			{
// 				_dependencyStatus = task.Result;
// 				if (_dependencyStatus == DependencyStatus.Available)
// 				{
// 					//If they are avalible Initialize Firebase
// 					Auth = FirebaseAuth.DefaultInstance;
// 					databaseReference = FirebaseDatabase.GetInstance("https://trainingbuddy-81bca-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
// 				}
// 				else
// 				{
// 					Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
// 				}
// 			});
//
// 			databaseReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance).RootReference;
// 		}
//
// 		// Method for writing data to the database
// 		public void WriteData(string path, string data)
// 		{
// 			databaseReference.Child(path).SetRawJsonValueAsync(data);
// 		}
//
// 		// Method for reading data from the database
// 		public async Task<DataSnapshot> ReadUserData(string path)
// 		{
// 			// var tcs = new TaskCompletionSource<DataSnapshot>();
// 			
// 			var snapshot = await databaseReference.Child("Users").Child(Auth.CurrentUser.UserId).Child(path).GetValueAsync();
// 			
// 			return snapshot;
// 		}
//
// 		public IEnumerator SignIn(string _email, string _password)
// 	    {
// 	        //Call the Firebase auth signin function passing the email and password
// 	        var LoginTask = Auth.SignInWithEmailAndPasswordAsync(_email, _password);
// 	        //Wait until the task completes
// 	        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
// 	    
// 	        if (LoginTask.Exception != null)
// 	        {
// 	            //If there are errors handle them
// 	            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
// 	            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
// 	            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
// 	    
// 	            string message = "Login Failed!";
// 	            switch (errorCode)
// 	            {
// 	                case AuthError.MissingEmail:
// 	                    message = "Missing Email";
// 	                    break;
// 	                case AuthError.MissingPassword:
// 	                    message = "Missing Password";
// 	                    break;
// 	                case AuthError.WrongPassword:
// 	                    message = "Wrong Password";
// 	                    break;
// 	                case AuthError.InvalidEmail:
// 	                    message = "Invalid Email";
// 	                    break;
// 	                case AuthError.UserNotFound:
// 	                    message = "Account does not exist";
// 	                    break;
// 	            }
// 	        }
// 	        else
// 	        {
// 		        UIManager.instance.ProfileScreen();
// 	        }
// 	    }
// 		
// 		public void SignOut()
// 		{
// 			Auth.SignOut();
// 			UIManager.instance.LoginScreen();
// 		}
// 	}
// }