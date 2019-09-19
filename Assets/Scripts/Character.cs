using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<Vector3> place = new List<Vector3>();
    public int curPlace = 0;

    void Start()
    {
        for (int i = 0; i < 11; i++)
            place.Add(new Vector3(-20 + i*3.5f, 1, -10));
    }

    void Update()
    {
        transform.position = place[curPlace];

        //if (Input.GetKeyDown(KeyCode.A))
        //    curPlace++;
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
