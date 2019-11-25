using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    public Material itemMaterial;

    private void Awake()
    {
        //itemMaterial = new Material(Shader.Find("ItemFlicker"));
    }

    public void ItemOn()
    {
        itemMaterial.SetFloat("_haveItem", 1);
    }

    public void ItemOff()
    {
        itemMaterial.SetFloat("_haveItem", 0);
    }

    public void ItemUse(int num)
    {
        if (Board.myChar == null ||
            num < 0 || num > Character.itemNum)
            return;

        Board.myChar.Item((ItemName)num);
    }
}
