using UnityEngine;
using UnityEngine.UI;

public class UI_Open : MonoBehaviour
{
    [SerializeField] Image BGVillage;
    [SerializeField] Image BGCamp;
    [SerializeField] RectTransform UI_Onnx;
    [SerializeField] RectTransform UI_API;
    [SerializeField] Closing_UI closingButton;
    [SerializeField] GameObject Building;

    private void Update()
    {
        while (Building != null && Input.GetKeyDown(KeyCode.E))
        {
            if (Building.gameObject.tag == "Town")
            {
                BGVillage.gameObject.SetActive(true);
                UI_API.gameObject.SetActive(true);
                closingButton = FindAnyObjectByType<Closing_UI>();
                closingButton.GetGameObjectUI(BGVillage, BGCamp, UI_API);
            }
            else
            {
                BGCamp.gameObject.SetActive(true);
                UI_Onnx.gameObject.SetActive(true);
                closingButton = FindAnyObjectByType<Closing_UI>();
                closingButton.GetGameObjectUI(BGVillage, BGCamp, UI_Onnx);
            }
            this.gameObject.SetActive(false);
        }
    }

    public void GetBuilding(GameObject building)
    {
        Building = building;
    }

    public void EraseBuildingObject()
    {
        Building = null;
    }
}
