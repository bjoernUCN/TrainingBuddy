using UnityEngine;
using UnityEngine.UI;

namespace Navigation
{
	public abstract class BaseButtonClick : MonoBehaviour
	{
		protected virtual void Awake() => GetComponent<Button>().onClick.AddListener(ButtonClicked);

		private void OnDestroy() => GetComponent<Button>().onClick.AddListener(ButtonClicked);

		public abstract void ButtonClicked();
	}
}