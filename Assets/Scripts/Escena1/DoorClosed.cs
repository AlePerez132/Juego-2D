using UnityEngine;

public class DoorClosed : MonoBehaviour
{
    public GameObject door_opened;  // Referencia a la puerta abierta
    public GameObject door_closed;  // Referencia a la puerta cerrada
    private bool playerInDoor = false;

    void Start()
    {
        door_closed.SetActive(true);   // Aseguramos que la puerta cerrada está activa al inicio
        door_opened.SetActive(false);  // Aseguramos que la puerta abierta está desactivada al inicio
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDoor = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDoor = false;
        }
    }

    void Update()
    {
        if (playerInDoor && Input.GetKeyDown(KeyCode.Space))  // Primer pulsación para abrir la puerta
        {
            door_closed.SetActive(false);  // Desactivar la puerta cerrada
            door_opened.SetActive(true);   // Activar la puerta abierta
            Debug.Log("Puerta abierta.");
        }
    }
}
