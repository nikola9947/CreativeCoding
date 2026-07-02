using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    public Behaviour outlineComponent;

    private void Start()
    {
        if (outlineComponent != null)
            outlineComponent.enabled = false;
    }

    private void OnMouseEnter()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null && !gameManager.CanUseWorldInteraction())
            return;

        if (outlineComponent != null)
            outlineComponent.enabled = true;
    }

    private void OnMouseExit()
    {
        if (outlineComponent != null)
            outlineComponent.enabled = false;
    }
}