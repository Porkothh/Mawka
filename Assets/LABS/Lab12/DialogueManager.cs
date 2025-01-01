using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox; 
    public bool hasBeenTriggered = false;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(false);
    }


    void Update()
    {
        
    }

    public IEnumerator<WaitForEndOfFrame> TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForEndOfFrame();
        }
        hasBeenTriggered = true;
    }

    public void StartDialogue (Dialogue dialogue)
    {
        dialogueBox.SetActive(true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        GameManagerLabs.Instance.OnDialogueComplete();
    }

    public void NextSentence()
    {
        DisplayNextSentence();
    }
}
