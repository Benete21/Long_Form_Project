using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------Audio Clip----------")]
    public AudioClip background;
    public AudioClip Jump;   // 46 or 47
    public AudioClip shootGloo;   //Sound 21 or 32 0r 40 in Casual Game sounds U6
    public AudioClip glooCollision;
    public AudioClip psiBulletCollision;
    public AudioClip pyrokinesis;
    public AudioClip hitObstacle;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
