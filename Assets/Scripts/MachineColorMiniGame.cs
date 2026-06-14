using UnityEngine;
using UnityEngine.UI;

public class MachineColorMiniGame : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject miniGameCamera;

    [Header("UI")]
    public GameObject miniGamePanel;
    public Image targetColorImage;
    public Image currentColorImage;

    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    [Header("Settings")]
    public Color targetColor = Color.green;
    public float tolerance = 0.12f;
    public float minimumStartDifference = 0.6f;

    private bool miniGameActive = false;

    private void Start()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(false);

        if (redSlider != null)
            redSlider.onValueChanged.AddListener(OnColorChanged);

        if (greenSlider != null)
            greenSlider.onValueChanged.AddListener(OnColorChanged);

        if (blueSlider != null)
            blueSlider.onValueChanged.AddListener(OnColorChanged);
    }

    public void StartMiniGame()
    {
        miniGameActive = true;

        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(true);

        if (miniGamePanel != null)
            miniGamePanel.SetActive(true);

        if (targetColorImage != null)
            targetColorImage.color = targetColor;

        SetRandomStartColorFarFromTarget();

        UpdateCurrentColor();

        Debug.Log("MACHINE COLOR MINIGAME STARTED");
    }

    private void SetRandomStartColorFarFromTarget()
    {
        int attempts = 0;

        do
        {
            if (redSlider != null)
                redSlider.value = Random.Range(0f, 1f);

            if (greenSlider != null)
                greenSlider.value = Random.Range(0f, 1f);

            if (blueSlider != null)
                blueSlider.value = Random.Range(0f, 1f);

            attempts++;

        } while (GetCurrentColorDifference() < minimumStartDifference && attempts < 100);
    }

    private void OnColorChanged(float value)
    {
        if (!miniGameActive)
            return;

        UpdateCurrentColor();

        if (IsColorCloseEnough())
        {
            FinishMiniGame();
        }
    }

    private void UpdateCurrentColor()
    {
        if (currentColorImage == null)
            return;

        currentColorImage.color = GetCurrentColor();
    }

    private Color GetCurrentColor()
    {
        float r = redSlider != null ? redSlider.value : 0f;
        float g = greenSlider != null ? greenSlider.value : 0f;
        float b = blueSlider != null ? blueSlider.value : 0f;

        return new Color(r, g, b);
    }

    private float GetCurrentColorDifference()
    {
        Color currentColor = GetCurrentColor();

        return
            Mathf.Abs(currentColor.r - targetColor.r) +
            Mathf.Abs(currentColor.g - targetColor.g) +
            Mathf.Abs(currentColor.b - targetColor.b);
    }

    private bool IsColorCloseEnough()
    {
        return GetCurrentColorDifference() <= tolerance;
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

        Debug.Log("MACHINE CALIBRATED!");

        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.RepairCurrentStation();
        }
    }
}