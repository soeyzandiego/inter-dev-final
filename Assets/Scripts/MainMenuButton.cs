using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public void Play()
    {
        FindObjectOfType<SceneLoader>().QueueScene(1);
    }
}
