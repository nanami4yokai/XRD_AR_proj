using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public TextMeshProUGUI dialogueText;
    public CanvasGroup dialogueCanvasGroup;
    public Image avatarImage;
    public Image dialogueBoxImage;
    public Image continueButtonImage;

    public GameObject critterpediaButton; // Reference to the Critterpedia Button outside the fading collection

    public event System.Action OnTomNookDialogueEnd;
    public event Action OnBlathersDialogueEnd;

    private bool waitingForCritters = false;
    private bool critterCollectionComplete = false;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueCanvasGroup.alpha = 0f;
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;

        // Ensure the Critterpedia button starts inactive
        if (critterpediaButton != null)
        {
            critterpediaButton.SetActive(false);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting convo with " + dialogue.name);

        avatarImage.sprite = dialogue.avatar;
        dialogueBoxImage.sprite = dialogue.dialogueBoxImage;
        continueButtonImage.sprite = dialogue.continueButtonImage;

        dialogueCanvasGroup.alpha = 1f;
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (waitingForCritters && !critterCollectionComplete)
        {
            Debug.Log("Waiting for critter collection before continuing dialogue.");
            HideDialogue();
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        ShowDialogue();
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (sentences.Count == 1 && !critterCollectionComplete)
        {
            waitingForCritters = true;
            Debug.Log("Pausing dialogue for critter collection.");
            OnTomNookDialogueEnd?.Invoke();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void ResumeDialogue()
    {
        waitingForCritters = false;
        ShowDialogue();
        DisplayNextSentence();
    }

    public void CompleteCritterCollection()
    {
        Debug.Log("Critter collection completed!");
        critterCollectionComplete = true;
        waitingForCritters = false;
        ResumeDialogue();
    }

    void EndDialogue()
    {
        Debug.Log("End of convo");

        HideDialogue();

        if (critterCollectionComplete)
        {
            OnBlathersDialogueEnd?.Invoke();
            ActivateCritterpediaButton(); // Activate the Critterpedia button
        }
        else
        {
            OnTomNookDialogueEnd?.Invoke();
        }
    }

    private void ActivateCritterpediaButton()
    {
        if (critterpediaButton != null)
        {
            critterpediaButton.SetActive(true);
            Debug.Log("Critterpedia button activated.");
        }
        else
        {
            Debug.LogWarning("Critterpedia Button is not assigned!");
        }
    }

    private void HideDialogue()
    {
        dialogueCanvasGroup.alpha = 0f;
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;
    }

    private void ShowDialogue()
    {
        dialogueCanvasGroup.alpha = 1f;
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;
    }
}
