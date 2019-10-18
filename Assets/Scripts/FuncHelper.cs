using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuncHelper
{
    public static IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
            //Debug.Log(operation.progress);
        }
    }

    public static void SetPlayerData(int maxHp, int curHp, int atk, int def, int num)
    {
        if (num == 1)
            SetPlayer1Data(maxHp, curHp, atk, def);
        else if (num == 2)
            SetPlayer2Data(maxHp, curHp, atk, def);
    }

    public static void GetPlayerData(ref int maxHp, ref int curHp, ref int atk, ref int def, int num)
    {
        if (num == 1)
            GetPlayer1Data(ref maxHp, ref curHp, ref atk, ref def);
        else if (num == 2)
            GetPlayer2Data(ref maxHp, ref curHp, ref atk, ref def);
    }

    public static void SetPlayer1Data(int maxHp, int curHp, int atk, int def)
    {
        PlayerPrefs.SetInt("MaxHp", maxHp);
        PlayerPrefs.SetInt("CurHp", curHp);
        PlayerPrefs.SetInt("Atk", atk);
        PlayerPrefs.SetInt("Def", def);
    }

    public static void GetPlayer1Data(ref int maxHp, ref int curHp, ref int atk, ref int def)
    {
        maxHp = PlayerPrefs.GetInt("MaxHp");
        curHp = PlayerPrefs.GetInt("CurHp");
        atk = PlayerPrefs.GetInt("Atk");
        def = PlayerPrefs.GetInt("Def");
    }

    public static void SetPlayer2Data(int maxHp, int curHp, int atk, int def)
    {
        PlayerPrefs.SetInt("MaxHp2", maxHp);
        PlayerPrefs.SetInt("CurHp2", curHp);
        PlayerPrefs.SetInt("Atk2", atk);
        PlayerPrefs.SetInt("Def2", def);
    }

    public static void GetPlayer2Data(ref int maxHp, ref int curHp, ref int atk, ref int def)
    {
        maxHp = PlayerPrefs.GetInt("MaxHp2");
        curHp = PlayerPrefs.GetInt("CurHp2");
        atk = PlayerPrefs.GetInt("Atk2");
        def = PlayerPrefs.GetInt("Def2");
    }

    public static void SetPlayerHPHalf()
    {
        int max = PlayerPrefs.GetInt("MaxHp");
        PlayerPrefs.SetInt("CurHp", max / 2);
        max = PlayerPrefs.GetInt("MaxHp2");
        PlayerPrefs.SetInt("CurHp2", max / 2);
    }
}
