using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BalanceMiniGame : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject miniGameCamera;

    [Header("UI")]
    public GameObject miniGamePanel;
    public Slider balanceSlider;
    public RectTransform arrowImage;

    [Header("Palette")]
    public Transform pallet;

    [Header("Settings")]
    public float moveSpeed = 60f;
    public float driftSpeed = 20f;
    public float successRange = 10f;
    public float requiredBalanceTime = 3f;
    public float arrowMoveWidth = 160f;

    private float balanceValue;
    private float balancedTimer;
    private bool miniGameActive = false;

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

        float delta = Time.unscaledDeltaTime;

        balanceValue += driftSpeed * delta;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                balanceValue -= moveSpeed * delta;

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                balanceValue += moveSpeed * delta;
        }

        balanceValue = Mathf.Clamp(balanceValue, -100f, 100f);

        if (balanceSlider != null)
            balanceSlider.value = balanceValue;

        UpdateArrow();

        if (pallet != null)
        {
            pallet.localRotation = Quaternion.Euler(
                0f,
                0f,
                balanceValue * 0.15f
            );
        }

        if (Mathf.Abs(balanceValue) <= successRange)
        {
            balancedTimer += delta;

            if (balancedTimer >= requiredBalanceTime)
            {
                FinishMiniGame();
            }
        }
        else
        {
            balancedTimer = 0f;
        }
    }

    private void UpdateArrow()
    {
        if (arrowImage == null)
            return;

        float normalized = Mathf.InverseLerp(-100f, 100f, balanceValue);

        float xPos = Mathf.Lerp(
            -arrowMoveWidth / 2f,
            arrowMoveWidth / 2f,
            normalized
        );

        arrowImage.anchoredPosition = new Vector2(
            xPos,
            arrowImage.anchoredPosition.y
        );
    }

    public void StartMiniGame()
    {
        miniGameActive = true;

        balanceValue = Random.Range(-80f, 80f);
        balancedTimer = 0f;

        if (balanceSlider != null)
            balanceSlider.value = balanceValue;

        UpdateArrow();

        

        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(true);

        if (miniGamePanel != null)
            miniGamePanel.SetActive(true);

        Debug.Log("BALANCE MINIGAME STARTED");
    }

    private void FinishMiniGame()
    {
        miniGameActive = false;

        if (pallet != null)
            pallet.localRotation = Quaternion.identity;

        if (miniGamePanel != null)
            miniGamePanel.SetActive(false);

        if (miniGameCamera != null)
            miniGameCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.SetActive(true);

        

        Debug.Log("PALLET STABILIZED");

        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.RepairCurrentStation();
        }
    }
}