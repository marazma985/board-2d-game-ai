using UnityEngine;
using UnityEngine.UI;

public sealed class DiceRollButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TurnSystem turnSystem;
    [SerializeField] private BattleSystem battleSystem;

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

        if (battleSystem != null)
            battleSystem.BattleStateChanged += RefreshButtonState;

        RefreshButtonState();
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

        if (battleSystem != null)
            battleSystem.BattleStateChanged -= RefreshButtonState;
    }

    private void OnValidate()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void RequestRoll()
    {
        if (battleSystem != null && battleSystem.IsBattleActive)
        {
            battleSystem.RollBattleDice();
            RefreshButtonState();
            return;
        }

        if (turnSystem == null)
            return;

        turnSystem.TryRollDice();
    }

    private void HandleTurnStateChanged(TurnState state)
    {
        RefreshButtonState();
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

    public void SetBattleSystem(BattleSystem newBattleSystem)
    {
        if (battleSystem != null && isActiveAndEnabled)
            battleSystem.BattleStateChanged -= RefreshButtonState;

        battleSystem = newBattleSystem;

        if (battleSystem != null && isActiveAndEnabled)
            battleSystem.BattleStateChanged += RefreshButtonState;

        RefreshButtonState();
    }

    private void RefreshButtonState()
    {
        if (button == null)
            return;

        if (battleSystem != null && battleSystem.IsBattleActive)
        {
            button.interactable = battleSystem.CanUseBattleDice;
            return;
        }

        button.interactable = turnSystem != null && turnSystem.CanRoll;
    }
}
