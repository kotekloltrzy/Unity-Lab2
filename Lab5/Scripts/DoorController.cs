using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // Drzwi, które maj¹ siê podnosiæ
    public float openHeight = 5f; // Wysokoœæ, na któr¹ drzwi siê podnios¹
    public float speed = 2f; // Prêdkoœæ otwierania/zamykania

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    void Start()
    {
        // Pozycja zamkniêtych drzwi
        closedPosition = door.position;
        // Pozycja otwartych drzwi (przesuniêcie w osi Y)
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void Update()
    {
        // Interpolacja pozycji drzwi w zale¿noœci od stanu isOpening
        if (isOpening)
        {
            door.position = Vector3.Lerp(door.position, openPosition, speed * Time.deltaTime);
        }
        else
        {
            door.position = Vector3.Lerp(door.position, closedPosition, speed * Time.deltaTime);
        }
    }

    // Wykrywanie kolizji wejœcia do triggera
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = true;
        }
    }

    // Wykrywanie kolizji wyjœcia z triggera
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = false;
        }
    }
}