using System.Collections;
using UnityEngine;

public class ForkliftPickup : MonoBehaviour
{
    [Header("Objects")]
    public Transform forklift;
    public Transform pallet;

    [Header("Pickup Full Pallet")]
    public Transform pickupPoint;

    [Header("Remove Full Pallet")]
    public Transform exitPoint;

    [Header("Get New Pallet")]
    public Transform newPalletPoint;

    [Header("Deliver New Pallet")]
    public Transform forkliftDropPoint;
    public Transform palletDropPoint;

    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    private bool isMoving = false;

    public void PickupPallet(EndWorkerStation station)
    {
        if (!isMoving)
        {
            StartCoroutine(PickupRoutine(station));
        }
    }

    private IEnumerator PickupRoutine(EndWorkerStation station)
    {
        isMoving = true;

        // 1. Zur vollen Palette fahren
        yield return MoveTo(pickupPoint.position);

        yield return new WaitForSeconds(waitTime);

        // 2. Volle Palette aufnehmen
        pallet.SetParent(forklift);

        // 3. Volle Palette wegfahren
        yield return MoveTo(exitPoint.position);

        yield return new WaitForSeconds(waitTime);

        // 4. Palette abgeben
        pallet.SetParent(null);
        pallet.gameObject.SetActive(false);

        // 5. Palette zählen
        if (gameManager != null)
        {
            gameManager.PalletDelivered();
        }

        yield return new WaitForSeconds(waitTime);

        // 6. Neue leere Palette holen
        pallet.gameObject.SetActive(true);

        pallet.position = newPalletPoint.position;
        pallet.rotation = newPalletPoint.rotation;

        // 7. Leere Palette aufnehmen
        pallet.SetParent(forklift);

        yield return new WaitForSeconds(waitTime);

        // 8. Zur Produktionslinie fahren
        yield return MoveTo(forkliftDropPoint.position);

        yield return new WaitForSeconds(waitTime);

        // 9. Neue Palette absetzen
        pallet.SetParent(null);

        pallet.position = palletDropPoint.position;
        pallet.rotation = palletDropPoint.rotation;

        yield return new WaitForSeconds(waitTime);

        // 10. Produktion wieder starten
        station.NewPalletArrived();

        isMoving = false;
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(forklift.position, target) > 0.05f)
        {
            forklift.position = Vector3.MoveTowards(
                forklift.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}