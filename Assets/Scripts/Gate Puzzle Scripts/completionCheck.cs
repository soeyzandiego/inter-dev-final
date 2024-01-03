using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionCheck : MonoBehaviour
{
    [SerializeField] GameObject gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GatePuzzleCircles"))
        {
            gameManager.GetComponent<GameManager>().PuzzleComplete();
            gameManager.GetComponent<ObjectClick>().SpawnObjectDialogue("gatecomplete");
            Destroy(gameObject);
        }
    }
}
