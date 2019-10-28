using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public List<GameObject> dices = new List<GameObject>();
    public static List<GameObject> diceUIs = new List<GameObject>();
    private string galleryDie = "d6-red";
    bool canRoll = true;
    int diceNum = 2;
    public static int[] diceFunc = new int[5]; // 0: HP, 1: ATK, 2: DEF, 3: MOVE, 4: GOLD

    static Character[] player = new Character[2];
    CharacterStatus[] pStatus = new CharacterStatus[2];
    Character myChar, enemyChar;
    CharacterStatus myStatus;

    Transform statusText;
    Text HpText, AtkText, DefText, GoldText;

    GameObject spawnPoint = null;

    public GameObject canvas;
    GameObject eyeUI;
    GameObject eyeObj;
    Image eyeImg;
    Sprite[] dice_eye = new Sprite[6];

    // 클라이언트 넘버 대체 변수 (p1: 0, p2: 1)
    static public int charCode = 0;

    static bool turn = false;
    static int turnNum = 0; // 짝수: p1턴, 홀수: p2턴

    void Awake()
    {
        spawnPoint = GameObject.Find("spawnPoint");

        for(int i=0; i<dice_eye.Length; i++)
            dice_eye[i] = Resources.Load<Sprite>("eye" + (i+1));
        eyeUI = Resources.Load<GameObject>("Eye");

        player[0] = GameObject.Find("PlayerOne").GetComponent<Character>();
        pStatus[0] = player[0].GetComponent<CharacterStatus>();
        player[1] = GameObject.Find("PlayerTwo").GetComponent<Character>();
        pStatus[1] = player[1].GetComponent<CharacterStatus>();
        diceFunc.Initialize();

        statusText = GameObject.Find("Status").GetComponent<Transform>();
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();
        GoldText = GameObject.Find("GoldText").GetComponent<Text>();

        myChar = player[charCode];
        myStatus = pStatus[charCode];
        enemyChar = player[1 - charCode];

        if (turnNum < 1)
        {
            pStatus[0].StatusInitialize();
            FuncHelper.SetPlace(0, 0);
        }

        GetPlayerPlace();
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
                }
            }
        }

        // Debug Code
        //myChar = player[charCode];
        //myStatus = pStatus[charCode];
    }

    private void OnGUI()
    {
        HpText.text = string.Format("HP: {0} / {1}", myStatus.Hp % 1000, myStatus.Hp / 1000);
        AtkText.text = "ATK: " + myStatus.Atk;
        DefText.text = "DEF: " + myStatus.Def;
        GoldText.text = myStatus.Gold + " Gold";

        if (GUI.Button(new Rect(0, 0, 200, 100), "To Combat (Debug)"))
        {
            FuncHelper.SetPlayerData(myStatus.MaxHp, myStatus.CurHp, myStatus.Atk, myStatus.Def, myStatus.Gold);
            SavePlayerPlace();
            StartCoroutine(FuncHelper.LoadScene("Combat"));
        }
        if (GUI.Button(new Rect(0, 100, 200, 100), "Change CharCode (Debug)"))
        {
            charCode = 1 - charCode;
        }
    }

    public static void SavePlayerPlace()
    {
        FuncHelper.SetPlace(player[0].curPlace, player[1].curPlace);
    }

    public void GetPlayerPlace()
    {
        FuncHelper.GetPlace(ref player[0].curPlace, ref player[1].curPlace);
    }

    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }

    // 주사위를 굴리는 코드 (Dice 버튼)
    public void UpdateRoll()
    {
        if (turnNum % 2 != charCode) // 자기 턴이 아닐 경우
            return;

        if (CheckRolling() || !canRoll)
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

    public void DiceUIRemove()
    {
        Destroy(eyeObj);
    }

    public void DiceUIMove()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        eyeObj.transform.position = Input.mousePosition;
    }

    // 배치된 주사위대로 진행하는 코드 (Play 버튼)
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

        // 주사위 기능 작동
        for (int i = 0; i < diceFunc.Length; i++)
        {
            if (diceFunc[i] > 0)
                DiceUsing(i, diceFunc[i]);
        }

        // 주사위 초기화 및 턴 변경
        for (int i = 0; i < diceUIs.Count; i++)
            Destroy(diceUIs[i]);
        for (int i = 0; i < diceFunc.Length; i++)
            diceFunc[i] = 0;
        diceUIs.Clear();
        canRoll = true;
        turn = !turn;
        turnNum++;
    }

    // 주사위 작동
    void DiceUsing(int func, int val)
    {
        int pNum = (!turn) ? 0 : 1; // false턴: p1, true턴: p2
        switch (func)
        {
            case 0:
                pStatus[pNum].Hp = val;
                break;
            case 1:
                pStatus[pNum].Atk = val;
                break;
            case 2:
                pStatus[pNum].Def = val;
                break;
            case 3:
                player[pNum].GetMove(val);
                break;
            case 4:
                pStatus[pNum].Gold = 7 - val;
                break;
        }
    }
}
