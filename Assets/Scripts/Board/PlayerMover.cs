using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public sealed class PlayerMover : MonoBehaviour
{
    [Serializable]
    public sealed class MoveCompletedEvent : UnityEvent<BoardTile>
    {
    }

    [SerializeField] private BoardManager boardManager;
    [SerializeField, Min(0.01f)] private float movementSpeed = 4f;
    [SerializeField, Min(0f)] private float stopDuration = 0.08f;
    [SerializeField] private Vector3 tileLocalOffset;
    [SerializeField] private MoveCompletedEvent onMoveCompleted = new MoveCompletedEvent();

    private Coroutine moveCoroutine;
    private Action moveCompletedCallback;

    public bool IsMoving => moveCoroutine != null;
    public BoardManager BoardManager => boardManager;
    public float MovementSpeed => movementSpeed;
    public MoveCompletedEvent OnMoveCompleted => onMoveCompleted;

    public void MoveSteps(int steps)
    {
        MoveSteps(steps, null);
    }

    public void MoveSteps(int steps, Action onCompleted)
    {
        if (IsMoving)
            return;

        moveCompletedCallback = onCompleted;
        moveCoroutine = StartCoroutine(MoveStepsRoutine(Mathf.Max(0, steps)));
    }

    public void SetBoardManager(BoardManager newBoardManager)
    {
        boardManager = newBoardManager;
    }

    private IEnumerator MoveStepsRoutine(int steps)
    {
        if (boardManager == null)
        {
            CompleteMove(null);
            yield break;
        }

        BoardTile currentTile = null;

        for (var i = 0; i < steps; i++)
        {
            currentTile = boardManager.AdvanceToNextTile();
            if (currentTile == null)
                break;

            yield return MoveToTile(currentTile);
            currentTile.Enter();

            if (stopDuration > 0f && i < steps - 1)
                yield return new WaitForSeconds(stopDuration);
        }

        CompleteMove(currentTile ?? boardManager.CurrentTile);
    }

    private IEnumerator MoveToTile(BoardTile tile)
    {
        var targetPosition = tile.transform.TransformPoint(tileLocalOffset);

        while ((transform.position - targetPosition).sqrMagnitude > 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.SetParent(tile.transform, true);
        transform.localPosition = tileLocalOffset;
    }

    private void CompleteMove(BoardTile finalTile)
    {
        moveCoroutine = null;
        onMoveCompleted.Invoke(finalTile);
        moveCompletedCallback?.Invoke();
        moveCompletedCallback = null;
    }

    private void Reset()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        CacheTileLocalOffset();
    }

    private void OnValidate()
    {
        if (boardManager == null)
            boardManager = FindFirstObjectByType<BoardManager>();

        if (tileLocalOffset == Vector3.zero)
            CacheTileLocalOffset();
    }

    private void Awake()
    {
        if (tileLocalOffset == Vector3.zero)
            CacheTileLocalOffset();
    }

    private void CacheTileLocalOffset()
    {
        if (transform.parent == null)
            return;

        if (transform.parent.GetComponent<BoardTile>() == null)
            return;

        tileLocalOffset = transform.localPosition;
    }
}
