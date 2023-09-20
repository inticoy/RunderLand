using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StateBar : MonoBehaviour
{
	public TMP_Text distanceText;
	public TMP_Text canvasDistanceText;
	public TMP_Text unitText;	
	public Image LoadingBar;
	public Player player;
	public LocationModule locationModule;
	public PlayButton playButton;
	[SerializeField] TMP_Text countdownText;
	
	private float distance;
	private float fadeSpeed = 1.2f;
	private float startTime;		
	private int fadeMode;
	private bool isLoadingEnd;
	private bool isCountDownGoing;
	private bool isCountDownEnd;

	public bool GetIsCountDownEnd()
    {
		return (isCountDownEnd);
    }

	public bool GetIsLoadingEnd()
    {
		return (isLoadingEnd);
    }

	public bool GetIsCountDownGoing()
	{
		return (isCountDownGoing);
	}

	// Start is called before the first frame update
	void Start()
    {
		unitText.text = "";
		isCountDownEnd = false;
		isLoadingEnd = false;
		isCountDownGoing = false;
		startTime = -1;
    }

    // Update is called once per frame
    void Update()
    {		
		// Loading State
		if (!locationModule.isLocationModuleReady)
		{			
			distanceText.text = "Loading";

			if (fadeMode == 0)
			{
				float newAlpha = LoadingBar.color.a - fadeSpeed * Time.deltaTime;
				newAlpha = Mathf.Clamp01(newAlpha);
				LoadingBar.color = new Color(LoadingBar.color.r, LoadingBar.color.g, LoadingBar.color.b, newAlpha);
				if (newAlpha <= 0f)
					fadeMode = 1;
			}
			else
			{
				float newAlpha = LoadingBar.color.a + fadeSpeed * Time.deltaTime;
				newAlpha = Mathf.Clamp01(newAlpha);
				LoadingBar.color = new Color(LoadingBar.color.r, LoadingBar.color.g, LoadingBar.color.b, newAlpha);
				if (newAlpha >= 1f)
					fadeMode = 0;
			}
		}
		// Play button pressed State
		else if (playButton.GetIsButtonPressed())
		{			
			if (startTime == -1)
			{
				isCountDownGoing = true;
				startTime = Time.time + 3;
			}
			else if (Time.time < startTime)
			{
				distanceText.text = Mathf.Floor(startTime - Time.time + 1).ToString();
                LoadingBar.fillAmount = 1 - (startTime - Time.time) + Mathf.Floor(startTime - Time.time);

				if (countdownText != null)
				{
					float newAlpha = (startTime - Time.time) - Mathf.Floor(startTime - Time.time);
					countdownText.text = Mathf.Floor(startTime - Time.time + 1).ToString();
					newAlpha = Mathf.Clamp01(newAlpha);
					countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, newAlpha);
					float scale = -newAlpha * 0.002f + 0.005f;
					countdownText.transform.localScale = new Vector3(scale, scale, scale);
				}
			}
			else
			{
				isCountDownGoing = false;
				if (countdownText != null && Time.time < startTime + 1)
				{
					float newAlpha = (startTime - Time.time) - Mathf.Floor(startTime - Time.time);
					countdownText.text = "GO!";
					newAlpha = Mathf.Clamp01(newAlpha);
					countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, newAlpha);
					float scale = -newAlpha * 0.002f + 0.005f;
					countdownText.transform.localScale = new Vector3(scale, scale, scale);
				}
				else
					countdownText.text = "";
				isCountDownEnd = true;				
				canvasDistanceText.text = (distance).ToString("0.00") + " km";
                LoadingBar.fillAmount = distance - (int)distance;
				unitText.text = "kilometer";
			}		
		}
		// Loading End State, Waiting for Press
		else
		{
			distanceText.text = "";
            playButton.gameObject.SetActive(true);

			LoadingBar.color = new Color(LoadingBar.color.r, LoadingBar.color.g, LoadingBar.color.b, 1);
		}
    }
}
