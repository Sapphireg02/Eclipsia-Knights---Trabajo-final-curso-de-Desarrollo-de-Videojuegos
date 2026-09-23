using UnityEngine;

public class espada : MonoBehaviour
{
    public GameObject cartelUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cartelUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cartelUI.SetActive(true);
            other.GetComponent<satsuma>().espadaCercana = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cartelUI.SetActive(false);
            other.GetComponent<satsuma>().espadaCercana = null;
        }
    }

    public void destruirEspada()
    {
        Destroy(gameObject);
    }

}

