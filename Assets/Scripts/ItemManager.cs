﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemManager : MonoBehaviour
{
    public Material itemMaterial;
    public Material nonItemMaterial;
    public GameObject itemParent;
    Image[] items = new Image[Character.itemNum];

    private void Awake()
    {
        //itemMaterial = new Material(Shader.Find("ItemFlicker"));
        items = itemParent.GetComponentsInChildren<Image>();
    }

    public void ItemOn(int itemNum)
    {
        if (itemNum >= items.Length || itemNum < 0)
            return;

        items[itemNum].material = itemMaterial;
        //itemMaterial.SetFloat("_haveItem", 1);
    }

    public void ItemOff(int itemNum)
    {
        if (itemNum >= items.Length || itemNum < 0)
            return;

        items[itemNum].material = nonItemMaterial;
        //itemMaterial.SetFloat("_haveItem", 0);
    }

    public void ItemUse(int num)
    {
        if (Board.myChar == null ||
            num < 0 || num > Character.itemNum)
            return;

        Board.myChar.Item((ItemName)num);
    }
}
