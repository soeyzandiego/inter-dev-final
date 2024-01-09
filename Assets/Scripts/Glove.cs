using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : Clickable
{
    [SerializeField] GameObject puzzle;
    GameObject logicPuzzleCanvas;

    void Start()
    {
        logicPuzzleCanvas = GameObject.FindWithTag("LogicPuzzleCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (HoveringThis())
        {
            if (Input.GetMouseButtonDown(0))
            {
                ObjectClick.OnContinue onContinue = SpawnPuzzle;
                FindObjectOfType<ObjectClick>().SetOnContinue(onContinue);
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void SpawnPuzzle()
    {
        GameObject newPuzzle = Instantiate(puzzle, logicPuzzleCanvas.transform);
        LogicPuzzleManager puzzleManager = newPuzzle.GetComponent<LogicPuzzleManager>();
        puzzleManager.gameManager = FindObjectOfType<GameManager>();

        puzzleManager.canvas = logicPuzzleCanvas;
    }
}
