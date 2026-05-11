using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.forward;
    public float speed = 2f;

    private HashSet<string> stopReasons = new HashSet<string>();

    public bool IsStopped()
    {
        return stopReasons.Count > 0;
    }

    public void AddStopReason(string reason)
    {
        stopReasons.Add(reason);
        Debug.Log(gameObject.name + " STOP: " + reason);
    }

    public void RemoveStopReason(string reason)
    {
        stopReasons.Remove(reason);
        Debug.Log(gameObject.name + " RELEASE: " + reason);
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        if (rb == null || rb.isKinematic)
            return;

        if (IsStopped())
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }

        rb.linearVelocity = new Vector3(
            moveDirection.x * speed,
            rb.linearVelocity.y,
            moveDirection.z * speed
        );
    }
}