using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Music clip that you want to play
    public AudioClip backgroundMusic;

    void Awake()
    {
        // Ensure only one music manager exists
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        // Don't destroy this GameObject on scene load to keep the music playing
        DontDestroyOnLoad(gameObject);

        // Get the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();

        // Assign the background music clip
        audioSource.clip = backgroundMusic;

        // Set the AudioSource to loop
        audioSource.loop = true;

        // Play the music
        audioSource.Play();
    }

    // You can stop the music using this method
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // You can pause the music using this method
    public void PauseMusic()
    {
        audioSource.Pause();
    }

    // You can resume the music using this method
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }
}
