using UnityEngine;

public class OutroManager : MonoBehaviour
{
    [Header("Outro")]
    public GameObject outroCanvas;

    [Header("Images")]
    public GameObject winImage;
    public GameObject gameOverImage;

    [Header("Managers")]
    public MenuFlowManager menuFlowManager;
    public GameManager gameManager;

    private void Start()
    {
        HideOutro();
    }

    public void ShowWin()
    {
        ShowOutro();

        if (winImage != null)
            winImage.SetActive(true);

        if (gameOverImage != null)
            gameOverImage.SetActive(false);

        Debug.Log("OUTRO: WIN");
    }

    public void ShowGameOver()
    {
        ShowOutro();

        if (winImage != null)
            winImage.SetActive(false);

        if (gameOverImage != null)
            gameOverImage.SetActive(true);

        Debug.Log("OUTRO: GAME OVER");
    }

    public void BackToDifficulty()
    {
        HideOutro();

        if (gameManager != null)
            gameManager.ResetForNewGame();

        if (menuFlowManager != null)
            menuFlowManager.ShowDifficulty();
    }

    private void ShowOutro()
    {
        if (outroCanvas == null)
        {
            Debug.LogError("Outro Canvas not assigned!");
            return;
        }

        outroCanvas.SetActive(true);

        Canvas canvas = outroCanvas.GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 9999;
        }

        outroCanvas.transform.SetAsLastSibling();
    }

    private void HideOutro()
    {
        if (outroCanvas != null)
            outroCanvas.SetActive(false);

        if (winImage != null)
            winImage.SetActive(false);

        if (gameOverImage != null)
            gameOverImage.SetActive(false);
    }
}