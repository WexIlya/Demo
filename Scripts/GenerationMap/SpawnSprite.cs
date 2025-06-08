using UnityEngine;
using System.Collections;

public class SpawnSprite : MonoBehaviour
{
    [SerializeField] GameObject ForestSprite;
    [SerializeField] GameObject MountainSprite;
    [SerializeField] GameObject LakeSprite;
    [SerializeField] GameObject FieldSprite;
    [SerializeField] GameObject FlowersSprite;
    [SerializeField] GameObject SandSprite;


    private void Update()
    {
        StartCoroutine(CoroutineAnotherCell());
    }

    public void SpawnForest()
    {
        Instantiate(ForestSprite, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SpawnMountain()
    {
        Instantiate(MountainSprite, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SpawnLake()
    {
        Instantiate(LakeSprite, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SpawnField()
    {
        Instantiate(FieldSprite, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SpawnFlowers()
    {
        Instantiate(FlowersSprite, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SpawnSand()
    {
        Instantiate(SandSprite, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator CoroutineAnotherCell()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnField();
        StopCoroutine(CoroutineAnotherCell());
    }
}
