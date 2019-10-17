using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // static ?
    public List<GameObject> dices = new List<GameObject>();
    public static List<GameObject> diceUIs = new List<GameObject>();
    //public static int diceCount = 0;
    private string galleryDie = "d6-red";
    bool canRoll = true;
    int diceNum = 2;
    public static int[] diceFunc = new int[5]; // 0: HP, 1: ATK, 2: DEF, 3: MOVE, 4: GOLD

    Character player;
    CharacterStatus playerStatus;

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

        player = GameObject.Find("PlayerOne").GetComponent<Character>();
        playerStatus = player.GetComponent<CharacterStatus>();
        diceFunc.Initialize();
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
        if (CheckRolling() || !canRoll) // || diceCount > 0)
            return;

        canRoll = false;
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
    
    //public void DiceToUI(int eyes)
    //{
    //    eyeObj = Instantiate(eyeUI) as GameObject;
    //    eyeObj.transform.SetParent(canvas.transform);
    //    diceUIs.Add(eyeObj);
    //    eyeImg = eyeObj.GetComponent<Image>();
    //    eyeImg.sprite = dice_eye[eyes-1];
    //    Debug.Log("Check");

    //    //var testImg = Instantiate(test) as GameObject;
    //    //testImg.transform.SetParent(canvas.transform, false);
    //}

    public void DiceUIRemove()
    {
        Destroy(eyeObj);
    }

    public void DiceUIMove()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        eyeObj.transform.position = Input.mousePosition;
    }

    public void DicePlay()
    {
        // 주사위 위치와 눈값을 받아옴
        for (int i = 0; i < diceUIs.Count; i++)
        {
            DiceUse diceTemp = diceUIs[i].GetComponent<DiceUse>();
            int func = diceTemp.FuncValue;
            int val = diceTemp.Value;

            if (func >= 0)
                diceFunc[func] = val;
        }

        // 주사위가 모두 배치돼야 실행되도록
        int playCheck = 0;
        for (int i = 0; i < diceFunc.Length; i++)
        {
            if (diceFunc[i] > 0)
                playCheck++;
        }

        if (playCheck != diceNum)
            return;

        for (int i = 0; i < diceFunc.Length; i++)
        {
            if (diceFunc[i] > 0)
                DiceUsing(i, diceFunc[i]);
        }

        for (int i = 0; i < diceUIs.Count; i++)
            Destroy(diceUIs[i]);

        for (int i = 0; i < diceFunc.Length; i++)
            diceFunc[i] = 0;

        diceUIs.Clear();
        canRoll = true;
    }

    void DiceUsing(int func, int val)
    {
        switch (func)
        {
            case 0:
                playerStatus.Hp = val;                
                break;
            case 1:
                playerStatus.Atk = val;
                break;
            case 2:
                playerStatus.Def = val;
                break;
            case 3:
                player.GetMove(val);
                break;
            case 4:
                playerStatus.Gold = 7 - val;
                break;
        }
        Debug.Log(func + " " + val);
        //diceCount--;
    }
}
