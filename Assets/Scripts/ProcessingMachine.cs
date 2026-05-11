using System.Collections;
using UnityEngine;

public class ProcessingMachine : MonoBehaviour
{
    public GameObject outputPackagePrefab;
    public Transform outputPoint;
    public Material processedMaterial;

    public float processingDelay = 1f;

    [Header("Conveyors")]
    public ConveyorBelt inputConveyor;
    public ConveyorBelt outputConveyor;

    private bool isProcessing = false;
    private const string MACHINE_REASON = "MACHINE_BUSY";

    private void OnTriggerEnter(Collider other)
    {
        if (isProcessing)
            return;

        if (other.CompareTag("Package"))
        {
            isProcessing = true;

            if (inputConveyor != null)
                inputConveyor.AddStopReason(MACHINE_REASON);

            Destroy(other.gameObject);
            StartCoroutine(ProcessPackage());
        }
    }

    private IEnumerator ProcessPackage()
    {
        yield return new WaitForSeconds(processingDelay);

        while (outputConveyor != null && outputConveyor.IsStopped())
        {
            yield return null;
        }

        GameObject newPackage = Instantiate(
            outputPackagePrefab,
            outputPoint.position,
            outputPoint.rotation
        );

        Renderer renderer = newPackage.GetComponent<Renderer>();

        if (renderer != null && processedMaterial != null)
        {
            renderer.material = processedMaterial;
        }

        if (inputConveyor != null)
            inputConveyor.RemoveStopReason(MACHINE_REASON);

        isProcessing = false;
    }
}