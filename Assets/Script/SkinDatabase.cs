using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SkinDatabase", fileName = "SkinDatabase")]
public class SkinDatabase : ScriptableObject
{
    public List<SkinDataObject> skinDataObjects = new List<SkinDataObject>();

    public SkinDataObject GetSkinDataObject(SkinType skinType)
    {
        foreach (var item in skinDataObjects)
        {
            if(skinType == item.skinType)
            {
                return item;
            }    
        }
        return null;
    }    
}
[System.Serializable]
public class SkinDataObject
{
    public SkinType skinType;
    [PreviewField] public Sprite icon;
    public GameObject skinObj;
    public int price;
}
public enum SkinType
{
    None = -1,
    Skin1,
    Skin2,
    Skin3,
    Skin4,
    Skin5,
    Skin6,
    Skin7,
    Skin8,
    Skin9,
    Skin10,
    Skin11,
    Skin12,
    Skin13,
    Skin14,
    Skin15,
    Skin16,
    Skin17,
    Skin18,
    Skin19,
    Skin20
}