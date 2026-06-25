using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class IntroTypewriter : MonoBehaviour
{
    [Header("Menu Flow")]
    public MenuFlowManager menuFlowManager;

    [Header("Text")]
    [TextArea(4, 10)]
    public string fullText =
        "Welcome, Operator!\n\n" +
        "Keep the factory running by maintaining the workers, machines, robot and pallet station.\n\n" +
        "Repair breakdowns through minigames and deliver the required pallets before time runs out!";

    public float typingSpeed = 0.03f;

    private TextMeshProUGUI introText;
    private bool waitingForClick = false;

    private void Awake()
    {
        // Sucht automatisch den ersten TMP-Text im Intro-Canvas
        introText = GetComponentInChildren<TextMeshProUGUI>(true);

        if (introText == null)
            Debug.LogError("IntroTypewriter: Kein TextMeshProUGUI als Child gefunden!");
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        waitingForClick = false;

        if (introText == null)
            return;

        StartCoroutine(TypeText());
    }

    private void Update()
    {
        if (!waitingForClick)
            return;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            waitingForClick = false;

            if (menuFlowManager != null)
                menuFlowManager.ShowDifficulty();
        }
    }

    private IEnumerator TypeText()
    {
        introText.text = "";

        foreach (char c in fullText)
        {
            introText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        waitingForClick = true;
    }
}