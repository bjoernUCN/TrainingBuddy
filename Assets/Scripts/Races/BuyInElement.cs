using Firebase.Database;
using TMPro;
using TrainingBuddy.Managers;
using TrainingBuddy.Users;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace TrainingBuddy.Races
{
    public class BuyInElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Button button;
        
        public void SetName(string raceName)
        {
            titleText.text = raceName;
        }
        
        public async void SetButtonAction(string lobbyId, int price)
        {
            button.onClick.AddListener(async () =>
            {
                DataSnapshot data = await DatabaseManager.Instance.ReadCurrentUserData();
                var steps = (long)data.Child("StepCount").Value;

                if (steps < price)
                {
                    RaceManager.Instance.MainScreen();
                    return;
                }
            
                await DatabaseManager.Instance.WriteCurrentUserData("StepCount", steps - price);
                await DatabaseManager.Instance.WriteCurrentUserData("BuyIn", price);
                GameManager.Instance.RaceData.JoinRace(lobbyId);
            });
        }
    }
}
