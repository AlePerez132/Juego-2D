using UnityEngine;
using UnityEngine.SceneManagement;

public class Contador : MonoBehaviour
{
    public static Contador instancia;

    public int segundosJugados = 0;
    private float tiempoAcumulado = 0f;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;

        if (escenaActual == 0) //si esta en la escena 0 se resetea el tiempo y no crece
        {
            segundosJugados = 0;
            tiempoAcumulado = 0f;
            return;
        }
        //si no esta en la escena 0 se empieza a contar el tiempo  
        tiempoAcumulado += Time.deltaTime;
        if (tiempoAcumulado >= 1f)
        {
            segundosJugados++;
            tiempoAcumulado = 0f;
        }
    }
}
