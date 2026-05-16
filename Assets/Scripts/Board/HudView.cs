using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class HudView : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private InventorySlotView[] inventorySlots;

    private void OnEnable()
    {
        Subscribe();
        RefreshAll();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void SetPlayerStats(PlayerStats newPlayerStats)
    {
        if (playerStats == newPlayerStats)
            return;

        Unsubscribe();
        playerStats = newPlayerStats;
        Subscribe();
        RefreshAll();
    }

    public void RefreshAll()
    {
        if (playerStats == null)
        {
            UpdateLevel(0);
            UpdateHp(0, heartImages != null ? heartImages.Length : 0);
            return;
        }

        UpdateLevel(playerStats.Level);
        UpdateHp(playerStats.CurrentHp, playerStats.MaxHp);
        RefreshSlots();
    }

    private void Subscribe()
    {
        if (playerStats == null)
            return;

        playerStats.OnHpChanged += UpdateHp;
        playerStats.OnLevelChanged += UpdateLevel;
    }

    private void Unsubscribe()
    {
        if (playerStats == null)
            return;

        playerStats.OnHpChanged -= UpdateHp;
        playerStats.OnLevelChanged -= UpdateLevel;
    }

    private void UpdateHp(int currentHp, int maxHp)
    {
        if (heartImages == null)
            return;

        for (var i = 0; i < heartImages.Length; i++)
        {
            var heart = heartImages[i];
            if (heart == null)
                continue;

            var isFilled = i < currentHp;
            heart.enabled = i < maxHp;
            heart.sprite = isFilled ? fullHeartSprite : emptyHeartSprite;
            heart.color = isFilled ? Color.white : new Color(1f, 1f, 1f, 0.45f);
        }
    }

    private void UpdateLevel(int newLevel)
    {
        if (levelText != null)
            levelText.text = $"LVL {newLevel}";
    }

    private void RefreshSlots()
    {
        if (inventorySlots == null)
            return;

        for (var i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != null)
                inventorySlots[i].Refresh();
        }
    }
}
