using System;
using UnityEngine;
using UnityEngine.UI;

namespace Navigation
{
	public class SwipeControls : MonoBehaviour
	{
		private Vector2 startTouchPosition;
		private Vector2 endTouchPosition;
		
		private void Update()
		{
			
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				startTouchPosition = Input.GetTouch(0).position;
			}

			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				endTouchPosition = Input.GetTouch(0).position;

				if (endTouchPosition.x < startTouchPosition.x)
				{
					Debug.Log("Back");
				}

				if (endTouchPosition.x > startTouchPosition.x)
				{
					Debug.Log("Forward");
				}
			}
		}
	}
}