﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    Character player;

    private void Awake()
    {
        player = GameObject.Find("PlayerOne").GetComponent<Character>();
    }

    public void ItemUse(int num)
    {
        if (num < 0 || num > Character.itemNum)
            return;

        player.Item((ItemName)num);
    }
}
