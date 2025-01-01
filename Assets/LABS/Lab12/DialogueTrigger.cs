using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager DialogueManager;
    public bool hasBeenTriggered;
    public Dialogue shortdialogue;

    public GameManagerLabs gameManager;


    void Start() {

        ResetTrigger();
        DialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update() {
        hasBeenTriggered = DialogueManager.hasBeenTriggered;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            
            if (!hasBeenTriggered)
            {

                TriggerDialogue();
                
                
            }
            else
            {
                
                ShowShortMessage();
            }
        }
    }


    public void TriggerDialogue()
    {
        gameManager.StartDialogue(dialogue);
        
    }

    private void ShowShortMessage()
    {
        string shortMessage = GetShortMessage();
        gameManager.StartDialogue(shortdialogue);
    }

    private string GetShortMessage()
    {
      
        return dialogue.sentences.Length > 0 ? dialogue.sentences[0] : "Нет доступного диалога";
    }

    public void ResetTrigger()
    {
        hasBeenTriggered = false;
    }
}
