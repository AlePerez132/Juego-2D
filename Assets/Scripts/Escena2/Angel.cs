using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour
{
    public GameObject Trigger;
    public bool bossFight = false;
    private bool corrutinaActiva = false;
    private bool esperaInicialCompletada = false;
    private bool plataformasActivas = false;

    public GameObject[] swordPrefabs;  // Array con los 3 prefabs de espadas

    public GameObject plataformas;

    public Vector3 plataformasAbajo;
    public Vector3 plataformasArriba;
    public float duracionMovimiento = 4f;

    private Trigger triggerScript;
    private Animator anim;
    private int contador = 0;

    int vida = 30;
    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        plataformasAbajo = new Vector3(6.7f, -10f, 0f);
        plataformasArriba = new Vector3(6.7f, -2.3f, 0f);
        triggerScript = Trigger.GetComponent<Trigger>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bossFight = triggerScript.bossFight;

        if (bossFight && !corrutinaActiva && !plataformasActivas) // Condición adicional para evitar conflicto
        {
            StartCoroutine(BossFightSequence());
        }
    }

    private IEnumerator BossFightSequence()
    {
        corrutinaActiva = true;

        if (!esperaInicialCompletada)
        {
            yield return new WaitForSeconds(2.5f);
            esperaInicialCompletada = true;
        }

        int randomIndex = Random.Range(0, swordPrefabs.Length);
        GameObject swordPattern = Instantiate(swordPrefabs[randomIndex], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.75f);

        anim.SetTrigger("Attack");

        Rigidbody2D[] swords = swordPattern.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D sword in swords)
        {
            sword.bodyType = RigidbodyType2D.Dynamic;
            sword.gravityScale = 1.8f;
        }

        yield return new WaitForSeconds(2f);

        Destroy(swordPattern);
        contador++;
        if (contador == 4)
        {
            plataformasActivas = true; // Activamos el estado de las plataformas
            contador = 0;
            StartCoroutine(MoverPlataformas());
        }

        corrutinaActiva = false;
    }

    private IEnumerator MoverPlataformas()
    {
        plataformasActivas = true; // Activamos para indicar que las plataformas están en movimiento

        float duracion = duracionMovimiento;
        float elapsed = 0f;

        // Movimiento de subida
        while (elapsed < duracion)
        {
            float t = elapsed / duracion;
            plataformas.transform.position = Vector3.Lerp(plataformasAbajo, plataformasArriba, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        plataformas.transform.position = plataformasArriba;

        // Esperar 10 segundos arriba
        yield return new WaitForSeconds(10f);

        // Movimiento de bajada
        elapsed = 0f;
        while (elapsed < duracion)
        {
            float t = elapsed / duracion;
            plataformas.transform.position = Vector3.Lerp(plataformasArriba, plataformasAbajo, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        plataformas.transform.position = plataformasAbajo;

        plataformasActivas = false; // Desactivamos el estado de plataformas al finalizar el movimiento
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Morir();
        }
        else
        {
            //audioManager.reproducirEfecto(audioManager.angelRecibirDaño); 
            //aun no esta puesto este efecto de sonido
        }
    }

    void Morir()
    {
        anim.SetTrigger("Death");
        audioManager.reproducirEfecto(audioManager.angelMuerte);
        Destroy(gameObject, 0.8f);
    }
}
