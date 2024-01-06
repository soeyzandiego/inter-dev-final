using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionCheck : MonoBehaviour
{
    [SerializeField] GameObject entrance;
    [SerializeField] GameManager gameManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GatePuzzleCircles"))
        {
            gameManager.PuzzleComplete();
            gameManager.GetComponent<ObjectClick>().SpawnObjectDialogue("gatecomplete");

            ObjectClick.OnContinue onContinue = GoBackToEntrance;
            gameManager.GetComponent<ObjectClick>().SetOnContinue(onContinue);
            Destroy(gameObject);
        }
    }

    void GoBackToEntrance()
    {
        gameManager.moveToRoom(entrance);
        gameManager.WalkModeToggle();
    }
}
