using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 degreesPerSecond = new(15f, 30f, 45f);

    private void Update()
    {
        transform.Rotate(degreesPerSecond * Time.deltaTime);
    }
}