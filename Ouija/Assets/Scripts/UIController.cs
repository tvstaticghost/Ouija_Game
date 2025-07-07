using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StateManager;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] Button option1;
    [SerializeField] TextMeshProUGUI option1text;
    [SerializeField] Button option2;
    [SerializeField] TextMeshProUGUI option2text;
    [SerializeField] Slider sliderScript;

    private bool option1Selected = false;
    private bool option2Selected = false;

    private string currentAnswer1 = null;
    private string currentAnswer2 = null;

    private readonly List<(string, string, string, string)> questioning1 = new List<(string, string, string, string)>
    {
        ("Are you there?", "How did you die?", "yes", "knife"),
        ("Did you live here?", "Who killed you?", "yes", "dontknow"),
        ("Did you die during the day or night?", "What's the last thing you remember?", "night", "water")
    };

    public bool canSelectOption = true;

    private void Start()
    {
        Debug.Log(StateManager.Instance.currentDialogueState);
        UpdateText();
        canSelectOption = true;
    }

    public void Button1Press()
    {
        if (canSelectOption)
        {
            Debug.Log("Selected button 1");
            sliderScript.StartASequence(currentAnswer1);
            option1Selected = true;
            CheckIfBothOptionsSelected();
        }
        else
        {
            Debug.Log("Can't select option right now");
        }
    }
    public void Button2Press()
    {
        if (canSelectOption)
        {
            Debug.Log("Selected button 2");
            sliderScript.StartASequence(currentAnswer2);
            option2Selected = true;
            CheckIfBothOptionsSelected();
        }
        else
        {
            Debug.Log("Can't select option right now");
        }
    }

    private void UpdateText()
    {
        switch (StateManager.Instance.currentDialogueState)
        {
            case DialogueStates.STARTING:
                option1text.text = questioning1[0].Item1;
                option2text.text = questioning1[0].Item2;
                currentAnswer1 = questioning1[0].Item3;
                currentAnswer2 = questioning1[0].Item4;
                break;
            case DialogueStates.MIDDLE:
                option1text.text = questioning1[1].Item1;
                option2text.text = questioning1[1].Item2;
                currentAnswer1 = questioning1[1].Item3;
                currentAnswer2 = questioning1[1].Item4;
                break;
            case DialogueStates.END:
                option1text.text = questioning1[2].Item1;
                option2text.text = questioning1[2].Item2;
                currentAnswer1 = questioning1[2].Item3;
                currentAnswer2 = questioning1[2].Item4;
                break;
            default:
                Debug.Log("All Dialogue Exhausted");
                break;
        }
    }

    //This is a test - kinda stupid
    private void CheckIfBothOptionsSelected()
    {
        if (option1Selected && option2Selected)
        {
            switch (StateManager.Instance.currentDialogueState)
            {
                case DialogueStates.STARTING:
                    StateManager.Instance.SetDialogueState(DialogueStates.MIDDLE);
                    break;
                case DialogueStates.MIDDLE:
                    StateManager.Instance.SetDialogueState(DialogueStates.END);
                    break;
                default:
                    Debug.Log("All Dialogue Exhausted");
                    break;
            }

            option1Selected = false;
            option2Selected = false;
            UpdateText();
        }
        else
        {
            return;
        }
    }

}
