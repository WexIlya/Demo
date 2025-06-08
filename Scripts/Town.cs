using UnityEngine;

public class Town : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            collision.GetComponent<UI_Open>().GetBuilding(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            collision.GetComponent<UI_Open>().EraseBuildingObject();
        }
    }
}