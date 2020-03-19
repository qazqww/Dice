using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    Transform canvas;
    GameObject itemWindow;
    GameObject diceSlot;
    GameObject diceButton;
    GameObject turnInfo;
    GameObject moveLimit;
    Transform statusText;
    Text HpText, AtkText, DefText, GoldText;

    static BoardManager instance;
    public static BoardManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("BoardManager", typeof(BoardManager));
                instance = obj.GetComponent<BoardManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        itemWindow = canvas.Find("Items").gameObject;
        diceSlot = canvas.Find("DiceSlot").gameObject;
        diceButton = canvas.Find("DiceBtn").gameObject;
        turnInfo = canvas.Find("turninfo").gameObject;
        moveLimit = diceSlot.transform.Find("MoveLimit").gameObject;       

        statusText = canvas.Find("Status").transform;
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();
        GoldText = statusText.Find("GoldText").GetComponent<Text>();
        
        diceSlot.SetActive(false);
    }

    public void MyTurnReady()
    {
        itemWindow.SetActive(true);
        diceButton.SetActive(true);
        turnInfo.SetActive(false);
    }

    public void MyTurn()
    {
        itemWindow.SetActive(false);
        diceButton.SetActive(false);
        diceSlot.SetActive(true);
    }

    public void MyTurnEnd()
    {
        diceSlot.SetActive(false);
    }

    public void EnemyTurn()
    {
        itemWindow.SetActive(false);
        diceButton.SetActive(false);
        turnInfo.SetActive(true);
    }

    public void MoveLockIcon(bool status)
    {
        moveLimit.SetActive(status);
    }

    public void GameEnd(int winner)
    {
        itemWindow.SetActive(false);
        diceSlot.SetActive(false);
        diceButton.SetActive(false);
        moveLimit.SetActive(false);
        turnInfo.SetActive(true);
        turnInfo.GetComponent<Text>().text = string.Format("Player {0} 승리!!", winner+1);
    }

    public void SetStatusText(int hp, int atk, int def, int gold)
    {
        HpText.text = string.Format("HP: {0} / {1}", hp % 1000, hp / 1000);
        AtkText.text = "ATK: " + atk;
        DefText.text = "DEF: " + def;
        GoldText.text = gold + " Gold";
    }
}
