using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour
{
    float Distance = 1.5f;
    public bool spawned = false;
    static bool EndspawnSpecialCell = false;

    private void Update()
    {
        if (EndspawnSpecialCell)
        {
            StartCoroutine(CoroutineEnvironment());
        }
    }
    void SpawnEnvironment() 
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Distance);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("EmptyCell"))
            {
                switch (this.gameObject.tag)
                {
                    case "Forest":
                        hit.GetComponent<SpawnSprite>().SpawnField();
                        break;
                    case "Mountain":
                        hit.GetComponent<SpawnSprite>().SpawnSand();
                        break;
                    case "Lake":
                        hit.GetComponent<SpawnSprite>().SpawnFlowers();
                        break;
                }
            }
        }

        spawned = true;

        StopCoroutine(CoroutineEnvironment());
        FindFirstObjectByType<StorageOfEmptyCells>().FindAllEmptyCells();
    }

    public void isEndOfSpawn() 
    {
        EndspawnSpecialCell = true;
    }

    IEnumerator CoroutineEnvironment()
    {
        switch (this.gameObject.tag)
        {
            case "Forest":
                yield return new WaitForSeconds(0.3f);
                SpawnEnvironment();
                break;
            case "Mountain":
                yield return new WaitForSeconds(0.2f);
                SpawnEnvironment();
                break;
            case "Lake":
                yield return new WaitForSeconds(0.1f);
                SpawnEnvironment();
                break;
        }
    }
}
