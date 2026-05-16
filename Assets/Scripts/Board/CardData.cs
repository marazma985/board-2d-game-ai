using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Board Game/Card Data")]
public sealed class CardData : ScriptableObject
{
    [SerializeField] private string cardId;
    [SerializeField] private string cardName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite cardSprite;

    public string CardId => cardId;
    public string CardName => cardName;
    public string Description => description;
    public Sprite CardSprite => cardSprite;
}
