using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    public GameObject door_opened;  // Referencia a la puerta abierta
    private bool playerInDoor = false;

    void Start()
    {
        door_opened.SetActive(false);  // La puerta abierta comienza desactivada
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDoor = true;  // El jugador entra en la puerta
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDoor = false;  // El jugador sale de la puerta
        }
    }

    void Update()
    {
        if (playerInDoor && Input.GetKeyDown(KeyCode.Space))  // Segunda pulsación para cambiar de escena
        {
            StartCoroutine(OpenDoorAndChangeScene());  // Llamamos a la coroutine para cambiar de escena
        }
    }

    IEnumerator OpenDoorAndChangeScene()
    {
        yield return new WaitForSeconds(1f);  // Esperar 1 segundo

        SceneManager.LoadScene("Escena2");  // Cambiar a la escena deseada
        Debug.Log("Cambio de escena realizado.");
    }
}
