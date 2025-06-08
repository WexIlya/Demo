using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f; 
    public float waitTime;           
    public float neighborSearchRadius = 1.5f; 
    public float cellSnapDistance = 0.1f; 

    private GameObject currentCell;       
    private bool isMoving = false;
    private LayerMask cellLayerMask;      


    void Start()
    {
        cellLayerMask = LayerMask.GetMask("Cell");
        FindCurrentCell();
        StartCoroutine(WanderRoutine());
    }

    void FindCurrentCell()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, cellSnapDistance, cellLayerMask);
        if (hits.Length > 0)
        {
            GameObject closestCell = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider2D hit in hits)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestCell = hit.gameObject;
                }
            }
            currentCell = closestCell;
        }
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            waitTime = Random.Range(1,5);
            yield return new WaitForSeconds(waitTime);

            if (!isMoving && currentCell != null)
            {
                List<GameObject> neighborCells = GetNeighborCells();
                if (neighborCells.Count > 0)
                {
                    GameObject targetCell = neighborCells[Random.Range(0, neighborCells.Count)];
                    StartCoroutine(MoveToCell(targetCell));
                }
            }
        }
    }

    List<GameObject> GetNeighborCells()
    {
        List<GameObject> neighbors = new List<GameObject>();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            currentCell.transform.position,
            neighborSearchRadius,
            cellLayerMask
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != currentCell && IsDirectNeighbor(hit.gameObject))
            {
                neighbors.Add(hit.gameObject);
            }
        }

        return neighbors;
    }

    bool IsDirectNeighbor(GameObject cell)
    {
        Vector2 dir = cell.transform.position - currentCell.transform.position;
        return Mathf.Abs(dir.x) < 1.1f && Mathf.Abs(dir.y) < 1.1f &&
               (Mathf.Abs(dir.x) < 0.1f || Mathf.Abs(dir.y) < 0.1f);
    }

    IEnumerator MoveToCell(GameObject targetCell)
    {
        isMoving = true;
        Vector2 targetPos = targetCell.transform.position;

        float direction = targetPos.x - transform.position.x;
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }

        while (Vector2.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        currentCell = targetCell;
        isMoving = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (currentCell != null)
        {
            Gizmos.DrawWireSphere(currentCell.transform.position, neighborSearchRadius);
        }

        Gizmos.color = Color.green;
        if (currentCell != null)
        {
            Gizmos.DrawWireCube(currentCell.transform.position, Vector3.one * 0.9f);
        }
    }
}