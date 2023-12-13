using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class completionCheck : MonoBehaviour
{
    public GameObject gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GatePuzzleCircles"))
        {
            gameManager.GetComponent<GameManager>().PuzzleComplete();
            gameManager.GetComponent<ObjectClick>().SpawnObjectDialogue("gatecomplete");
            Destroy(this.gameObject);
        }
    }
}
