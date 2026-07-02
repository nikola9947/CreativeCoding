using System.Collections;
using UnityEngine;

public class RoboterClickAnimation : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animation")]
    public string clickTrigger = "Click";
    public float animationDuration = 3f;

    [Header("Audio")]
    public AudioSource robotAudio;

    private bool isPlaying = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (robotAudio == null)
            robotAudio = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null && !gameManager.CanUseWorldInteraction())
            return;

        if (isPlaying)
            return;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        isPlaying = true;

        if (animator != null)
        {
            animator.ResetTrigger(clickTrigger);
            animator.SetTrigger(clickTrigger);
        }

        if (robotAudio != null)
        {
            robotAudio.Stop();
            robotAudio.Play();
        }

        yield return new WaitForSeconds(animationDuration);

        if (robotAudio != null && robotAudio.isPlaying)
            robotAudio.Stop();

        isPlaying = false;
    }
}