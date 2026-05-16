using UnityEngine;
using UnityEngine.UI;

public sealed class DiceRollButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TurnSystem turnSystem;

    private void Reset()
    {
        button = GetComponent<Button>();
        turnSystem = FindAnyObjectByType<TurnSystem>();
    }

    private void OnEnable()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(RequestRoll);

        if (turnSystem != null)
        {
            turnSystem.StateChanged += HandleTurnStateChanged;
            turnSystem.DiceRolled += HandleDiceRolled;
            HandleTurnStateChanged(turnSystem.State);
        }
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(RequestRoll);

        if (turnSystem != null)
        {
            turnSystem.StateChanged -= HandleTurnStateChanged;
            turnSystem.DiceRolled -= HandleDiceRolled;
        }
    }

    private void OnValidate()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void RequestRoll()
    {
        if (turnSystem == null)
            return;

        turnSystem.TryRollDice();
    }

    private void HandleTurnStateChanged(TurnState state)
    {
        if (button != null)
            button.interactable = state == TurnState.WaitingForRoll;
    }

    private void HandleDiceRolled(int value)
    {
        Debug.Log($"Dice rolled: {value}");
    }

    public void SetTurnSystem(TurnSystem newTurnSystem)
    {
        if (turnSystem != null && isActiveAndEnabled)
        {
            turnSystem.StateChanged -= HandleTurnStateChanged;
            turnSystem.DiceRolled -= HandleDiceRolled;
        }

        turnSystem = newTurnSystem;

        if (turnSystem != null && isActiveAndEnabled)
        {
            turnSystem.StateChanged += HandleTurnStateChanged;
            turnSystem.DiceRolled += HandleDiceRolled;
            HandleTurnStateChanged(turnSystem.State);
        }
    }
}
