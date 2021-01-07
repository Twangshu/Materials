using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BagData
{
    public Dictionary<Item, int> itemIdAndCount = new Dictionary<Item, int>();

    public void AddItem(Item item,int count=1)
    {
        if(itemIdAndCount.ContainsKey(item))
        {
            itemIdAndCount[item] += count;
        }
        else
        {
            itemIdAndCount.Add(item, count);
        }
    }

    public void DecreaseItem(Item item,int count=1)
    {
        if (itemIdAndCount.ContainsKey(item))
        {
            itemIdAndCount[item] -= count;
            if (itemIdAndCount[item] == 0)
                itemIdAndCount.Remove(item);
            else if(itemIdAndCount[item] < 0)
                Log.LogAssert("不应发生的物体数量减少情况");
        }
        else
        {
            Log.LogAssert("不应发生的物体数量减少情况");
        }
    }

}

/// <summary>
/// 用于重构后的背包系统
/// 11.24 cloudtian
/// </summary>
public class BagManager : Singleton<BagManager>
{
    public BagData weaponBagData = new BagData();
    public BagData consumaleBagData = new BagData();
    public BagData materialBagData = new BagData();
    public BagData otherBagData = new BagData();


    public void GetItem(int id, int count = 1)
    {
        var item = ItemManager.Instance.GetItemById(id);
        GetItem(item, count);
    }
    public void DecreaseItem(Item item,int count = 1)
    {
        Log.LogDebug("玩家失去了", count, "个", item.itemName);
        switch (item.type)
        {
            case 0://武器
                weaponBagData.DecreaseItem(item, count);
                break;
            case 1://消耗品
                consumaleBagData.DecreaseItem(item, count);
                break;
            case 2://材料
                materialBagData.DecreaseItem(item, count);
                break;
            case 3://其他
                otherBagData.DecreaseItem(item, count);
                break;
            default:
                break;
        }
    }

    public void GetItem(Item item, int count = 1)
    {
        EventCenter.Broadcast(EventDefine.ShowGetItemMessage, item, count);
        Log.LogDebug("玩家获得了", count, "个", item.itemName);
        switch (item.type)
        {
            case 0://武器
                weaponBagData.AddItem(item, count);
                break;
            case 1://消耗品
                consumaleBagData.AddItem(item, count);
                break;
            case 2://材料
                materialBagData.AddItem(item, count);
                break;
            case 3://其他
                otherBagData.AddItem(item, count);
                break;
            default:
                break;
        }
    }

    public BagData GetBagData(int type)
    {
        switch (type)
        {
            case 0://武器
                return weaponBagData;
            case 1://消耗品
                return consumaleBagData;
            case 2://材料
                return materialBagData;
            case 3://其他
                return otherBagData;
            default:
                break;
        }
        Log.LogAssert("未能找到的物品类型");
        return null;
    }

    public int GetItemCount(int id)
    {
        var item = ItemManager.Instance.GetItemById(id);
        int count;
        switch (item.type)
        {
            case 0://武器
                if(weaponBagData.itemIdAndCount.TryGetValue(item, out count))
                {
                    return count;
                }
                return 0;
            case 1://消耗品
                if (consumaleBagData.itemIdAndCount.TryGetValue(item, out count))
                {
                    return count;
                }
                return 0;
            case 2://材料
                if (materialBagData.itemIdAndCount.TryGetValue(item, out count))
                {
                    return count;
                }
                return 0;
            case 3://其他
                if (otherBagData.itemIdAndCount.TryGetValue(item, out count))
                {
                    return count;
                }
                return 0;
            default:
                return 0;
        }
    }

    
    
}
