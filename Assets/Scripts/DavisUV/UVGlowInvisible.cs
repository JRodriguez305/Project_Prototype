using UnityEngine;

public class UVGlowInvisible : MonoBehaviour
{
    public Light uvFlashlight;
    public float raycastDistance = 20f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false; // start completely invisible
    }

    void Update()
    {
        if (uvFlashlight.enabled)
        {
            Ray ray = new Ray(uvFlashlight.transform.position, uvFlashlight.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance) && hit.transform == transform)
            {
                rend.enabled = true; // show object when hit
            }
            else
            {
                rend.enabled = false; // hide object when not hit
            }
        }
        else
        {
            rend.enabled = false; // flashlight off → hide
        }
    }
}

