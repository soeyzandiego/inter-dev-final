using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour
{
    public GameObject puzzle;
    public GameObject logicPuzzleCanvas;

    void Start()
    {
        logicPuzzleCanvas = GameObject.FindWithTag("LogicPuzzleCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (LogicPuzzleManager.logicOpen) { return; }
        if (DialogueManager.state != DialogueManager.DialogueStates.NONE) { return; }
        if (GameManager.loadPanel) { return; }
        if (GameManager.walkMode) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            // raycast bs
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(mousePosition, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    GameObject newPuzzle = Instantiate(puzzle, logicPuzzleCanvas.transform);
                    LogicPuzzleManager puzzleManager = puzzle.GetComponent<LogicPuzzleManager>();
                    puzzleManager.gameManager = FindObjectOfType<GameManager>();

                    puzzleManager.canvas = logicPuzzleCanvas;
                }
            }
        }
    }
}
