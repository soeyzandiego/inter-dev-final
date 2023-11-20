using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectClick : MonoBehaviour
{
    public TMP_Text dialogueText; // Reference to the TextMeshPro Text component for dialogue
    public GameObject dialogueBox; // Reference to the dialogue box UI GameObject
    public Button continueButton; // Reference to the "Continue" button 
    public Image grimoireBox; // Reference to the Image for the dialogue 


    // all the game objects in the scene go here
    private Dictionary<string, string> objectDialogues = new Dictionary<string, string>();

    private void Start()
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
            grimoireBox.gameObject.SetActive(false);
        }

        // add the objects here with ("objectTag", "Dialogue...");
        objectDialogues.Add("rock", "That gate leads to the town");

        // Initially, hide the "Continue" button
        continueButton.gameObject.SetActive(false);

        continueButton.onClick.AddListener(OnContinueButtonClicked); // Attach a click event to the "Continue" button
    }

    private void Update()
    {
        // if there's a conversation going on, don't look for object clicks
        if (DialogueManager.state != DialogueManager.DialogueStates.NONE) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            // raycast bs
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(mousePosition, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // if the mouse is colliding with somehting with a box collider
            if (hit.collider != null)
            {
                // get the string of the tag of hte object
                string objectName = hit.collider.gameObject.tag;

                // if then the objectDialogues Dictionary has that object tag and dialouge with it
                if (objectDialogues.ContainsKey(objectName))
                {
                    // set the dialouge box to active
                    dialogueBox.SetActive(true);

                    // set the image to active
                    grimoireBox.gameObject.SetActive(true);

                    // and show the dialouge
                    dialogueText.text = objectDialogues[objectName];

                    // Show the "Continue" button
                    continueButton.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnContinueButtonClicked()
    {
        dialogueBox.SetActive(false);
        continueButton.gameObject.SetActive(false);
        grimoireBox.gameObject.SetActive(false);
    }
}
