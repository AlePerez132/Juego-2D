using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    public GameObject door_opened;  
    private bool playerInDoor = false;

    void Start()
    {
        door_opened.SetActive(false); 
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
        if (playerInDoor && Input.GetKeyDown(KeyCode.Space))  
        {
            StartCoroutine(OpenDoorAndChangeScene()); 
    }

    IEnumerator OpenDoorAndChangeScene()
    {
        yield return new WaitForSeconds(0.5f); 

        SceneManager.LoadScene("Escena2");  
        Debug.Log("Cambio de escena realizado.");
    }
}
}
