using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    // Phát âm thanh một lần
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Dừng âm thanh
    public void StopSound()
    {
        audioSource.Stop();
    }
}
