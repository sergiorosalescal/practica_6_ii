using UnityEngine;

// Cubo y esfera: cambian de color cuando se seleccionan con el rayo.
// El método cambiar_color() se llama desde el evento "Select Entered" del XR Grab Interactable.
public class xr_cambiar_color : MonoBehaviour
{
    private Renderer object_renderer;

    void Start()
    {
        object_renderer = GetComponent<Renderer>();
    }

    public void cambiar_color()
    {
        Debug.Log("Seleccionado: " + gameObject.name);
        object_renderer.material.color = Random.ColorHSV();
    }
}
