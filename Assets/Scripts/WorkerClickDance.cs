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
        if (workerAnimation != null)
            workerAnimation.PlayDance();
    }
}