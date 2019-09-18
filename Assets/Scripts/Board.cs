using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<GameObject> dices = new List<GameObject>();
    private string galleryDie = "d6-red";

    void Start()
    {
        
    }

    void Update()
    {
        UpdateRoll();
        //if(dices.Count != 0)
        //    Debug.Log(isRolling());
    }

    private GameObject spawnPoint = null;
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

    void UpdateRoll()
    {
        spawnPoint = GameObject.Find("spawnPoint");
        // check if we have to roll dice
        if (Input.GetMouseButtonDown(Dice.MOUSE_LEFT_BUTTON))
        {
            Dice.Clear();
            string[] a = galleryDie.Split('-');
            Dice.Roll("2" + a[0], galleryDie, spawnPoint.transform.position, Force());
        }
        else if(Input.GetMouseButtonDown(Dice.MOUSE_RIGHT_BUTTON))
        {
            if (isRolling())
                return;

            int sum = 0;
            for (int i = 0; i < dices.Count; i++)
                sum += dices[i].GetComponent<Die>().value;

            Debug.Log(sum);
        }
    }

    bool isRolling()
    {
        for (int i = 0; i < dices.Count; i++)
        {
            Rigidbody rb = dices[i].GetComponent<Rigidbody>();
            if (rb.velocity == Vector3.zero)
                return false;
        }
        return true;
    }
}
