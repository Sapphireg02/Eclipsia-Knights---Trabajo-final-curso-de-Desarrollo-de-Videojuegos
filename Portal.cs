using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int enemigosRequeridos = 3;
    public tutorialUI cartelUI;

    public static int enemigosDerrotados = 0;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
            
        }

        satsuma jugadorScript = other.GetComponent<satsuma>();
        if (jugadorScript == null)
        {
            Debug.LogWarning("No se enncontro el script en el jugador");
            return;
        }
            

        if (enemigosDerrotados >= enemigosRequeridos)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            int enemigosFaltantes = enemigosRequeridos - enemigosDerrotados;
            jugadorScript.tutorialUI.MostrarMensaje($"Derrota a {enemigosFaltantes} enemigos más para abrir el portal");
        }
    }

}
