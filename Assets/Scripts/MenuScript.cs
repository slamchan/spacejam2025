using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject[] backgroundImages;
    public AudioSource audioSource; // AudioSource for playing button sounds
    public AudioClip storyButtonSound;
    public AudioSource musicAudioSource;
    public AudioClip menuMusic; // Music for the menu
    // Optional: Overload method to dynamically set the scene name and load it
    public void ChangeScene(string targetSceneName)
    {
        SceneManager.LoadScene(targetSceneName);
    }
    // Method to activate a specific background and hide the rest
    public void ShowBackground(int index)
    {
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            PlaySound(storyButtonSound);
            backgroundImages[i].SetActive(i == index); // Only activate the selected background
        }
    }

    private void Start()
    {
        PlayMenuMusic(); // Start the background music when the menu loads
    }

    private void PlayMenuMusic()
    {
        if (musicAudioSource != null && menuMusic != null)
        {
            musicAudioSource.clip = menuMusic;
            musicAudioSource.loop = true; // Loop the music
            musicAudioSource.Play();
        }
    }

    // Button events for each background
    public void OnStoryButtonClick()
    {
        PlaySound(storyButtonSound);
        ShowBackground(0); // Show the first background (Story)
    }

    public void OnHowToButtonClick()
    {
        PlaySound(storyButtonSound);
        ShowBackground(1); // Show the second background (How To)
    }

    public void OnCreditButtonClick()
    {
        PlaySound(storyButtonSound);
        ShowBackground(2); // Show the third background (Credit)
    }

    public void OnBackButtonClick()
    {
        PlaySound(storyButtonSound);
        ShowBackground(3); // Show the fourth background (Main Menu or default)
    }
    public void OnQuitButtonClick()
    {
        Debug.Log("QuitGame() called - Application.Quit() doesn't work in the editor.");
        Application.Quit();
    }


    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
