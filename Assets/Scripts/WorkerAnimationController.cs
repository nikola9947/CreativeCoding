using System.Collections;
using UnityEngine;

public class WorkerAnimationController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource danceAudio;

    [Header("State Names")]
    public string boredState = "Bored";
    public string workState = "WorkerAnimation";
    public string danceState = "TwistDance";
    public string dizzyState = "DizzyIdle";

    [Header("Durations")]
    public float workDuration = 1.2f;
    public float danceDuration = 2.5f;
    public float danceSoundDuration = 2.5f;

    private Coroutine currentRoutine;
    private bool isDizzy = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (danceAudio == null)
            danceAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBored();
    }

    public void PlayBored()
    {
        if (isDizzy)
            return;

        PlayState(boredState);
    }

    public void PlayWork()
    {
        if (isDizzy)
            return;

        PlayTemporaryState(workState, workDuration);
    }

    public void PlayDance()
    {
        if (isDizzy)
            return;

        if (danceAudio != null)
        {
            danceAudio.Stop();
            danceAudio.Play();

            StartCoroutine(StopDanceAudioAfterDelay());
        }

        PlayTemporaryState(danceState, danceDuration);
    }

    public void SetDizzy(bool dizzy)
    {
        isDizzy = dizzy;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        if (isDizzy)
        {
            PlayState(dizzyState);
        }
        else
        {
            PlayState(boredState);
        }
    }

    private void PlayTemporaryState(string stateName, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine =
            StartCoroutine(
                TemporaryStateRoutine(
                    stateName,
                    duration
                )
            );
    }

    private IEnumerator TemporaryStateRoutine(
        string stateName,
        float duration)
    {
        PlayState(stateName);

        yield return new WaitForSeconds(duration);

        if (!isDizzy)
            PlayState(boredState);

        currentRoutine = null;
    }

    private IEnumerator StopDanceAudioAfterDelay()
    {
        yield return new WaitForSeconds(danceSoundDuration);

        if (danceAudio != null)
            danceAudio.Stop();
    }

    private void PlayState(string stateName)
    {
        if (animator == null)
        {
            Debug.LogWarning(name + ": Animator missing!");
            return;
        }

        animator.CrossFade(stateName, 0.1f);
    }
}