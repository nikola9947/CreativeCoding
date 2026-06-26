using System.Collections;
using UnityEngine;

public class RoboterClickAnimation : MonoBehaviour
{
    public Animator animator;
    public string clickTrigger = "Click";
    public float cooldown = 3f;

    private bool isPlaying = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("ROBOT CLICKED");

        if (isPlaying)
            return;

        StartCoroutine(PlayClickAnimation());
    }

    private IEnumerator PlayClickAnimation()
    {
        isPlaying = true;

        if (animator != null)
        {
            animator.ResetTrigger("Work");
            animator.ResetTrigger(clickTrigger);
            animator.SetTrigger(clickTrigger);
        }

        yield return new WaitForSeconds(cooldown);

        isPlaying = false;
    }
}