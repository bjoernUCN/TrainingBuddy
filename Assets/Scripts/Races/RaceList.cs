using System.Collections.Generic;
using TrainingBuddy.Managers;
using UnityEngine;

namespace TrainingBuddy.Races
{
    public class RaceList : MonoBehaviour
    {
        [SerializeField] private GameObject raceListPrefab;
        
        public async void AddRaces(List<string> races)
        {
            CleanUp();
            foreach (string raceName in races)
            {
                var parsedName = await DatabaseManager.Instance.ReadSpecificUserData(raceName);
                
                var race = Instantiate(raceListPrefab, transform);
                race.GetComponent<RaceElement>().SetRaceName(parsedName.Child("UserName").Value + "'s Race");
                race.GetComponent<RaceElement>().SetButtonAction(raceName);
            }
        }
        
        private void CleanUp()
        {
            var elements = gameObject.GetComponentsInChildren<RaceElement>();
            foreach (var element in elements)
            {
                Destroy(element.gameObject);
            }
        }
    }
}
