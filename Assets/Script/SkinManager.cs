using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] Transform skinParent;
    private void Start()
    {
        var skinData = DatabaseManager.Instance.skinDatabase.GetSkinDataObject(DataManager.GetSelectedSkin());
        if(skinData != null) Instantiate(skinData.skinObj, skinParent);
    }
}
