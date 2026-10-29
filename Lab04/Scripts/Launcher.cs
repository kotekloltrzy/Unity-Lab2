using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody projectilePrefab;
    [SerializeField, Tooltip("Punkt wylotu - dziecko wyrzutni")]
    private Transform muzzle;

    [Header("Celowanie")]
    [SerializeField, Range(0f, 80f)] private float pitch = 20f;      // k¹t w pionie [°]
    [SerializeField, Range(-45f, 45f)] private float yaw = 0f;       // k¹t w poziomie [°]
    [SerializeField, Range(1f, 300f)] private float impulse = 40f;   // [N·s]
    [SerializeField] private float aimSpeed = 30f;                   // [°/s]
    [SerializeField] private float powerSpeed = 20f;                 // [N·s na sekundê]

    [Header("Strza³y")]
    [SerializeField, Range(1, 20)] private int maxShots = 3;
    [SerializeField] private float projectileLifetime = 15f;

    [Header("Trajektoria")]
    [SerializeField, Range(10, 100)] private int trajectoryPoints = 40;
    [SerializeField] private float trajectoryStep = 0.05f;           // [s] miêdzy punktami

    private LineRenderer line;

    public int ShotsLeft { get; private set; }
    public Rigidbody LastProjectile { get; private set; }

    void Awake() => line = GetComponent<LineRenderer>();

    void Start() => ResetShots();

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        // TODO 4: strza³ki ?/? zmieniaj¹ yaw, ?/? zmieniaj¹ pitch (aimSpeed · Time.deltaTime),
        //         klawisze W/S zmieniaj¹ impulse (powerSpeed · Time.deltaTime).
        //         Ogranicz wartoœci do zakresów z [Range] (Mathf.Clamp).
        //         PodpowiedŸ: kb.leftArrowKey.isPressed

        float aimDelta = aimSpeed * Time.deltaTime;
        float powerDelta = powerSpeed * Time.deltaTime;

        if (kb.leftArrowKey.isPressed) yaw -= aimDelta;
        if (kb.rightArrowKey.isPressed) yaw += aimDelta;
        if (kb.upArrowKey.isPressed) pitch += aimDelta;
        if (kb.downArrowKey.isPressed) pitch -= aimDelta;
        if (kb.wKey.isPressed) impulse += powerDelta;
        if (kb.sKey.isPressed) impulse -= powerDelta;

        yaw = Mathf.Clamp(yaw, -45f, 45f);
        pitch = Mathf.Clamp(pitch, 0f, 80f);
        impulse = Mathf.Clamp(impulse, 1f, 300f);


        transform.rotation = Quaternion.Euler(-pitch, yaw, 0f);   // minus: dodatni k¹t wokó³ X pochyla w dó³
        DrawTrajectory();

        if (kb.spaceKey.wasPressedThisFrame) Fire();
    }

    [ContextMenu("TEST / Strza³")]
    public void Fire()
    {
        if (ShotsLeft <= 0) { Debug.Log("Brak strza³ów"); return; }
        ShotsLeft--;

        Rigidbody p = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        // TODO 5: nadaj pociskowi impuls o wartoœci impulse wzd³u¿ muzzle.forward
        float masa = p.mass;
        p.AddForce(muzzle.forward * impulse, ForceMode.Impulse);
        Destroy(p.gameObject, projectileLifetime);
        LastProjectile = p;

        Debug.Log($"Masa: {masa}kg, Strza³: J = {impulse:0} N·s, v0 = {impulse / p.mass:0.0} m/s, zosta³o {ShotsLeft}");
    }

    public void ResetShots() => ShotsLeft = maxShots;

    private void DrawTrajectory()
    {
        Vector3 p0 = muzzle.position;
        Vector3 v0 = muzzle.forward * (impulse / projectilePrefab.mass);   // ?v = J / m
        Vector3 g = Physics.gravity;

        line.positionCount = trajectoryPoints;
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * trajectoryStep;
            // TODO 6: po³o¿enie w rzucie ukoœnym: p(t) = p0 + v0·t + ?·g·t?
            Vector3 p = p0 + v0*t + 0.5f*g * t * t;
            line.SetPosition(i, p);
        }
    }
}