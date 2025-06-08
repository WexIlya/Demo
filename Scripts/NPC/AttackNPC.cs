using UnityEngine;

public class AttackNPC : MonoBehaviour
{
    [SerializeField] float reloading = 2f;
    [SerializeField] float currentReloading = 2f;
    [SerializeField] int damage;
    int chance;

    private void Update()
    {
        if (currentReloading < reloading)
        {
            currentReloading += 1 * Time.deltaTime;
        }
    }

    public void Attañk(GameObject enemy)
    {
        if (currentReloading >= reloading)
        {
            AimForTheTarget();
            if (chance >= 5)
            {
                damage += Random.Range(0, 4);
                enemy.GetComponent<EnemyHP>().TakeDamage(damage);
                currentReloading = 0;
            }
            else
            {
                Debug.Log("Ïðîìàõ!");
                currentReloading = 0;
            }
        }
    }

    public int AimForTheTarget()
    {
        chance = Random.Range(0,8);
        return chance;
    }
}
