using UnityEngine;

public sealed class LuckyHitCardEffect : ICardEffect
{
    public bool Resolve(CardData card)
    {
        Debug.Log("Lucky Hit card effect resolved.");
        return true;
    }
}
