using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public GameManager gameManager;
    public MenuFlowManager menuFlowManager;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        if (menuFlowManager == null)
            menuFlowManager = FindFirstObjectByType<MenuFlowManager>();
    }

    public void Easy()
    {
        Debug.Log("EASY CLICKED");
        SelectDifficulty(0);
    }

    public void Medium()
    {
        Debug.Log("MEDIUM CLICKED");
        SelectDifficulty(1);
    }

    public void Hard()
    {
        Debug.Log("HARD CLICKED");
        SelectDifficulty(2);
    }

    private void SelectDifficulty(int difficulty)
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager fehlt!");
            return;
        }

        if (menuFlowManager == null)
        {
            Debug.LogError("MenuFlowManager fehlt!");
            return;
        }

        if (difficulty == 0)
            gameManager.ApplyEasyDifficulty();

        if (difficulty == 1)
            gameManager.ApplyMediumDifficulty();

        if (difficulty == 2)
            gameManager.ApplyHardDifficulty();

        menuFlowManager.ShowMainCanvas();
        gameManager.StartGame();
    }
}