using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

namespace TrainingBuddy.Managers
{
	public class FirebaseManager : Singleton<FirebaseManager>
	{
		public FirebaseAuth Auth { get; set; }

		// Firebase database reference
		public DatabaseReference databaseReference { get; private set; }

		private DependencyStatus _dependencyStatus;

		private void Awake()
		{
			// Initialize Firebase
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
			{
				_dependencyStatus = task.Result;
				if (_dependencyStatus == DependencyStatus.Available)
				{
					//If they are avalible Initialize Firebase
					Auth = FirebaseAuth.DefaultInstance;
					databaseReference = FirebaseDatabase.GetInstance("https://trainingbuddy-81bca-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
				}
				else
				{
					Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
				}
			});

			databaseReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance).RootReference;
		}

		// Method for writing data to the database
		public void WriteData(string path, string data)
		{
			databaseReference.Child(path).SetRawJsonValueAsync(data);
		}

		// Method for reading data from the database
		public async DataSnapshot ReadUserData(string path)
		{
			var tcs = new TaskCompletionSource<DataSnapshot>();
			
			var snapshot = await databaseReference.Child("Users").Child(Auth.CurrentUser.UserId).Child(path).GetValueAsync();
			
			return snapshot;
		}
		
		public void SignOutButton()
		{
			Auth.SignOut();
			UIManager.instance.LoginScreen();
		}
	}
}