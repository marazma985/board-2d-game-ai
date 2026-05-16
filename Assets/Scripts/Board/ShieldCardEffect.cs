using UnityEngine;

public sealed class ShieldCardEffect : ICardEffect
{
    public bool Resolve(CardData card)
    {
        Debug.Log("Shield card effect resolved.");
        return true;
    }
}
