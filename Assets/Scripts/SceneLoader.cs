using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader instance;

    int queuedSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<SceneLoader>().Length > 1) { Destroy(gameObject); }
        else { DontDestroyOnLoad(gameObject); }
    }

    public void QueueScene(int index)
    {
        queuedSceneIndex = index;
        GetComponent<Animator>().SetTrigger("FadeOut");
    }

    // used by animation event
    public void LoadQueued()
    {
        SceneManager.LoadScene(queuedSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
