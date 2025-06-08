using UnityEngine;

public class ChooseCell : MonoBehaviour
{
    [SerializeField] GameObject[] storage;
    float MinDistance = 2.0f;
    int MaxAmountSpecialCell = 5;
    int CurrentAmountSpecialCell = 0;

    private void Start()
    {
        transform.position = FindFirstObjectByType<SpawnCell>().transform.position;
    }

    private void Update()
    {
        ChooseEmptyCellForSpawnSprite();
    }

    void ChooseEmptyCellForSpawnSprite()
    {
        int index;
        storage = FindFirstObjectByType<StorageOfEmptyCells>().emptycells;
        while (CurrentAmountSpecialCell < MaxAmountSpecialCell)
        {
            GameObject choosenCell = storage[Random.Range(0, storage.Length)];
            float distance = Vector2.Distance(transform.position, choosenCell.transform.position);
            index = Random.Range(1, 4);

            if (distance > MinDistance)
            {

                switch (index)
                {
                    case 1:
                        choosenCell.GetComponent<SpawnSprite>().SpawnForest();
                        break;
                    case 2:
                        choosenCell.GetComponent<SpawnSprite>().SpawnMountain();
                        break;
                    case 3:
                        choosenCell.GetComponent<SpawnSprite>().SpawnLake();
                        break;
                }
                storage = FindFirstObjectByType<StorageOfEmptyCells>().emptycells;
                CurrentAmountSpecialCell++;
            }

            else continue;
        }
        FindFirstObjectByType<StorageOfEmptyCells>().FindAllEmptyCells();
        FindFirstObjectByType<Environment>().isEndOfSpawn();
    }
}
