using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // static ?
    public List<GameObject> dices = new List<GameObject>();
    public static int diceCount = 0;
    private string galleryDie = "d6-red";
    bool diceActive = false;

    [HideInInspector]
    public int score = 0;
    public Text scoreText;

    GameObject spawnPoint = null;

    public GameObject canvas;
    GameObject eyeUI;
    GameObject eyeObj;
    Image eyeImg;
    Sprite[] dice_eye = new Sprite[6];

    void Start()
    {
        spawnPoint = GameObject.Find("spawnPoint");

        for(int i=0; i<dice_eye.Length; i++)
            dice_eye[i] = Resources.Load<Sprite>("eye" + (i+1));
        eyeUI = Resources.Load<GameObject>("Eye");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

    // 주사위를 굴리는 코드
    public void UpdateRoll()
    {
        if (CheckRolling() || diceCount > 0)
            return;

        DiceBasic.canChange = false;
        Dice.Clear();
        string[] a = galleryDie.Split('-');
        Dice.Roll("2" + a[0], galleryDie, spawnPoint.transform.position, Force());
    }

    // 주사위가 구르고 있는지 return해주는 코드
    public bool CheckRolling()
    {
        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i].GetComponent<Die>().rolling)
                return true;
        }
        return false;
    }
    
    public void DiceToUI(int eyes)
    {
        eyeObj = Instantiate(eyeUI) as GameObject;
        eyeObj.transform.SetParent(canvas.transform);
        eyeImg = eyeObj.GetComponent<Image>();
        eyeImg.sprite = dice_eye[eyes-1];
        Debug.Log("Check");

        //var testImg = Instantiate(test) as GameObject;
        //testImg.transform.SetParent(canvas.transform, false);
    }

    public void DiceUIRemove()
    {
        Destroy(eyeObj);
    }

    public void DiceUIMove()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        eyeObj.transform.position = Input.mousePosition;
    }
}
