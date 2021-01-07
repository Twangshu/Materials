using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IComparable
{
    public int ID;
    public string itemName;
    public string description;
    public int price;
    public int weight;
    public string icon;
    public int quality;//0普通1稀有2史诗3传说
    public int type;//0武器1消耗品2材料3配方

    public Item()
    {
        ID = -1;
    }

    public Item(int id, string name, int type, int quality, string des, int price, int weight, string sprite)
    {
        this.ID = id;
        this.itemName = name;
        this.type = type;
        this.quality = quality;
        this.description = des;
        this.price = price;
        this.weight = weight;
        this.icon = sprite;
    }

    public virtual void Use()
    {

    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if ((obj.GetType().Equals(this.GetType())) == false)
        {
            return false;
        }
        var temp = (Item)obj;

        return ID.Equals(temp.ID);

    }


    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }


    public virtual int CompareTo(object obj)
    {
        try
        {
            return ID > ((Item)obj).ID ? 1 : -1;
        }
        catch (System.Exception e)
        {
            Log.LogAssert(e);
            return 0;
        }
    }
}
