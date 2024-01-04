using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : Clickable
{
    [SerializeField] GameObject puzzle;
    [SerializeField] GameObject logicPuzzleCanvas;

    void Start()
    {
        logicPuzzleCanvas = GameObject.FindWithTag("LogicPuzzleCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (!clickable) { return; }

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
                    LogicPuzzleManager puzzleManager = newPuzzle.GetComponent<LogicPuzzleManager>();
                    puzzleManager.gameManager = FindObjectOfType<GameManager>();

                    puzzleManager.canvas = logicPuzzleCanvas;
                }
            }
        }
    }

    public override void SetClickable(bool _clickable)
    {
        clickable = _clickable;
    }
}
