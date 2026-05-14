using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuSpriteButton : MonoBehaviour
{
    public enum VisualKind
    {
        Continue,
        NewGame,
        Settings,
        Exit
    }

    [SerializeField] private VisualKind kind;
    [SerializeField] private bool continueAvailable;
    [SerializeField] private Image targetImage;
    [SerializeField] private Button button;

    [Header("Sprites")]
    [SerializeField] private Sprite continueAvailableSprite;
    [SerializeField] private Sprite continueLockedSprite;
    [SerializeField] private Sprite newGameSprite;
    [SerializeField] private Sprite settingsSprite;
    [SerializeField] private Sprite exitSprite;

    public VisualKind Kind
    {
        get => kind;
        set
        {
            kind = value;
            ApplyVisual();
        }
    }

    public bool ContinueAvailable
    {
        get => continueAvailable;
        set
        {
            continueAvailable = value;
            ApplyVisual();
        }
    }

    private void Reset()
    {
        targetImage = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Awake()
    {
        ApplyVisual();
    }

    private void OnValidate()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
        if (button == null)
            button = GetComponent<Button>();

        ApplyVisual();
    }

    [ContextMenu("Apply Visual")]
    public void ApplyVisual()
    {
        if (targetImage == null)
            return;

        targetImage.sprite = PickSprite();
        targetImage.color = Color.white;
        targetImage.preserveAspect = true;
        targetImage.type = Image.Type.Simple;

        if (button != null)
        {
            button.transition = Selectable.Transition.None;
            button.interactable = kind != VisualKind.Continue || continueAvailable;
        }
    }

    private Sprite PickSprite()
    {
        switch (kind)
        {
            case VisualKind.Continue:
                return continueAvailable ? continueAvailableSprite : continueLockedSprite;
            case VisualKind.NewGame:
                return newGameSprite;
            case VisualKind.Settings:
                return settingsSprite;
            case VisualKind.Exit:
                return exitSprite;
            default:
                return null;
        }
    }
}
