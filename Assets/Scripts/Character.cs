﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemName
{
    DiceAdd,
    DiceUpgrade,    
    HpPotion,
    EnemyBack,
    num
}

enum LandType
{
    Ground,
    Lake,
    Desert, // == Water
    Clay, // == Iron, Coal
    Stone,
    Gold,
    Goal
}

public class Character : MonoBehaviour
{
    Dictionary<Vector3, LandType> places = new Dictionary<Vector3, LandType>();
    public Transform landSide;
    public static int curPlace = 0;
    bool atDesert = false;

    CharacterStatus status;

    public Character enemy;
    CharacterStatus enemyStat;

    Transform statusText;
    Text HpText;
    Text AtkText;
    Text DefText;
    Text GoldText;

    public const int itemNum = (int)ItemName.num;
    int[] itemValue = new int[itemNum] { 3, 4, 5, 6 };
    bool[] itemOn = new bool[itemNum];

    void Start()
    {
        for (int i = 1; i <= 28; i++)
        {
            Transform tempTr = landSide.Find("HexTile_" + i).GetComponent<Transform>();
            LandType tempLand = (LandType)Enum.Parse(typeof(LandType), tempTr.tag);
            Vector3 pos = landSide.rotation * tempTr.localPosition;
            places.Add(pos, tempLand);
        }
        places.Add(new Vector3(0, 0, 0), LandType.Goal);

        status = GetComponent<CharacterStatus>();
        enemyStat = enemy.transform.GetComponent<CharacterStatus>();

        statusText = GameObject.Find("Status").GetComponent<Transform>();
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();
        GoldText = GameObject.Find("GoldText").GetComponent<Text>();
    }

    void Update()
    {
        //transform.position = place[curPlace];
        transform.position = places.ElementAt(curPlace).Key;
    }

    private void OnGUI()
    {
        HpText.text = string.Format("HP: {0} / {1}", status.Hp % 1000, status.Hp / 1000);
        AtkText.text = "ATK: " + status.Atk;
        DefText.text = "DEF: " + status.Def;
        GoldText.text = status.Gold + " Gold";
    }

    public void GetMove(int moveCount)
    {
        if (atDesert)
        {
            moveCount--;
            atDesert = false;
        }
        StartCoroutine(CharacterMove(moveCount));
    }

    IEnumerator CharacterMove(int moveCount)
    {
        while (moveCount > 0)
        {
            if(curPlace == 28)
            {
                // 도착 함수
                Debug.Log("Player 도착");
                yield break;
            }
            curPlace++;
            moveCount--;
            yield return new WaitForSeconds(0.33f);
        }
        EndMove();
    }

    void EndMove()
    {
        LandType curLand = places.ElementAt(curPlace).Value;

        switch (curLand)
        {
            case LandType.Ground:
                status.HpHeal(2);
                break;
            case LandType.Lake:
                status.HpHeal(5);
                break;
            case LandType.Desert:
                atDesert = true;
                break;
            case LandType.Clay:
                FuncHelper.SetPlayerData(status.MaxHp, status.CurHp, status.Atk, status.Def);
                StartCoroutine(FuncHelper.LoadScene("Combat"));
                break;
            case LandType.Stone:
                status.Gold = 2;
                break;
            case LandType.Gold:
                status.Gold = 4;
                break;
            case LandType.Goal:
                Debug.Log("Game End");
                break;
        }
    }

    public void Item(ItemName name)
    {
        int num = (int)name;
        if (num < 0 || num > itemNum)
            return;

        if (!itemOn[num])
        {
            if (status.PayGold(itemValue[num]))
                itemOn[num] = true;
        }
        else
        {
            Debug.Log(string.Format("{0} 아이템 사용", name));
            itemOn[num] = false;

            switch(num)
            {
                case (int)ItemName.DiceAdd:
                    break;
                case (int)ItemName.DiceUpgrade:
                    break;
                case (int)ItemName.HpPotion:
                    status.HpHeal(30);
                    break;
                case (int)ItemName.EnemyBack:
                    break;
            }
        }
    }
}
