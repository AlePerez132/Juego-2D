using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    private string nombreEscena = "Juego"; // Nombre de la escena a cargar

    public void CambiarDeEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
