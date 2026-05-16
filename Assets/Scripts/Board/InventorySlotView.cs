using UnityEngine;
using UnityEngine.UI;

public sealed class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private Color emptyColor = new Color(0.22f, 0.2f, 0.35f, 0.9f);
    [SerializeField] private Color occupiedColor = Color.white;

    public bool IsOccupied => itemIcon != null;
    public Sprite ItemIcon => itemIcon;

    private void OnEnable()
    {
        Refresh();
    }

    private void OnValidate()
    {
        Refresh();
    }

    public void SetItemIcon(Sprite newItemIcon)
    {
        itemIcon = newItemIcon;
        Refresh();
    }

    public void Clear()
    {
        itemIcon = null;
        Refresh();
    }

    public void Refresh()
    {
        if (backgroundImage != null)
            backgroundImage.color = IsOccupied ? occupiedColor : emptyColor;

        if (iconImage == null)
            return;

        iconImage.sprite = itemIcon;
        iconImage.enabled = itemIcon != null;
        iconImage.color = Color.white;
    }
}
