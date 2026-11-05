using UnityEngine;
using TMPro;

// Pistola: actualiza el texto de la UI al agarrarla y al soltarla.
// al_agarrar() se llama desde "Select Entered" y al_soltar() desde "Select Exited" del XR Grab Interactable.
public class xr_actualizar_ui_agarre : MonoBehaviour
{
    public TMP_Text texto_ui;
    public string nombre_objeto = "la pistola";

    private int veces_agarrado = 0;

    void Start()
    {
        mostrar_mensaje("Busca " + nombre_objeto);
    }

    public void al_agarrar()
    {
        veces_agarrado++;
        mostrar_mensaje("Has cogido " + nombre_objeto + "\nVeces: " + veces_agarrado);
    }

    public void al_soltar()
    {
        mostrar_mensaje("Has soltado " + nombre_objeto);
    }

    private void mostrar_mensaje(string mensaje)
    {
        Debug.Log(mensaje);
        if (texto_ui != null)
        {
            texto_ui.text = mensaje;
        }
    }
}
