using UnityEngine;
using UnityEngine.Rendering;

public class Enemigo : MonoBehaviour
{

    public int velocidad;
    public GameObject jugador;

    public int distanciaPersecucion;

    public float distanciaAtaque = 5;

    public float daño = 10f;
    public float tiempoEntreAtaques = 1f;

    private float temporizadorAtaque;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugador = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jugador.transform.position);

        if(distancia < distanciaPersecucion)
        {
            //Solo rota en Y
            Vector3 objetivo = jugador.transform.position;
            objetivo.y = transform.position.y;
            transform.LookAt(objetivo);

            //Avanza si está más lejos que la distancia de ataque
            if (distancia > distanciaAtaque)
            {
                transform.position = Vector3.MoveTowards(transform.position, jugador.transform.position, velocidad * Time.deltaTime);
            }
            else
            {
                //-----Ataque-----
                temporizadorAtaque += Time.deltaTime;
                if (temporizadorAtaque >= tiempoEntreAtaques)
                {
                    VidaJugador vidaJugador = jugador.GetComponent<VidaJugador>();
                    if (vidaJugador != null)
                    {
                        vidaJugador.RecibirDaño(daño);
                    }
                    temporizadorAtaque = 0f;
                }
            }

                
        }
    }
}
