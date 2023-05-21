using Firebase.Database;
using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TrainingBuddy.Races
{
    public class Lobby : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text LobbyTitle { get; private set; }
        [SerializeField] private GameObject lobbyPlayerElementPrefab;
        
        public async void Initialize(string lobbyId)
        {
            CleanUp();
            var i = 1;
            var lobbyData = await DatabaseManager.Instance.GetLobbyData(lobbyId);
            foreach (DataSnapshot data in lobbyData.Children)
            {
                if (data.Key == "Timestamp")
                {
                    continue;
                }

                var userData = await DatabaseManager.Instance.ReadSpecificUserData(data.Key);
                var race = Instantiate(lobbyPlayerElementPrefab, transform);
                
                race.GetComponent<LobbyPlayerElement>().SetPlayerNumber(i.ToString());
                race.GetComponent<LobbyPlayerElement>().SetPlayerName(userData.Child("UserName").Value.ToString());

                i++;
            }
        }

        public async void Leave()
        {
            var lobbyId = await DatabaseManager.Instance.ReadCurrentUserData("Lobby");
            if (lobbyId.Value.ToString() == DatabaseManager.Instance.Auth.CurrentUser.UserId)
            {
                // Im host. Kick everyone and destroy Lobby
                var lobbyData = await DatabaseManager.Instance.GetLobbyData(lobbyId.Value.ToString());
                foreach (DataSnapshot data in lobbyData.Children)
                {
                    if (data.Key == "Timestamp")
                    {
                        continue;
                    }

                    await DatabaseManager.Instance.KickFromRace(lobbyId.Value.ToString(), data.Key);
                }

                await DatabaseManager.Instance.DestroyRace(lobbyId.Value.ToString());
            }
            else
            {
                // Im not host. Leave Lobby
                await DatabaseManager.Instance.KickFromRace(lobbyId.Value.ToString(), DatabaseManager.Instance.Auth.CurrentUser.UserId);
            }
            
            RaceManager.Instance.MainScreen();
        }

        private void CleanUp()
        {
            var elements = gameObject.GetComponentsInChildren<LobbyPlayerElement>();
            foreach (var element in elements)
            {
                Destroy(element.gameObject);
            }
        }
    }
}
