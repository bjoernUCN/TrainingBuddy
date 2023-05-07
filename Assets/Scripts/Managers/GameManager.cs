using TrainingBuddy.Races;
using TrainingBuddy.Users;
using UnityEngine;
using UnityEngine.Android;

namespace TrainingBuddy.Managers
{
	public class GameManager : Singleton<GameManager>
	{
		[field:SerializeField] public UserData UserData { get; private set; }
		[field:SerializeField] public RaceData RaceData { get; private set; }
		
		private new void Awake()
		{
			if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
			{
				Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
			}
			
			if (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
			{
				Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
			}
		}
	}
}