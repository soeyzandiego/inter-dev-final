using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogicPuzzleManager : MonoBehaviour
{
    [SerializeField] GameObject logicObjectPrefab; // Prefab for the draggable evidence pieces.
    [SerializeField] GameObject logicSlotPrefab; // Prefab for the empty evidence slot that the evidence pieces should snap to.
    [SerializeField] GameObject logicWheelPrefab; // Prefab for the logic wheel (e.g "has always had permission from", "only recently got permission from", etc.
    [SerializeField] GameObject[] logicSlot = new GameObject[2]; // Reference to the logic slots, to check for correct evidence.
    [SerializeField] GameObject[] logicWheel = new GameObject[2]; // Reference to the logic wheels, to check for correct evidence.
    [SerializeField] int[] correctEvidence = new int[4]; // Array of correct evidence indexes
    [SerializeField] GameObject selectedItem; // The item currently being dragged.
    [SerializeField] Vector3 selectOffset; // Vector offset to ensure the dragged item doesnt move into clipping plane while being dragged.
    [SerializeField] Sprite[] logicObjectSprites = new Sprite[8]; // Array of sprites to assign to draggable evidence pieces.
    [SerializeField] string[] logicObjectDesc = new string[8]; // Array of strings to assign to draggable evidence pieces. (depends on whether the sprites for the evidence pieces includes the text or not.
    [SerializeField] string[] logicWheelDesc = new string[8]; // descriptions to be assigned to the logicwheel objects.
    [SerializeField] GameObject[] logicObjects = new GameObject[8]; // Reference to logicObjects (possibly not necessary?)
    [SerializeField] Vector2 hLobjectOffset = new Vector2(3, 0); // horizontal spacing for instantiating logic objects.
    [SerializeField] Vector2 vLobjectOffset = new Vector2(0, 3);  // vertical spacing for instantiating logic objects.
    [SerializeField] GameObject canvas; // associated canvas object's game object.

    void Start()
    {
        int count = 0;
        // Instantiating logic object prefabs (the draggable evidence pieces)
        for(int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                logicObjects[count] = Instantiate(logicObjectPrefab, canvas.transform);
                LogicObject temp = logicObjects[count].GetComponent<LogicObject>(); //assigns the current evidence piece to temp, to instantiate fields in LogicObject
                temp.transform.position = hLobjectOffset * (j - 1.5f) + vLobjectOffset * (i + 0.25f);
                //temp.GetComponent<SpriteRenderer>().sprite = logicObjectSprites[count]; Commented out for testing. PLEASE UNCOMMENT THIS WHEN U NEED TO PUT IN UR DESCRIPTIONS N WHATNOT.
                //temp.GetComponent<TextMeshPro>().text = logicObjectDesc[count];
                count++; 
                temp.evidenceNum = count; //assigns evidence number identifier, to be used when checking the logic puzzle for correctness.
            }
        }

        count = 0;
        for(int i = 0; i < 2; i++)
        {
            logicSlot[i] = Instantiate(logicSlotPrefab, canvas.transform);
            logicSlot[i].transform.position = new Vector3((i - .5f) * 8 - 2.5f, -2, 1);
            logicSlot[i].GetComponent<LogicSlot>().rightEvidence = correctEvidence[i * 2];
            
            
            logicWheel[i] = Instantiate(logicWheelPrefab, canvas.transform);
            logicWheel[i].transform.position = new Vector3((i - .5f) * 8 + 1.5f, -2, 1);
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

    public bool CheckCorrect()
    {
        for (int i = 0; i < 2; i++) 
        {
            LogicSlot tempSlot = logicSlot[i].GetComponent<LogicSlot>();
            LogicWheel tempWheel = logicWheel[i].GetComponent<LogicWheel>();
            if (!tempSlot.CheckEvidence() || !tempWheel.correctEvidence)
            {
                return false;
            }
        }
        Debug.Log("Works");
        return true;
    }
}
