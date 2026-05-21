using System.Collections;
using UnityEngine;

public class ForkliftPickup : MonoBehaviour
{
    public Transform forklift;
    public Transform pallet;

    public Transform homePoint;
    public Transform exitPoint;

    public GameManager gameManager;

    public float moveSpeed = 5f;
    public float waitTime = 1f;

    private bool isMoving = false;

    public void PickupPallet(EndWorkerStation station)
    {
        if (isMoving)
            return;

        StartCoroutine(PalletRoutine(station));
    }

    private IEnumerator PalletRoutine(EndWorkerStation station)
    {
        isMoving = true;

        Debug.Log("1 Palette aufnehmen");
        pallet.SetParent(forklift, true);

        yield return new WaitForSeconds(waitTime);

        Debug.Log("2 Zum Exit fahren");
        yield return MoveTo(exitPoint.position);

        yield return new WaitForSeconds(waitTime);

        Debug.Log("3 Kartons entfernen");
        ClearOnlyCartons();

        if (gameManager != null)
            gameManager.PalletDelivered();

        yield return new WaitForSeconds(waitTime);

        Debug.Log("4 Zurück zum HomePoint fahren");
        yield return MoveTo(homePoint.position);

        yield return new WaitForSeconds(waitTime);

        Debug.Log("5 Palette absetzen");
        pallet.SetParent(null, true);

        if (station != null)
            station.NewPalletArrived();

        isMoving = false;

        Debug.Log("6 Forklift fertig");
    }

    private void ClearOnlyCartons()
    {
        foreach (Transform slot in pallet)
        {
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                Destroy(slot.GetChild(i).gameObject);
            }
        }

        Debug.Log("Only cartons removed. Slots kept.");
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        if (forklift == null)
        {
            Debug.LogError("Forklift Transform fehlt!");
            yield break;
        }

        while (Vector3.Distance(forklift.position, targetPosition) > 0.05f)
        {
            forklift.position = Vector3.MoveTowards(
                forklift.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        forklift.position = targetPosition;
    }
}