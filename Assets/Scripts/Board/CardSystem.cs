using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardSystem : MonoBehaviour
{
    private const int MaxHandSize = 3;
    private const string SmallHealCardId = "small_heal";
    private const string ShieldCardId = "shield";
    private const string LuckyHitCardId = "lucky_hit";

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private List<CardData> hand = new List<CardData>(MaxHandSize);

    private readonly Dictionary<string, ICardEffect> effectsByCardId = new Dictionary<string, ICardEffect>();

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

    public bool UseCard(CardData card)
    {
        if (card == null || !hand.Contains(card))
            return false;

        Debug.Log($"Card used: {card.CardName}");

        if (!ResolveCard(card))
            return false;

        RemoveCard(card);
        return true;
    }

    public void RegisterEffect(string cardId, ICardEffect effect)
    {
        if (string.IsNullOrWhiteSpace(cardId) || effect == null)
            return;

        effectsByCardId[cardId] = effect;
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

    private void Awake()
    {
        InitializeEffects();
    }

    private void NotifyHandChanged()
    {
        OnHandChanged?.Invoke(hand);
    }

    private bool ResolveCard(CardData card)
    {
        if (string.IsNullOrWhiteSpace(card.CardId))
        {
            Debug.LogWarning($"Card '{card.CardName}' has no card id.");
            return false;
        }

        if (effectsByCardId.Count == 0)
            InitializeEffects();

        if (effectsByCardId.TryGetValue(card.CardId, out var effect))
            return effect.Resolve(card);

        Debug.LogWarning($"No card effect registered for card id '{card.CardId}'.");
        return false;
    }

    private void InitializeEffects()
    {
        effectsByCardId.Clear();
        RegisterEffect(SmallHealCardId, new SmallHealCardEffect(playerStats));
        RegisterEffect(ShieldCardId, new ShieldCardEffect());
        RegisterEffect(LuckyHitCardId, new LuckyHitCardEffect());
    }
}
