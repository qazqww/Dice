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

    public static void SetPlayerData(int maxHp, int curHp, int atk, int def, int gold)
    {
        PlayerPrefs.SetInt("MaxHp", maxHp);
        PlayerPrefs.SetInt("CurHp", curHp);
        PlayerPrefs.SetInt("Atk", atk);
        PlayerPrefs.SetInt("Def", def);
        PlayerPrefs.SetInt("Gold", gold);
    }

    public static void GetPlayerData(ref int maxHp, ref int curHp, ref int atk, ref int def, ref int gold)
    {
        maxHp = PlayerPrefs.GetInt("MaxHp");
        curHp = PlayerPrefs.GetInt("CurHp");
        atk = PlayerPrefs.GetInt("Atk");
        def = PlayerPrefs.GetInt("Def");
        gold = PlayerPrefs.GetInt("Gold");
    }

    public static void SetPlayerHPHalf()
    {
        int max = PlayerPrefs.GetInt("MaxHp");
        PlayerPrefs.SetInt("CurHp", max / 2);
    }

    public static void SetPlace(int p1Place, int p2Place)
    {
        PlayerPrefs.SetInt("P1Place", p1Place);
        PlayerPrefs.SetInt("P2Place", p2Place);
    }

    public static void GetPlace(ref int p1Place, ref int p2Place)
    {
        p1Place = PlayerPrefs.GetInt("P1Place");
        p2Place = PlayerPrefs.GetInt("P2Place");
    }

    /*
    public static void SetPlayerData(int maxHp, int curHp, int atk, int def, int gold, int num)
    {
        if (num == 1)
            SetPlayer1Data(maxHp, curHp, atk, def, gold);
        else if (num == 2)
            SetPlayer2Data(maxHp, curHp, atk, def, gold);
    }

    public static void GetPlayerData(ref int maxHp, ref int curHp, ref int atk, ref int def, ref int gold, int num)
    {
        if (num == 1)
            GetPlayer1Data(ref maxHp, ref curHp, ref atk, ref def, ref gold);
        else if (num == 2)
            GetPlayer2Data(ref maxHp, ref curHp, ref atk, ref def, ref gold);
    }

    public static void SetPlayer1Data(int maxHp, int curHp, int atk, int def, int gold)
    {
        PlayerPrefs.SetInt("MaxHp", maxHp);
        PlayerPrefs.SetInt("CurHp", curHp);
        PlayerPrefs.SetInt("Atk", atk);
        PlayerPrefs.SetInt("Def", def);
        PlayerPrefs.SetInt("Gold", gold);
    }

    public static void GetPlayer1Data(ref int maxHp, ref int curHp, ref int atk, ref int def, ref int gold)
    {
        maxHp = PlayerPrefs.GetInt("MaxHp");
        curHp = PlayerPrefs.GetInt("CurHp");
        atk = PlayerPrefs.GetInt("Atk");
        def = PlayerPrefs.GetInt("Def");
        gold = PlayerPrefs.GetInt("Gold");
    }

    public static void SetPlayer2Data(int maxHp, int curHp, int atk, int def, int gold)
    {
        PlayerPrefs.SetInt("MaxHp2", maxHp);
        PlayerPrefs.SetInt("CurHp2", curHp);
        PlayerPrefs.SetInt("Atk2", atk);
        PlayerPrefs.SetInt("Def2", def);
        PlayerPrefs.SetInt("Gold2", gold);
    }
    
    public static void GetPlayer2Data(ref int maxHp, ref int curHp, ref int atk, ref int def, ref int gold)
    {
        maxHp = PlayerPrefs.GetInt("MaxHp2");
        curHp = PlayerPrefs.GetInt("CurHp2");
        atk = PlayerPrefs.GetInt("Atk2");
        def = PlayerPrefs.GetInt("Def2");
        gold = PlayerPrefs.GetInt("Gold2");
    }
    
    public static void SetPlayerHPHalf()
    {
        int max = PlayerPrefs.GetInt("MaxHp");
        PlayerPrefs.SetInt("CurHp", max / 2);
        max = PlayerPrefs.GetInt("MaxHp2");
        PlayerPrefs.SetInt("CurHp2", max / 2);
    }
    */
}
