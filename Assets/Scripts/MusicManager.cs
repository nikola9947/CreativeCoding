using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music")]
    public AudioSource musicSource;

    private bool musicEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Gespeicherte Einstellung laden
            musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

            ApplyMusicState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;

        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMusicState();
    }

    public bool IsMusicEnabled()
    {
        return musicEnabled;
    }

    private void ApplyMusicState()
    {
        if (musicSource == null)
            return;

        musicSource.mute = !musicEnabled;
    }
}