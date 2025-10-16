using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolderBehaviour : MonoBehaviour
{
    [SerializeField]
    List<Dialogue> dialogues = new List<Dialogue>();

    public static Action<Dialogue> OnSayDialogue;

    private bool hasShownDialogue = false;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SayRandomDialogue();
        }
    }

    public void SayRandomDialogue()
    {
        if (hasShownDialogue || dialogues.Count == 0)
        {
            return;
        }

        OnSayDialogue?.Invoke(dialogues[UnityEngine.Random.Range(0, dialogues.Count)]);

        hasShownDialogue = true;
    }
}
