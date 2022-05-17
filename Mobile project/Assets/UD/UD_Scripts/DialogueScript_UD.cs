using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueScript_UD : MonoBehaviour
{
    [Header("Dialogue Sentence")]
    public string[] dialogueSentences;
    private int currentDialogueSentence;

    [Header("Dialogue Box")]
    public Button nextSentenceButton;
    public TMP_Text textUI;

    [Header("Typing Effect")]
    public float delay = 0.1f;
    private string currentText;

    [Header("Last Sentence")]
    public UnityEvent lastSentenceEvent;

    [Header("End Dialogue")]
    public UnityEvent endDialogueEvent;


    void Start()
    {
        currentDialogueSentence = 0;
        StartDialogue();
        nextSentenceButton.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        StartCoroutine(ShowText(dialogueSentences[currentDialogueSentence]));
    }

    public void NextSentence()
    {
        currentDialogueSentence++;

        if(currentDialogueSentence < dialogueSentences.Length - 1)
        {
            StartCoroutine(ShowText(dialogueSentences[currentDialogueSentence]));
        }
        if (currentDialogueSentence == dialogueSentences.Length - 1)
        {
            lastSentenceEvent.Invoke();
            StartCoroutine(ShowText(dialogueSentences[currentDialogueSentence]));
        }
        if (currentDialogueSentence >= dialogueSentences.Length)
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        endDialogueEvent.Invoke();
    }

    IEnumerator ShowText(string textToShow)
    {
        for (int i = 0; i < textToShow.Length; i++)
        {
            currentText = textToShow.Substring(0, i);
            textUI.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        nextSentenceButton.gameObject.SetActive(true);
    }
}
