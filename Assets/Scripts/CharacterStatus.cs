﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    int maxHp = 100;
    int curHp = 100;
    public int Hp
    {
        get { return maxHp*1000 + curHp; }
        set
        {
            maxHp += value;
            curHp += value;
        }
    }
    public void HpUp(int plus)
    {
        maxHp += plus;
        curHp += plus;
    }

    int atk = 10;
    public int Atk {
        get { return atk; }
        set { atk += value; }
        }

    int def = 0;
    public int Def
    {
        get { return def; }
        set { def += value; }
    }

    int gold = 0;
    public int Gold
    {
        get { return gold; }
        set { gold += value; }
    }
    public bool PayGold(int value)
    {
        if(gold >= value)
        {
            gold -= value;
            return true;
        }

        return false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
