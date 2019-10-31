using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    Character myChar;

    int maxHp = 100;
    int curHp = 50;
    public int Hp
    {
        get { return maxHp*1000 + curHp; } // maxHp = Hp/1000, curHp = Hp%1000
        set
        {
            maxHp += value;
            curHp += value;
        }
    }
    public int MaxHp
    {
        get { return maxHp; }
    }
    public int CurHp
    {
        get { return curHp; }
    }
    public void HpHeal(int plus)
    {
        curHp += plus;
        if (curHp > maxHp)
            curHp = maxHp;
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

    int gold = 1000;
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
        myChar = GetComponent<Character>();
        FuncHelper.GetPlayerData(ref maxHp, ref curHp, ref atk, ref def, ref gold, Board.charCode);
    }

    void Update()
    {
        
    }

    public void StatusInitialize()
    {
        FuncHelper.SetPlayerData(50, 35, 10, 2, 0, Board.charCode);
        FuncHelper.GetPlayerData(ref maxHp, ref curHp, ref atk, ref def, ref gold, Board.charCode);
    }
}
