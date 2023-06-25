using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TrainingBuddy.Utility;
using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class DatabaseManager : Singleton<DatabaseManager>
	{
	    private DependencyStatus _dependencyStatus;
	    public FirebaseAuth Auth { get; set; }
	    public DatabaseReference DatabaseReference { get; set; }
	    
	    // Initialization
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
	        //Set the authentication Instance object
	        Auth = FirebaseAuth.DefaultInstance;
	        DatabaseReference = FirebaseDatabase.GetInstance("https://trainingbuddy-81bca-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
	    }

	    // Users
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
			    UIManager.Instance.ProfileScreen();
		    }
	    }
	    
	    public void SignOut()
	    {
		    Auth.SignOut();
		    UIManager.Instance.LoginScreen();
	    }
	    
		public async Task<DataSnapshot> ReadCurrentUserData(string path = null)
		{
			Task<DataSnapshot> DBTask = path == null ? DatabaseReference.Child("Users").Child(Auth.CurrentUser.UserId).GetValueAsync() : DatabaseReference.Child("Users").Child(Auth.CurrentUser.UserId).Child(path).GetValueAsync();

			return await DBTask;
		}
		
		public async Task<DataSnapshot> ReadSpecificUserData(string userId)
		{
			Task<DataSnapshot> DBTask = DatabaseReference.Child("Users").Child(userId).GetValueAsync();

			return await DBTask;
		}
		
		private async Task<DataSnapshot> GetAllUsers()
		{
			Task<DataSnapshot> DBTask = DatabaseReference.Child("Users").GetValueAsync();
			
			return await DBTask;
		}
		
		public async Task<DataSnapshot> GetLobbyData(string lobbyId)
		{
			Task<DataSnapshot> DBTask = DatabaseReference.Child("Races").Child(lobbyId).GetValueAsync();
			
			return await DBTask;
		}
		
		public async Task<bool> WriteCurrentUserData(string path, object data)
		{
			Task DBTask = DatabaseReference.Child("Users").Child(Auth.CurrentUser.UserId).Child(path).SetValueAsync(data);

			await DBTask;

			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}

			return true;
		}
		
		public async Task<bool> WriteSpecificUserData(string userId, string path, object data)
		{
			Task DBTask = DatabaseReference.Child("Users").Child(userId).Child(path).SetValueAsync(data);

			await DBTask;

			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}

			return true;
		}
		
		public async Task<List<string>> NearbyUsers(int range)
		{
			if (!Input.location.isEnabledByUser)
			{
				return null;
			}

			if (Input.location.status != LocationServiceStatus.Running)
			{
				Input.location.Start();
			}
			
			var userData = await GetAllUsers();
			
			return UtilityMethods.FindUsersInRange(userData, Input.location.lastData.latitude, Input.location.lastData.longitude, range);
		}
		
		// Races
		private async Task<bool> JoinOwnRace(string key, object data, string subPath)
		{
			Task DBTask = DatabaseReference.Child("Races").Child(Auth.CurrentUser.UserId).Child(key).Child(subPath).SetValueAsync(data);

			await DBTask;

			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}

			return true;
		}
		
		private async Task<bool> JoinRace(string key, object data, string subPath, string lobbyId)
		{
			Task DBTask = subPath == null ? DatabaseReference.Child("Races").Child(lobbyId).Child(key).SetValueAsync(data) : DatabaseReference.Child("Races").Child(lobbyId).Child(key).Child(subPath).SetValueAsync(data);
			
			await DBTask;
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}
		
			return true;
		}
		
		public async Task<bool> KickFromRace(string lobbyId, string playerId)
		{
			Task DBTask = DatabaseReference.Child("Races").Child(lobbyId).Child(playerId).RemoveValueAsync();
			
			await DBTask;
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}

			await WriteSpecificUserData(playerId, "Lobby", "");
		
			return true;
		}
		
		public async Task<bool> DestroyRace(string lobbyId)
		{
			Task DBTask = DatabaseReference.Child("Races").Child(lobbyId).RemoveValueAsync();
			
			await DBTask;
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}
		
			return true;
		}

		public async Task<string> IsInLobby()
		{
			var data = await GetAllRaces();
			
			foreach (var race in data.Children)
			{
				if (race.Key == Auth.CurrentUser.UserId)
				{
					return race.Key;
				}

				foreach (var raceData in race.Children)
				{
					if (raceData.Key == Auth.CurrentUser.UserId)
					{
						return raceData.Key;
					}
				}
			}
			
			return null;
		}

		public async Task<bool> HostLobby()
		{
			var data = await GetAllRaces();
			
			foreach (var race in data.Children)
			{
				if (race.Key == Auth.CurrentUser.UserId)
				{
					Debug.Log("ALREADY GOT A ROOM!!!");
					//TODO: Enter Room
					return false;
				}
			}
			await CreateLobby();
			await JoinOwnRace(Auth.CurrentUser.UserId, "Host", "Role");
			await JoinOwnRace(Auth.CurrentUser.UserId, 0, "Status");
			await WriteCurrentUserData("Lobby", Auth.CurrentUser.UserId);
			return true;
		}
		
		private async Task<bool> CreateLobby()
		{
			Task DBTask = DatabaseReference.Child("Races").Child(Auth.CurrentUser.UserId).Child("Timestamp").SetValueAsync(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

			await DBTask;

			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				return false;
			}

			return true;
		}
		
		public async void JoinLobby(string lobby)
		{
			var data = await GetAllRaces();
			
			foreach (var race in data.Children)
			{
				if (race.Key == lobby)
				{
					if (race.Children.Any(player => player.Key == Auth.CurrentUser.UserId))
					{
						Debug.Log("ALREADY IN THE ROOM!!!");
						return;
					}
					await JoinRace(Auth.CurrentUser.UserId, "Client", "Role", race.Key);
					await JoinRace(Auth.CurrentUser.UserId, 0, "Status", race.Key);
					await WriteCurrentUserData("Lobby", race.Key);
					return;
				}
			}
		}

		public async Task<List<string>> FindNearbyLobbies()
		{
			var nearbyLobbies = new List<string>();
			var userList = await NearbyUsers(10);
			var raceList = await GetAllRaces();
			
			foreach (DataSnapshot raceListChild in raceList.Children)
			{
				foreach (string user in userList)
				{
					if (raceListChild.Key == user)
					{
						nearbyLobbies.Add(raceListChild.Key);
					}
				}
			}

			return nearbyLobbies;
		}
		
		private async Task<DataSnapshot> GetAllRaces()
		{
			Task<DataSnapshot> DBTask = DatabaseReference.Child("Races").GetValueAsync();

			return await DBTask;
		}
		
		public async void Train()
		{
			await InvestInTraining();
		}
		private async Task<bool> InvestInTraining()
		{
			DataSnapshot data = await ReadCurrentUserData();
			var steps = (long)data.Child("StepCount").Value;
			var experience = Convert.ToInt32(data.Child("ExperiencePoints").Value);
			var spdPoints = (long)data.Child("SpeedPoints").Value;
			var accPoints = (long)data.Child("AccelerationPoints").Value;

			float investCap = GameManager.Instance.UserData.InvestCap;
			if (steps < investCap)
			{
				UIManager.Instance.ProfileScreen();
				GameManager.Instance.UserData.UpdateStepCount();
				return false;
			}

			float expIncrease = GameManager.Instance.UserData.ExpIncrease;
			int userLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(1 + 8 * (experience + investCap) / expIncrease)) / 2);

			await WriteCurrentUserData("Level", userLevel);
			await WriteCurrentUserData("StepCount", steps - investCap);
			await WriteCurrentUserData("ExperiencePoints", experience + investCap);
			
			float skillPointsPerLevel = GameManager.Instance.UserData.SkillPointsPerLevel;
			var totalPoints = userLevel * skillPointsPerLevel;
			totalPoints -= (int)spdPoints;
			totalPoints -= (int)accPoints;
			
			await WriteCurrentUserData("SkillPoints", totalPoints);
			
			UIManager.Instance.ProfileScreen();
			GameManager.Instance.UserData.UpdateStepCount();

			return true;
		}
	}
}