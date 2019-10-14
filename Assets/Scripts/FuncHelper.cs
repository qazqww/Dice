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

    public static void SetPlayerData(int maxHp, int curHp, int atk, int def)
    {
        PlayerPrefs.SetInt("MaxHp", maxHp);
        PlayerPrefs.SetInt("CurHp", curHp);
        PlayerPrefs.SetInt("Atk", atk);
        PlayerPrefs.SetInt("Def", def);
    }

    public static void SetPlayerHPHalf()
    {
        int max = PlayerPrefs.GetInt("MaxHp");
        PlayerPrefs.SetInt("CurHp", max/2);
    }

    public static void GetPlayerData(ref int maxHp, ref int curHp, ref int atk, ref int def)
    {
        maxHp = PlayerPrefs.GetInt("MaxHp");
        curHp = PlayerPrefs.GetInt("CurHp");
        atk = PlayerPrefs.GetInt("Atk");
        def = PlayerPrefs.GetInt("Def");
    }
}
