using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace TrainingBuddy.Races
{
    public class RaceElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Button button;
        
        public void SetRaceName(string raceName)
        {
            titleText.text = raceName;
        }
        
        public void SetButtonAction(string lobbyId)
        {
            button.onClick.AddListener(() => GameManager.Instance.RaceData.JoinRace(lobbyId));
        }
    }
}
