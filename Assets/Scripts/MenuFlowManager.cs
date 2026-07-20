using UnityEngine;
using UnityEngine.InputSystem;

public class MenuFlowManager : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject titleCanvas;
    public GameObject introCanvas;
    public GameObject difficultyCanvas;
    public GameObject mainCanvas;

    private bool waitingForTitleClick = true;

    private void Start()
    {
        ShowTitleCanvas();
    }

    private void Update()
    {
        if (!waitingForTitleClick)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShowIntroCanvas();
        }
    }

    private void ShowTitleCanvas()
    {
        waitingForTitleClick = true;

        SetCanvas(titleCanvas, true);
        SetCanvas(introCanvas, false);
        SetCanvas(difficultyCanvas, false);
        SetCanvas(mainCanvas, false);

        Debug.Log("MENU: Title shown");
    }

    private void ShowIntroCanvas()
    {
        waitingForTitleClick = false;

        SetCanvas(titleCanvas, false);
        SetCanvas(introCanvas, true);
        SetCanvas(difficultyCanvas, false);
        SetCanvas(mainCanvas, false);

        Debug.Log("MENU: Intro shown");
    }

    public void ShowDifficulty()
    {
        waitingForTitleClick = false;

        SetCanvas(titleCanvas, false);
        SetCanvas(introCanvas, false);
        SetCanvas(difficultyCanvas, true);
        SetCanvas(mainCanvas, false);

        Debug.Log("MENU: Difficulty shown");
    }

    public void ShowMainCanvas()
    {
        waitingForTitleClick = false;

        SetCanvas(titleCanvas, false);
        SetCanvas(introCanvas, false);
        SetCanvas(difficultyCanvas, false);
        SetCanvas(mainCanvas, true);

        Debug.Log("MENU: Main shown");
    }

    private void SetCanvas(GameObject canvasObject, bool active)
    {
        if (canvasObject == null)
            return;

        canvasObject.SetActive(active);
    }
}