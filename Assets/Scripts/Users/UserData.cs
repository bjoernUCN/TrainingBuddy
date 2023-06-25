using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase.Database;
using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TrainingBuddy.Users
{
	public class UserData : MonoBehaviour
	{
		[field:SerializeField] public int ExpIncrease { get; private set; }
		[field:SerializeField] public int SkillPointsPerLevel { get; private set; }
		[field:SerializeField] public int InvestCap { get; private set; }
		[field:SerializeField] public TMP_Text Username { get; set; }
		[field:SerializeField] public TMP_Text SkillPoints { get; set; }
		[field:SerializeField] public TMP_Text AccelerationPoints { get; set; }
		[field:SerializeField] public TMP_Text SpeedPoints { get; set; }
		[field:SerializeField] public TMP_Text Level { get; set; }
		[field:SerializeField] public TMP_Text ExperiencePoints { get; set; }
		[field:SerializeField] public Image ExpBarFill { get; set; }

		private bool isLocationUpdaterRunning;
		private bool isStepCounterRunning;
		private int localStepCount;
		
		[field:SerializeField] public TMP_Text StepTest { get; set; }

		private void Update()
		{
			if (isStepCounterRunning)
			{
				localStepCount = StepCounter.current.stepCounter.ReadValue();
			}
		}

		public async void LoadUserData()
		{
			var data = await DatabaseManager.Instance.ReadCurrentUserData();
			
			Username.text = data.Child("UserName").Value.ToString();
			SkillPoints.text = "Skill Points: " + data.Child("SkillPoints").Value;
			AccelerationPoints.text = data.Child("AccelerationPoints").Value.ToString();
			SpeedPoints.text = data.Child("SpeedPoints").Value.ToString();

			var expInt = Convert.ToInt32(data.Child("ExperiencePoints").Value);
			var userLevel = Convert.ToInt32(data.Child("Level").Value);
			
			int expNeededToCurrentLevel = (userLevel - 1) * 10000 * userLevel / 2;
			int expNeededToNextLevel = userLevel * 10000 * (userLevel + 1) / 2;
			int maxExp = expNeededToNextLevel - expNeededToCurrentLevel;
		        
			Level.text = "Level: " + userLevel;
			ExperiencePoints.text = (expInt - expNeededToCurrentLevel) + "/"+ maxExp +" XP";
			ExpBarFill.fillAmount = (expInt - expNeededToCurrentLevel) / (float) maxExp;
		}
		
		public void StartStepCounter()
		{
			if (isStepCounterRunning)
			{
				return;
			}
			
			if (Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
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
					StepCounterHandler();
					isStepCounterRunning = true;
				}
			}
		}

		private async Task StepCounterHandler(float delay = 10f)
		{
			while (true)
			{
				if (localStepCount <= 0)
				{
					await Task.Delay(1000);
					continue;
				}

				UpdateStepCount();
					
				await Task.Delay((int)delay * 1000);
			}
		}

		public async void UpdateStepCount()
		{
			DataSnapshot data = await DatabaseManager.Instance.ReadCurrentUserData();
			var stepSnapshot = (long)data.Child("StepSnapshot").Value; 
			var savedStepCount = (long)data.Child("StepCount").Value; 


			if (localStepCount >= stepSnapshot)
			{
				long newStepCount = savedStepCount + (localStepCount - stepSnapshot);
					
				await DatabaseManager.Instance.WriteCurrentUserData("StepCount", newStepCount);
				StepTest.text = "StepCount: " + newStepCount;
			}
				
			await DatabaseManager.Instance.WriteCurrentUserData("StepSnapshot", localStepCount);
		}
		
		public void StartLocationUpdater()
		{
			if (isLocationUpdaterRunning)
			{
				return;
			}
			
			if (Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
			{
				if (!Input.location.isEnabledByUser)
				{
					return;
				}

				if (Input.location.status != LocationServiceStatus.Running)
				{
					Input.location.Start();
				}
				
				LocationHandler();
				isLocationUpdaterRunning = true;
			}
		}

		private async Task LocationHandler(float delay = 10f)
		{
			while (true)
			{
				if (Input.location.status == LocationServiceStatus.Failed)
				{
					print("Unable to determine device location");
				}

				await DatabaseManager.Instance.WriteCurrentUserData("Latitude", Input.location.lastData.latitude);
				await DatabaseManager.Instance.WriteCurrentUserData("Longitude", Input.location.lastData.longitude);

				await Task.Delay((int)delay * 1000);
			}
		}
	}
}