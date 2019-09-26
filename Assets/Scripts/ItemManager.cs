﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    Character player;
    Transform Item1;

    private void Awake()
    {
        player = GameObject.Find("PlayerOne").GetComponent<Character>();
        Item1 = transform.Find("Item1");
    }

    public void ItemOne()
    {
        player.Item1();
    }
}