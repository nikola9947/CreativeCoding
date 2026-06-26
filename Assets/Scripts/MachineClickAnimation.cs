using System.Collections;
using UnityEngine;

public class MachineClickAnimation : MonoBehaviour
{
    public Animator animator;

    [Tooltip("Name des Animation States")]
    public string animationState = "MachineAnimation";

    public float animationDuration = 3f;

    private bool isPlaying = false;

    private void OnMouseDown()
    {
        if (isPlaying)
            return;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        isPlaying = true;

        animator.Play(animationState);

        yield return new WaitForSeconds(animationDuration);

        animator.Play("Idle");

        isPlaying = false;
    }
}