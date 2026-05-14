using UnityEngine;
using UnityEngine.EventSystems;

public sealed class MainMenuButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform target;
    [SerializeField] private float hoverLift = 5f;
    [SerializeField] private float moveSmoothTime = 0.055f;

    private Vector2 baseAnchoredPosition;
    private Vector2 moveVelocity;
    private bool initialized;
    private bool hovering;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnValidate()
    {
        if (target == null)
            target = transform as RectTransform;
    }

    private void Update()
    {
        Initialize();

        var desiredPosition = baseAnchoredPosition + (hovering ? Vector2.up * hoverLift : Vector2.zero);
        target.anchoredPosition = Vector2.SmoothDamp(target.anchoredPosition, desiredPosition, ref moveVelocity, moveSmoothTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    private void Initialize()
    {
        if (initialized)
            return;

        if (target == null)
            target = transform as RectTransform;

        if (target == null)
            return;

        baseAnchoredPosition = target.anchoredPosition;
        initialized = true;
    }
}
