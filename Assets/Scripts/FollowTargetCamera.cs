using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public bool lookAtTarget = true;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}