using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WorkerMiniGame : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject miniGameCamera;
    public Camera miniGameCam;

    [Header("Clickable Object")]
    public GameObject fanObject;

    [Header("UI")]
    public Slider recoverySlider;
    public GameObject sliderUI;

    [Header("Settings")]
    public float clicksNeeded = 20f;

    private float currentRecovery = 0f;
    private bool miniGameActive = false;

    private void Start()
    {
        ResetMiniGame();
    }

    private void Update()
    {
        if (!miniGameActive)
            return;

        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (miniGameCam == null || fanObject == null)
                return;

            Ray ray = miniGameCam.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == fanObject ||
                    hit.collider.transform.IsChildOf(fanObject.transform))
                {
                    FanClick();
                }
            }
        }
    }

    public void StartMiniGame()
    {
        miniGameActive = true;
        currentRecovery = 0f;

        if (sliderUI != null)
            sliderUI.SetActive(true);

        if (recoverySlider != null)
            recoverySlider.value = 0f;

        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(true);

        Debug.Log("WORKER MINIGAME STARTED");
    }

    public void ResetMiniGame()
    {
        miniGameActive = false;
        currentRecovery = 0f;

        if (recoverySlider != null)
            recoverySlider.value = 0f;

        if (sliderUI != null)
            sliderUI.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.SetActive(true);
    }

    public void FanClick()
    {
        if (!miniGameActive)
            return;

        currentRecovery++;

        if (recoverySlider != null)
            recoverySlider.value = currentRecovery / clicksNeeded;

        Debug.Log("Fan clicked: " + currentRecovery + "/" + clicksNeeded);

        if (currentRecovery >= clicksNeeded)
        {
            FinishMiniGame();
        }
    }

    private void FinishMiniGame()
    {
        Debug.Log("WORKER RECOVERED");

        ResetMiniGame();

        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.RepairCurrentStation();
        }
    }
}