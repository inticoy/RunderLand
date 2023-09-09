using UnityEngine;

public class InstantiateAvatar : MonoBehaviour
{
    public Transform parentObject;

    private GameObject prefabToInstantiate;
    private GameObject newPrefabInstance;

    void Start()
    {
        prefabToInstantiate = Resources.Load<GameObject>("AnimeMan");
        newPrefabInstance = Instantiate(prefabToInstantiate, parentObject);
    }

    void Update()
    {
        
    }
}
