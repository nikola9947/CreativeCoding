using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class IntroTypewriter : MonoBehaviour
{
    [Header("Panels")]
    public GameObject titlePanel;
    public GameObject introPanel;
    public GameObject mainCanvas;

    [Header("Intro")]
    public TextMeshProUGUI introText;
    public GameObject startButton;

    [Header("Game")]
    public GameManager gameManager;

    [TextArea(4, 10)]
    public string fullText =
        "Welcome, Operator!\n\n" +
        "Keep the factory running by maintaining the workers, machines, robot and pallet station.\n\n" +
        "Repair breakdowns through minigames and deliver 10 pallets before time runs out!";

    public float typingSpeed = 0.03f;

    private bool titleActive = true;
    private bool introActive = false;
    private bool isTyping = false;
    private bool gameStarted = false;

    private void Start()
    {
        if (titlePanel != null)
            titlePanel.SetActive(true);

        if (introPanel != null)
            introPanel.SetActive(false);

        if (mainCanvas != null)
            mainCanvas.SetActive(false);

        if (startButton != null)
            startButton.SetActive(false);
    }

    private void Update()
    {
        if (gameStarted)
            return;

        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (titleActive)
            {
                ShowIntro();
                return;
            }

            if (introActive && !isTyping)
            {
                StartGame();
            }
        }
    }

    private void ShowIntro()
    {
        titleActive = false;
        introActive = true;

        if (titlePanel != null)
            titlePanel.SetActive(false);

        if (introPanel != null)
            introPanel.SetActive(true);

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        if (introText != null)
            introText.text = "";

        foreach (char c in fullText)
        {
            if (introText != null)
                introText.text += c;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;

        if (startButton != null)
            startButton.SetActive(true);
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;

        Debug.Log("START GAME TRIGGERED");

        if (introPanel != null)
            introPanel.SetActive(false);

        if (titlePanel != null)
            titlePanel.SetActive(false);

        if (mainCanvas != null)
            mainCanvas.SetActive(true);

        if (gameManager != null)
            gameManager.StartGame();
    }
}