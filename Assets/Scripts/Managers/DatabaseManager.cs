using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class DatabaseManager : Singleton<DatabaseManager>
	{
	    private DependencyStatus _dependencyStatus;
	    public FirebaseAuth Auth { get; set; }
	    public DatabaseReference DatabaseReference { get; set; }

	    protected override void Awake()
	    {
	        //Check that all of the necessary dependencies for Firebase are present on the system
	        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
	        {
	            _dependencyStatus = task.Result;
	            if (_dependencyStatus == DependencyStatus.Available)
	            {
	                //If they are avalible Initialize Firebase
	                InitializeFirebase();
	            }
	            else
	            {
	                Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
	            }
	        });
	    }

	    private void InitializeFirebase()
	    {
	        Debug.Log("Setting up Firebase Auth");
	        //Set the authentication instance object
	        Auth = FirebaseAuth.DefaultInstance;
	        DatabaseReference = FirebaseDatabase.GetInstance("https://trainingbuddy-81bca-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
	    }
	    
	    public void ReadUserData(Action<Task<DataSnapshot>> callback)
	    {
		    StartCoroutine(ReadUserDataCoroutine("", callback));
	    }
	    
	    public void ReadUserData(string path, Action<Task<DataSnapshot>> callback)
	    {
		    StartCoroutine(ReadUserDataCoroutine(path, callback));
	    }
	    
		private IEnumerator ReadUserDataCoroutine(string path, Action<Task<DataSnapshot>> callback)
		{
			Task<DataSnapshot> DBTask;

			DBTask = path == "" ? DatabaseReference.Child("Users").Child(Auth.CurrentUser.UserId).GetValueAsync() : DatabaseReference.Child("Users").Child(Auth.CurrentUser.UserId).Child(path).GetValueAsync();
			
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
			
			callback(DBTask);
		}
	    
	    public IEnumerator SignIn(string _email, string _password)
	    {
		    //Call the Firebase auth signin function passing the email and password
		    var LoginTask = Auth.SignInWithEmailAndPasswordAsync(_email, _password);
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
			    UIManager.instance.ProfileScreen();
		    }
	    }
	    
	    public void SignOut()
	    {
		    Auth.SignOut();
		    UIManager.instance.LoginScreen();
	    }
	}
}