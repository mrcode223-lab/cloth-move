using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public bool isLockEscape;
    private static readonly Stack<BasePopUp> StBackObj = new Stack<BasePopUp>();

    protected virtual void Start()
    {
        Debug.Log($"Start Scene : <color=yellow>{gameObject.name}</color>");
    }
    
    protected virtual void Update()
    {
        if (isLockEscape)
            return;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ProcessBackBtn();
        }
    }

    protected virtual void ProcessBackBtn()
    {
        if (StBackObj.Count != 0)
        {
            if (StBackObj.Peek())
            {
                StBackObj.Peek().Close();
            }
        }
        else
        {
            OnBaseBack();
        }
    }
    
    public static void AddNewBackObj(BasePopUp obj)
    {
        StBackObj.Push(obj);
        SettingOderLayerPopup();
    }

    private static void SettingOderLayerPopup()
    {
        //todo
        //if (_backObjs == null && _backObjs.Count <= 0)
        //return;
        // BackObj[] lst_backObjs = _backObjs.ToArray();
        // int lenght = lst_backObjs.Length;
        // int index = 0;
        // for (int i = lenght - 1; i >= 0; i--)
        // {
        //     if (lst_backObjs[i].isPopup)
        //     {
        //         if (lst_backObjs[i].popupCanvas != null)
        //         {
        //             lst_backObjs[i].popupCanvas.sortingOrder = BASE_INDEX_LAYER + index;
        //             lst_backObjs[i].popupCanvas.planeDistance = 5;
        //             lst_backObjs[i].indexSortingLayerPopup = BASE_INDEX_LAYER + index;
        //             lst_backObjs[i].IDLayerIndexPopup = lst_backObjs[i].popupCanvas.sortingLayerID;
        //             if (lst_backObjs[i].actionChangeLayer != null)
        //                 lst_backObjs[i].actionChangeLayer();
        //             index++;
        //         }
        //     }
        // }
    }
    
    protected virtual void OnBaseBack()
    {
    }
}
