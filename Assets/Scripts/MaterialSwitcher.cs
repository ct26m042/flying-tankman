using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    private Renderer targetRenderer;
    [SerializeField] private Material materialA;
    [SerializeField] private Material materialB;

    private bool usingA = true;

    private void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();
        SetMaterialA();
    }

    public void ToggleMaterial()
    {
        if (usingA) SetMaterialB();
        else SetMaterialA();
    }

    private void SetMaterialA()
    {
        targetRenderer.material = materialA;
        usingA = true;
    }

    private void SetMaterialB()
    {
        targetRenderer.material = materialB;
        usingA = false;
    }
}