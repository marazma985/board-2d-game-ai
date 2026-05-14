using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuCursor : MonoBehaviour
{
    public enum CursorState
    {
        Normal,
        Hover,
        Pressed
    }

    public static MainMenuCursor Instance { get; private set; }

    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Vector2 cursorSize = new Vector2(110f, 132f);
    [SerializeField] private Vector2 hotspotPivot = new Vector2(0.34f, 0.84f);

    private CursorState state = CursorState.Normal;
    private bool hoveringButton;
    private bool pressingButton;

    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        ApplyState(CursorState.Normal);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        Cursor.visible = true;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = !hasFocus ? true : false;
    }

    private void Update()
    {
        FollowMouse();

        if (!Input.GetMouseButton(0) && pressingButton)
            SetPressed(false);
    }

    public void SetHover(bool isHovering)
    {
        hoveringButton = isHovering;
        if (!pressingButton)
            ApplyState(hoveringButton ? CursorState.Hover : CursorState.Normal);
    }

    public void SetPressed(bool isPressed)
    {
        pressingButton = isPressed;
        ApplyState(pressingButton ? CursorState.Pressed : hoveringButton ? CursorState.Hover : CursorState.Normal);
    }

    private void FollowMouse()
    {
        if (canvas == null || cursorTransform == null)
            return;

        var canvasRect = (RectTransform)canvas.transform;
        var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, camera, out var localPosition))
            cursorTransform.anchoredPosition = localPosition;
    }

    private void ApplyState(CursorState nextState)
    {
        state = nextState;

        if (cursorTransform != null)
        {
            cursorTransform.pivot = hotspotPivot;
            cursorTransform.sizeDelta = cursorSize;
            cursorTransform.SetAsLastSibling();
        }

        if (cursorImage == null)
            return;

        cursorImage.sprite = PickSprite(state);
        cursorImage.color = Color.white;
        cursorImage.preserveAspect = true;
        cursorImage.raycastTarget = false;
    }

    private Sprite PickSprite(CursorState cursorState)
    {
        switch (cursorState)
        {
            case CursorState.Hover:
                return hoverSprite != null ? hoverSprite : normalSprite;
            case CursorState.Pressed:
                return pressedSprite != null ? pressedSprite : hoverSprite != null ? hoverSprite : normalSprite;
            default:
                return normalSprite;
        }
    }
}
