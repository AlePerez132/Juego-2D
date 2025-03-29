using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfiguracionVolumen : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider deslizadorMusica;
    public Slider deslizadorEfectos;
    public Slider deslizadorGeneral;


    private void Start()
    {
        if (PlayerPrefs.HasKey("volumenMusica"))
        {
            CargarConfiguracionMusica();
        } else
        {
            EstablecerVolumenMusica();
        }

        if (PlayerPrefs.HasKey("volumenGeneral"))
        {
            CargarConfiguracionGeneral();
        }
        else
        {
            EstablecerVolumenGeneral();
        }

        if (PlayerPrefs.HasKey("volumenEfectos"))
        {
            CargarConfiguracionEfectos();
        }
        else
        {
            EstablecerVolumenEfectos();
        }
    }

    public void EstablecerVolumenMusica()
    {
        float volumen = deslizadorMusica.value;
        audioMixer.SetFloat("musica", Mathf.Log10(volumen)*20);
        PlayerPrefs.SetFloat("volumenMusica", volumen);
    }

    public void EstablecerVolumenEfectos()
    {
        float volumen = deslizadorEfectos.value;
        audioMixer.SetFloat("efectos", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("volumenEfectos", volumen);
    }

    public void EstablecerVolumenGeneral()
    {
        float volumen = deslizadorGeneral.value;
        audioMixer.SetFloat("general", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("volumenGeneral", volumen);
    }

    private void CargarConfiguracionMusica()
    {
        deslizadorMusica.value = PlayerPrefs.GetFloat("volumenMusica");
        EstablecerVolumenMusica();
    }

    private void CargarConfiguracionGeneral()
    {
        deslizadorGeneral.value = PlayerPrefs.GetFloat("volumenGeneral");
        EstablecerVolumenGeneral();
    }

    private void CargarConfiguracionEfectos()
    {
        deslizadorEfectos.value = PlayerPrefs.GetFloat("volumenEfectos");
        EstablecerVolumenEfectos();
    }
}
