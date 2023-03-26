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
		
		
		[field:SerializeField] public TMP_Text StepTest { get; set; }

		private void Update()
		{
			if (StepCounter.current != null)
			{
				StepTest.text = "Step test: " + StepCounter.current.stepCounter.ReadValue();
			}
		}

		public IEnumerator LoadUserData()
		{
		    //Get the currently logged in user data
		    var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).GetValueAsync();
		
		    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
		    if (DBTask.Exception != null)
		    {
		        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
		    }
		    else if (DBTask.Result.Value == null)
		    {
		        //No data exists yet
		        // xpField.text = "0";
		        // killsField.text = "0";
		        // deathsField.text = "0";
		    }
		    else
		    {
		        //Data has been retrieved
		        DataSnapshot snapshot = DBTask.Result;
		        
		        Username.text = snapshot.Child("UserName").Value.ToString();
		        SkillPoints.text = "Skill Points: " + snapshot.Child("SkillPoints").Value;
		        AccelerationPoints.text = snapshot.Child("AccelerationPoints").Value.ToString();
		        SpeedPoints.text = snapshot.Child("SpeedPoints").Value.ToString();

		        var expInt = Convert.ToInt32(snapshot.Child("ExperiencePoints").Value);
		        int userLevel = Mathf.CeilToInt(expInt / expIncrease);
		        float maxExp = userLevel * expIncrease;
		        
		        Level.text = "Level: " + userLevel;
		        ExperiencePoints.text = snapshot.Child("ExperiencePoints").Value + "/"+ maxExp +" XP";
		        ExpBarFill.fillAmount = expInt / maxExp;
		    }
		}
		
		public IEnumerator UpdateUsernameDatabase(string username)
		{
			//Set the currently logged in user username in the database
			var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("UserName").SetValueAsync(username);
		
			yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
		
			if (DBTask.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
			}
		}
		
		public IEnumerator UpdateSkillPoints(int skillPoints)
		{
		    //Set the currently logged in user deaths
		    var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("SkillPoints").SetValueAsync(skillPoints);
		
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
			var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("AccelerationPoints").SetValueAsync(accelerationPoints);
		
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
			var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("SpeedPoints").SetValueAsync(speedPoints);
		
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
			var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("ExperiencePoints").SetValueAsync(experiencePoints);
		
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
			var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("Level").SetValueAsync(level);
		
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
		
		public IEnumerator UpdateStepSnapshot(int snapshot)
		{
			//Set the currently logged in user deaths
			var DBTask = DatabaseManager.Instance.DbReference.Child("Users").Child(DatabaseManager.Instance.Auth.CurrentUser.UserId).Child("StepSnapshot").SetValueAsync(snapshot);
		
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
	}
}