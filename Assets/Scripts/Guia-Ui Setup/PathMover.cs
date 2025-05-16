using UnityEngine;
using System.Collections;


public class PathMover : MonoBehaviour
{
    [System.Serializable]
    public class PathSettings
    {
        public string[] interactionPaths = new string[] { "IntroPath", "Manglar1", "Manglar2" };
        public float defaultMoveTime = 8f;
        public iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
    }

    [Header("Configuración")]
    [SerializeField] private PathSettings pathSettings;

    [Header("Eventos")]
    public UnityEngine.Events.UnityEvent OnPathStart;
    public UnityEngine.Events.UnityEvent OnPathComplete;

    private int currentPathIndex = 0;
    private bool isMoving = false;
    private Coroutine movementRoutine;

    public bool IsMoving => isMoving;
    public int CurrentInteractionIndex => currentPathIndex;

    public void StartExperience()
    {
        if (movementRoutine != null) StopCoroutine(movementRoutine);
        currentPathIndex = 0;
        movementRoutine = StartCoroutine(MoveToPath(pathSettings.interactionPaths[currentPathIndex]));
    }

    public void GoToNextPath()
    {
        if (!CanMoveToNextPath()) return;

        StopCurrentMovement();
        currentPathIndex++;
        string nextPath = pathSettings.interactionPaths[currentPathIndex];
        movementRoutine = StartCoroutine(MoveToPath(nextPath));
    }

    private IEnumerator MoveToPath(string pathName)
    {
        OnPathStart?.Invoke();
        isMoving = true;

        var path = iTweenPath.GetPath(pathName);
        
        float duration = pathSettings.defaultMoveTime;
        float elapsed = 0f;

        iTween.MoveTo(gameObject, iTween.Hash(
            "path", path,
            "time", duration,
            "easetype", pathSettings.easeType,
            "orienttopath", true,
            "looktime", 0.2f
            )
        );

        while (elapsed < duration && isMoving)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        OnPathComplete?.Invoke();
    }

    private bool CanMoveToNextPath()
    {
        return currentPathIndex < pathSettings.interactionPaths.Length - 1;
    }

    private void StopCurrentMovement()
    {
        if (isMoving)
        {
            iTween.Stop(gameObject);
            if (movementRoutine != null) StopCoroutine(movementRoutine);
            isMoving = false;
        }
    }

    public void JumpToSpecificPath(int pathIndex)
    {
        if (pathIndex < 0 || pathIndex >= pathSettings.interactionPaths.Length) return;

        StopCurrentMovement();
        currentPathIndex = pathIndex;
        movementRoutine = StartCoroutine(MoveToPath(pathSettings.interactionPaths[currentPathIndex]));
    }
    public void SetMoveTime(float newMoveTime)
    {
        pathSettings.defaultMoveTime = newMoveTime;

        // Opcional: Si quieres que afecte inmediatamente al path actual
        if (isMoving)
        {
            StopCurrentMovement();
            movementRoutine = StartCoroutine(MoveToPath(pathSettings.interactionPaths[currentPathIndex]));
        }
    }
    public void ResetPathSystem()
    {
        StopCurrentMovement();
        currentPathIndex = 0;
        transform.position = iTweenPath.GetPath(pathSettings.interactionPaths[0])[0];
    }
}