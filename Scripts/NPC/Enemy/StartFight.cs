using Cinemachine;
using UnityEngine;

public class StartFight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindAnyObjectByType<Corpsecollector>().WhoStartFight(this.gameObject);
            collision.GetComponent<Collider2D>().enabled = false;
            switch (this.gameObject.tag)
            {
                case "Goblin":
                    WhoEnemy.GetTypeOfEnemy(this.gameObject.tag);
                    GameObject.FindGameObjectWithTag("FightCamera").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
                    GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
                    for (int i = 0; i < spawners.Length; i++)
                    {
                        if(spawners[i].GetComponent<SpawnPlayerSquad>() != null) spawners[i].GetComponent<SpawnPlayerSquad>().enabled = true;
                        else spawners[i].GetComponent<SpawnEnemySquad>().enabled = true;
                    }
                    break;
                case "Wolf":
                    WhoEnemy.GetTypeOfEnemy(this.gameObject.tag);
                    GameObject.FindGameObjectWithTag("FightCamera").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
                    spawners = GameObject.FindGameObjectsWithTag("Spawner");
                    for (int i = 0; i < spawners.Length; i++)
                    {
                        if (spawners[i].GetComponent<SpawnPlayerSquad>() != null) spawners[i].GetComponent<SpawnPlayerSquad>().enabled = true;
                        else spawners[i].GetComponent<SpawnEnemySquad>().enabled = true;
                    }
                    break;
            }
        }
    }
}
