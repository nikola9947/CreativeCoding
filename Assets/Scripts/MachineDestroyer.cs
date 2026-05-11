using UnityEngine;

public class MachineDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            Destroy(other.gameObject);
        }
    }
}