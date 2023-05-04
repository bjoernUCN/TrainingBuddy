using TrainingBuddy.Users;
using UnityEngine;
using UnityEngine.Android;

namespace TrainingBuddy.Managers
{
	public class GameManager : Singleton<GameManager>
	{
		// public static GameManager instance;
		
		[field:SerializeField] public UserData UserData { get; private set; }
		
		private new void Awake()
		{
			// if (instance == null)
			// {
			// 	instance = this;
			// }
			// else if (instance != null)
			// {
			// 	Debug.Log("Instance already exists, destroying object!");
			// 	Destroy(this);
			// }

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