using System.Collections;
using UnityEngine;

public class RobotArmTransfer : MonoBehaviour
{
    [Header("Transfer")]
    public Transform dropPoint;

    [Header("Conveyor")]
    public ConveyorBelt inputConveyor;

    [Header("Robot Animation")]
    public Animator robotAnimator;
    public string workTriggerName = "Work";
    public float animationLength = 0.867f;

    [Header("Movement")]
    public float liftHeight = 1.5f;
    public float moveDuration = 0.5f;

    private bool isMoving = false;
    private const string ROBOT_BUSY_REASON = "ROBOT_BUSY";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        if (isMoving)
        {
            if (inputConveyor != null)
                inputConveyor.AddStopReason(ROBOT_BUSY_REASON);

            return;
        }

        StartCoroutine(TransferPackage(other.gameObject));
    }

    private IEnumerator TransferPackage(GameObject package)
    {
        isMoving = true;

        if (inputConveyor != null)
            inputConveyor.AddStopReason(ROBOT_BUSY_REASON);

        if (robotAnimator != null)
        {
            float totalMoveTime = moveDuration * 3f;

            robotAnimator.speed = animationLength / totalMoveTime;

            robotAnimator.ResetTrigger(workTriggerName);
            robotAnimator.SetTrigger(workTriggerName);

            Debug.Log("Robot trigger fired!");
        }
        else
        {
            Debug.LogWarning("Robot Animator missing!");
        }

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

        Quaternion uprightRotation = Quaternion.Euler(
            90f,
            dropPoint.eulerAngles.y,
            0f
        );

        yield return MoveObject(package.transform, startPos, liftPos, startRot, startRot);
        yield return MoveObject(package.transform, liftPos, dropLiftPos, startRot, uprightRotation);
        yield return MoveObject(package.transform, dropLiftPos, dropPoint.position, uprightRotation, uprightRotation);

        if (rb != null)
            rb.isKinematic = false;

        if (robotAnimator != null)
            robotAnimator.speed = 1f;

        isMoving = false;

        if (inputConveyor != null)
            inputConveyor.RemoveStopReason(ROBOT_BUSY_REASON);
    }

    private IEnumerator MoveObject(
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