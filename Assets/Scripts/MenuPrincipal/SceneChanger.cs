using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class CambiarEscena : MonoBehaviour
{
    private string nombreEscena = "Escena1"; // Nombre de la escena a cargar
    public ButtonManager buttonManager;

    public void CambiarDeEscena()
    {
        switch (buttonManager.dificultad)
        {
            case "facil":
                PlayerPrefs.SetInt("vidaMax", 400);
                break;
            case "normal":
                PlayerPrefs.SetInt("vidaMax", 200);
                break;
            case "dificil":
                PlayerPrefs.SetInt("vidaMax", 100);
                break;
        }
        PlayerPrefs.SetInt("vidaActual", PlayerPrefs.GetInt("vidaMax"));
        StartCoroutine(CambiarEscenaDespuesDeSonido());
    }

    //es para que siempre se reproduzca el sonido antes de cargar la escena
    IEnumerator CambiarEscenaDespuesDeSonido()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(nombreEscena);
    }
}
