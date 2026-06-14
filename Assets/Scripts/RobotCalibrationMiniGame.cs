using UnityEngine;
using UnityEngine.UI;

public class RobotCalibrationMiniGame : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject miniGameCamera;

    [Header("UI")]
    public GameObject miniGamePanel;

    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;

    [Header("Settings")]
    public float pointerSpeed = 1.5f;
    public float targetValue = 0.5f;
    public float tolerance = 0.08f;

    private bool miniGameActive = false;

    private bool xDone = false;
    private bool yDone = false;
    private bool zDone = false;

    private void Start()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(false);
    }

    private void Update()
    {
        if (!miniGameActive)
            return;

        if (!xDone && xSlider != null)
            xSlider.value = Mathf.PingPong(Time.unscaledTime * pointerSpeed, 1f);

        if (!yDone && ySlider != null)
            ySlider.value = Mathf.PingPong(Time.unscaledTime * pointerSpeed * 1.2f, 1f);

        if (!zDone && zSlider != null)
            zSlider.value = Mathf.PingPong(Time.unscaledTime * pointerSpeed * 1.4f, 1f);
    }

    public void StartMiniGame()
    {
        miniGameActive = true;

        xDone = false;
        yDone = false;
        zDone = false;

        if (xSlider != null)
            xSlider.value = 0f;

        if (ySlider != null)
            ySlider.value = 0f;

        if (zSlider != null)
            zSlider.value = 0f;

        

        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(true);

        if (miniGamePanel != null)
            miniGamePanel.SetActive(true);

        Debug.Log("ROBOT CALIBRATION STARTED");
    }

    public void CalibrateCurrentAxis()
    {
        if (!miniGameActive)
            return;

        if (!xDone)
        {
            TryCalibrate(ref xDone, xSlider);
            return;
        }

        if (!yDone)
        {
            TryCalibrate(ref yDone, ySlider);
            return;
        }

        if (!zDone)
        {
            TryCalibrate(ref zDone, zSlider);
            return;
        }
    }

    private void TryCalibrate(ref bool axisDone, Slider slider)
    {
        if (slider == null)
            return;

        float difference = Mathf.Abs(slider.value - targetValue);

        if (difference <= tolerance)
        {
            axisDone = true;

            if (xDone && yDone && zDone)
            {
                FinishMiniGame();
            }
        }
    }

    private void FinishMiniGame()
    {
        miniGameActive = false;

        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.SetActive(true);

        

        Debug.Log("ROBOT CALIBRATED!");

        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.RepairCurrentStation();
        }
    }
}