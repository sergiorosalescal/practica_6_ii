using UnityEngine;

// Hace que una luz cambie de intensidad cada poco tiempo, como una vela.
public class parpadeo_luz : MonoBehaviour
{
    public float intensidad_minima = 0.5f;
    public float intensidad_maxima = 2.5f;
    public float tiempo_entre_cambios = 0.1f;

    private Light luz;
    private float contador = 0f;

    void Start()
    {
        luz = GetComponent<Light>();
    }

    void Update()
    {
        contador += Time.deltaTime;
        if (contador >= tiempo_entre_cambios)
        {
            luz.intensity = Random.Range(intensidad_minima, intensidad_maxima);
            contador = 0f;
        }
    }
}
