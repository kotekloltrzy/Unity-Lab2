using System.Collections;
using UnityEngine;

public class LevelJudge : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private TowerBuilder tower;
    [SerializeField, Tooltip("Maks. czas czekania na uspokojenie sceny [s]")]
    private float maxWait = 10f;

    private bool judging;

    // GetComponentsInChildren pomija obiekty nieaktywne (patrz TowerBuilder.Clear)
    public int TargetsLeft => tower.GetComponentsInChildren<Target>().Length;

    void Update()
    {
        if (judging) return;
        if (TargetsLeft == 0 || launcher.ShotsLeft == 0)
            StartCoroutine(Judge());
    }

    private IEnumerator Judge()
    {
        judging = true;
        Debug.Log("Czekam, a¿ scena siê uspokoi...");

        float t = 0f;
        // TODO 7: czekaj klatka po klatce (yield return null), dopóki scena nie œpi
        //         i nie minê³o maxWait sekund
        while(!SceneAsleep() && t < maxWait)
        {
            yield return null;
            t += Time.deltaTime;
        }

        int left = TargetsLeft;
        if (left == 0)
            Debug.Log($"<color=yellow>WYGRANA! Gwiazdki: {1 + launcher.ShotsLeft}</color>");
        else
            Debug.Log($"<color=red>PRZEGRANA - zosta³o celów: {left}</color>");
    }

    private bool SceneAsleep()
    {
        foreach (Rigidbody rb in tower.GetComponentsInChildren<Rigidbody>())
            if (!rb.IsSleeping()) return false;

        // TODO 8: sprawdŸ te¿ ostatni pocisk - uwaga, launcher.LastProjectile mo¿e byæ null
        //         (jeszcze nie strzelano albo pocisk zosta³ ju¿ zniszczony)
        // Rigidbody projectileRb = launcher.LastProjectile != null ?

        return true;
    }

    [ContextMenu("TEST / Czy scena œpi?")]
    private void TestAsleep() => Debug.Log($"Scena œpi: {SceneAsleep()}, celów: {TargetsLeft}");

    [ContextMenu("TEST / Nowa runda")]
    private void NewRound()
    {
        StopAllCoroutines();
        judging = false;
        tower.Build();
        launcher.ResetShots();
    }
}