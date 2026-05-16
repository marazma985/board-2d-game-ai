using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Board Game/Enemy Data")]
public sealed class EnemyData : ScriptableObject
{
    [SerializeField] private string enemyId;
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite enemySprite;
    [SerializeField, Min(1)] private int level = 1;
    [SerializeField, Min(0)] private int bonusPower;
    [SerializeField] private MonsterPenaltyType penaltyType;
    [SerializeField, Min(0)] private int penaltyValue = 1;

    public string EnemyId => enemyId;
    public string EnemyName => enemyName;
    public Sprite EnemySprite => enemySprite;
    public int Level => level;
    public int BonusPower => bonusPower;
    public MonsterPenaltyType PenaltyType => penaltyType;
    public int PenaltyValue => penaltyValue;
}
