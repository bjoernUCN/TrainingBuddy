using System.Collections.Generic;
using TrainingBuddy.Managers;
using UnityEngine;

namespace TrainingBuddy.Races
{
    public class RaceList : MonoBehaviour
    {
        [SerializeField] private GameObject raceListPrefab;
        [SerializeField] private GameObject buyInPrefab;
        
        public async void AddRaces(List<string> races)
        {
            CleanUp();
            foreach (string raceName in races)
            {
                var parsedName = await DatabaseManager.Instance.ReadSpecificUserData(raceName);
                
                var race = Instantiate(raceListPrefab, transform);
                race.GetComponent<RaceListElement>().SetRaceName(parsedName.Child("UserName").Value + "'s Race");
                race.GetComponent<RaceListElement>().SetButtonAction(raceName);
            }
        }

        public async void ShowBuyIn(string lobbyId)
        {
            CleanUp();
            for (var i = 0; i < 3; i++)
            {
                var price = (i + 1) * 500;
                var buyIn = Instantiate(buyInPrefab, transform);
                buyIn.GetComponent<BuyInElement>().SetName("" + price);
                buyIn.GetComponent<BuyInElement>().SetButtonAction(lobbyId, price);
            }
        }
        
        private void CleanUp()
        {
            var raceElements = gameObject.GetComponentsInChildren<RaceListElement>();
            foreach (var element in raceElements)
            {
                Destroy(element.gameObject);
            }
            
            var buyInElements = gameObject.GetComponentsInChildren<BuyInElement>();
            foreach (var element in buyInElements)
            {
                Destroy(element.gameObject);
            }
        }
    }
}
