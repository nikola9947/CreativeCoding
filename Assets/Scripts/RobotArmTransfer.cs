using System.Collections;
using UnityEngine;

public class RobotArmTransfer : MonoBehaviour
{
    public Transform dropPoint;
    public ConveyorBelt inputConveyor;

    public float liftHeight = 1.5f;
    public float moveDuration = 1f;

    private bool isMoving = false;
    private const string ROBOT_REASON = "ROBOT_BUSY";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        if (isMoving)
        {
            inputConveyor.AddStopReason(ROBOT_REASON);
            return;
        }

        StartCoroutine(MovePackage(other.gameObject));
    }

    private IEnumerator MovePackage(GameObject package)
    {
        isMoving = true;
        inputConveyor.AddStopReason(ROBOT_REASON);

        Rigidbody rb = package.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Vector3 startPos = package.transform.position;
        Quaternion startRot = package.transform.rotation;

        Vector3 liftPos = startPos + Vector3.up * liftHeight;
        Vector3 dropLiftPos = dropPoint.position + Vector3.up * liftHeight;

        Quaternion uprightRotation = Quaternion.Euler(90f, dropPoint.eulerAngles.y, 0f);

        yield return Move(package.transform, startPos, liftPos, startRot, startRot);
        yield return Move(package.transform, liftPos, dropLiftPos, startRot, uprightRotation);
        yield return Move(package.transform, dropLiftPos, dropPoint.position, uprightRotation, uprightRotation);

        if (rb != null)
            rb.isKinematic = false;

        isMoving = false;
        inputConveyor.RemoveStopReason(ROBOT_REASON);
    }

    private IEnumerator Move(
        Transform obj,
        Vector3 fromPos,
        Vector3 toPos,
        Quaternion fromRot,
        Quaternion toRot
    )
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = timer / moveDuration;

            obj.position = Vector3.Lerp(fromPos, toPos, t);
            obj.rotation = Quaternion.Lerp(fromRot, toRot, t);

            yield return null;
        }

        obj.position = toPos;
        obj.rotation = toRot;
    }
}