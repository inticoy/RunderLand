using UnityEngine;

public class InstantiateAvatar : MonoBehaviour
{
    public Transform parentObject;

    private GameObject prefabToInstantiate;

    void Start()
    {
        prefabToInstantiate = Resources.Load<GameObject>(PlayerPrefs.GetString("avatar"));
        Instantiate(prefabToInstantiate, parentObject);
    }
}
