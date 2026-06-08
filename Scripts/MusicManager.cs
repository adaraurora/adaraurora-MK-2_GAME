using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private AudioSource musicSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = GetComponent<AudioSource>();

        float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.volume = volume;
    }

    public void SetVolume(float volume)
    {
        Debug.Log("Volume: " + volume);
        GetComponent<AudioSource>().volume = volume;
    }
}