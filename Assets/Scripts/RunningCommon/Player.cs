using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	public GPXLogger								GPXLogger;
	public List<Tuple<GPSData, double, Vector3>>	route = new List<Tuple<GPSData, double, Vector3>>();
	public LocationModule  							LocationModule;
	public StateBar									stateBar;
	public TMP_Text									playertext;
	//public GameObject								speedLine;
	public int										size;

	private bool									isPaused;
	private double									totalDist = 0;
	private double									prevTotalDist = 0;
	private double									longitude, latitude, altitude;
	private double									sectionDist;
	private double									distUnit;
	private GPSData									currGPSData, prevGPSData;

    public List<Tuple<GPSData, double, Vector3>> getRoute()
	{
		return (route);
	}

	public double GetTotalDist()
	{
		return (totalDist);
	}

	public void ToggleIsPaused()
    {
		isPaused = !isPaused;
    }

	public void Start()
    {		
		isPaused = false;
		distUnit = 0.01;
		currGPSData = new GPSData(LocationModule.latitude, LocationModule.longitude, LocationModule.altitude);
		StartCoroutine(UpdateLocation());
		StartCoroutine(UpdateDistance());
	}

	public IEnumerator UpdateDistance()
    {
        while (true)
		{
			yield return new WaitForSecondsRealtime(0.02f);
			if (!isPaused)
				totalDist += distUnit;		
		}		
    }

	public IEnumerator UpdateLocation()
    {		
		while (true)
		{	
			yield return new WaitForSecondsRealtime(1f);
			//double decimalDist = totalDist - Math.Floor(totalDist);

            //if (decimalDist >= 0.95 || decimalDist <= 0.05)
            //{
            //    //if (decimalDist == 0)

            //        speedLine.SetActive(true);
            //}
            //else
            //    speedLine.SetActive(false);

			///debug///
            playertext.text = totalDist.ToString();
			///////////            

			//prevTotalDist = totalDist;		
			prevGPSData = currGPSData;

			latitude = LocationModule.latitude;
			longitude = LocationModule.longitude;
			altitude = LocationModule.altitude;
			currGPSData = new GPSData(latitude, longitude, altitude);

			sectionDist = GPSUtils.CalculateDistance(prevGPSData, currGPSData);
			if (sectionDist >= 5)
            {
				if (size < 2)
				{
					continue;
				}
				else
                {
					latitude = route[size - 1].Item1.latitude + (route[size - 1].Item1.latitude - route[size - 2].Item1.latitude);
					longitude = route[size - 1].Item1.longitude + (route[size - 1].Item1.longitude - route[size - 2].Item1.longitude);
					altitude = route[size - 1].Item1.altitude + (route[size - 1].Item1.altitude - route[size - 2].Item1.altitude);
					currGPSData.latitude = latitude;
					currGPSData.longitude = longitude;
					currGPSData.altitude = altitude;
					sectionDist = route[size - 1].Item2 - route[size - 2].Item2;
				}
            }
			distUnit = Math.Abs(sectionDist * 0.02);
            route.Add(Tuple.Create(currGPSData, totalDist, LocationModule.GetDirectionVector()));			
			size++;
			GPXLogger.AppendTrackPointToGPXFile(latitude, longitude, altitude);
        }
    }
}