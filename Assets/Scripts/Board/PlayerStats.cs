using System;
using UnityEngine;

public sealed class PlayerStats : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHp = 5;
    [SerializeField, Min(0)] private int currentHp = 5;
    [SerializeField, Min(1)] private int level = 1;

    public event Action<int, int> OnHpChanged;
    public event Action<int> OnLevelChanged;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public int Level => level;

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        SetHp(currentHp - amount);
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        SetHp(currentHp + amount);
    }

    public void SetLevel(int newLevel)
    {
        newLevel = Mathf.Max(1, newLevel);
        if (level == newLevel)
            return;

        level = newLevel;
        OnLevelChanged?.Invoke(level);
    }

    [ContextMenu("Test Take 1 Damage")]
    private void TestTakeOneDamage()
    {
        TakeDamage(1);
    }

    [ContextMenu("Test Heal 1")]
    private void TestHealOne()
    {
        Heal(1);
    }

    private void Awake()
    {
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }

    private void OnValidate()
    {
        maxHp = Mathf.Max(1, maxHp);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        level = Mathf.Max(1, level);
    }

    private void SetHp(int newHp)
    {
        newHp = Mathf.Clamp(newHp, 0, maxHp);
        if (currentHp == newHp)
            return;

        currentHp = newHp;
        OnHpChanged?.Invoke(currentHp, maxHp);
    }
}
