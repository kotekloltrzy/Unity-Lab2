using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField, Tooltip("Materia³ kolejnych piêter (cyklicznie)")]
    private Block[] floorPrefabs;
    [SerializeField] private Target targetPrefab;

    [SerializeField, Range(1, 8)] private int floors = 3;
    [SerializeField, Range(1f, 4f)] private float span = 2f;   // rozstaw osi filarów [m]
    [SerializeField] private Vector3 pillarSize = new Vector3(0.3f, 1.5f, 1f);
    [SerializeField, Range(0.1f, 0.6f)] private float beamHeight = 0.3f;

    void Start() => Build();

    [ContextMenu("Przebuduj wie¿ê")]
    public void Build()
    {
        Clear();
        float y = 0f;   // wysokoœæ pod³ogi bie¿¹cego piêtra (lokalnie)

        for (int i = 0; i < floors; i++)
        {
            Block prefab = floorPrefabs[i % floorPrefabs.Length];

            // filary
            float py = y + pillarSize.y / 2f;
            SpawnBlock(prefab, new Vector3(-span / 2f, py, 0f), pillarSize, $"P{i}_Filar_L");
            SpawnBlock(prefab, new Vector3(span / 2f, py, 0f), pillarSize, $"P{i}_Filar_P");

            // TODO 3: postaw cel (targetPrefab) na œrodku piêtra, tak aby LE¯A£ na pod³odze piêtra.
            //         Nazwij go $"P{i}_Cel". Promieñ kulki = po³owa jej skali.

            // belka (strop) na filarach

            SpawnTarget(targetPrefab, new Vector3(0f, y, 0f),  $"P{i}_Cel");

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
        b.UpdateMass();   // Awake() policzy³ masê jeszcze dla skali 1×1×1!
    }

    private void SpawnTarget(Target prefab, Vector3 localPos, string targetName)
    {
        Target target = Instantiate(prefab, transform);
        target.name = targetName;
        target.transform.localRotation = Quaternion.identity;
        float targetRadius = target.transform.localScale.x / 2f;
        localPos.y += targetRadius;
        target.transform.localPosition = localPos;
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);   // Destroy dzia³a dopiero na koñcu klatki
            Destroy(child);
        }
    }
}