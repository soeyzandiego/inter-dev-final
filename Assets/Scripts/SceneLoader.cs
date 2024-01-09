using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader instance;

    bool loading = false;
    int queuedSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<SceneLoader>().Length > 1) { Destroy(gameObject); }
        else { DontDestroyOnLoad(gameObject); }
    }

    public void QueueScene(int index)
    {
        if (loading) { Debug.Log("already loading scene"); return; }

        queuedSceneIndex = index;
        loading = true;
        GetComponent<Animator>().SetTrigger("FadeOut");
    }

    // used by animation event
    public void LoadQueued()
    {
        SceneManager.LoadScene(queuedSceneIndex);

        if (queuedSceneIndex == 0) { GameManager.ResetGame(); }

        loading = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
