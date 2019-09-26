using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    List<KeyValuePair<bool, int>> ItemInfo = new List<KeyValuePair<bool, int>>();

    void Start()
    {
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 3));
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 4));
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 5));
        ItemInfo.Add(new KeyValuePair<bool, int>(false, 6));

        Debug.Log(ItemInfo[0]);
        Debug.Log(ItemInfo[0].Key);
        Debug.Log(ItemInfo[0].Value);
    }

    void Update()
    {

    }
}
