using UnityEngine;
using UnityEngine.SceneManagement;
using Qualcomm.Snapdragon.Spaces.Samples;

public class GameLoadManager : MonoBehaviour
{
    [SerializeField] private int idx;
    private InteractionManager _interactionManager;

    public void SetIdx(int idx)
    {
        this.idx = idx;
    }

    void Start()
    {
        _interactionManager ??= FindObjectOfType<InteractionManager>(true);
    }

    public void LoadSceneByName()
    {       
        _interactionManager?.SendHapticImpulse();
        string sceneName = (idx == 0) ? "RunningAloneScene" : (idx == 1) ? "RunningWithRecordScene" : "RunningWithFriendScene";
        SceneManager.LoadScene(sceneName);
    }
}
