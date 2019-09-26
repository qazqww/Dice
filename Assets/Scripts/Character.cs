using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    bool[] haveItem = new bool[4];
    List<KeyValuePair<bool, int>> ItemInfo = new List<KeyValuePair<bool, int>>();
    bool haveItem1 = false;

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

        ItemInfo.Add(new KeyValuePair<bool, int>(false, 3));
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 4));
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 5));
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 6));
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

    public void Item1()
    {
        if (!haveItem1)
        {
            if (status.PayGold(3))
                haveItem1 = true;
        }
        else
        {
            Debug.Log("아이템1 사용");
            haveItem1 = false;
        }
    }

    public void Item(int num)
    {
        if (num < 0 || num > haveItem.Length)
            return;

        if (!haveItem[num])
        {
            if (status.PayGold(3))
                haveItem[num] = true;
        }
        else
        {
            Debug.Log(string.Format("아이템{0} 사용", num));
            haveItem[num] = false;
        }
    }
}
