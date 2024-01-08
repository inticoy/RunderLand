using UnityEngine;
using UnityEngine.SceneManagement;
using Qualcomm.Snapdragon.Spaces.Samples;
using System.Collections;

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
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadMyAsyncScene(sceneName));
    }

    IEnumerator LoadMyAsyncScene(string sceneName)
    {
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
