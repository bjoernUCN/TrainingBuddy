using System;
using System.Collections;
using Firebase.Database;
using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TrainingBuddy.Users
{
	public class UserData : MonoBehaviour
	{
		[SerializeField] private float expIncrease = 500f;
		[field:SerializeField] public TMP_Text Username { get; set; }
		[field:SerializeField] public TMP_Text SkillPoints { get; set; }
		[field:SerializeField] public TMP_Text AccelerationPoints { get; set; }
		[field:SerializeField] public TMP_Text SpeedPoints { get; set; }
		[field:SerializeField] public TMP_Text Level { get; set; }
		[field:SerializeField] public TMP_Text ExperiencePoints { get; set; }
		[field:SerializeField] public Image ExpBarFill { get; set; }

		public DataSnapshot UserDBSnapshot { get; set;}

		public Coroutine LocationUpdater;
		
		[field:SerializeField] public TMP_Text StepTest { get; set; }

		private void Update()
		{
			// if (StepCounter.current != null && snapshotSuccess)
			// {
			// 	StepTest.text = "Step test: " + (StepCounter.current.stepCounter.ReadValue() - Convert.ToInt32(UserDBSnapshot.Child("StepSnapshot").Value));
			// }
		}

		private IEnumerator SnapshotDatabase()
		{
			var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).GetValueAsync();
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
			else if (DBTask.Result.Value == null)
			{
				//No data exists yet
			}
			else
			{
				UserDBSnapshot = DBTask.Result;
			}
		}

		public void LoadUserData()
		{
			StartCoroutine(SnapshotDatabase());

			if (UserDBSnapshot == null)
			{
				return;
			}
		        
			Username.text = UserDBSnapshot.Child("UserName").Value.ToString();
			SkillPoints.text = "Skill Points: " + UserDBSnapshot.Child("SkillPoints").Value;
			AccelerationPoints.text = UserDBSnapshot.Child("AccelerationPoints").Value.ToString();
			SpeedPoints.text = UserDBSnapshot.Child("SpeedPoints").Value.ToString();

			var expInt = Convert.ToInt32(UserDBSnapshot.Child("ExperiencePoints").Value);
			int userLevel = Mathf.CeilToInt(expInt / expIncrease);
			float maxExp = userLevel * expIncrease;
		        
			Level.text = "Level: " + userLevel;
			ExperiencePoints.text = UserDBSnapshot.Child("ExperiencePoints").Value + "/"+ maxExp +" XP";
			ExpBarFill.fillAmount = expInt / maxExp;
		}

		public object LoadUserData(string field)
		{
			StartCoroutine(SnapshotDatabase());

			var returnObject = new object();

			if (UserDBSnapshot == null)
			{
				return returnObject;
			}

			returnObject = field switch
			{
				"UserName" => UserDBSnapshot.Child("UserName").Value, 
				"SkillPoints" => UserDBSnapshot.Child("SkillPoints").Value, 
				"AccelerationPoints" => UserDBSnapshot.Child("AccelerationPoints").Value, 
				"SpeedPoints" => UserDBSnapshot.Child("SpeedPoints").Value, 
				"ExperiencePoints" => UserDBSnapshot.Child("ExperiencePoints").Value, 
				"StepSnapshot" => UserDBSnapshot.Child("StepSnapshot").Value, 
				"LocationSnapshot" => UserDBSnapshot.Child("LocationSnapshot").Value, 
				_ => returnObject
			};

			return returnObject;
		}
		
		public IEnumerator UpdateUsernameDatabase(string username)
		{
			//Set the currently logged in user username in the database
			var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("UserName").SetValueAsync(username);
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
		}
		
		public IEnumerator UpdateSkillPoints(int skillPoints)
		{
		    //Set the currently logged in user deaths
		    var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("SkillPoints").SetValueAsync(skillPoints);
		
		    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
		    if (DBTask.Exception != null)
		    {
		        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
		    }
		    else
		    {
		        // SkillPoints are now updated
		    }
		}
		
		public IEnumerator UpdateAccelerationPoints(int accelerationPoints)
		{
			//Set the currently logged in user deaths
			var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("AccelerationPoints").SetValueAsync(accelerationPoints);
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
			else
			{
				// AccelerationPoints are now updated
			}
		}

		public IEnumerator UpdateSpeedPoints(int speedPoints)
		{
			//Set the currently logged in user deaths
			var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("SpeedPoints").SetValueAsync(speedPoints);
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
			else
			{
				// SpeedPoints are now updated
			}
		}

		public IEnumerator UpdateExperiencePoints(int experiencePoints)
		{
			//Set the currently logged in user deaths
			var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("ExperiencePoints").SetValueAsync(experiencePoints);
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
			else
			{
				// ExperiencePoints are now updated
			}
		}
		
		public IEnumerator UpdateLevel(int level)
		{
			//Set the currently logged in user deaths
			var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("Level").SetValueAsync(level);
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
			else
			{
				// ExperiencePoints are now updated
			}
		}
		
		public IEnumerator UpdateStepSnapshot(int? snapshot = null)
		{
			if (snapshot != null)
			{
				var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("StepSnapshot").SetValueAsync(snapshot);
		
				yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
				if (DBTask.Exception != null)
				{
					Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				}
				
				// Do shit
				
				yield return true;
			}
			
			while (true)
			{
				if (StepCounter.current == null)
				{
					InputSystem.AddDevice<StepCounter>();
				}
			       
				if (!StepCounter.current.enabled)
				{
					InputSystem.EnableDevice(StepCounter.current);
					if (StepCounter.current.enabled)
					{
						Debug.Log("StepCounter is enabled");
					}
				}
				
				if (StepCounter.current != null)
				{
					var stepSnapshot = FirebaseManager.Instance.ReadUserData("StepSnapshot").Value;
					// Update Snapshot
					// var stepSnapshot = LoadUserData("StepSnapshot");
					int.TryParse(stepSnapshot.ToString() , out int stepSnapshotInt);
					if (stepSnapshotInt == -1 || (StepCounter.current.stepCounter.ReadValue() < stepSnapshotInt && StepCounter.current.stepCounter.ReadValue() != 0))
					{
						var SnapshotTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("StepSnapshot").SetValueAsync(stepSnapshot);
		
						yield return new WaitUntil(predicate: () => SnapshotTask.IsCompleted);
		
						if (SnapshotTask.Exception != null)
						{
							Debug.LogWarning(message: $"Failed to register task with {SnapshotTask.Exception}");
						}
					}
					
					var steps = StepCounter.current.stepCounter.ReadValue() - (int)stepSnapshot;
					
					var StepTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("Steps").SetValueAsync(steps);
		
					yield return new WaitUntil(predicate: () => StepTask.IsCompleted);
		
					if (StepTask.Exception != null)
					{
						Debug.LogWarning(message: $"Failed to register task with {StepTask.Exception}");
					}
					
					yield return new WaitForSeconds(20f);
				}
			}
		}

		private void HandleStepSnapshotCallback(DataSnapshot obj)
		{
			throw new NotImplementedException();
		}

		public IEnumerator UpdateLocation()
		{
			if (!Input.location.isEnabledByUser)
			{
				yield break;
			}

			if (Input.location.status != LocationServiceStatus.Running)
			{
				Input.location.Start();
			}

			while (true)
			{
				if (Input.location.status == LocationServiceStatus.Failed)
				{
					print("Unable to determine device location");
					yield break;
				}

				var DBTask = FirebaseManager.Instance.databaseReference.Child("Users").Child(FirebaseManager.Instance.Auth.CurrentUser.UserId).Child("Location").SetValueAsync(Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
				
				yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
				
				if (DBTask.Exception != null)
				{
					Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
				}
				
				// If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
				// print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

				yield return new WaitForSeconds(20f);
			}
			
			// Waits until the location service initializes
			// int maxWait = 20;
			// while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
			// {
			// 	yield return new WaitForSeconds(1);
			// 	maxWait--;
			// }
			//
			// // If the service didn't initialize in 20 seconds this cancels location service use.
			// if (maxWait < 1)
			// {
			// 	print("Timed out");
			// 	yield break;
			// }
		
			// If the connection failed this cancels location service use.
			
		
			// Stops the location service if there is no need to query location updates continuously.
			Input.location.Stop();
			
			
			//Set the currently logged in user deaths
			// var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("Location").SetValueAsync(snapshot);
			//
			// yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
			//
			// if (DBTask.Exception != null)
			// {
			// 	Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			// }
			// else
			// {
			// 	// ExperiencePoints are now updated
			// }
			
			// IEnumerator Start()
			// {
			// 	// Check if the user has location service enabled.
			// 	if (!Input.location.isEnabledByUser)
			// 		yield break;
			//
			// 	// Starts the location service.
			// 	Input.location.Start();
			//
			// 	// Waits until the location service initializes
			// 	int maxWait = 20;
			// 	while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
			// 	{
			// 		yield return new WaitForSeconds(1);
			// 		maxWait--;
			// 	}
			//
			// 	// If the service didn't initialize in 20 seconds this cancels location service use.
			// 	if (maxWait < 1)
			// 	{
			// 		print("Timed out");
			// 		yield break;
			// 	}
			//
			// 	// If the connection failed this cancels location service use.
			// 	if (Input.location.status == LocationServiceStatus.Failed)
			// 	{
			// 		print("Unable to determine device location");
			// 		yield break;
			// 	}
			// 	else
			// 	{
			// 		// If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
			// 		print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
			// 	}
			//
			// 	// Stops the location service if there is no need to query location updates continuously.
			// 	Input.location.Stop();
			// }
		}
	}
}