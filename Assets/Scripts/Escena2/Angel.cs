using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour
{
    public GameObject Trigger;
    public bool bossFight = false;
    public GameObject[] swordPrefabs;  // Array con los 3 prefabs de espadas

    private Trigger triggerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Accede al script Trigger
        triggerScript = Trigger.GetComponent<Trigger>();
    }

    // Update is called once per frame
    void Update()
    {
        bossFight = triggerScript.bossFight;
        if (bossFight)
        {
            Debug.Log("Bossfight esta a true (ANGEL)");
            // Si la pelea de boss ha comenzado, empezamos la secuencia
            StartCoroutine(BossFightSequence());
        }
    }

    // Corutina que maneja la secuencia de la pelea con el boss
    private IEnumerator BossFightSequence()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        Debug.Log("He esperado 3 segundos");

        // Instanciar el prefab aleatoriamente
        int randomIndex = Random.Range(0, swordPrefabs.Length);
        GameObject swordPattern = Instantiate(swordPrefabs[randomIndex], transform.position, Quaternion.identity);
        Debug.Log("He instanciado el prefab");

        // Esperar 1 segundo después de instanciar el prefab
        yield return new WaitForSeconds(1f);
        Debug.Log("He esperado 1 segundo");

        // Aquí puedes agregar un Rigidbody a las espadas para que caigan
        Rigidbody[] swords = swordPattern.GetComponentsInChildren<Rigidbody>();

        // Hacer que las espadas caigan (activamos la gravedad en el Rigidbody)
        foreach (Rigidbody sword in swords)
        {
            sword.isKinematic = false;  // Desactivamos el modo cinemático para permitir la caída
            sword.AddForce(Vector3.down * 10f, ForceMode.Impulse);  // Fuerza para que caigan al suelo
        }

        // Si quieres que las espadas "bajen" aún más después de caer, puedes agregar más lógica
        yield return new WaitForSeconds(2f);  // Espera que caigan completamente antes de terminar la secuencia
    }
}
