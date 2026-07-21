using System.Collections;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [Header("Package")]
    public GameObject packagePrefab;
    public Transform spawnPoint;

    [Header("Conveyor")]
    public ConveyorBelt firstConveyor;

    [Header("Settings")]
    public float workInterval = 2f;

    [Header("Animation")]
    public WorkerAnimationController workerAnimation;

    private Coroutine workCoroutine;

    private void Start()
    {
        StartSpawner();
    }

    public void StartSpawner()
    {
        if (workCoroutine != null)
            return;

        workCoroutine = StartCoroutine(WorkLoop());

        Debug.Log("WorkerSpawner gestartet");
    }

    public void StopSpawner()
    {
        if (workCoroutine == null)
            return;

        StopCoroutine(workCoroutine);
        workCoroutine = null;

        Debug.Log("WorkerSpawner gestoppt");
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
        if (packagePrefab == null)
        {
            Debug.LogError("WorkerSpawner: Kein Package Prefab zugewiesen.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("WorkerSpawner: Kein SpawnPoint zugewiesen.");
            return;
        }

        if (workerAnimation != null)
            workerAnimation.PlayWork();

        Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("Paket gespawnt");
    }
}