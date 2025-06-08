using UnityEngine;

public class SpawnBuildings : MonoBehaviour
{
    [SerializeField] int roll;
    int index;
    static int AmountBuildings = 0;
    int MaxAmountBuildings = 4;
    [SerializeField] GameObject Town;
    [SerializeField] GameObject Tents;

    private void Update()
    {
        if (FindFirstObjectByType<StorageOfEmptyCells>().emptycells.Length == 0)
        {

            SpawnBuildings[] anothercell = FindObjectsByType<SpawnBuildings>(FindObjectsSortMode.None);
            index = Random.Range(0, anothercell.Length);
            if (AmountBuildings < MaxAmountBuildings)
            {
                anothercell[index].Rolling();
                if (anothercell[index].roll <= 3)
                {
                    anothercell[index].SpawnTents();
                    anothercell[index].gameObject.tag = "busy";
                }
                else
                {
                    anothercell[index].SpawnTown();
                    anothercell[index].gameObject.tag = "busy";
                }
            }
            EndOfSpawn();
        }
    }

    void Rolling()
    {
        roll = Random.Range(1,8);
    }

    void SpawnTown()
    {
        Instantiate(Town,transform.position,Quaternion.identity);
        AmountBuildings++;
    }

    void SpawnTents()
    {
        Instantiate(Tents, transform.position, Quaternion.identity);
        AmountBuildings++;
    }

    void EndOfSpawn()
    {
        if (AmountBuildings == MaxAmountBuildings)
        {
            this.enabled = false;
        }
    }
}