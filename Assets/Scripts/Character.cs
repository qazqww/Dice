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

public class Character : MonoBehaviour
{
    List<Vector3> place = new List<Vector3>();
    public int curPlace = 0;

    CharacterStatus status;

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
            Transform tempTr = GameObject.Find("HexTile_" + i).GetComponent<Transform>();
            place.Add(tempTr.localPosition);
        }
        place.Add(new Vector3(0, 0, 0));

        status = GetComponent<CharacterStatus>();

        statusText = GameObject.Find("Status").GetComponent<Transform>();
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();
        GoldText = GameObject.Find("GoldText").GetComponent<Text>();
    }

    void Update()
    {
        transform.position = place[curPlace];
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
        StartCoroutine(CharacterMove(moveCount));
    }

    IEnumerator CharacterMove(int moveCount)
    {
        while (moveCount > 0)
        {
            curPlace++;
            moveCount--;
            yield return new WaitForSeconds(0.5f);
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
                    break;
                case (int)ItemName.EnemyBack:
                    break;
            }
        }
    }
}
