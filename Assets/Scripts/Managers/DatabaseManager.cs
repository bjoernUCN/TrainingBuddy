using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace TrainingBuddy.Managers
{
	public class DatabaseManager : Singleton<DatabaseManager>
	{
		//Firebase variables
	    [Header("Firebase")]
	    private DependencyStatus _dependencyStatus;
	    [field:SerializeField] public FirebaseAuth Auth { get; set; }
	    [field:SerializeField] public DatabaseReference DbReference { get; set; }

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
	        DbReference = FirebaseDatabase.GetInstance("https://trainingbuddy-81bca-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
	    }
	    
	    // //Function for the sign out button
	    public void SignOutButton()
	    {
		    Auth.SignOut();
		    UIManager.instance.LoginScreen();
	    }
	    // //Function for the save button
	    // public void SaveDataButton()
	    // {
	    //     StartCoroutine(UpdateUsernameAuth(usernameField.text));
	    //     StartCoroutine(UpdateUsernameDatabase(usernameField.text));
	    //
	    //     StartCoroutine(UpdateXp(int.Parse(xpField.text)));
	    //     StartCoroutine(UpdateKills(int.Parse(killsField.text)));
	    //     StartCoroutine(UpdateDeaths(int.Parse(deathsField.text)));
	    // }
	    // //Function for the scoreboard button
	    // public void ScoreboardButton()
	    // {        
	    //     StartCoroutine(LoadScoreboardData());
	    // }
	}
}