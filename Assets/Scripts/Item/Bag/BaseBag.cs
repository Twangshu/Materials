using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BaseBag : MonoBehaviour
{
    public int weight;
    public int bagType;//0武器1消耗品2材料3其他
    public List<BaseSlot> slotList = new List<BaseSlot>();
    public Transform wholeBag;
    public GameObject slot;
    public Button switchButton;
    private GridLayoutGroup layoutGroup;
    public virtual void Awake()
    {
        layoutGroup = wholeBag.GetComponent<GridLayoutGroup>();
        //Log.LogDebug(switchButton.transform.position, "....", switchButton.transform.localPosition, "....", switchButton.transform.GetComponent<RectTransform>().anchoredPosition) ;
        switchButton.onClick.AddListener(SwitchBag);
    }

    protected virtual void Start()
    {
        //slotList = GetComponentsInChildren<BaseSlot>();
    }

    public virtual void SwitchBag()
    {
        EventCenter.Broadcast(EventDefine.SelectBag, switchButton.transform);
        transform.SetAsLastSibling();
        //重置物品选择框
        BaseSlot tempSlot = null;
        EventCenter.Broadcast(EventDefine.ShowItemDetail, tempSlot);
    }
    public bool GetItem(int id,int count)
    {
        var item = ItemManager.Instance.GetItemById(id);
        return GetItem(item,count);
    }
    public bool GetItem(Item item,int count)
    {
        if (item == null)
        {
            Log.LogAssert("id对应物品不存在");
            return false;
        }
        if (!CheckBagWeight(item))
        {
            Log.LogHint("背包重量达到上限");
            return false;
        }
        if (item.type == 0)//武器这种一个格子只能有一件的物品
        {
            StoreNewItem(item,count);//新开一个物品槽
            return true;
        }
        else//可叠加
        {
            BaseSlot slot = FindSameIDSlot(item);
            if (slot != null)
            {
                slot.StoreItem(item,count);
                return true;
            }
            else
            {
                StoreNewItem(item);//新开一个物品槽
                GetItem(item, count - 1);
                return true;
            }
        }
    }

    protected virtual void StoreNewItem(Item item,int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            var newSlot = Instantiate(slot, wholeBag).GetComponent<BaseSlot>();
            newSlot.Init(item);
            slotList.Add(newSlot);
            UpdateBagWeight();
        }
    }

    private void UpdateBagWeight()
    {
        var rect = wholeBag.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ((slotList.Count -1) / 5 +1) * (layoutGroup.cellSize.y+layoutGroup.spacing.y) + layoutGroup.padding.top);
    }

    protected virtual bool CheckBagWeight(Item item)
    {
        if (GameData.currentWeight + item.weight <= GameData.maxWeight)
            return true;
        return false;
    }
    private BaseSlot FindSameIDSlot(Item item)
    {
        foreach (BaseSlot slot in slotList)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemID() == item.ID)
            {
                return slot;
            }
        }
        return null;
    }

    public virtual int CalculateTaskItemCount(int id)
    {
        int allCount = 0;
        foreach (var slot in slotList)
        {
            if(slot.item.ID.Equals(id))
            {
                allCount += slot.count;
            }
        }
        return allCount;
    }

    public virtual void OnEnable()
    {
        var datas = BagManager.Instance.GetBagData(bagType);
        var sortDatas = from objDic in datas.itemIdAndCount orderby objDic.Key descending select objDic;
        foreach (var data in sortDatas)
        {
            GetItem(data.Key, data.Value);
        }
        //当前是否为背包最高层
        if (transform.GetSiblingIndex().Equals(transform.parent.childCount - 1))
            SwitchBag();
    }

    public void OnDisable()
    {
        for (int i = 0; i < wholeBag.transform.childCount-1; i++)//加一是因为本来有一个预设体
        {
            Destroy(wholeBag.transform.GetChild(i+1).gameObject);
        }
        slotList.Clear();
    }
}
