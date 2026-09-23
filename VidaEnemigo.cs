using UnityEngine;
using UnityEngine.UI;

public class VidaEnemigo : MonoBehaviour
{
    public float vidaMaxima = 50f;
    public float vidaActual;

    public Slider barraVida;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaActual = vidaMaxima;
        barraVida.maxValue = vidaMaxima;
        barraVida.value = vidaActual;
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0f, vidaMaxima);

        barraVida.value = vidaActual;

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    void Morir()
    {
        Portal.enemigosDerrotados++;
        Destroy(gameObject);
    }

}
