using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour
{
    public GameObject Trigger;
    public bool bossFight = false;
    private bool corrutinaActiva = false;
    private bool esperaInicialCompletada = false;

    public GameObject[] swordPrefabs;  // Array con los 3 prefabs de espadas

    public GameObject plataformas;
    public Vector3 plataformasAbajo = new Vector3(6.7f, -10f, 10f);
    public Vector3 plataformasArriba = new Vector3(6.7f, -2.3f, 10f);
    public float duracionMovimiento = 4f;

    private Trigger triggerScript;
    private Animator anim;
    private int contador = 0;

    void Start()
    {
        triggerScript = Trigger.GetComponent<Trigger>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bossFight = triggerScript.bossFight;

        if (bossFight && !corrutinaActiva)
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
            sword.gravityScale = 1.3f;
        }

        yield return new WaitForSeconds(2.5f);

        Destroy(swordPattern);
        contador++;
        if (contador == 2)
        {
            contador = 0;
            StartCoroutine(MoverPlataformas());
        }

        corrutinaActiva = false;
    }

    private IEnumerator MoverPlataformas()
    {
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
    }
}
