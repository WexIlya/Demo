using Cinemachine;
using UnityEngine;

public class MovementInFight : MonoBehaviour
{
    float speed = 1.1f;
    [SerializeField] GameObject target;
    float MaxDistance = 1f;
    public float currentdistance;

    private void Start()
    {
        RefreshTarget();
        CheckDistabce();
    }

    private void FixedUpdate()
    {
        if (target != null && currentdistance > MaxDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
            CheckDistabce();
        }
        else if (currentdistance <= MaxDistance)
        {
            Debug.Log("Àòàêà!");
            this.GetComponent<AttackNPC>().Attañk(target);
        }
        if (target.GetComponent<EnemyHP>().GiveHP() <= 0)
        {
            RefreshTarget();
            if (target == null)
            {
                FindAnyObjectByType<Corpsecollector>().Winner("player");
                GameObject.FindGameObjectWithTag("FightCamera").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 9;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().EndOfFight();
                Destroy(this.gameObject);
            } 
        }
    }

    public void CheckDistabce()
    {
        currentdistance = Vector2.Distance(this.transform.position, target.transform.position);
    }

    void RefreshTarget()
    {
        target = GameObject.FindGameObjectWithTag("enemy");
    }
}