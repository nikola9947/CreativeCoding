using UnityEngine;

public class WorkerClickDance : MonoBehaviour
{
    public WorkerAnimationController workerAnimation;

    private void Awake()
    {
        if (workerAnimation == null)
            workerAnimation = GetComponent<WorkerAnimationController>();
    }


    private void OnMouseDown()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        // Während eines Minispiels keine Klick-Animation zulassen
        if (gameManager != null && gameManager.IsMiniGameRunning())
            return;

        workerAnimation.PlayDance();
    }
}