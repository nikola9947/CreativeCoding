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
        if (sliderUI != null)
            sliderUI.SetActive(false);

        StartMiniGame();
    }

    private void Update()
    {
        if (!miniGameActive)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = miniGameCam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == fanObject || hit.collider.transform.IsChildOf(fanObject.transform))
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

        Time.timeScale = 0f;

        mainCamera.SetActive(false);
        miniGameCamera.SetActive(true);

        Debug.Log("MINIGAME STARTED");
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
            FinishMiniGame();
    }

    private void FinishMiniGame()
    {
        miniGameActive = false;

        if (sliderUI != null)
            sliderUI.SetActive(false);

        Time.timeScale = 1f;

        mainCamera.SetActive(true);
        miniGameCamera.SetActive(false);

        Debug.Log("WORKER RECOVERED");
    }
}