using Managers;
using Firebase;
using Firebase.Database;
using UnityEngine;


public class DatabaseManager : Singleton<DatabaseManager>
{
	private DatabaseReference _dbReference;
    void Start()
    {
	    _dbReference = FirebaseDatabase.GetInstance("https://trainingbuddy-81bca-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
    }

    public void NewUser(string name, string password)
    {
	    User user = new User(name, password);
	    string json = JsonUtility.ToJson(user);

	    _dbReference.Child("users").SetRawJsonValueAsync(json);
    }
}
