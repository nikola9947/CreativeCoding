using System.Collections;
using UnityEngine;
using TMPro;

public class IntroTypewriter : MonoBehaviour
{
    public GameObject introCanvas;
    public TextMeshProUGUI introText;
    public GameObject startButton;

    public GameManager gameManager;

    [TextArea(4, 10)]
    public string fullText =
        "Welcome, Operator!\n\n" +
        "Keep the factory running by maintaining the workers, machines, robot and pallet station.\n\n" +
        "Repair breakdowns through minigames and deliver 10 pallets before time runs out!";

    public float typingSpeed = 0.03f;

    private bool isTyping = true;

    private void Start()
    {
        if (startButton != null)
            startButton.SetActive(false);

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        introText.text = "";

        foreach (char c in fullText)
        {
            introText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;

        if (startButton != null)
            startButton.SetActive(true);
    }

    public void StartGame()
    {
        if (isTyping)
            return;

        if (gameManager != null)
            gameManager.StartGame();

        introCanvas.SetActive(false);
    }
}