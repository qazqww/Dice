using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Vector3> place = new List<Vector3>();
    public int curPlace = 0;

    void Start()
    {
        for (int i = 1; i <= 28; i++)
        {
            Transform tempTr = GameObject.Find("HexTile_" + i).GetComponent<Transform>();
            place.Add(tempTr.localPosition);
        }
        place.Add(new Vector3(0, 0, 0));
    }

    void Update()
    {
        transform.position = place[curPlace];
    }

    public void GetMove(int moveCount)
    {
        StartCoroutine(CharacterMove(moveCount));
    }

    IEnumerator CharacterMove(int moveCount)
    {
        while (moveCount > 0)
        {
            curPlace++;
            moveCount--;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
