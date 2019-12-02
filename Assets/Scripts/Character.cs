using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemName
{
    HpPotion,
    DiceUp,
    DiceAdd,
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
    Client client;
    CharacterStatus status;
    ItemManager itemManager;

    Dictionary<Vector3, LandType> places = new Dictionary<Vector3, LandType>();
    public Transform landSide;
    public int curPlace = 0;
    bool atDesert = false;

    public const int itemNum = (int)ItemName.num;       // 아이템 개수
    int[] itemValue = new int[itemNum] { 3, 3, 5, 6 };  // 아이템 가격
    static bool[] itemHave = new bool[itemNum];         // 아이템 보유 여부
    static public int itemOn = -1;                      // 활성화된 아이템

    void Start()
    {
        client = GameObject.Find("Client").GetComponent<Client>();
        status = GetComponent<CharacterStatus>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

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

        //if (Input.GetKeyDown(KeyCode.Q))
        //    itemManager.ItemOn(0);
            
        //else if (Input.GetKeyDown(KeyCode.A))
        //    itemManager.ItemOff(0);
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

    public void BackMove()
    {
        if (curPlace == 0)
            return;

        curPlace--;
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
                //FuncHelper.SetPlayerData(status.MaxHp, status.CurHp, status.Atk, status.Def, status.Gold, Board.charCode);
                Board.SavePlayerPlace();
                client.SaveStatus();
                if(Client.dataSync >= 2)
                    client.ToCombatScene();
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

        if (!itemHave[num]) // 아이템이 없을 경우 구매
        {
            if (status.PayGold(itemValue[num]))
            {
                itemHave[num] = true;
                itemManager.ItemOn(num);
            }
        }
        else // 아이템이 있을 경우 (턴 준비단계이면) 사용 가능
        {
            if (!Board.turnReady)
                return;

            switch (num)
            {
                case (int)ItemName.HpPotion:
                    status.HpHeal(20);
                    break;
                case (int)ItemName.DiceAdd:
                    if (itemOn == -1)
                        itemOn = (int)ItemName.DiceAdd;
                    break;
                case (int)ItemName.DiceUp:
                    if (itemOn == -1)
                        itemOn = (int)ItemName.DiceUp;
                    break;                
                case (int)ItemName.EnemyBack:
                    client.CharMove(-1);
                    break;
            }

            Debug.Log(string.Format("{0} 아이템 사용", name));
            itemHave[num] = false;
            itemManager.ItemOff(num);
        }
    }
}
