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
}
