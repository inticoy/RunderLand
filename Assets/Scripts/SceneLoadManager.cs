using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Qualcomm.Snapdragon.Spaces.Samples;

public class SceneLoadManager : MonoBehaviour
{
    private InteractionManager _interactionManager;

    void Start()
    {
        _interactionManager ??= FindObjectOfType<InteractionManager>(true);
    }

    public void LoadSceneByName(string sceneName)
    {
        _interactionManager?.SendHapticImpulse();
        SceneManager.LoadScene(sceneName);
    }
}
