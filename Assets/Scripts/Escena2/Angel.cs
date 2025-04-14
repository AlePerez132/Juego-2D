using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour
{
    public GameObject vallas;
    private Vector3 posicionObjetivoVallas;
    public GameObject escaleras;
    private Vector3 posicionObjetivoEscaleras;

    public CameraScript camara;

    public GameObject Trigger;
    public bool bossFight = false;
    private bool corrutinaActiva = false;
    private bool esperaInicialCompletada = false;
    private bool plataformasActivas = false;
    private bool muerto = false;

    public GameObject[] swordPrefabs;

    public GameObject plataformas;

    public Vector3 plataformasAbajo;
    public Vector3 plataformasArriba;
    public float duracionMovimiento = 4f;

    private Trigger triggerScript;
    private Animator anim;
    private int contador = 0;

    public int vida;
    AudioManager audioManager;

    private bool procesoMuerteActivo = false;
    private bool corrutinaVallasTerminada = false;
    private bool corrutinaEscalerasTerminada = false;

    void Start()
    {
        posicionObjetivoVallas = new Vector3(0, -4, 10f);
        posicionObjetivoEscaleras = new Vector3(0, 0, 0f);
        vida = 30;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        plataformasAbajo = new Vector3(6.7f, -10f, 0f);
        plataformasArriba = new Vector3(6.7f, -2.3f, 0f);
        triggerScript = Trigger.GetComponent<Trigger>();
        anim = GetComponent<Animator>();

        Rigidbody2D rbVallas = vallas.GetComponent<Rigidbody2D>();
        if (rbVallas != null)
            rbVallas.bodyType = RigidbodyType2D.Kinematic;

        Rigidbody2D rbEscaleras = escaleras.GetComponent<Rigidbody2D>();
        if (rbEscaleras != null)
            rbEscaleras.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        bossFight = triggerScript.bossFight;

        if (bossFight && !corrutinaActiva && !plataformasActivas && !muerto)
        {
            StartCoroutine(BossFightSequence());
        }
        else if (muerto && !procesoMuerteActivo)
        {
            PostMuerte();
            procesoMuerteActivo = true;
        }

        if (corrutinaVallasTerminada && corrutinaEscalerasTerminada)
        {
            Destroy(gameObject, 0.8f);
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
        GameObject swordPattern = Instantiate(swordPrefabs[randomIndex], new Vector3(24.6f, -1, -1), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        anim.SetTrigger("Attack");

        Rigidbody2D[] swords = swordPattern.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D sword in swords)
        {
            sword.gravityScale = 2.5f;
        }

        yield return new WaitForSeconds(2f);

        Destroy(swordPattern);
        contador++;
        if (contador == 4)
        {
            plataformasActivas = true;
            contador = 0;
            StartCoroutine(MoverPlataformas());
        }

        corrutinaActiva = false;
    }

    private IEnumerator MoverPlataformas()
    {
        plataformasActivas = true;

        float duracion = duracionMovimiento;
        float elapsed = 0f;

        // Subida
        while (elapsed < duracion)
        {
            float t = elapsed / duracion;
            plataformas.transform.position = Vector3.Lerp(plataformasAbajo, plataformasArriba, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        plataformas.transform.position = plataformasArriba;

        yield return new WaitForSeconds(6);

        // Bajada
        elapsed = 0f;
        while (elapsed < duracion)
        {
            float t = elapsed / duracion;
            plataformas.transform.position = Vector3.Lerp(plataformasArriba, plataformasAbajo, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        plataformas.transform.position = plataformasAbajo;

        plataformasActivas = false;
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
            //audioManager.reproducirEfecto(audioManager.angelRecibirDanio); 
        }
    }

    void Morir()
    {
        muerto = true;
        anim.SetTrigger("Death");
        audioManager.reproducirEfecto(audioManager.angelMuerte);
    }

    public void PostMuerte()
    {
        StartCoroutine(MoverObjeto(vallas, vallas.transform.position, posicionObjetivoVallas, 0.5f));
        StartCoroutine(MoverObjeto(escaleras, escaleras.transform.position, posicionObjetivoEscaleras, 0.5f));

        if (camara != null)
        {
            camara.AjustarAlturaYZoom(0f, 0f, 0.5f);
        }
        else
        {
            Debug.LogWarning("Camara no asignada en el script Angel.");
        }
    }

    IEnumerator MoverObjeto(GameObject objeto, Vector3 inicio, Vector3 destino, float duracion)
    {
        float tiempo = 0f;
        Rigidbody2D rb = objeto.GetComponent<Rigidbody2D>();

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            Vector3 nuevaPos = Vector3.Lerp(inicio, destino, tiempo / duracion);
            nuevaPos.z = destino.z; // Asegura que se actualice la Z

            if (rb != null && rb.bodyType == RigidbodyType2D.Kinematic)
                rb.MovePosition(nuevaPos);
            else
                objeto.transform.position = nuevaPos;

            yield return null;
        }

        // Asegura que la posición final sea exacta
        if (rb != null && rb.bodyType == RigidbodyType2D.Kinematic)
            rb.MovePosition(destino);
        else
            objeto.transform.position = destino;

        if (objeto == vallas)
            corrutinaVallasTerminada = true;
        else if (objeto == escaleras)
            corrutinaEscalerasTerminada = true;
    }
}
