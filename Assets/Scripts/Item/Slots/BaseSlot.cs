using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlot : MonoBehaviour
{
    public int count;
    public Item item;
    private float targetScale = 1f;
    private Button clickButton;


    private void Awake()
    {
        clickButton = GetComponent<Button>();
        clickButton.onClick.AddListener(() => EventCenter.Broadcast(EventDefine.ShowItemDetail, this));
    }

    /// <summary>
    /// 加载Item
    /// </summary>
    /// <param name="item"></param>
    public void Init(Item item, int count = 1)
    {
        this.item = item;
        this.count = count;
        UpdateUI();

    }

    public void SetIcon(object res)
    {

        transform.GetChild(0).GetComponent<Image>().sprite = Utils.Tex2Sprite(res as Texture2D);

    }

    public void AddAmount(int amount = 1)
    {
        this.count += amount;
        UpdateUI();
    }

    public void ReduceAmount(int amount = 1)
    {
        this.count -= amount;
        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        Tween tweener = transform.DOScale(new Vector3(targetScale, targetScale, targetScale), 0.3f);
        tweener.SetUpdate(true);
    }

    public int GetItemID()
    {
        return item.ID;
    }

    public void StoreItem(Item item, int count = 1)
    {
        AddAmount(count);
    }
}
