using UnityEngine;

public class Corpsecollector : MonoBehaviour
{
    [SerializeField] GameObject[] DeadBody;
    GameObject enemyStartFight;
    string whoWin;

    public void CollectDeadObjects()
    {
        DeadBody = GameObject.FindGameObjectsWithTag("Dead");
    }

    public void CleareFightArea()
    {
        foreach (GameObject d in DeadBody)
        {
            Destroy(d);
        }
        CollectDeadObjects();
    }

    public void WhoStartFight(GameObject enemy)
    {
        enemyStartFight = enemy;
    }
    public void Winner(string w)
    {
        whoWin = w;
        if (whoWin == "player") Destroy(enemyStartFight);
    }

    public void SpecialForPlayer() 
    {
        Destroy(GameObject.FindGameObjectWithTag("npc"));
    }
}
