using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrainingBuddy.Navigation
{
	public class LoadScene : MonoBehaviour
	{
		[SerializeField] private SceneReference _sceneReference;

		public void Load()
		{
			SceneManager.LoadScene(_sceneReference.SceneName);
		}
	}
}
