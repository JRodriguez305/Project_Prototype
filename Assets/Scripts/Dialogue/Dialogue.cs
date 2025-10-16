using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    [TextArea]
    public string speech;
    public UnityEvent OnDialogueDisplayed;
}
