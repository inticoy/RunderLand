using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Qualcomm.Snapdragon.Spaces.Samples;

public class StartSceneLoadManager : MonoBehaviour
{

    public GameObject man;
    public float movementStep = 0.001f;
    public int totalSteps = 5000;
    public float delayTime = 1.0f; // Delay time in seconds


    private InteractionManager _interactionManager;

    void Start()
    {

        _interactionManager ??= FindObjectOfType<InteractionManager>(true);
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(MoveManGraduallyAndLoad(sceneName));
    }

    private IEnumerator MoveManGraduallyAndLoad(string sceneName)
    {
        // Start both coroutines simultaneously
        StartCoroutine(MoveManGradually());
        yield return new WaitForSeconds(delayTime);

        _interactionManager?.SendHapticImpulse();
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator MoveManGradually()
    {

        for (int i = 0; i < totalSteps; i++)
        {
            //man.transform.position = Vector3.Lerp(transform.position, targetPosition, totalSteps);

            man.transform.position += -0.25f * movementStep * Vector3.Normalize(man.transform.parent.forward);
            man.transform.position += -0.25f * movementStep * Vector3.Normalize(man.transform.parent.up);
            man.transform.localScale += new Vector3(movementStep, movementStep, movementStep);
            yield return null; // Wait for one frame
        }

        // Ensure that the final position is set correctly
        //man.transform.position = targetPosition;
    }
}