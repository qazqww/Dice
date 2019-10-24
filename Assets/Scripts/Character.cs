using System;
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
    CharacterStatus status;

    Dictionary<Vector3, LandType> places = new Dictionary<Vector3, LandType>();
    public Transform landSide;
    public int curPlace = 0;
    bool atDesert = false;

    public const int itemNum = (int)ItemName.num;
    int[] itemValue = new int[itemNum] { 3, 4, 5, 6 };
    static bool[] itemOn = new bool[itemNum];

    void Start()
    {
        status = GetComponent<CharacterStatus>();

        for (int i = 1; i <= 28; i++)
        {
            Transform tempTr = landSide.Find("HexTile_" + i).GetComponent<Transform>();
            LandType tempLand = (LandType)Enum.Parse(typeof(LandType), tempTr.tag);
            Vector3 pos = landSide.rotation * tempTr.localPosition;
            places.Add(pos, tempLand);
        }
        places.Add(new Vector3(0, 0, 0), LandType.Goal);
    }

    void Update()
    {
        transform.position = places.ElementAt(curPlace).Key;
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
                status.HpHeal(10);
                break;
            case LandType.Desert:
                atDesert = true;
                break;
            case LandType.Clay:
                FuncHelper.SetPlayerData(status.MaxHp, status.CurHp, status.Atk, status.Def, status.Gold);
                Board.SavePlayerPlace();
                StartCoroutine(FuncHelper.LoadScene("Combat"));
                break;
            case LandType.Stone:
                status.Gold = 2;
                break;
            case LandType.Gold:
                status.Gold = 5;
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

        if (!itemOn[num]) // 아이템이 없을 경우 구매
        {
            if (status.PayGold(itemValue[num]))
                itemOn[num] = true;
        }
        else // 아이템이 있을 경우 사용
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
