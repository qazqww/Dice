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
        }
    }

    public static void SetPlayerData(int maxHp, int curHp, int atk, int def, int gold, int charCode)
    {
        if (charCode == 0)
        {
            PlayerPrefs.SetInt("MaxHp0", maxHp);
            PlayerPrefs.SetInt("CurHp0", curHp);
            PlayerPrefs.SetInt("Atk0", atk);
            PlayerPrefs.SetInt("Def0", def);
            PlayerPrefs.SetInt("Gold0", gold);
        }
        else if (charCode == 1)
        {
            PlayerPrefs.SetInt("MaxHp1", maxHp);
            PlayerPrefs.SetInt("CurHp1", curHp);
            PlayerPrefs.SetInt("Atk1", atk);
            PlayerPrefs.SetInt("Def1", def);
            PlayerPrefs.SetInt("Gold1", gold);
        }
    }

    public static void GetPlayerData(ref int maxHp, ref int curHp, ref int atk, ref int def, ref int gold, int charCode)
    {
        if (charCode == 0)
        {
            maxHp = PlayerPrefs.GetInt("MaxHp0");
            curHp = PlayerPrefs.GetInt("CurHp0");
            atk = PlayerPrefs.GetInt("Atk0");
            def = PlayerPrefs.GetInt("Def0");
            gold = PlayerPrefs.GetInt("Gold0");
        }
        else if (charCode == 1)
        {
            maxHp = PlayerPrefs.GetInt("MaxHp1");
            curHp = PlayerPrefs.GetInt("CurHp1");
            atk = PlayerPrefs.GetInt("Atk1");
            def = PlayerPrefs.GetInt("Def1");
            gold = PlayerPrefs.GetInt("Gold1");
        }
    }

    public static void SetPlayerHP(int hp, int charCode)
    {
        if (charCode == 0)
        {
            PlayerPrefs.SetInt("CurHp0", hp);
        }
        else if (charCode == 1)
        {
            PlayerPrefs.SetInt("CurHp1", hp);
        }
    }

    public static void SetPlayerHPHalf(int charCode)
    {
        if (charCode == 0)
        {
            int max = PlayerPrefs.GetInt("MaxHp0");
            PlayerPrefs.SetInt("CurHp0", max / 2);
        }
        else if (charCode == 1)
        {
            int max = PlayerPrefs.GetInt("MaxHp1");
            PlayerPrefs.SetInt("CurHp1", max / 2);
        }
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
}
