using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    CharacterClone player1;
    CharacterClone player2;

    Transform statusText;
    Text HpText;
    Text AtkText;
    Text DefText;    

    Transform statusText2;
    Text HpText2;
    Text AtkText2;
    Text DefText2;

    Text resultText;

    void Start()
    {
        player1 = GameObject.Find("PlayerOne").GetComponent<CharacterClone>();
        player2 = GameObject.Find("PlayerTwo").GetComponent<CharacterClone>();

        statusText = GameObject.Find("Status1").GetComponent<Transform>();
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();        

        statusText2 = GameObject.Find("Status2").GetComponent<Transform>();
        HpText2 = statusText.Find("HP").GetComponent<Text>();
        AtkText2 = statusText.Find("ATK").GetComponent<Text>();
        DefText2 = statusText.Find("DEF").GetComponent<Text>();

        resultText = GameObject.Find("Result").GetComponent<Text>();
    }

    void Update()
    {
        
    }

    private void OnGUI()
    {
        HpText.text = string.Format("HP: {0} / {1}", player1.CurHp, player1.MaxHp);
        AtkText.text = "ATK: " + player1.Atk;
        DefText.text = "DEF: " + player1.Def;

        HpText2.text = string.Format("HP: {0} / {1}", player2.CurHp, player2.MaxHp);
        AtkText2.text = "ATK: " + player2.Atk;
        DefText2.text = "DEF: " + player2.Def;
    }
}
