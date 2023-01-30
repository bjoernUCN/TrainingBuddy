using Navigation;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
	public class CreateUser : BaseButtonClick
	{
		[SerializeField] private TMP_Text username;
		[SerializeField] private TMP_Text password;
		
		public override void ButtonClicked()
		{
			DatabaseManager.Instance.NewUser(username.text, password.text);
		}
	}
}