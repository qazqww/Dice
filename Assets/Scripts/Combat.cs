using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    public static bool isEnd = false;
    public static int result = -1;
    bool endEffect = true;

    float elapsedTime;

    Client client;

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
    int maxHp2, curHp2, atk2, def2, temp2;

    void Awake()
    {
        AudioManager.Instance.PlayBackground(BackgroundType.bgm_combat);

        client = GameObject.Find("Client").GetComponent<Client>();

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

        FuncHelper.GetPlayerData(ref maxHp, ref curHp, ref atk, ref def, ref temp, 0);
        player1.SetStatus(maxHp, curHp, atk, def);
        FuncHelper.GetPlayerData(ref maxHp2, ref curHp2, ref atk2, ref def2, ref temp2, 1);
        player2.SetStatus(maxHp2, curHp2, atk2, def2);

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
                //SceneMng.Instance.Enable(Scene.Game);
                SceneManager.LoadScene("Game");
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

        if (elapsedTime >= 0.2f && endEffect)
        {
            switch (result)
            {
                case 0:
                    resultText.text = "Draw!";
                    FuncHelper.SetPlayerHPHalf(0);
                    FuncHelper.SetPlayerHPHalf(1);
                    break;
                case 1:
                    if (Board.charCode == 0)
                        AudioManager.Instance.PlayBackground(BackgroundType.combat_victory, false);
                    else if (Board.charCode == 1)
                    {
                        AudioManager.Instance.PlayBackground(BackgroundType.combat_defeat, false);
                        Board.moveLocked = true;
                    }
                    resultText.text = "Player 1 Win!";
                    FuncHelper.SetPlayerHP(player1.CurHp, 0);
                    FuncHelper.SetPlayerHPHalf(1);
                    break;
                case 2:
                    if (Board.charCode == 1)
                        AudioManager.Instance.PlayBackground(BackgroundType.combat_victory, false);
                    else if (Board.charCode == 0)
                    {
                        AudioManager.Instance.PlayBackground(BackgroundType.combat_defeat, false);
                        Board.moveLocked = true;
                    }
                    resultText.text = "Player 2 Win!";
                    FuncHelper.SetPlayerHPHalf(0);
                    FuncHelper.SetPlayerHP(player2.CurHp, 1);
                    break;
            }

            int num = Random.Range(1, 4);
            string str = "voice_female_c_death_0" + num;
            AudioManager.Instance.PlayUISound(str);
            endEffect = false;
        }
    }
}
