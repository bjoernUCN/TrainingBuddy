using TMPro;
using TrainingBuddy.Managers;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace TrainingBuddy.Races
{
    public class LobbyPlayerElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private TMP_Text nameText;
        
        public void SetPlayerNumber(string number)
        {
            numberText.text = number;
        }
        
        public void SetPlayerName(string name)
        {
            nameText.text = name;
        }
    }
}
