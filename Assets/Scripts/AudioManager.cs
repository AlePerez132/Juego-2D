using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("--------------AUDIO SOURCE--------------")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("--------------AUDIO CLIPS--------------")]
    [Header("EFECTOS")]
    public AudioClip botonSimple;
    public AudioClip botonEntrada;
    public AudioClip salto;
    public AudioClip andar;
    public AudioClip caer;
    public AudioClip recibirDanio;
    public AudioClip espadazo;
    public AudioClip muerteMago;
    public AudioClip muerteEnemigo;
    public AudioClip lanzarBolaFuego;
    public AudioClip esqueletoRecibirDanio;
    public AudioClip impactoBolaFuego;
    public AudioClip ataqueEspadasAngel;
    public AudioClip angelRecibirDanio;
    public AudioClip angelMuerte;
    public AudioClip curacion;
    [Header("MUSICA")]
    public AudioClip musicaLobby;
    public AudioClip musicaEscena1;
    public AudioClip musicaEscena2;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoadedEvent;
            MusicaInicial();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
    }

    public void reproducirEfecto(AudioClip efecto)
    {
        SFXSource.PlayOneShot(efecto);
    }

    private void OnSceneLoadedEvent(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        cambiarMusica(scene.name); 
    }

    private void MusicaInicial()
    {
        if (musicSource.clip == null)
        {
            cambiarMusica("MainMenu"); 
        }
    }

    public void cambiarMusica(string nombreEscena)
    {
        AudioClip clipSeleccionado = null;

        switch (nombreEscena)
        {
            case "MainMenu":
                clipSeleccionado = musicaLobby;
                break;

            case "Escena1":
                clipSeleccionado = musicaEscena1;
                break;

            case "Escena2":
                clipSeleccionado = musicaEscena2;
                break;

            default:
                clipSeleccionado = musicaLobby; // M�sica por defecto
                break;
        }

        // Solo cambia si es necesario
        if (musicSource.clip != clipSeleccionado || !musicSource.isPlaying)
        {
            musicSource.clip = clipSeleccionado;
            musicSource.Play();
        }
    }
}
