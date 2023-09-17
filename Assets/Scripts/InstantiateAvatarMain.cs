using UnityEngine;

public class InstantiateAvatarMain : MonoBehaviour
{
    public Transform parentObject;

    private GameObject prefabToInstantiate;
    [SerializeField] private GameObject InstantiatedObject;

    void Start()
    {
        if (PlayerPrefs.GetString("avatar") == "BicycleMan")
        {
            prefabToInstantiate = Resources.Load<GameObject>(PlayerPrefs.GetString("avatar") + "Main");
        }
        else
        {
            prefabToInstantiate = Resources.Load<GameObject>(PlayerPrefs.GetString("avatar"));
        }

        InstantiatedObject = Instantiate(prefabToInstantiate, parentObject);
       
        InstantiatedObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        InstantiatedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        InstantiatedObject.transform.localPosition = new Vector3(0, -0.4f, 0);
    }

    private void Update()
    {
        //InstantiatedObject.transform.localPosition = new Vector3(0, -0.4f, 0);
        //if (PlayerPrefs.GetString("avatar") == "BicycleMan")
        //{
        //    InstantiatedObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        //}
    }
}
