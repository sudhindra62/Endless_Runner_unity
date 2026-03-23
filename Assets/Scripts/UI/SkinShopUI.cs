using UnityEngine;
using UnityEngine.UI;


public class SkinShopUI : MonoBehaviour
{
    public GameObject skinButtonPrefab;
    public Transform skinButtonContainer;

    private SkinDatabase _skinDatabase;

    private void Start()
    {
        _skinDatabase = ServiceLocator.Get<SkinDatabase>();
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (Transform child in skinButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (SkinData skin in _skinDatabase.GetAllSkins())
        {
            GameObject skinButtonGO = Instantiate(skinButtonPrefab, skinButtonContainer);
            skinButtonGO.GetComponent<SkinButtonUI>().Setup(skin);
        }
    }
}
