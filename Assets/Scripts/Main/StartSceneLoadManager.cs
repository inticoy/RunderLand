using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Qualcomm.Snapdragon.Spaces.Samples;

public class StartSceneLoadManager : MonoBehaviour
{
    public Animator manAnimator;
    public GameObject man;
    public float movementStep = 0.005f;
    public int totalSteps = 100;
    public float delayTime = 5f; // Delay time in seconds


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

        manAnimator.SetBool("is Run", true);
        for (int i = 0; i < totalSteps; i++)
        {
            //man.transform.position = Vector3.Lerp(transform.position, targetPosition, totalSteps);

            man.transform.position += -1.0f * movementStep * Vector3.Normalize(man.transform.parent.forward);
            //man.transform.position += -0.25f * movementStep * Vector3.Normalize(man.transform.parent.up);
            //man.transform.localScale += new Vector3(movementStep, movementStep, movementStep);
            yield return null; // Wait for one frame
        }
        for (int i = 0; i < 5; i++)
        {
            man.transform.position += 0.001f * Vector3.Normalize(man.transform.forward);
            man.transform.Rotate(0, -18f, 0);
            yield return null;
        }
        //man.transform.Rotate(0, -90, 0);
        StartCoroutine(MoveManGradually(sceneName));
        for (int i = 0; i < totalSteps * 2; i++)
        {
            //man.transform.position = Vector3.Lerp(transform.position, targetPosition, totalSteps);
            man.transform.position += -0.5f * movementStep * Vector3.Normalize(-man.transform.parent.right);
            //man.transform.position += -0.25f * movementStep * Vector3.Normalize(man.transform.parent.up);
            //man.transform.localScale += new Vector3(movementStep, movementStep, movementStep);
            yield return null; // Wait for one frame
        }

        // Ensure that the final position is set correctly
        //man.transform.position = targetPosition;
        // Start both coroutines simultaneously
    }

    private IEnumerator MoveManGradually(string sceneName)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        _interactionManager?.SendHapticImpulse();
        SceneManager.LoadScene(sceneName);
    }
}