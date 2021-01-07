using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class pickItem : MonoBehaviour
{
    private SphereCollider s_collider;
    private Highlighter hobj;
    public Item itemData;
    public int id;

    private void Awake()
    {
        GetItemData();
        hobj = GetComponent<Highlighter>();
        s_collider = GetComponent<SphereCollider>();
    }



    private void GetItemData()
    {
        itemData = ItemManager.Instance.GetItemById(id);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals(Tags.Player))
        {
            hobj.ConstantOn(new Color(151, 255, 255));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(Tags.Player))
        {
            hobj.ConstantOff();
        }
    }

    private void BePicked()
    {
        BagManager.Instance.GetItem(id);
        s_collider.enabled = false;
        Destroy(gameObject);
    }
}
