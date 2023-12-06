using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectClick : MonoBehaviour
{
    [Header("Dialogue")]
    public TMP_Text dialogueText; // Reference to the TextMeshPro Text component for dialogue
    public GameObject dialogueBox; // Reference to the dialogue box UI GameObject
    public Button continueButton; // Reference to the "Continue" button 
    public Image grimoireBox; // Reference to the Image for the dialogue 

    [Header("Audio")]
    public AudioClip clickSound;

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
<<<<<<< HEAD
        objectDialogues.Add("g_gate", "It looks like a locked gate?");
        objectDialogues.Add("g_post", "Straight to the point, aren't we.");
        objectDialogues.Add("g_skull", "Glad to know the town name is based off... something...");
        objectDialogues.Add("g_cacti", "Awe look, some small cacti.");
        objectDialogues.Add("g_sand", "Wow, lots and lots of sand.\nI best stick to the path as to not get my fabluous outfit too dirty. ");
        objectDialogues.Add("g_map", "Oh hey, whats that?\nOh great! It's a map!");
        objectDialogues.Add("g_tree", "Looks like everything is dead around here...");
        objectDialogues.Add("g_sky", "It's so hot out here. I should have worn something lighter.\nI guess the desert isn't made for detectives. Or at least stylish ones.");
        objectDialogues.Add("os_sign", "I wonder what that is pointing towards?");
        objectDialogues.Add("os_bucket", "I don't even want to image what that is for.");
        objectDialogues.Add("os_shelter", "Oh hey! It's the rest stop.");
        objectDialogues.Add("os_cactus", "Hello spikey friend.");
        objectDialogues.Add("os_sand", "More sand...");
        objectDialogues.Add("os_sky", "Even the sky is depressed at how hot it is.");
        objectDialogues.Add("ra_skull", "Is this what she meant by 'follow the animals'?");
        objectDialogues.Add("ra_cacti", "Glad to see my friends are here");
        objectDialogues.Add("ra_entrance", "Is this the entrance to the temple?\nLooks like some of the stuff that was covering it was broken.");
        objectDialogues.Add("ra_glove", "What's this?\nIs this one of sally's gloves?");
        objectDialogues.Add("ra_rocks", "Epic, something else other than sand.");
        objectDialogues.Add("i_apple", "You know what they say about apples...\nAlthough I doubt there are any doctors out here.\nMaybe that is the point.");
        objectDialogues.Add("i_chair", "Very... Uh... Cozy in here...");
        objectDialogues.Add("i_bottles", "Thank everything I was geting pretty thirsty.");
        objectDialogues.Add("i_light", "'All the better to see you with my dear.'");
        objectDialogues.Add("i_bottles", "Thank everything I was geting pretty thirsty.");
        objectDialogues.Add("i_area", "You would think with all this space to build, the place would maybe be.... bigger?");
        objectDialogues.Add("h_bed", "Looks comfy.");
        objectDialogues.Add("h_yarn", "I wonder when I get older if I will also love knitting?");
        objectDialogues.Add("h_around", "How long has she lived here?");
        objectDialogues.Add("h_books", "Hmm. I wonder what these are about?");
        objectDialogues.Add("h_heater", "I guess it does get cold at night in the desert.");

=======
        objectDialogues.Add("rock", "It looks like a locked gate?");
        objectDialogues.Add("post", "Straight to the point, aren't we.");
        objectDialogues.Add("skull", "Glad to know the town name is based off... something...");
        objectDialogues.Add("cacti", "Awe look, some small cacti.");
        objectDialogues.Add("sand", "Wow, lots and lots of sand. \r\nI best stick to the path as to not get my fabluous outfit too dirty. ");
        objectDialogues.Add("map", "Oh hey, whats that?\r\nOh great! It's a map!");
        objectDialogues.Add("tree", "Looks like everything is dead around here...");
        objectDialogues.Add("sky", "It's so hot out here. I should have worn something lighter.\r\nI guess the desert isn't made for detectives. Or at least stylish ones.");
        objectDialogues.Add("sign", "I wonder what that is pointing towards?");
        objectDialogues.Add("bucket", "I don't even want to image what that is for.");
        objectDialogues.Add("shelter", "Oh hey! It's the rest stop.");
        objectDialogues.Add("cactus", "Hello spikey friend.");
        objectDialogues.Add("yarn", "I wonder when I get older if I will also love knitting?");
        objectDialogues.Add("around", "How long has she lived here?");
        objectDialogues.Add("books", "Hmm. I wonder what these are about?");
        objectDialogues.Add("heater", "I guess it does get cold at night in the desert.");
        objectDialogues.Add("bed", "Looks comfy.");
        objectDialogues.Add("apple", "You know what they say about apples...\r\nAlthough I doubt there are any doctors out here. \r\nMaybe that is the point.");
        objectDialogues.Add("chair", "Very... Uh... Cozy in here...");
        objectDialogues.Add("bottles", "Thank everything I was geting pretty thirsty.");
        objectDialogues.Add("light", "All the better to see you with my dear.");
        objectDialogues.Add("area", "You would think with all this space to build, the place would maybe be.... bigger?");
        objectDialogues.Add("skull2", "Is this what she meant by follow the animals?");
        objectDialogues.Add("entrance", "Is this the entrance to the temple?\r\nIt looks like some of the stuff that was covering it was broken.");
        objectDialogues.Add("gloves", "What's this?\r\nIs this one of sally's gloves?");
        objectDialogues.Add("rocks", "Epic, something else other than sand.");
>>>>>>> origin/main


        // Initially, hide the "Continue" button
        continueButton.gameObject.SetActive(false);

        continueButton.onClick.AddListener(OnContinueButtonClicked); // Attach a click event to the "Continue" button
    }

    private void Update()
    {
        // if there's a conversation going on, don't look for object clicks
        if (DialogueManager.state != DialogueManager.DialogueStates.NONE) { return; }
        // if a panel is open, don't look for dialogue clicks
        if (GameManager.loadPanel) { return; }

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
                    if (clickSound != null) { SoundManager.PlaySound(clickSound); }

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
