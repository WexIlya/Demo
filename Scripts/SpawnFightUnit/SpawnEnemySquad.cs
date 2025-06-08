using UnityEngine;

public class SpawnEnemySquad : MonoBehaviour
{
    [SerializeField] GameObject[] Wolfs;
    [SerializeField] GameObject[] Goblins;
    static int pos = 0;
    [SerializeField] public static int index = 0;
    private void Update()
    {
        string who = WhoEnemy.TypeOfEnemy;

        if (index < 2)
        {
            switch (who)
            {
                case "Goblin":
                        GameObject enemy = Instantiate(Goblins[index], transform.position, Quaternion.identity);
                        enemy.transform.position = new Vector2(enemy.transform.position.x + pos, enemy.transform.position.y);
                        enemy.gameObject.tag = "enemy";
                        enemy.GetComponent<EnemyController>().enabled = false;
                        enemy.GetComponent<EnemyHP>().enabled = true;
                        enemy.GetComponent<EnemyMovement>().enabled = true;
                        enemy.GetComponent<EnemyAttack>().enabled = true;
                        pos++;
                        index++;
                    break;
                case "Wolf":
                        enemy = Instantiate(Wolfs[index], transform.position, Quaternion.identity);
                        enemy.transform.position = new Vector2(enemy.transform.position.x + pos, enemy.transform.position.y);
                        enemy.gameObject.tag = "enemy";
                        enemy.GetComponent<EnemyController>().enabled = false;
                        enemy.GetComponent<EnemyHP>().enabled = true;
                        enemy.GetComponent<EnemyMovement>().enabled = true;
                        enemy.GetComponent<EnemyAttack>().enabled = true;
                        pos++;
                        index++;
                    break;
            }
        }
    }
}
