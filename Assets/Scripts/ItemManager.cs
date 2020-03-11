using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemManager : MonoBehaviour
{
    public Material itemMaterial;
    public Material nonItemMaterial;
    Image[] items = new Image[Character.itemNum];

    private void Awake()
    {
        items = GetComponentsInChildren<Image>();

        for (int i = 0; i < Character.itemHave.Length; i++) {
            ItemOff(i);
            if (Character.itemHave[i])
                ItemOn(i);
        }
    }

    public void ItemOn(int itemNum)
    {
        if (itemNum >= items.Length || itemNum < 0)
            return;

        items[itemNum].material = itemMaterial;
    }

    public void ItemOff(int itemNum)
    {
        if (itemNum >= items.Length || itemNum < 0)
            return;

        items[itemNum].material = nonItemMaterial;
    }

    public void ItemUse(int num)
    {
        if (Board.myChar == null || num < 0 || num > Character.itemNum)
            return;

        Board.myChar.Item((ItemName)num);
    }
}
