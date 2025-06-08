using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int MaxHP;
    [SerializeField] int HP;
    SpriteRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(int dmg)
    {
        if (HP >= 0)
        {
            HP -= dmg;
            StartCoroutine(Damage());
        }
        else Death();
    }

    public void Healing(int heal)
    {
        if (HP < MaxHP)
        {
            HP += heal;
        }
    }

    void Death()
    {
        if (HP <= 0)
        {
            spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 0.8f);
            this.gameObject.tag = "Dead";
        }
    }
    public int GiveHP()
    {
        return HP;
    }

    IEnumerator Damage()
    {
        spriteRenderer.color = new Color(1f,0.3f,0.3f,1f);
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        StopCoroutine(Damage());
    }
}
