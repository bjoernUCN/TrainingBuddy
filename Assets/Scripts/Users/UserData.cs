using System;
using System.Collections;
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
		[SerializeField] private float expIncrease = 500f;
		[field:SerializeField] public TMP_Text Username { get; set; }
		[field:SerializeField] public TMP_Text SkillPoints { get; set; }
		[field:SerializeField] public TMP_Text AccelerationPoints { get; set; }
		[field:SerializeField] public TMP_Text SpeedPoints { get; set; }
		[field:SerializeField] public TMP_Text Level { get; set; }
		[field:SerializeField] public TMP_Text ExperiencePoints { get; set; }
		[field:SerializeField] public Image ExpBarFill { get; set; }

		public Coroutine LocationUpdater;
		private bool isLocationUpdaterRunning;
		
		private Coroutine stepCounterRoutine;
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

		public void LoadUserData()
		{
			DatabaseManager.Instance.ReadUserData(dbTask =>
			{
				if (dbTask.IsFaulted)
				{
					Debug.LogError("Error reading user data: " + dbTask.Exception);
				}
				else if (dbTask.IsCompleted)
				{
					DataSnapshot dataSnapshot = dbTask.Result;
					Username.text = dataSnapshot.Child("UserName").Value.ToString();
					SkillPoints.text = "Skill Points: " + dataSnapshot.Child("SkillPoints").Value;
					AccelerationPoints.text = dataSnapshot.Child("AccelerationPoints").Value.ToString();
					SpeedPoints.text = dataSnapshot.Child("SpeedPoints").Value.ToString();

					var expInt = Convert.ToInt32(dataSnapshot.Child("ExperiencePoints").Value);
					int userLevel = Mathf.CeilToInt(expInt / expIncrease);
					float maxExp = userLevel * expIncrease;
		        
					Level.text = "Level: " + userLevel;
					ExperiencePoints.text = dataSnapshot.Child("ExperiencePoints").Value + "/"+ maxExp +" XP";
					ExpBarFill.fillAmount = expInt / maxExp;
				}
			});
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
					isStepCounterRunning = true;
					
					stepCounterRoutine = StartCoroutine(StepCounterCoroutine());
				}
			}
		}

		private IEnumerator StepCounterCoroutine(float delay = 10f)
		{
			while (true)
			{
				DatabaseManager.Instance.ReadUserData("StepSnapshot", dbTask =>
				{
					if (dbTask.IsCompleted)
					{
						var result = dbTask.Result;
						var stepSnapshot = (long)result.Value;
						
						if (stepSnapshot == -1 && localStepCount > 0)
						{
							DatabaseManager.Instance.WriteUserData("StepSnapshot", localStepCount);
						} 
						else if (localStepCount > 0 && localStepCount < stepSnapshot)
						{
							DatabaseManager.Instance.WriteUserData("StepSnapshot", localStepCount);
						}
						
						DatabaseManager.Instance.WriteUserData("StepCount", localStepCount - stepSnapshot);
						StepTest.text = "StepCount: " + (localStepCount - stepSnapshot);
					}
				});

				yield return new WaitForSeconds(delay);
			}
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
				
				LocationUpdater = StartCoroutine(UpdateLocationCoroutine());
			}
		}

		private IEnumerator UpdateLocationCoroutine(float delay = 10f)
		{
			while (true)
			{
				if (Input.location.status == LocationServiceStatus.Failed)
				{
					print("Unable to determine device location");
				}

				DatabaseManager.Instance.WriteUserData("Latitude", Input.location.lastData.latitude);
				DatabaseManager.Instance.WriteUserData("Longitude", Input.location.lastData.longitude);

				yield return new WaitForSeconds(delay);
			}
		}
	}
}