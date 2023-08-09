using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StateBar : MonoBehaviour
{
	public TMP_Text distanceText;
	public TMP_Text unitText;	
	public Image LoadingBar;
	public Player player;
	public LocationModule locationModule;
	public PlayButton playButton;
	[SerializeField] TMP_Text countdownText;
	
	private float distance;
	private float fadeSpeed = 1.2f;
	private float startTime = -1;		
	private int fadeMode;
	private bool isLoadingEnd = false;
	private bool isCountDownEnd = false;

	public bool GetIsCountDownEnd()
    {
		return (isCountDownEnd);
    }

	public bool GetIsLoadingEnd()
    {
		return (isLoadingEnd);
    }

	// Start is called before the first frame update
	void Start()
    {
			unitText.text = "";
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
				if (countdownText != null && Time.time < startTime + 1)
				{
                    float newAlpha = (startTime - Time.time) - Mathf.Floor(startTime - Time.time);
					countdownText.text = "GO!";
                    newAlpha = Mathf.Clamp01(newAlpha);
                    countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, newAlpha);
                    float scale = -newAlpha * 0.002f + 0.005f;
                    countdownText.transform.localScale = new Vector3(scale, scale, scale);
                }
				isCountDownEnd = true;
				distance = Mathf.Floor((float)player.GetTotalDist() / 1000);
				distanceText.text = (distance).ToString("0.00");
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
