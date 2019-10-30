﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    public static bool isEnd = false;
    public static int result = -1;

    float elapsedTime;

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

    int maxHp, curHp, atk, def, temp;

    void Start()
    {
        player1 = GameObject.Find("PlayerOne").GetComponent<CharacterClone>();
        player2 = GameObject.Find("PlayerTwo").GetComponent<CharacterClone>();

        statusText = GameObject.Find("Status1").GetComponent<Transform>();
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();        

        statusText2 = GameObject.Find("Status2").GetComponent<Transform>();
        HpText2 = statusText2.Find("HP").GetComponent<Text>();
        AtkText2 = statusText2.Find("ATK").GetComponent<Text>();
        DefText2 = statusText2.Find("DEF").GetComponent<Text>();
        
        resultText = GameObject.Find("Result").GetComponent<Text>();

        // 클라이언트 넘버에 따라 자기 캐릭터에 스탯을 넣음
        FuncHelper.GetPlayerData(ref maxHp, ref curHp, ref atk, ref def, ref temp, Board.charCode);

        if (Board.charCode == 0)
        {
            player1.SetStatus(maxHp, curHp, atk, def);
            player2.SetStatus(150, 80, 13, 5);
        }
        else if (Board.charCode == 1)
        {
            player1.SetStatus(150, 80, 13, 5);
            player2.SetStatus(maxHp, curHp, atk, def);
        }

        isEnd = false;
        result = -1;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (isEnd)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 3f) // 전투씬 종료
            {
                isEnd = false;
                SceneManager.LoadScene("Board");
                //StartCoroutine(FuncHelper.LoadScene("Board"));
            }
        }
    }

    private void OnGUI()
    {
        HpText.text = string.Format("HP: {0} / {1}", player1.CurHp, player1.MaxHp);
        AtkText.text = "ATK: " + player1.Atk;
        DefText.text = "DEF: " + player1.Def;

        HpText2.text = string.Format("HP: {0} / {1}", player2.CurHp, player2.MaxHp);
        AtkText2.text = "ATK: " + player2.Atk;
        DefText2.text = "DEF: " + player2.Def;

        if (elapsedTime >= 0.2f)
        {
            switch (result)
            {
                case 0:
                    resultText.text = "Draw.";
                    break;
                case 1:
                    resultText.text = "Player 1 Win.";
                    break;
                case 2:
                    resultText.text = "Player 2 Win.";
                    break;
            }
        }
    }
}
