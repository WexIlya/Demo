using UnityEngine;
using UnityEngine.UI;

public class Closing_UI : MonoBehaviour
{
    [SerializeField] Image VillageUI;
    [SerializeField] Image CampUI;
    [SerializeField] RectTransform UI;
    [SerializeField] UI_Open Player;

    public void GetGameObjectUI(Image BG_V, Image BG_C, RectTransform ui)
    {
        VillageUI = BG_V;
        CampUI = BG_C;
        UI = ui;
    }

    public void ClosingUIWithNPC()
    {
        VillageUI.gameObject.SetActive(false);
        CampUI.gameObject.SetActive(false);
        UI.gameObject.SetActive(false);
        Player.gameObject.SetActive(true);
    }
}
