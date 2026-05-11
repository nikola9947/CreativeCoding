using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Goal")]
    public int targetPallets = 10;

    [Header("Timer")]
    public float timeLimit = 60f;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI palletText;
    public TextMeshProUGUI resultText;

    private int deliveredPallets = 0;
    private float timer;
    private bool gameEnded = false;

    private void Start()
    {
        timer = timeLimit;

        UpdateUI();

        if (resultText != null)
        {
            resultText.text = "";
        }
    }

    private void Update()
    {
        if (gameEnded)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;

            CheckResult();
        }

        UpdateUI();
    }

    public void PalletDelivered()
    {
        if (gameEnded)
            return;

        deliveredPallets++;

        UpdateUI();

        Debug.Log(
            "Pallet delivered: "
            + deliveredPallets
            + "/"
            + targetPallets
        );

        if (deliveredPallets >= targetPallets)
        {
            WinGame();
        }
    }

    private void CheckResult()
    {
        if (deliveredPallets >= targetPallets)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;

        Debug.Log("YOU WIN!");

        if (resultText != null)
        {
            resultText.text = "YOU WIN!";
        }
    }

    private void LoseGame()
    {
        gameEnded = true;

        Debug.Log("YOU LOSE!");

        if (resultText != null)
        {
            resultText.text = "YOU LOSE!";
        }
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text =
                "TIME: "
                + Mathf.CeilToInt(timer).ToString();
        }

        if (palletText != null)
        {
            palletText.text =
                "PALLETS: "
                + deliveredPallets
                + "/"
                + targetPallets;
        }
    }
}