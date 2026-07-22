using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MusicButton : MonoBehaviour
{
    [Header("Icons")]
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateIcon();
    }

    public void ToggleMusic()
    {
        if (MusicManager.Instance == null)
            return;

        MusicManager.Instance.ToggleMusic();
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (buttonImage == null || MusicManager.Instance == null)
            return;

        buttonImage.sprite = MusicManager.Instance.IsMusicEnabled()
            ? musicOnSprite
            : musicOffSprite;
    }
}