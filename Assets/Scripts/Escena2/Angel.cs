using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour
{
    public GameObject Trigger;
    public bool bossFight = false;
    private bool corrutinaActiva = false;
    private bool esperaInicialCompletada = false;
    public GameObject[] swordPrefabs;  // Array con los 3 prefabs de espadas

    private Trigger triggerScript;

    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Accede al script Trigger
        triggerScript = Trigger.GetComponent<Trigger>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bossFight = triggerScript.bossFight;
        if (bossFight && !corrutinaActiva)
        {
            // Si la pelea de boss ha comenzado, empezamos la secuencia
            StartCoroutine(BossFightSequence());
        }
    }

    // Corutina que maneja la secuencia de la pelea con el boss
    private IEnumerator BossFightSequence()
    {
        corrutinaActiva = true;
        // Esperar 3 segundos
        if (!esperaInicialCompletada)
        {
            yield return new WaitForSeconds(2.5f);
            esperaInicialCompletada = true;
        }

        // Instanciar el prefab aleatoriamente
        int randomIndex = Random.Range(0, swordPrefabs.Length);
        GameObject swordPattern = Instantiate(swordPrefabs[randomIndex], transform.position, Quaternion.identity);

        // Esperar 1 segundo después de instanciar el prefab
        yield return new WaitForSeconds(0.75f);

        anim.SetTrigger("Attack");
        // Aquí puedes agregar un Rigidbody a las espadas para que caigan
        Rigidbody2D[] swords = swordPattern.GetComponentsInChildren<Rigidbody2D>();

        // Hacer que las espadas caigan (activamos la gravedad en el Rigidbody)
        foreach (Rigidbody2D sword in swords)
        {
            sword.bodyType = RigidbodyType2D.Dynamic;
        }
        
        yield return new WaitForSeconds(2.5f);  // Espera que caigan completamente antes de terminar la secuencia
        Destroy(swordPattern);
        corrutinaActiva = false;
    }
}
