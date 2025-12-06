using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSlot : MonoBehaviour
{
    [SerializeField] Image border;
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buttonSelect;
    [SerializeField] private Button buttonSelected;
    [SerializeField] private Button buttonBuy;
    SkinDataObject currentData;
    private void Start()
    {
        buttonSelect.onClick.AddListener(OnSelectClicked);
        buttonSelected.onClick.AddListener(OnSelectedClicked);
        buttonBuy.onClick.AddListener(OnBuyClicked);
        UnityBase.DesignPattern.Observer.Instance.AddObserver("SelectSkin", OnSelectSkin);
    }
    private void OnDestroy()
    {
        UnityBase.DesignPattern.Observer.Instance.RemoveObserver("SelectSkin", OnSelectSkin);
    }
    void OnSelectSkin(object data)
    {
        UpdateButtonState();
    }    
    public void Init(SkinDataObject skinDataObject)
    {
        currentData = skinDataObject;
        icon.sprite = skinDataObject.icon;
        if (priceText != null)
            priceText.text = currentData.price.ToString();
        buttonSelect.gameObject.SetActive(DataManager.IsSkinOwned(skinDataObject.skinType));
        buttonBuy.gameObject.SetActive(!DataManager.IsSkinOwned(skinDataObject.skinType));
        buttonSelected.gameObject.SetActive(skinDataObject.skinType == DataManager.GetSelectedSkin());

        buttonBuy.interactable = DataManager.Coin >= skinDataObject.price;

        if (DataManager.GetSelectedSkin() == skinDataObject.skinType)
        {
            SetActiveBorder(true);
        }    
        else
        {
            SetActiveBorder(false);
        }    
    }    
    private void OnSelectClicked()
    {
        DataManager.SetSelectedSkin(currentData.skinType);
        UpdateButtonState();
        SkinViewManager.Instance.UpdateSlot(this);
        UnityBase.DesignPattern.Observer.Instance.Notify("SelectSkin");
    }
    public void SetActiveBorder(bool activeBorder)
    {
        border.enabled = activeBorder;
    }    
    private void OnSelectedClicked()
    {
        // Không cần làm gì thêm, chỉ để hiện rõ trạng thái đang chọn
    }
    public void OnClickSlot()
    {
        SkinViewManager.Instance.ShowSkin(currentData.skinType);
        SkinViewManager.Instance.UpdateSlot(this);
    }    
    private void OnBuyClicked()
    {
        int playerCoins = DataManager.Coin;

        if (playerCoins >= currentData.price)
        {
            DataManager.Coin -= currentData.price;
            DataManager.AddOwnedSkin(currentData.skinType);
            // Sau khi mua xong tự động chọn skin đó
            DataManager.SetSelectedSkin(currentData.skinType);
            UnityBase.DesignPattern.Observer.Instance.Notify("SelectSkin");
            UpdateButtonState();
        }
        else
        {
            Debug.Log("Không đủ tiền để mua skin này!");
            // Có thể hiện popup báo "Not enough coins" ở đây
        }
        SkinViewManager.Instance.UpdateSlot(this);
    }
    private void UpdateButtonState()
    {
        bool isOwned = DataManager.IsSkinOwned(currentData.skinType);
        bool isSelected = DataManager.GetSelectedSkin() == currentData.skinType;
        buttonBuy.interactable = currentData.price <= DataManager.Coin;
        // Tắt hết trước
        buttonSelect.gameObject.SetActive(false);
        buttonSelected.gameObject.SetActive(false);
        buttonBuy.gameObject.SetActive(false);

        if (isOwned)
        {
            if (isSelected)
            {
                buttonSelected.gameObject.SetActive(true);
            }
            else
            {
                buttonSelect.gameObject.SetActive(true);
            }
        }
        else
        {
            buttonBuy.gameObject.SetActive(true);
            if (priceText != null)
                priceText.text = currentData.price.ToString();
        }
    }
}
