using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class IntroTypewriter : MonoBehaviour
{

        private void Update()
    {
    if (Mouse.current != null &&
        Mouse.current.leftButton.wasPressedThisFrame)
    {
        Debug.Log("Mouse click detected by DifficultySelector");
    }
    }
    
    [Header("UI")]
    public TextMeshProUGUI introText;
    public GameObject continueButton;

    [Header("Menu Flow")]
    public MenuFlowManager menuFlowManager;

    [Header("Text")]
    [TextArea(4, 10)]
    public string fullText =
        "Welcome, Operator!\n\n" +
        "Keep the factory running by maintaining the workers, machines, robot and pallet station.\n\n" +
        "Repair breakdowns through minigames and deliver the required pallets before time runs out!";

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    private bool isTyping = false;

    private void OnEnable()
    {
        StopAllCoroutines();

        if (continueButton != null)
            continueButton.SetActive(false);

        if (introText != null)
            StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        introText.text = "";

        foreach (char c in fullText)
        {
            introText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;

        if (continueButton != null)
            continueButton.SetActive(true);
    }

    public void ContinueToDifficulty()
    {
        if (isTyping)
            return;

        if (menuFlowManager != null)
            menuFlowManager.ShowDifficulty();
    }
}