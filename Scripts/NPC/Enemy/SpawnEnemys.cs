using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    int MaxAmountEnemys = 7;

    int CurrentAmountEnemys = 0;

    GameObject[] CellsToSpawn;

    [SerializeField] GameObject[] EnemiesField;


    private void Update()
    {
        if (FindFirstObjectByType<SpawnBuildings>().enabled == false)
        {
            TakeCellToSpawn();
            if (EnemiesField.Length != 0)
            {
                SpawnEnemy();
            }
        }
    }

    public void TakeCellToSpawn() 
    {
            CellsToSpawn = GameObject.FindGameObjectsWithTag("free");
    }

    public void SpawnEnemy()
    {
        Vector2 playerPosition = FindFirstObjectByType<PlayerControl>().transform.position;

        while (CurrentAmountEnemys < MaxAmountEnemys)
        {
            int index = Random.Range(0, CellsToSpawn.Length);
            float Distance = Vector2.Distance(CellsToSpawn[index].transform.position, playerPosition);

            if (Distance >= 5f)
            {
                GameObject enemy = EnemiesField[Random.Range(0, EnemiesField.Length)];
                Instantiate(enemy, CellsToSpawn[index].transform.position, Quaternion.identity);
                CurrentAmountEnemys++;
                CellsToSpawn[index].tag = "busy";
            }
            else continue;
        }
    }
}
