using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public enum DialogueStates
    { STARTING, MIDDLE, END }

    public DialogueStates currentDialogueState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetDialogueState(DialogueStates newState)
    {
        currentDialogueState = newState;
        Debug.Log("Dialogue state changed to: " + newState);
    }

}
