using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerVR : MonoBehaviour
{
    [Header("Sterowanie")]
    [Tooltip("CenterEyeAnchor z Camera Rig – ruch jest liczony wzglêdem kierunku patrzenia")]
    [SerializeField] private Transform head;

    [Tooltip("Przyspieszenie kuli w m/s?")]
    [SerializeField] private float acceleration = 1.5f;

    [Tooltip("Maksymalna prêdkoœæ kuli w m/s")]
    [SerializeField] private float maxSpeed = 0.6f;

    [Header("Interfejs")]
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject winText;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 startLocalPosition;
    private int count;
    private int totalPickUps;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startLocalPosition = transform.localPosition;
    }

    private void Start()
    {
        totalPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;
        count = 0;
        winText.SetActive(false);
        UpdateCountText();
    }

    private void Update()
    {
        // Ga³ka lewego kontrolera (Primary = lewy kontroler Touch)
        moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Zabezpieczenie: kula spad³a z planszy ? wraca na start
        if (transform.localPosition.y < startLocalPosition.y - 0.5f)
            ResetBall();
    }

    private void FixedUpdate()
    {
        // "Przód" = kierunek patrzenia gracza rzutowany na poziom
        Vector3 forward = Vector3.ProjectOnPlane(head.forward, Vector3.up);

        // Gdy gracz patrzy prosto w dó³, forward jest bliski zeru – wtedy bierzemy "górê" g³owy
        if (forward.sqrMagnitude < 0.01f)
            forward = Vector3.ProjectOnPlane(head.up, Vector3.up);

        forward.Normalize();
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 direction = forward * moveInput.y + right * moveInput.x;

        rb.AddForce(direction * acceleration, ForceMode.Acceleration);

        // Ograniczenie prêdkoœci w poziomie
        Vector3 velocity = rb.linearVelocity;
        Vector3 flat = new(velocity.x, 0f, velocity.z);
        if (flat.magnitude > maxSpeed)
        {
            flat = flat.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(flat.x, velocity.y, flat.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PickUp")) return;

        other.gameObject.SetActive(false);
        count++;
        UpdateCountText();

        if (count >= totalPickUps)
            winText.SetActive(true);
    }

    private void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = startLocalPosition;
    }

    private void UpdateCountText()
    {
        countText.text = $"Zebrane: {count}/{totalPickUps}";
    }
}