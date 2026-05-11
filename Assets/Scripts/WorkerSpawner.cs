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
    public float minimumSafeInterval = 1.2f;

    [Header("Stress")]
    public float stress = 0f;
    public float stressIncrease = 20f;
    public float stressRecovery = 8f;
    public float maxStress = 100f;

    [Header("Breakdown")]
    public float breakdownDuration = 6f;

    private bool isBrokenDown = false;
    private bool isWorking = false;

    private void Start()
    {
        StartCoroutine(WorkLoop());
    }

    private void Update()
    {
        if (!isBrokenDown && !isWorking && stress > 0f)
        {
            stress -= stressRecovery * Time.deltaTime;
            stress = Mathf.Clamp(stress, 0f, maxStress);
        }
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            if (isBrokenDown)
            {
                yield return null;
                continue;
            }

            if (firstConveyor != null && firstConveyor.IsStopped())
            {
                isWorking = false;
                yield return null;
                continue;
            }

            isWorking = true;

            SpawnPackage();

            if (workInterval < minimumSafeInterval)
            {
                stress += stressIncrease;
                stress = Mathf.Clamp(stress, 0f, maxStress);
            }

            if (stress >= maxStress)
            {
                StartCoroutine(Breakdown());
            }

            yield return new WaitForSeconds(workInterval);
        }
    }

    private void SpawnPackage()
    {
        Instantiate(
            packagePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }

    private IEnumerator Breakdown()
    {
        isBrokenDown = true;
        isWorking = false;

        Debug.Log("WORKER EXHAUSTED!");

        yield return new WaitForSeconds(breakdownDuration);

        stress = 0f;
        isBrokenDown = false;

        Debug.Log("WORKER RECOVERED!");
    }
}