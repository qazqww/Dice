using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Dictionary<int, int> itemInfo = new Dictionary<int, int>();
    bool[] haveItem = new bool[4];

    // 이름, 소유 여부, 가격
    //{index, {name, price } }
    //bool[] haveitem => haveitem[indexer]

    void Start()
    {
        itemInfo.Add(0, 4);
        itemInfo.Add(1, 5);
        itemInfo.Add(2, 5);
        itemInfo.Add(3, 7);
    }

    void Update()
    {

    }
}
