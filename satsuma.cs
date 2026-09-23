using UnityEngine;
using System.Collections;

public class satsuma : MonoBehaviour
{
    public float velocidad = 5f;
    public Rigidbody rb;
    public Transform camara;

    public Transform puntoSuelo;
    public float radioSuelo = 0.3f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    static Animator anim;

    private bool isAttacking = false;

    public GameObject espadaMano;
    public espada espadaCercana;
    private bool espadaEquipada = false;

    public tutorialUI tutorialUI;

    public float dañoJugador = 20f;
    public float rangoAtaque = 2.5f;
    public LayerMask capaEnemigo;

    public ParticleSystem particulasAtaque;

    //Tutorial
    enum EstadoTutorial
    {
        Mover,
        Saltar,
        TomarEspada,
        MoverCamara,
        Atacar,
        Completo
    }

    EstadoTutorial estadoActual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim = GetComponent<Animator>();
        espadaMano.SetActive(false);

        estadoActual = EstadoTutorial.Mover;

        StartCoroutine(MostrarMensajeInicial());
        
    }

    // Update is called once per frame
    void Update()
    {
        enSuelo = Physics.CheckSphere(puntoSuelo.position, radioSuelo, capaSuelo);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        //Direccion relativa a la camara
        if (direccion.magnitude >= 0.1f && !isAttacking)
        {
            Vector3 forwardCam = camara.forward;
            Vector3 rightCam = camara.right;

            forwardCam.y = 0f;
            rightCam.y = 0f;

            Vector3 movimiento = forwardCam * vertical + rightCam * horizontal;

            transform.position += movimiento * velocidad * Time.deltaTime;

            //Rotacion de personaje hacia la direccion de movimiento
            Quaternion rotacion = Quaternion.LookRotation(movimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 10f);

            anim.SetBool("isWalk", true);

            if(estadoActual == EstadoTutorial.Mover)
            {
                estadoActual = EstadoTutorial.Saltar;
                tutorialUI.MostrarMensaje("Usa espacio para saltar");
            }
        }

        else
        {
            anim.SetBool("isWalk", false);
        }

        //-----SALTO-----

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            anim.SetTrigger("isJump");

            if (estadoActual == EstadoTutorial.Saltar)
            {
                estadoActual = EstadoTutorial.TomarEspada;
                tutorialUI.MostrarMensaje("Acercate a la espada y tomala con F");
            }
        }

        //-----ATAQUE-----

        if (Input.GetMouseButtonDown(0) && !isAttacking && espadaEquipada)
        {
            StartCoroutine(AttackCooldown());   

            if (estadoActual == EstadoTutorial.Atacar)
            {
                estadoActual = EstadoTutorial.Completo;
                StartCoroutine(TutorialCompleto());
            }

        }

        //-----EQUIPAR ESPADA-----

        if (espadaCercana != null && Input.GetKeyDown(KeyCode.F))
        {
            EquiparEspada();

            if(estadoActual == EstadoTutorial.TomarEspada)
            {
                estadoActual = EstadoTutorial.MoverCamara;
                tutorialUI.MostrarMensaje("Mueve el Mouse para rotar la cámara");
            }
        }

        //-----ROTACION CAMARA-----

        if (estadoActual == EstadoTutorial.MoverCamara)
        {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
            {
                estadoActual = EstadoTutorial.Atacar;
                tutorialUI.MostrarMensaje("Haz click izquierdo para atacar");
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(0.9f);

        HacerDaño();

        if (particulasAtaque != null)
        {
            particulasAtaque.Play();
        }

        yield return new WaitForSeconds(0.6f);
        isAttacking = false;
    }

    IEnumerator TutorialCompleto()
    {
        tutorialUI.MostrarMensaje("Derrota a los enemigos y sigue el camino");
        yield return new WaitForSeconds(10f);
        tutorialUI.OcultarMensaje();
    }

        void EquiparEspada()
    {
        espadaMano.SetActive(true);
        espadaEquipada = true;
        espadaCercana.cartelUI.SetActive(false);
        espadaCercana.destruirEspada();
        espadaCercana = null;
    }

    void HacerDaño()
    {
        Collider[] enemigosGolpeados = Physics.OverlapSphere(transform.position, rangoAtaque, capaEnemigo);

        foreach (Collider enemigo in enemigosGolpeados)
        {
            VidaEnemigo vidaEnemigo = enemigo.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDaño(dañoJugador);
            }
        }
    }

    IEnumerator MostrarMensajeInicial()
    {
        yield return null;
        tutorialUI.MostrarMensaje("Usa WASD para moverte");
    }
    
}
