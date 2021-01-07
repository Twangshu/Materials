using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<Item> itemList = new List<Item>();



    //private void ParseItemJson()
    //{
    //    //itemList = new List<Item>();
    //    //文本在unity里面为TextAsset类型
    //    var itemText = ResManager.Instance.Load("Config/Items") as TextAsset;
    //    var itemsJson = itemText.text;//物品信息的Json格式
    //    var j = new JSONObject(itemsJson);
    //    foreach (var temp in j.list)
    //    {

    //        var id = (int)(temp["id"].n);
    //        var name = temp["name"].str;
    //        var quality = (int)temp["quality"].n;
    //        var description = temp["description"].str;
    //        var buyPrice = (int)(temp["price"].n);
    //        var weight = (int)(temp["weight"].n);
    //        var sprite = temp["sprite"].str;
    //        var type = (int)temp["type"].n;

    //        Item item = null;
    //        switch (type)
    //        {
    //            case 0:
    //                int atk = (int)temp["ATK"].n;
    //                var weaponType = (int)temp["weaponType"].n;
    //                item = new Weapon(id, name, type, quality, description, buyPrice, weight,sprite,weaponType,atk);
    //                break;
    //            case 1:
    //                var hp = (int)temp["hp"].n;
    //                var healthRate = (float)temp["healthRate"].f;
    //                item = new Consumable(id, name,type,quality, description, buyPrice,weight, sprite,healthRate);
    //                break;
    //            case 2:
    //                item = new MaterialItem(id, name, type, quality, description, buyPrice, weight, sprite);
    //                break;
    //            case 3:
    //                int ID1 = (int)temp["id1"].n;
    //                int ID2 = (int)temp["id2"].n;
    //                int ID3 = (int)temp["id3"].n;
    //                int spawnID = (int)temp["spawnID"].n;
    //                int amount1 = (int)temp["amount1"].n;
    //                int amount2 = (int)temp["amount2"].n;
    //                int amount3 = (int)temp["amount3"].n;
    //                item = new Recipe(id, name, type, quality, description, buyPrice,weight, sprite, ID1, ID2, ID3, amount1, amount2, amount3, spawnID);
    //                break;
    //            default:
    //                break;
    //        }
    //        itemList.Add(item);
    //    }
    //}

    public Item GetItemById(int id)
    {
        if (itemList.Count <= id)
        {
            Log.LogAssert("物品的ID越界,id为",id);
        }
        if (itemList[id] != null)
        {
            return itemList[id];
        }
        else
        {
            Log.LogAssert("未找到id为", id, "的物体");
            return null;
        }
        
    }

    //private void ParseNpcJson()
    //{
    //    var npcText = ResManager.Instance.Load("Config/NpcsConfig") as TextAsset;
    //    var npcJson = npcText.text;
    //    var j = new JSONObject(npcJson);
    //    foreach (var temp in j.list)
    //    {

    //        var id = (int)(temp["id"].n);
    //        var name = temp["name"].str;
    //        var dialogs = temp["dialogs"].str;
    //        var selections = temp["selections"].str;

    //        var npc = new NpcData(id,name,dialogs, selections);
    //        npcList.Add(npc);
    //    }
    //}

    //public NpcData GetNpcById(int id)
    //{
    //    if(npcList.Count<=id)
    //    {
    //        Log.LogAssert("npc的ID越界");
    //    }
    //    if (npcList[id] != null)
    //        return npcList[id];
    //    else
    //    {
    //        Log.LogAssert("未找到id为", id, "的npc");
    //        return null;
    //    }
    //}
}
