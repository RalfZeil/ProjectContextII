using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogueManager
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public string dialogueText;
        public bool isSad;
        public bool isHappy;
        public bool isConfused;
        public bool isNeutral;
        public bool isAngry;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public GameObject characterObject;
        public Image characterImage; // UI Image component
        public Sprite sadSprite;
        public Sprite happySprite;
        public Sprite confusedSprite;
        public Sprite neutralSprite;
        public Sprite angrySprite;
    }

    [ExecuteInEditMode]
    public class DialogueSystem2 : MonoBehaviour
    {
        public TMPro.TMP_Text dialogueText;
        public GameObject Background;
        public TMPro.TMP_Text speakerNameText;
        public DialogueLine[] dialogueLines;
        public float typingSpeed = 0.05f; // Typing speed for dialogue animation

        public List<CharacterData> characters = new List<CharacterData>();

        private int currentLine = 0;
        private Coroutine typingCoroutine;
        private Dictionary<string, GameObject> characterObjects = new Dictionary<string, GameObject>();
        private Dictionary<string, Image> characterImages = new Dictionary<string, Image>();

        private void Start()
        {
            foreach (CharacterData character in characters)
            {
                characterObjects.Add(character.characterName, character.characterObject);
                characterImages.Add(character.characterName, character.characterImage);
            }

            DisplayDialogueLine();
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(0))
                    {
                        // Triple-click detected, jump to the end of the text
                        JumpToEndOfText();
                    }
                    else
                    {
                        // Single-click detected, skip to the end of the current line
                        StopTypingAnimation();
                        dialogueText.text = dialogueLines[currentLine].dialogueText;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    // Enter key pressed, go to the next line
                    NextLine();
                }
            }
        }

        private void NextLine()
        {
            currentLine++;

            if (currentLine < dialogueLines.Length)
            {
                DisplayDialogueLine();
            }
            else
            {
                // Dialogue is finished
                EndDialogue();
            }
        }

        private void DisplayDialogueLine()
        {
            if (currentLine < dialogueLines.Length)
            {
                string dialogue = dialogueLines[currentLine].dialogueText;
                string speakerName = dialogueLines[currentLine].speakerName;

                speakerNameText.text = speakerName;

                StopTypingAnimation();
                typingCoroutine = StartCoroutine(TypeDialogue(dialogue));

                SetImageBasedOnDialogue(dialogueLines[currentLine]);
            }
            else
            {
                // Dialogue is finished
                EndDialogue();
            }
        }

        private IEnumerator TypeDialogue(string dialogue)
        {
            dialogueText.text = "";

            for (int i = 0; i < dialogue.Length; i++)
            {
                dialogueText.text += dialogue[i];

                if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(0))
                {
                    // Double-click detected, jump to the end of the text
                    JumpToEndOfText();
                    yield break;
                }

                yield return new WaitForSeconds(typingSpeed);
            }
        }

        private void JumpToEndOfText()
        {
            StopTypingAnimation();
            dialogueText.text = dialogueLines[currentLine].dialogueText;
        }

        private void StopTypingAnimation()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }

        private void SetImageBasedOnDialogue(DialogueLine dialogueLine)
        {
            foreach (KeyValuePair<string, GameObject> characterObject in characterObjects)
            {
                characterObject.Value.SetActive(false);
            }

            if (!characterObjects.ContainsKey(dialogueLine.speakerName))
            {
                Debug.LogWarning("Character with the name '" + dialogueLine.speakerName + "' is not found.");
                return;
            }

            GameObject activeCharacter = characterObjects[dialogueLine.speakerName];
            Image activeImage = characterImages[dialogueLine.speakerName];

            activeImage.gameObject.SetActive(true);

            // Update the image sprite based on the emotions specified in DialogueLine
            if (dialogueLine.isSad)
                activeImage.sprite = characters.Find(x => x.characterName == dialogueLine.speakerName).sadSprite;
            else if (dialogueLine.isHappy)
                activeImage.sprite = characters.Find(x => x.characterName == dialogueLine.speakerName).happySprite;
            else if (dialogueLine.isConfused)
                activeImage.sprite = characters.Find(x => x.characterName == dialogueLine.speakerName).confusedSprite;
            else if (dialogueLine.isNeutral)
                activeImage.sprite = characters.Find(x => x.characterName == dialogueLine.speakerName).neutralSprite;
            else if (dialogueLine.isAngry)
                activeImage.sprite = characters.Find(x => x.characterName == dialogueLine.speakerName).angrySprite;
            else
                Debug.LogWarning("No emotion specified for character '" + dialogueLine.speakerName + "'.");

            activeCharacter.SetActive(true);
        }

        private void EndDialogue()
        {
            Debug.Log("Dialogue ended");

            dialogueText.gameObject.SetActive(false);
            speakerNameText.gameObject.SetActive(false);

            // Deactivate all gameobjects
            foreach (GameObject characterObject in characterObjects.Values)
            {
                characterObject.SetActive(false);
                gameObject.SetActive(false);
                Background.SetActive(false);
            }

            // SceneManager.LoadScene(sceneToLoad);
        }
    }
}
