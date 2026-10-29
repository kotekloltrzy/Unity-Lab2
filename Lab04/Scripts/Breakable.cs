using UnityEngine;

// Wspólna klasa bazowa: wszystko, co mo¿e pêkn¹æ od uderzenia.
[RequireComponent(typeof(Rigidbody))]
public class Breakable : MonoBehaviour
{
    [SerializeField, Tooltip("Impuls [N·s], powy¿ej którego obiekt pêka")]
    private float strength = 6f;

    [SerializeField, Tooltip("Czas [s] po pojawieniu siê, w którym uderzenia s¹ ignorowane")]
    private float graceTime = 0.5f;

    protected Rigidbody rb;
    private float spawnTime;
    private bool broken;

    public bool IsBroken => broken;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.time;
    }

    void OnCollisionEnter(Collision c)
    {
        // Wie¿a "osiada" chwilê po zbudowaniu - te kontakty pomijamy
        if (Time.time - spawnTime < graceTime) return;

        float j = c.impulse.magnitude;   // [N·s]
        // TODO 2: jeœli j > strength:
        //         - wypisz Debug.Log: kto pêk³, od czego (c.gameObject.name) i jaki by³ impuls,
        //         - wywo³aj Break()
        if (j > strength)
        {
            Debug.Log(c.gameObject.name);
            Break();
        }
    }

    public void Break()
    {
        if (broken) return;   // kilka kontaktów w jednym kroku = kilka wywo³añ!
        broken = true;
        OnBreak();
        Destroy(gameObject);
    }

    // Miejsce na reakcjê klas pochodnych (log, punkty, od³amki...)
    protected virtual void OnBreak() { }

    [ContextMenu("TEST / Pêknij")]
    private void TestBreak() => Break();
}