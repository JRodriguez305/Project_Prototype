using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxBehaviour : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI speechTMP;

    Coroutine _dialogueInstance = null;

    private void OnEnable()
    {
        DialogueHolderBehaviour.OnSayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        DialogueHolderBehaviour.OnSayDialogue -= DisplayDialogue;
    }

    public void DisplayDialogue(Dialogue _dialogue)
    {
        if (_dialogueInstance != null)
            StopCoroutine(_dialogueInstance);

        _dialogueInstance = StartCoroutine( DisplayDialogueSequence(_dialogue) );
    }

    private IEnumerator DisplayDialogueSequence(Dialogue _dialogue)
    {
        transform.localScale = Vector3.one;

        _dialogue.OnDialogueDisplayed?.Invoke();

        speechTMP.text = "";

        for (int i = 0; i < _dialogue.speech.Length; i++)
        {
            speechTMP.text += _dialogue.speech[i];
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        transform.localScale = Vector3.zero;

        _dialogueInstance = null;
    }
}
