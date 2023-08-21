using System;
using System.Collections.Generic;
using BedtimeCore.Utility;
using TrainingBuddy.Races;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace TrainingBuddy.Managers
{
	public class RaceManager : Singleton<RaceManager>
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private GameObject mainCanvas;
		[SerializeField] private GameObject mainUI;
		[SerializeField] private GameObject hostUI;
		[SerializeField] private GameObject lobbyUI;
		[SerializeField] private RaceList RaceList;
		[SerializeField] private Lobby lobby;
		
		[SerializeField] private GameObject runnerPrefab;
		[SerializeField] private List<Track> tracks = new ();
		
		private GameObject raceListUI => RaceList.gameObject;
		
		private void ClearScreen()
		{
			mainUI.SetActive(false);
			hostUI.SetActive(false);
			lobbyUI.SetActive(false);
			raceListUI.SetActive(false);
		}
		
		public async void MainScreen()
		{
			var lobbyId = await DatabaseManager.Instance.IsInLobby();
			if (lobbyId != null)
			{
				LobbyScreen(lobbyId);
				return;
			}
			
			ClearScreen();
			mainUI.SetActive(true);
		}
		
		public async void RaceListScreen()
		{
			ClearScreen();
			raceListUI.SetActive(true);

			var lobbyList = await DatabaseManager.Instance.FindNearbyLobbies();
			RaceList.AddRaces(lobbyList);
		}
		
		public async void BuyIn(string lobbyId)
		{
			ClearScreen();
			raceListUI.SetActive(true);

			RaceList.ShowBuyIn(lobbyId);
		}
		
		public void HostRaceScreen()
		{
			ClearScreen();
			hostUI.SetActive(true);

			GameManager.Instance.RaceData.HostRace();
			LobbyScreen(DatabaseManager.Instance.Auth.CurrentUser.UserId);
		}
		
		public void LobbyScreen(string lobbyId)
		{
			ClearScreen();
			lobbyUI.SetActive(true);
			lobby.Initialize(lobbyId);
		}
		
		public void WatchRaceScreen()
		{
			ClearScreen();
			mainCanvas.SetActive(false);
			StartRace();
		}
		
		[InspectorButton]
		public void StartRace()
		{
			for (int i = 0; i < 5; i++)
			{
				GameObject runner = Instantiate(runnerPrefab, tracks[i].StartPos.position, Quaternion.identity);
				runner.GetComponent<RunOnSpline>().SplineContainer = tracks[i].Spline;
				runner.GetComponent<RunOnSpline>().targetMoveSpeed = 200f + i * 30f;
				runner.GetComponent<RunOnSpline>().acceleration = 10f;
				runner.transform.parent = tracks[i].Spline.transform;
				mainCamera.transform.parent = runner.transform;
				mainCamera.transform.localPosition = new Vector3(0, 2, -5); 
			}
		}
	}
    
	[Serializable]
	public struct Track
	{
		public SplineContainer Spline;
		public Transform StartPos;
	}
}