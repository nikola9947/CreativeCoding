using UnityEngine;

public class OutroManager : MonoBehaviour
{
    [Header("Outro Canvas")]
    public GameObject outroCanvas;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Menu")]
    public MenuFlowManager menuFlowManager;

    private void Start()
    {
        if (outroCanvas != null)
            outroCanvas.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowWin()
    {
        if (outroCanvas != null)
            outroCanvas.SetActive(true);

        if (winPanel != null)
            winPanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (outroCanvas != null)
            outroCanvas.SetActive(true);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
    
    public GameManager gameManager;

    public void BackToDifficulty()
    {
        if (outroCanvas != null)
            outroCanvas.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gameManager != null)
            gameManager.ResetForNewGame();

        if (menuFlowManager != null)
            menuFlowManager.ShowDifficulty();

        if (gameManager != null)
            gameManager.ResetForNewGame();

        if (menuFlowManager != null)
            menuFlowManager.ShowDifficulty();
    }
    
}