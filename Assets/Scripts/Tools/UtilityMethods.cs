using System;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

namespace TrainingBuddy.Utility

{
	public class UtilityMethods
	{
		public static List<string> FindUsersInRange(DataSnapshot userList, float latitude, float longitude, float radiusInKilometers)
		{
			var basePoint = ConvertLatLon(latitude, longitude);
			var usersInRange = new List<string>();

			foreach (DataSnapshot userListChild in userList.Children)
			{
				var userLatitude = 0f;
				var userLongitude = 0f;
				
				foreach (DataSnapshot dataSnapshot in userListChild.Children)
				{
					switch (dataSnapshot.Key)
					{
						case "Latitude":
							userLatitude = float.Parse(dataSnapshot.Value.ToString());
							break;
						case "Longitude":
							userLongitude = float.Parse(dataSnapshot.Value.ToString());
							break;
					}
				}

				var userPoint = ConvertLatLon(userLatitude, userLongitude);
				var distance = Vector3.Distance(basePoint, userPoint);
				
				if (radiusInKilometers >= distance)
				{
					usersInRange.Add(userListChild.Key);
				}
			}

			return usersInRange;
		}

		public static Vector3 ConvertLatLon(float latitude, float longitude)
		{
			var radius = 6371f;
			
			// Convert latitude and longitude to radians
			double latRad = latitude * Math.PI / 180.0;
			double lonRad = longitude * Math.PI / 180.0;

			// Calculate the 3D point (x, y, z) using the spherical coordinate system conversion formula
			float x = (float)(radius * Math.Cos(latRad) * Math.Cos(lonRad));
			float y = (float)(radius * Math.Cos(latRad) * Math.Sin(lonRad));
			float z = (float)(radius * Math.Sin(latRad));

			return new Vector3(x, y, z);
		}
	}
}