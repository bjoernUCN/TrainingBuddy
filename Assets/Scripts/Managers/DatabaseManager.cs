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
	    [field:SerializeField] public FirebaseUser User { get; set; }
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
	    //
	    //
	    //
	    // private IEnumerator UpdateUsernameAuth(string _username)
	    // {
	    //     //Create a user profile and set the username
	    //     UserProfile profile = new UserProfile { DisplayName = _username };
	    //
	    //     //Call the Firebase auth update user profile function passing the profile with the username
	    //     var ProfileTask = _user.UpdateUserProfileAsync(profile);
	    //     //Wait until the task completes
	    //     yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);
	    //
	    //     if (ProfileTask.Exception != null)
	    //     {
	    //         Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
	    //     }
	    //     else
	    //     {
	    //         //Auth username is now updated
	    //     }        
	    // }

	    //
	    // private IEnumerator LoadScoreboardData()
	    // {
	    //     //Get all the users data ordered by kills amount
	    //     var DBTask = _dbReference.Child("users").OrderByChild("kills").GetValueAsync();
	    //
	    //     yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
	    //
	    //     if (DBTask.Exception != null)
	    //     {
	    //         Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
	    //     }
	    //     else
	    //     {
	    //         //Data has been retrieved
	    //         DataSnapshot snapshot = DBTask.Result;
	    //
	    //         //Destroy any existing scoreboard elements
	    //         foreach (Transform child in scoreboardContent.transform)
	    //         {
	    //             Destroy(child.gameObject);
	    //         }
	    //
	    //         //Loop through every users UID
	    //         foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
	    //         {
	    //             string username = childSnapshot.Child("username").Value.ToString();
	    //             int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
	    //             int deaths = int.Parse(childSnapshot.Child("deaths").Value.ToString());
	    //             int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());
	    //
	    //             //Instantiate new scoreboard elements
	    //             GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
	    //             scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
	    //         }
	    //
	    //         //Go to scoareboard screen
	    //         UIManager.instance.ScoreboardScreen();
	    //     }
	    // }
	}
}