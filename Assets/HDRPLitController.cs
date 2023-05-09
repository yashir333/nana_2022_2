using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class HDRPLitController : MonoBehaviour
{
    public float emissionIntensity = 1.0f;
    public float emissionWeight = 1.0f;

    private MaterialPropertyBlock materialProperties;
    private Renderer renderer;

    private void Start()
    {
        // Create a new MaterialPropertyBlock to store the HDRP material properties
        materialProperties = new MaterialPropertyBlock();
        renderer = GetComponent<Renderer>();

        // Start the blink coroutine
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.75f);

            // Toggle the emission intensity
            emissionIntensity = (emissionIntensity == 0.1f) ? 5 : 0.1f;
        }
    }

    private void Update()
    {
        // Set the emission intensity and weight in the material properties
        materialProperties.SetColor("_EmissiveColor", new Color(emissionWeight, emissionWeight, emissionWeight, 1.0f) * emissionIntensity);

        // Update the material with the new material properties
        renderer.SetPropertyBlock(materialProperties);
    }

    private void OnDestroy()
    {
        // Release the material properties to prevent any potential memory leaks
        renderer.SetPropertyBlock(null);
    }
}
