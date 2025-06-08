using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
        public float moveSpeed = 5f;
        public float cellSize = 1f;
        private bool isMoving = false;

    void Update()
        {
            if (!isMoving)
            {
                Vector3 direction = Vector3.zero;
                if (Input.GetKeyDown(KeyCode.W)) direction = Vector3.up;
                if (Input.GetKeyDown(KeyCode.S)) direction = Vector3.down;
                if (Input.GetKeyDown(KeyCode.A)) direction = Vector3.left;
                if (Input.GetKeyDown(KeyCode.D)) direction = Vector3.right;

                if (direction != Vector3.zero)
                {
                    Vector3 targetPos = transform.position + direction * cellSize;
                    StartCoroutine(MoveToCell(targetPos));
                }
            }
        }

    public void EndOfFight()
    {
        this.GetComponent<Collider2D>().enabled = true;
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].GetComponent<SpawnPlayerSquad>() != null)
            {
                spawners[i].GetComponent<SpawnPlayerSquad>().enabled = false;
                SpawnPlayerSquad.index = 0;
            }
            else
            {
                spawners[i].GetComponent<SpawnEnemySquad>().enabled = false;
                SpawnEnemySquad.index = 0;
            } 
        }
        FindAnyObjectByType<Corpsecollector>().CollectDeadObjects();
        FindAnyObjectByType<Corpsecollector>().CleareFightArea();
        FindAnyObjectByType<Corpsecollector>().SpecialForPlayer();
    }

        IEnumerator MoveToCell(Vector3 targetPos)
        {
            isMoving = true;
            Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, LayerMask.GetMask("Cell"));
            if (hit != null)
            {
                while (Vector3.Distance(transform.position, targetPos) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        targetPos,
                        moveSpeed * Time.deltaTime
                    );
                    yield return null;
                }
            }
            isMoving = false;
        }
}
