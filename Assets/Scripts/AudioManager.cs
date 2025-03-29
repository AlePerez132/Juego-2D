using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------------AUDIO SOURCE--------------")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("--------------AUDIO CLIP--------------")]
    public AudioClip botonSimple;
    public AudioClip botonEntrada;
    public AudioClip musicaLobby;

    void Start()
    {
        musicSource.clip = musicaLobby;
        musicSource.Play();
    }

    public void reproducirEfecto(AudioClip efecto)
    {
        SFXSource.PlayOneShot(efecto);
    }
}
