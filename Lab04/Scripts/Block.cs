using UnityEngine;

public class Block : Breakable
{
    [SerializeField, Tooltip("Umowna gêstoœæ: masa = gêstoœæ × objêtoœæ")]
    private float density = 2f;

    protected override void Awake()
    {
        base.Awake();
        UpdateMass();
    }

    // Wywo³aj po ka¿dej zmianie skali klocka!
    public void UpdateMass()
    {
        Vector3 s = transform.lossyScale;
        // TODO 1: rb.mass = gêstoœæ × objêtoœæ prostopad³oœcianu
        //         (Cube ma wymiary 1×1×1, wiêc objêtoœæ = s.x · s.y · s.z)
        rb.mass = density * s.x * s.y * s.z;
    }
}