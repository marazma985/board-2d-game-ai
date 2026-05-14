using System;
using UnityEngine;
using UnityEngine.Events;

public sealed class DiceSystem : MonoBehaviour
{
    [Serializable]
    public sealed class DiceRolledEvent : UnityEvent<int>
    {
    }

    [SerializeField] private DiceRolledEvent onDiceRolled = new DiceRolledEvent();

    public DiceRolledEvent OnDiceRolled => onDiceRolled;

    public int Roll()
    {
        var value = UnityEngine.Random.Range(1, 7);
        onDiceRolled.Invoke(value);
        return value;
    }
}
