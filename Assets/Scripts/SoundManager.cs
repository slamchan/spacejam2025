using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager Instance;

    // Reference to the AudioSource component for background music
    private AudioSource musicAudioSource;

    // Reference to the AudioSource for one-time sounds
    private AudioSource soundEffectAudioSource;

    // Background music clip (assigned in Inspector)
    public AudioClip backgroundMusic;

    // One-time sound clip (assigned in Inspector)
    public AudioClip destructionSound;

    // player movement
    private AudioSource playerMovementAudioSource;
    public AudioClip playerMovementSound;

    void Awake()
    {
        // Ensure only one SoundManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance
        Instance = this;

        // Keep this object between scenes
        DontDestroyOnLoad(gameObject);

        // Find or create the music AudioSource (for background music)
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play(); // Start playing background music

        // Find or create the sound effect AudioSource (for one-time sounds)
        soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        soundEffectAudioSource.loop = false;  // One-time sound effect should not loop

        playerMovementAudioSource = gameObject.AddComponent<AudioSource>();
        playerMovementAudioSource.loop = true;
        playerMovementAudioSource.clip = playerMovementSound;
    }

    // Play the destruction sound effect (or any one-time sound)
    public void PlayDestructionSound()
    {
        if (destructionSound != null)
        {
            soundEffectAudioSource.PlayOneShot(destructionSound);
        }
    }

    public void SetPlayerMovementSound(bool active)
    {
        if (active && !playerMovementAudioSource.isPlaying)
        {
            playerMovementAudioSource.Play();
        }
        else if (!active && playerMovementAudioSource.isPlaying)
        {
            playerMovementAudioSource.Stop();
        }
    }
}
