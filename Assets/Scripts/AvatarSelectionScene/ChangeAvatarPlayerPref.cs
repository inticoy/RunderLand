using UnityEngine;

public class ChangeAvatarPlayerPref : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] GameObject animeMan;
    [SerializeField] GameObject realisiticMan;
    [SerializeField] GameObject bicycleMan;

    private Vector3 newPosition;

    void Start()
    {
        pointer = GameObject.Find("Pointer");

        if (!PlayerPrefs.HasKey("avatar"))
        {
            PlayerPrefs.SetString("avatar", "AnimeMan");
        }
        chooseAvatar(PlayerPrefs.GetString("avatar"));
    }

    public void chooseAvatar(string avatar)
    {
        PlayerPrefs.SetString("avatar", avatar);
        
        newPosition = pointer.transform.localPosition;
        if (avatar == "AnimeMan")
        {
            newPosition.x = -1.25f;
            animeMan.SetActive(true);
            realisiticMan.SetActive(false);
            bicycleMan.SetActive(false);
        }
        else if (avatar == "RealisticMan")
        {
            newPosition.x = 0.05f;
            animeMan.SetActive(false);
            realisiticMan.SetActive(true);
            bicycleMan.SetActive(false);
        }
        else
        {
            newPosition.x = 1.3f;
            animeMan.SetActive(false);
            realisiticMan.SetActive(false);
            bicycleMan.SetActive(true);
        }
        pointer.transform.localPosition = newPosition;
    }

}
