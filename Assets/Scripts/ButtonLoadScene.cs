using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Navigation
{
	[RequireComponent(typeof(Button))]
	public class ButtonLoadScene : BaseButtonClick
	{
		[SerializeField] private SceneReference _sceneReference;

		public SceneReference SceneReference
		{
			get => _sceneReference;
			set => _sceneReference = value;
		}
		
		public override void ButtonClicked()
		{
			throw new NotImplementedException();
		}
	}
}
