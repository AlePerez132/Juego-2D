using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int vidaMago = 100; // La vida del mago

    private void Awake()
    {
        // Asegura que solo haya una instancia del GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }
}
