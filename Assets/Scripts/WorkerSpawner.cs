using System.Collections;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [Header("Package")]
    public GameObject packagePrefab;
    public Transform spawnPoint;

    [Header("Conveyor Check")]
    public ConveyorBelt firstConveyor;

    [Header("Worker Speed")]
    public float workInterval = 2f;

    private void Start()
    {
        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            if (firstConveyor != null && firstConveyor.IsStopped())
            {
                yield return null;
                continue;
            }

            SpawnPackage();

            yield return new WaitForSeconds(workInterval);
        }
    }

    private void SpawnPackage()
    {
        if (packagePrefab == null || spawnPoint == null)
            return;

        Instantiate(
            packagePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}