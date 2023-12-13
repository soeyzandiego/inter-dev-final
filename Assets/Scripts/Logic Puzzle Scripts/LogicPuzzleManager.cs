using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicPuzzleManager : MonoBehaviour
{
    [SerializeField] GameObject logicObjectPrefab; // Prefab for the draggable evidence pieces.
    [SerializeField] GameObject logicSlotPrefab; // Prefab for the empty evidence slot that the evidence pieces should snap to.
    [SerializeField] GameObject logicWheelPrefab; // Prefab for the logic wheel (e.g "has always had permission from", "only recently got permission from", etc.
    [SerializeField] GameObject logicPuzzleBGPrefab;
    [SerializeField] GameObject confirmButtonPrefab;
    [SerializeField] GameObject textPrefab;
    [SerializeField] GameObject[] logicSlot = new GameObject[2]; // Reference to the logic slots, to check for correct evidence.
    [SerializeField] GameObject[] logicWheel = new GameObject[2]; // Reference to the logic wheels, to check for correct evidence.

    [SerializeField] string logicQuestion;
    [SerializeField] int[] correctEvidence = new int[4]; // Array of correct evidence indexes
    [SerializeField] GameObject selectedItem; // The item currently being dragged.
    [SerializeField] Vector3 selectOffset; // Vector offset to ensure the dragged item doesnt move into clipping plane while being dragged.
    [SerializeField] Sprite[] logicObjectSprites = new Sprite[8]; // Array of sprites to assign to draggable evidence pieces.
    [SerializeField] string[] logicObjectDesc = new string[8]; // Array of strings to assign to draggable evidence pieces. (depends on whether the sprites for the evidence pieces includes the text or not.
    [SerializeField] string[] logicWheelDesc = new string[8]; // descriptions to be assigned to the logicwheel objects.
    [SerializeField] GameObject[] logicObjects = new GameObject[8]; // Reference to logicObjects (possibly not necessary?)
    [SerializeField] Vector3 hLobjectOffset = new Vector3(3, 0, 0); // horizontal spacing for instantiating logic objects.
    [SerializeField] Vector3 vLobjectOffset = new Vector3(0, 1.75f, 0);  // vertical spacing for instantiating logic objects.
    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject question;
    [SerializeField] GameObject background;
    [SerializeField] public string clueID;
    [SerializeField] public GameObject canvas; // associated canvas object's game object.

    void Start()
    {
        foreach (GameObject clicker in FindObjectOfType<GameManager>().clicker)
        {
            if (clicker.GetComponent<Collider2D>() != null)
            {
                clicker.GetComponent<Collider2D>().enabled = false;
            }
        }

        confirmButton = Instantiate(confirmButtonPrefab, canvas.transform);
        confirmButton.GetComponent<Button>().onClick.AddListener(() => { CheckCorrect(); });
        confirmButton.transform.position = confirmButton.transform.position - (Vector3)vLobjectOffset;
        background = Instantiate(logicPuzzleBGPrefab, canvas.transform);
        background.transform.position = background.transform.position + new Vector3(0, 0, 10);
        question = Instantiate(textPrefab, canvas.transform);
        question.transform.position = new Vector3(0, 4, 0);
        question.GetComponent<TMP_Text>().text = logicQuestion;
        int count = 0;
        // Instantiating logic object prefabs (the draggable evidence pieces)
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                logicObjects[count] = Instantiate(logicObjectPrefab, canvas.transform);
                LogicObject temp = logicObjects[count].GetComponent<LogicObject>(); //assigns the current evidence piece to temp, to instantiate fields in LogicObject
                temp.transform.position = transform.position + hLobjectOffset * (j - 1.5f) - Vector3.right + vLobjectOffset * (i + 0.25f);
                //temp.GetComponent<SpriteRenderer>().sprite = logicObjectSprites[count]; Commented out for testing. PLEASE UNCOMMENT THIS WHEN U NEED TO PUT IN UR DESCRIPTIONS N WHATNOT.
                temp.GetComponentInChildren<TMP_Text>().text = logicObjectDesc[count];
                count++;
                temp.evidenceNum = count; //assigns evidence number identifier, to be used when checking the logic puzzle for correctness.
            }

            for (int j = 0; j < 2; j++)
            {
                logicObjects[count] = Instantiate(logicObjectPrefab, canvas.transform);
                LogicObject temp = logicObjects[count].GetComponent<LogicObject>(); //assigns the current evidence piece to temp, to instantiate fields in LogicObject
                temp.transform.position = transform.position + hLobjectOffset * (j + 0.5f) + Vector3.right + vLobjectOffset * (i + 0.25f);
                //temp.GetComponent<SpriteRenderer>().sprite = logicObjectSprites[count]; Commented out for testing. PLEASE UNCOMMENT THIS WHEN U NEED TO PUT IN UR DESCRIPTIONS N WHATNOT.
                temp.GetComponentInChildren<TMP_Text>().text = logicObjectDesc[count];
                count++;
                temp.evidenceNum = count; //assigns evidence number identifier, to be used when checking the logic puzzle for correctness.
            }
        }

        count = 0;
        for (int i = 0; i < 2; i++)
        {
            logicSlot[i] = Instantiate(logicSlotPrefab, canvas.transform);
            logicSlot[i].transform.position = transform.position + new Vector3((i - .5f) * 6 - 2f, -3.5f, 1);
            logicSlot[i].GetComponent<LogicSlot>().rightEvidence = correctEvidence[i * 2];


            logicWheel[i] = Instantiate(logicWheelPrefab, canvas.transform);
            logicWheel[i].transform.position = transform.position + new Vector3((i - .5f) * 6 + 1f, -3.5f, 1);
            logicWheel[i].GetComponent<LogicWheel>().rightEvidence = correctEvidence[i * 2 + 1];
            for (int j = 0; j < 4; j++)
            {
                logicWheel[i].GetComponentInChildren<LogicWheel>().evidence[j] = logicWheelDesc[count];
                count++;
            }
        }

    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D selectedObject = Physics2D.OverlapPoint(mousePosition);
            if (selectedObject != null && selectedObject.gameObject.tag.Equals("LogicObject"))
            {
                selectedItem = selectedObject.transform.gameObject;
                selectOffset = selectedItem.transform.position - mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedItem != null)
        {
            selectedItem.transform.position = selectedItem.GetComponent<LogicObject>().targetPos;
            selectedItem = null;
        }

        if (selectedItem != null)
        {
            selectedItem.transform.position = mousePosition + selectOffset;
        }
    }

    //public bool CheckCorrect()
    //{
    //    Debug.Log("Works");

    //    for (int i = 0; i < 2; i++)
    //    {
    //        LogicSlot tempSlot = logicSlot[i].GetComponent<LogicSlot>();
    //        LogicWheel tempWheel = logicWheel[i].GetComponent<LogicWheel>();
    //        if (!tempSlot.CheckEvidence() || !tempWheel.correctEvidence)
    //        {
    //            return false;
    //        }
    //    }

    //    GameManager.UnlockClue(clueID);
        
    //    return true;
    //}

    public void CheckCorrect()
    {
        Debug.Log("Works");

        for (int i = 0; i < 2; i++)
        {
            LogicSlot tempSlot = logicSlot[i].GetComponent<LogicSlot>();
            LogicWheel tempWheel = logicWheel[i].GetComponent<LogicWheel>();

            if (!tempSlot.CheckEvidence() || !tempWheel.correctEvidence)
            {
                return;
            }
        }

        Debug.Log("correct");
        GameManager.UnlockClue(clueID);
        Destroy(gameObject);

        return;
    }

    private void OnDestroy()
    {
        foreach (GameObject clicker in FindObjectOfType<GameManager>().clicker)
        {
            if (clicker.GetComponent<Collider2D>() != null)
            {
                clicker.GetComponent<Collider2D>().enabled = true;
            }
        }

        foreach (GameObject o in logicSlot)
        {
            Destroy(o);
        }
        foreach (GameObject o in logicWheel)
        {
            Destroy(o);
        }
        foreach (GameObject o in logicObjects)
        {
            Destroy(o);
        }
        Destroy(question);
        Destroy(confirmButton);
        Destroy(background);
    }
}
