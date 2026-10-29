using UnityEngine;

public class Target : Breakable
{
    [SerializeField, Tooltip("Poni¿ej tej wysokoœci cel uznajemy za str¹cony")]
    private float fallLimit = -3f;

    void FixedUpdate()
    {
        if (transform.position.y < fallLimit) Break();
    }

    protected override void OnBreak()
    {
        Debug.Log($"<color=lime>[{name}] CEL TRAFIONY</color>");
    }
}