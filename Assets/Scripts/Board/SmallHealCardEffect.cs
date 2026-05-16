using UnityEngine;

public sealed class SmallHealCardEffect : ICardEffect
{
    private const int HealAmount = 1;

    private readonly PlayerStats playerStats;

    public SmallHealCardEffect(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }

    public bool Resolve(CardData card)
    {
        if (playerStats == null)
        {
            Debug.LogWarning("Small Heal card effect requires PlayerStats.");
            return false;
        }

        playerStats.Heal(HealAmount);
        Debug.Log($"Small Heal restored {HealAmount} HP.");
        return true;
    }
}
