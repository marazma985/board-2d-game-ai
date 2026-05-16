using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class CardView : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Button button;

    private CardData currentCard;

    public event Action<CardData> Clicked;

    public CardData CurrentCard => currentCard;

    private void OnEnable()
    {
        if (button != null)
            button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(HandleClick);
    }

    public void SetCard(CardData card)
    {
        currentCard = card;

        if (cardImage != null)
        {
            cardImage.sprite = currentCard != null ? currentCard.CardSprite : null;
            cardImage.enabled = currentCard != null && currentCard.CardSprite != null;
        }
    }

    private void HandleClick()
    {
        if (currentCard != null)
            Clicked?.Invoke(currentCard);
    }
}
