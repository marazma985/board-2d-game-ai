using UnityEngine;
using UnityEngine.UI;

public sealed class DiceRollButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private DiceSystem diceSystem;
    [SerializeField] private PlayerMover playerMover;

    private void Reset()
    {
        button = GetComponent<Button>();
        diceSystem = FindFirstObjectByType<DiceSystem>();
        playerMover = FindFirstObjectByType<PlayerMover>();
    }

    private void OnEnable()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(RollAndMove);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(RollAndMove);
    }

    private void OnValidate()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void RollAndMove()
    {
        if (diceSystem == null || playerMover == null || playerMover.IsMoving)
            return;

        button.interactable = false;

        var value = diceSystem.Roll();
        Debug.Log($"Dice rolled: {value}");
        playerMover.MoveSteps(value, EnableButton);
    }

    private void EnableButton()
    {
        if (button != null)
            button.interactable = true;
    }
}
