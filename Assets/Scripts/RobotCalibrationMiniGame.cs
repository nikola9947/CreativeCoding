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

    public Button xCalibrateButton;
    public Button yCalibrateButton;
    public Button zCalibrateButton;

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
        ResetMiniGame();

        if (xCalibrateButton != null)
            xCalibrateButton.onClick.AddListener(CalibrateX);

        if (yCalibrateButton != null)
            yCalibrateButton.onClick.AddListener(CalibrateY);

        if (zCalibrateButton != null)
            zCalibrateButton.onClick.AddListener(CalibrateZ);
    }

    private void Update()
    {
        if (!miniGameActive)
            return;

        if (!xDone && xSlider != null)
            xSlider.value = Mathf.PingPong(Time.time * pointerSpeed, 1f);

        if (!yDone && ySlider != null)
            ySlider.value = Mathf.PingPong(Time.time * pointerSpeed * 1.2f, 1f);

        if (!zDone && zSlider != null)
            zSlider.value = Mathf.PingPong(Time.time * pointerSpeed * 1.4f, 1f);
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

        if (xCalibrateButton != null)
            xCalibrateButton.interactable = true;

        if (yCalibrateButton != null)
            yCalibrateButton.interactable = true;

        if (zCalibrateButton != null)
            zCalibrateButton.interactable = true;

        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(true);

        if (miniGamePanel != null)
            miniGamePanel.SetActive(true);

        Debug.Log("ROBOT CALIBRATION STARTED");
    }

    public void ResetMiniGame()
    {
        miniGameActive = false;

        xDone = false;
        yDone = false;
        zDone = false;

        if (xSlider != null)
            xSlider.value = 0f;

        if (ySlider != null)
            ySlider.value = 0f;

        if (zSlider != null)
            zSlider.value = 0f;

        if (xCalibrateButton != null)
            xCalibrateButton.interactable = true;

        if (yCalibrateButton != null)
            yCalibrateButton.interactable = true;

        if (zCalibrateButton != null)
            zCalibrateButton.interactable = true;

        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.SetActive(true);
    }

    public void CalibrateX()
    {
        if (!miniGameActive || xDone)
            return;

        if (IsSliderInRange(xSlider))
        {
            xDone = true;

            if (xCalibrateButton != null)
                xCalibrateButton.interactable = false;

            CheckFinished();
        }
    }

    public void CalibrateY()
    {
        if (!miniGameActive || yDone)
            return;

        if (IsSliderInRange(ySlider))
        {
            yDone = true;

            if (yCalibrateButton != null)
                yCalibrateButton.interactable = false;

            CheckFinished();
        }
    }

    public void CalibrateZ()
    {
        if (!miniGameActive || zDone)
            return;

        if (IsSliderInRange(zSlider))
        {
            zDone = true;

            if (zCalibrateButton != null)
                zCalibrateButton.interactable = false;

            CheckFinished();
        }
    }

    private bool IsSliderInRange(Slider slider)
    {
        if (slider == null)
            return false;

        return Mathf.Abs(slider.value - targetValue) <= tolerance;
    }

    private void CheckFinished()
    {
        if (xDone && yDone && zDone)
        {
            FinishMiniGame();
        }
    }

    private void FinishMiniGame()
    {
        Debug.Log("ROBOT CALIBRATED!");

        ResetMiniGame();

        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.RepairCurrentStation();
        }
    }
}