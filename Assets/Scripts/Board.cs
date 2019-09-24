using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // static 고려해보기
    public List<GameObject> dices = new List<GameObject>();
    private string galleryDie = "d6-red";
    bool diceActive = false;

    [HideInInspector]
    public int score = 0;
    public Text scoreText;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKeyDown)
            UpdateRoll();

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Dice")
                {
                    Die dice = hit.transform.GetComponent<Die>();
                    if (!dice.rolling)
                        Debug.Log(dice.value);
                }
            }
        }
    }

    private void OnGUI()
    {
        //scoreText.text = "Gold: " + score;
    }

    private GameObject spawnPoint = null;
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

    void UpdateRoll()
    {
        if (CheckRolling())
            return;

        spawnPoint = GameObject.Find("spawnPoint");
        // check if we have to roll dice

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DiceUse.canDrag = false;
            Dice.Clear();
            string[] a = galleryDie.Split('-');
            Dice.Roll("2" + a[0], galleryDie, spawnPoint.transform.position, Force());
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            int sum = 0;
            for (int i = 0; i < dices.Count; i++)
                sum += dices[i].GetComponent<Die>().value;

            Debug.Log(sum);
        }
    }

    public bool CheckRolling()
    {
        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i].GetComponent<Die>().rolling)
                return true;
        }
        return false;
    }
}
