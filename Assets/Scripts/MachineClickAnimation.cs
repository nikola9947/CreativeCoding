using System.Collections;
using UnityEngine;

public class MachineClickAnimation : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animation")]
    public string animationState = "MachineAnimation";
    public string idleState = "Idle";
    public float animationDuration = 3f;

    [Header("Audio")]
    public AudioSource machineAudio;

    private bool isPlaying = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (machineAudio == null)
            machineAudio = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (isPlaying)
            return;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        isPlaying = true;

        // Animation starten
        if (animator != null)
            animator.Play(animationState);

        // Sound starten
        if (machineAudio != null)
            machineAudio.Play();

        // Warten
        yield return new WaitForSeconds(animationDuration);

        // Zurück auf Idle
        if (animator != null)
            animator.Play(idleState);

        // Sound stoppen, falls er noch läuft
        if (machineAudio != null && machineAudio.isPlaying)
            machineAudio.Stop();

        isPlaying = false;
    }
}