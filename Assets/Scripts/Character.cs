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
    }

    void Update()
    {
        transform.position = place[curPlace];
    }

    private void OnGUI()
    {
        HpText.text = "HP: " + status.Hp%1000 + " / " + status.Hp/1000;
        AtkText.text = "ATK: " + status.Atk;
        DefText.text = "DEF: " + status.Def;
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
}
