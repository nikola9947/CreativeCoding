using UnityEngine;

public class OutroManager : MonoBehaviour
{
    [Header("Outro Canvas Root")]
    public GameObject outroCanvas;

    [Header("Result Images")]
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
        Debug.Log("OUTRO: SHOW WIN");
        ShowOutro();

        if (winImage != null)
            winImage.SetActive(true);

        if (gameOverImage != null)
            gameOverImage.SetActive(false);
    }

    public void ShowGameOver()
    {
        Debug.Log("OUTRO: SHOW GAME OVER");
        ShowOutro();

        if (winImage != null)
            winImage.SetActive(false);

        if (gameOverImage != null)
            gameOverImage.SetActive(true);
    }

    public void BackToDifficulty()
    {
        Debug.Log("OUTRO: BACK TO DIFFICULTY");

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
            Debug.LogError("OutroManager: Outro Canvas fehlt!");
            return;
        }

        Debug.Log("Outro object: " + outroCanvas.name);
        Debug.Log("Outro active before: " + outroCanvas.activeSelf);

        outroCanvas.SetActive(true);
        outroCanvas.transform.SetAsLastSibling();

        Canvas canvas = outroCanvas.GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;
        }

        Debug.Log("Outro active after: " + outroCanvas.activeSelf);
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