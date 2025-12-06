using System.Collections.Generic;
using TMPro;
using UnityBase.DesignPattern;
using UnityEngine;
using UnityEngine.UI;

public class SkinViewManager : SingletonMonoBehavier<SkinViewManager>
{
    
    [Header("References")]
    [SerializeField] private Transform parentSkin;
    [SerializeField] private Transform slotParent;
    [SerializeField] private SkinSlot skinSlot;
    List<SkinSlot> skinSlots = new List<SkinSlot>();
    private GameObject currentSkinInstance;
    private int currentIndex = 0;
    private SkinDataObject currentData;

    private void Start()
    {
        foreach (var data in DatabaseManager.Instance.skinDatabase.skinDataObjects)
        {
            var slot = Instantiate(skinSlot, slotParent);
            slot.Init(data);
            skinSlots.Add(slot);
        }
        if(DataManager.GetSelectedSkin() != SkinType.None) ShowSkin(DataManager.GetSelectedSkin());
    }
    public void UpdateSlot(SkinSlot skinSlot)
    {
        foreach (var slot in skinSlots)
        {
            if(slot == skinSlot)
            {
                slot.SetActiveBorder(true);
            }   
            else
            {
                slot.SetActiveBorder(false);
            }    
        }
    }    
    public void NextSkin()
    {
        if (DatabaseManager.Instance.skinDatabase.skinDataObjects.Count == 0) return;

        currentIndex++;
        if (currentIndex >= DatabaseManager.Instance.skinDatabase.skinDataObjects.Count)
            currentIndex = 0;

        //ShowSkinByIndex(currentIndex);
    }

    public void PreviousSkin()
    {
        if (DatabaseManager.Instance.skinDatabase.skinDataObjects.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = DatabaseManager.Instance.skinDatabase.skinDataObjects.Count - 1;

        //ShowSkinByIndex(currentIndex);
    }

    public void ShowSkin(SkinType skinType)
    {
        if (currentSkinInstance != null)
            Destroy(currentSkinInstance);

        currentData = DatabaseManager.Instance.skinDatabase.GetSkinDataObject(skinType);
        currentSkinInstance = Instantiate(currentData.skinObj, parentSkin);
        //currentSkinInstance.transform.localRotation = Quaternion.identity;
        //UpdateButtonState();
    }
    public SkinType GetCurrentSkinType()
    {
        if (DatabaseManager.Instance.skinDatabase.skinDataObjects.Count == 0)
            return SkinType.Skin1;

        return DatabaseManager.Instance.skinDatabase.skinDataObjects[currentIndex].skinType;
    }
}
