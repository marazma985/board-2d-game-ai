using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardSystem : MonoBehaviour
{
    private const int MaxHandSize = 3;

    [SerializeField] private List<CardData> hand = new List<CardData>(MaxHandSize);

    public event Action<IReadOnlyList<CardData>> OnHandChanged;

    public IReadOnlyList<CardData> Hand => hand;
    public int MaxCards => MaxHandSize;

    public void SetHand(List<CardData> cards)
    {
        hand.Clear();

        if (cards != null)
        {
            for (var i = 0; i < cards.Count && hand.Count < MaxHandSize; i++)
            {
                if (cards[i] != null)
                    hand.Add(cards[i]);
            }
        }

        NotifyHandChanged();
    }

    public bool AddCard(CardData card)
    {
        if (card == null || hand.Count >= MaxHandSize)
            return false;

        hand.Add(card);
        NotifyHandChanged();
        return true;
    }

    public bool RemoveCard(CardData card)
    {
        if (card == null || !hand.Remove(card))
            return false;

        NotifyHandChanged();
        return true;
    }

    public void UseCard(CardData card)
    {
        if (card == null || !hand.Contains(card))
            return;

        Debug.Log($"Card used: {card.CardName}");
    }

    private void OnValidate()
    {
        if (hand.Count <= MaxHandSize)
            return;

        hand.RemoveRange(MaxHandSize, hand.Count - MaxHandSize);
    }

    private void Start()
    {
        NotifyHandChanged();
    }

    private void NotifyHandChanged()
    {
        OnHandChanged?.Invoke(hand);
    }
}
