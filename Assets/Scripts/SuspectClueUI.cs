using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspectClueUI : MonoBehaviour
{
    [SerializeField] Image cluePicture;

    TMP_Text clueText;

    // Start is called before the first frame update
    void Awake()
    {
        clueText = GetComponentInChildren<TMP_Text>();
    }

    public void SetClue(Sprite pic, string text)
    {
        cluePicture.enabled = true;
        clueText.enabled = true;

        cluePicture.sprite = pic;
        clueText.text = text;
    }

    public void HideClue()
    {
        cluePicture.enabled = false;
        clueText.enabled = false;
    }
}
