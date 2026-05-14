using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuSkinSlots : MonoBehaviour
{
    [Header("Scene Art")]
    public Image background;
    public Image logo;
    public Image leftCharacter;
    public Image rightCharacter;

    [Header("Buttons")]
    public Image continueButton;
    public Image newGameButton;
    public Image settingsButton;
    public Image exitButton;

    [Header("Sprites")]
    public Sprite backgroundSprite;
    public Sprite logoSprite;
    public Sprite leftCharacterSprite;
    public Sprite rightCharacterSprite;
    public Sprite continueButtonSprite;
    public Sprite newGameButtonSprite;
    public Sprite settingsButtonSprite;
    public Sprite exitButtonSprite;

    private void Awake()
    {
        ApplySkin();
    }

    private void OnValidate()
    {
        ApplySkin();
    }

    [ContextMenu("Apply Skin")]
    public void ApplySkin()
    {
        SetSprite(background, backgroundSprite, true);
        SetSprite(logo, logoSprite, true);
        SetSprite(leftCharacter, leftCharacterSprite, true);
        SetSprite(rightCharacter, rightCharacterSprite, true);
        SetSprite(continueButton, continueButtonSprite, false);
        SetSprite(newGameButton, newGameButtonSprite, false);
        SetSprite(settingsButton, settingsButtonSprite, false);
        SetSprite(exitButton, exitButtonSprite, false);
    }

    private static void SetSprite(Image image, Sprite sprite, bool preserveAspect)
    {
        if (image == null || sprite == null)
            return;

        image.sprite = sprite;
        image.preserveAspect = preserveAspect;
        image.type = Image.Type.Simple;
    }
}
