using System;
using System.Collections.Generic;
using Firebase.Database;

namespace TrainingBuddy.Utility

{
	public class UtilityMethods
	{
		// public static List<Coordinate> FindPointsWithinRadius(List<Coordinate> points, Coordinate staticPoint, double radiusInMeters)
		// {
		// 	List<Coordinate> result = new List<Coordinate>();
		// 	double R = 6371000; // Earth's radius in meters
		//
		// 	foreach (var point in points)
		// 	{
		// 		double latDistance = ToRadians(point.Latitude - staticPoint.Latitude);
		// 		double lonDistance = ToRadians(point.Longitude - staticPoint.Longitude);
		//
		// 		double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
		// 		           Math.Cos(ToRadians(staticPoint.Latitude)) * Math.Cos(ToRadians(point.Latitude)) *
		// 		           Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
		//
		// 		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		// 		double distance = R * c;
		//
		// 		if (distance <= radiusInMeters)
		// 		{
		// 			result.Add(point);
		// 		}
		// 	}
		//
		// 	return result;
		// }
		//
		// public class Coordinate
		// {
		// 	public double Latitude { get; set; }
		// 	public double Longitude { get; set; }
		// }
		//
		// private static double ToRadians(double angle)
		// {
		// 	return angle * (Math.PI / 180);
		// }
		
		public static void FindUsersInRange(DataSnapshot userList, double radiusInMeters)
		{
			// var closestPoint = CoordinateUtil.FindPointsWithinRadius(points, staticPoint, radiusInMeters);
		}
		
		
		// public static class CoordinateUtil
		// {
		// 	public static List<Coordinate> FindPointsWithinRadius(List<Coordinate> points, Coordinate staticPoint, double radiusInMeters)
		// 	{
		// 		List<Coordinate> result = new List<Coordinate>();
		// 		double R = 6371000; // Earth's radius in meters
		//
		// 		foreach (var point in points)
		// 		{
		// 			double latDistance = ToRadians(point.Latitude - staticPoint.Latitude);
		// 			double lonDistance = ToRadians(point.Longitude - staticPoint.Longitude);
		//
		// 			double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
		// 			           Math.Cos(ToRadians(staticPoint.Latitude)) * Math.Cos(ToRadians(point.Latitude)) *
		// 			           Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
		//
		// 			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		// 			double distance = R * c;
		//
		// 			if (distance <= radiusInMeters)
		// 			{
		// 				result.Add(point);
		// 			}
		// 		}
		//
		// 		return result;
		// 	}
		//
		// 	private static double ToRadians(double angle)
		// 	{
		// 		return angle * (Math.PI / 180);
		// 	}
		// }
	}
}