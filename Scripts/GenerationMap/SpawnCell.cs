using UnityEngine;
using System.Collections;

public class SpawnCell : MonoBehaviour
{
    static int CurrentAmount = 0;
    [SerializeField] GameObject Uni_CellGrid;
    [SerializeField] GameObject Player;

    private void Start()
    {
        StartCoroutine(CoroutineGrid());
    }

    void Spawn()
    {
        if (CurrentAmount < 1)
        {
            CurrentAmount++;
            Instantiate(Uni_CellGrid, this.transform.position, Quaternion.identity);
        }
        else
        {
            StopCoroutine(CoroutineGrid());
            Destroy(this.gameObject);
        }
    }

    IEnumerator CoroutineGrid()
    {
        Spawn();
        FindFirstObjectByType<StorageOfEmptyCells>().FindAllEmptyCells();
        yield return new WaitForSeconds(2f);
    }
}
