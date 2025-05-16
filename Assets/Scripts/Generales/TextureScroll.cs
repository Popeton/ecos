using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    [SerializeField] private Material material; //  material a mover.
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.5f, 0.0f); // Velocidad en X e Y.

    void Update()
    {
        if (material != null)
        {
            Vector2 offset = Time.time * scrollSpeed;
            material.mainTextureOffset = offset; // Cambia las coordenadas UV.
        }
    }
}

