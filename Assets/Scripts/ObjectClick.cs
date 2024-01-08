using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Unity.Burst.CompilerServices;

public class ObjectClick : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] TMP_Text dialogueText; // Reference to the TextMeshPro Text component for dialogue
    [SerializeField] GameObject dialogueBox; // Reference to the dialogue box UI GameObject
    [SerializeField] Button continueButton; // Reference to the "Continue" button 
    [SerializeField] Image grimoireBox; // Reference to the Image for the dialogue 
    [SerializeField] GameObject map;

    [Header("Audio")]
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip continueSound;

    GameManager gameManager;

    public delegate void OnContinue();
    OnContinue onContinue;

    // all the game objects in the scene go here
    Dictionary<string, string> objectDialogues = new Dictionary<string, string>();

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        map = GameObject.FindGameObjectWithTag("map");
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
            grimoireBox.gameObject.SetActive(false);
        }

        // add the objects here with ("objectTag", "Dialogue...");
        objectDialogues.Add("gate", "It looks like a locked gate?");
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
        objectDialogues.Add("apple", "You know what they say about apples...\r\nAlthough I doubt there are any doctors out here.");
        objectDialogues.Add("chair", "Very... Uh... Cozy in here...");
        objectDialogues.Add("bottles", "Thank everything I was geting pretty thirsty.");
        objectDialogues.Add("light", "All the better to see you with my dear.");
        objectDialogues.Add("area", "You would think with all this space to build, the place would maybe be.... bigger?");
        objectDialogues.Add("skull2", "Is this what she meant by follow the animals?");
        objectDialogues.Add("entrance", "Is this the entrance to the temple?\r\nIt looks like some of the stuff that was covering it was broken.");
        objectDialogues.Add("gloves", "What's this?\r\nIs this one of sally's gloves?");
        objectDialogues.Add("rocks", "Epic, something else other than sand.");
        objectDialogues.Add("gatecomplete", "There's that! That wasn't so bad. I'll have to get one of these for myself...");
        objectDialogues.Add("lesters", "Ah so here is the fated rest stop!");


        // Initially, hide the "Continue" button
        continueButton.gameObject.SetActive(false);

        continueButton.onClick.AddListener(OnContinueButtonClicked); // Attach a click event to the "Continue" button
    }

    private void Update()
    {
        if (!GameManager.clickable) { return; }

        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

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
                    var glass = Instantiate(gameManager.magGlass, new Vector3(mousePos.x, mousePos.y, mousePos.z), Quaternion.identity);
                    Destroy(glass, 0.6f);

                    if (clickSound != null) { SoundManager.PlaySound(clickSound, 0.9f); }

                    StartCoroutine(delay(objectName));
                    foreach (Button button in gameManager.buttons)
                    {
                        button.interactable = false;
                    }
                    continueButton.interactable = true;
                }
            }
        }
    }

    IEnumerator delay(string name)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        //Debug.Log(name);
        SpawnObjectDialogue(name);
    }

    public void SpawnObjectDialogue(string objectName)
    {
        GameManager.DisableClickables();

        // set the dialouge box to active
        dialogueBox.SetActive(true);

        // set the image to active
        grimoireBox.gameObject.SetActive(true);

        // and show the dialouge
        dialogueText.text = objectDialogues[objectName];

        if(objectName.Equals("map"))
        {
            gameManager.UnlockMap();
            Destroy(map);
        }

        // Show the "Continue" button
        continueButton.gameObject.SetActive(true);
    }

    private void OnContinueButtonClicked()
    {
        if (continueSound != null) { SoundManager.PlaySound(continueSound, 0.9f); }
        dialogueBox.SetActive(false);
        continueButton.gameObject.SetActive(false);
        grimoireBox.gameObject.SetActive(false);
        foreach (Button button in gameManager.buttons)
        {
            button.interactable = true;
        }
        GameManager.EnableClickables();

        if (onContinue != null) { onContinue(); }
        onContinue = null;
    }

    public void SetOnContinue(OnContinue function)
    {
        onContinue = function;
    }
}
