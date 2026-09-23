using UnityEngine;
using TMPro;
using System.Collections;

public class tutorialUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI texto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.SetActive(false);
    }

    public void MostrarMensaje(string mensaje)
    {
        panel.SetActive(true);
        texto.text = mensaje;
    }

    public void OcultarMensaje()
    {
        panel.SetActive(false);
    }
}
