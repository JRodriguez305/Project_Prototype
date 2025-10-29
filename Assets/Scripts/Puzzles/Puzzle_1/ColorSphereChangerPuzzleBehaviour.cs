using UnityEngine;

public class ColorSphereChangerPuzzleBehaviour : MonoBehaviour
{
    public Material[] materials;
    public GameObject sphereSolution;
    public DoorSpawnerBehaviour doorSpawner;

    private Renderer sphereRenderer;
    private int matIndex = 0;

    private bool isSolved = false;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.material = materials[matIndex];
    }


    void OnMouseDown()
    {
        matIndex = (matIndex + 1) % materials.Length;
        sphereRenderer.material = materials[matIndex];

        CheckMatch();
    }

    void CheckMatch()
    {
        if (sphereSolution != null)
        {
            Renderer otherRenderer = sphereSolution.GetComponent<Renderer>();

            if ( otherRenderer.sharedMaterial == sphereRenderer.sharedMaterial)
            {
                Debug.Log("Puzzle Solved");
                isSolved = true;

                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;

                doorSpawner.SpawnDoor();
            }
        }
    }

    public Material GetCurrentMat()
    {
        return sphereRenderer.material;
    }
}
