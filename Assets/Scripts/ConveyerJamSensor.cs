using UnityEngine;

public class ConveyorJamSensor : MonoBehaviour
{
    public ConveyorBelt firstConveyor;

    public float jamTime = 2f;

    private float timer = 0f;
    private bool jamDetected = false;
    private int packagesInside = 0;

    private const string JAM_REASON = "JAM";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        packagesInside++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        timer += Time.deltaTime;

        if (timer >= jamTime && !jamDetected)
        {
            jamDetected = true;

            firstConveyor.AddStopReason(JAM_REASON);

            Debug.Log("JAM DETECTED - FIRST CONVEYOR STOPPED");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        packagesInside--;

        if (packagesInside <= 0)
        {
            packagesInside = 0;
            timer = 0f;
            jamDetected = false;

            firstConveyor.RemoveStopReason(JAM_REASON);

            Debug.Log("JAM CLEARED - FIRST CONVEYOR RELEASED");
        }
    }
}