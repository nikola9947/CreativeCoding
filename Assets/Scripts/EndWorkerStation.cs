using System.Collections;
using UnityEngine;

public class EndWorkerStation : MonoBehaviour
{
    [Header("Input")]
    public string packageTag = "Package";

    [Header("Cartons")]
    public GameObject cartonPrefab;
    public Transform[] cartonSlots;

    [Header("Rules")]
    public int boxesPerCarton = 4;
    public int cartonsPerPalette = 8;

    [Header("Packing")]
    public float packingDelay = 0.5f;

    [Header("Conveyor Control")]
    public ConveyorBelt[] conveyors;
    private const string NO_PALLET_REASON = "NO_PALLET";

    [Header("Forklift")]
    public ForkliftPickup forklift;

    private int currentBoxCount = 0;
    private int currentCartonIndex = 0;

    private GameObject currentCarton;
    private bool isPacking = false;
    private bool waitingForNewPallet = false;

    void Start()
    {
        SpawnNewCarton();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPacking || waitingForNewPallet)
            return;

        if (other.CompareTag(packageTag))
        {
            Destroy(other.gameObject);
            StartCoroutine(PackBox());
        }
    }

    private IEnumerator PackBox()
    {
        isPacking = true;

        yield return new WaitForSeconds(packingDelay);

        currentBoxCount++;

        Debug.Log("Carton " + (currentCartonIndex + 1) + ": " + currentBoxCount + "/" + boxesPerCarton);

        if (currentBoxCount >= boxesPerCarton)
        {
            currentBoxCount = 0;
            currentCartonIndex++;

            if (currentCartonIndex >= cartonsPerPalette)
            {
                Debug.Log("PALLET FULL!");

                waitingForNewPallet = true;

                StopAllConveyors();

                if (forklift != null)
                {
                    forklift.PickupPallet(this);
                }

                isPacking = false;
                yield break;
            }

            SpawnNewCarton();
        }

        isPacking = false;
    }

    private void SpawnNewCarton()
    {
        if (currentCartonIndex >= cartonSlots.Length)
            return;

        Transform slot = cartonSlots[currentCartonIndex];

        currentCarton = Instantiate(
            cartonPrefab,
            slot.position,
            slot.rotation,
            slot
        );

        Debug.Log("New empty carton created.");
    }

    private void StopAllConveyors()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
            {
                conveyor.AddStopReason(NO_PALLET_REASON);
            }
        }
    }

    private void ReleaseAllConveyors()
    {
        foreach (ConveyorBelt conveyor in conveyors)
        {
            if (conveyor != null)
            {
                conveyor.RemoveStopReason(NO_PALLET_REASON);
            }
        }
    }

    public void NewPalletArrived()
    {
        currentBoxCount = 0;
        currentCartonIndex = 0;
        waitingForNewPallet = false;

        ReleaseAllConveyors();

        SpawnNewCarton();

        Debug.Log("NEW PALLET ARRIVED!");
    }
}