```C#
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField, Tooltip("Materiał kolejnych pięter (cyklicznie)")]
    private Block[] floorPrefabs;
    [SerializeField] private Target targetPrefab;

    [SerializeField, Range(1, 8)] private int floors = 3;
    [SerializeField, Range(1f, 4f)] private float span = 2f;   // rozstaw osi filarów [m]
    [SerializeField] private Vector3 pillarSize = new Vector3(0.3f, 1.5f, 1f);
    [SerializeField, Range(0.1f, 0.6f)] private float beamHeight = 0.3f;

    void Start() => Build();

    [ContextMenu("Przebuduj wieżę")]
    public void Build()
    {
        Clear();
        float y = 0f;   // wysokość podłogi bieżącego piętra (lokalnie)

        for (int i = 0; i < floors; i++)
        {
            Block prefab = floorPrefabs[i % floorPrefabs.Length];

            // filary
            float py = y + pillarSize.y / 2f;
            SpawnBlock(prefab, new Vector3(-span / 2f, py, 0f), pillarSize, $"P{i}_Filar_L");
            SpawnBlock(prefab, new Vector3(span / 2f, py, 0f), pillarSize, $"P{i}_Filar_P");

            // TODO 3: postaw cel (targetPrefab) na środku piętra, tak aby LEŻAŁ na podłodze piętra.
            //         Nazwij go $"P{i}_Cel". Promień kulki = połowa jej skali.

            // belka (strop) na filarach

            SpawnBlock(targetPrefab, new Vector3(span / 1f, py, 0f), pillarSize, $"P{i}_Cel");

            y += pillarSize.y;
            Vector3 beamSize = new Vector3(span + pillarSize.x, beamHeight, pillarSize.z);
            SpawnBlock(prefab, new Vector3(0f, y + beamHeight / 2f, 0f), beamSize, $"P{i}_Belka");
            y += beamHeight;
        }
    }

    private void SpawnBlock(Block prefab, Vector3 localPos, Vector3 size, string blockName)
    {
        Block b = Instantiate(prefab, transform);
        b.name = blockName;
        b.transform.localPosition = localPos;
        b.transform.localRotation = Quaternion.identity;
        b.transform.localScale = size;
        b.UpdateMass();   // Awake() policzył masę jeszcze dla skali 1×1×1!
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);   // Destroy działa dopiero na końcu klatki
            Destroy(child);
        }
    }
}
```
