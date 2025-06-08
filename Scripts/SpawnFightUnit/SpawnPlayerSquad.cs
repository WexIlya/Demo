using UnityEngine;

public class SpawnPlayerSquad : MonoBehaviour
{
    [SerializeField] GameObject[] Squadmembers;
    [SerializeField] public static int index = 0;

    void Update()
    {
        if (index < Squadmembers.Length)
        {
            if (Squadmembers[index].gameObject.tag == "Player")
            {
                Squadmembers[index].GetComponent<UI_Open>().enabled = false;
                Squadmembers[index].GetComponent<PlayerControl>().enabled = false;
                Squadmembers[index].GetComponent<MovementInFight>().enabled = true;
            }
            Squadmembers[index].gameObject.tag = "npc";
            Squadmembers[index].GetComponent<MovementInFight>().enabled = true;
            Instantiate(Squadmembers[index], transform.position, Quaternion.identity);
            index++;
        }
    }
}
