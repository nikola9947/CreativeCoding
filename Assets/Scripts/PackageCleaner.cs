using UnityEngine;

public class PackageCleaner : MonoBehaviour
{
    public WorkerSpawner workerSpawner;

    public void DeleteAllPackages()
    {
        // Spawner anhalten
        if (workerSpawner != null)
            workerSpawner.StopSpawner();

        GameObject[] packages = GameObject.FindGameObjectsWithTag("Package");

        foreach (GameObject package in packages)
        {
            Destroy(package);
        }

        Debug.Log(packages.Length + " Pakete gelöscht.");

        // Spawner wieder starten
        if (workerSpawner != null)
            workerSpawner.StartSpawner();
    }
}