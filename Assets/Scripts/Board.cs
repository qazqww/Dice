using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Client client;

    public List<GameObject> dices = new List<GameObject>();
    public static List<GameObject> diceUIs = new List<GameObject>();
    private string galleryDie = "d6-red";
    bool canRoll = true;
    int diceNum = 2;
    public static int[] diceFunc = new int[6]; // 0: HP, 1: ATK, 2: DEF, 3: MOVE, 4: GOLD

    static Character[] player = new Character[2];
    static public Character myChar;
    CharacterStatus myStatus;

    Transform statusText;
    Text HpText, AtkText, DefText, GoldText;
    GameObject moveLimit;

    public Transform canvas;
    GameObject itemWindow;
    GameObject diceWindow;
    GameObject diceButton;
    public GameObject spawnPoint;

    // 클라이언트 넘버 대체 변수 (p1: 0, p2: 1)
    static public int charCode = -1;
    static public bool moveLocked = false;

    static public bool turn = false;
    static public bool turnReady = false; // 아이템을 쓸 수 있는 턴 준비 단계. Dice 하면 false
    static public int turnNum = 0; // 짝수: p1턴, 홀수: p2턴, turnNum % 2 == charCode이면 자기 턴
    public void TurnCheck()
    {
        if (turnNum % 2 == charCode)
        {
            turnReady = true;
            itemWindow.SetActive(true);
            diceButton.SetActive(true);
        }
    }

    static public bool ready = false;
    bool gameSet = false;

    public Text debugText;

    void Awake()
    {
        if(client == null)
            client = GameObject.Find("Client").GetComponent<Client>();
        if (canvas == null)
            canvas = GameObject.Find("Canvas").transform;
        if (spawnPoint == null)
            spawnPoint = GameObject.Find("spawnPoint");

        client.BoardConnect(this);

        //AudioManager.Instance.LoadClip<BackgroundType>("BGM/");
        //AudioManager.Instance.LoadClip<SoundType>("Sounds/");
        AudioManager.Instance.PlayBackground(BackgroundType.bgm_board);

        itemWindow = canvas.Find("Items").gameObject;
        diceWindow = canvas.Find("DiceUse").gameObject;
        diceButton = canvas.Find("Dice").gameObject;
        statusText = canvas.Find("Status").transform;
        HpText = statusText.Find("HP").GetComponent<Text>();
        AtkText = statusText.Find("ATK").GetComponent<Text>();
        DefText = statusText.Find("DEF").GetComponent<Text>();
        GoldText = statusText.Find("GoldText").GetComponent<Text>();
        moveLimit = diceWindow.transform.Find("MoveLimit").gameObject;

        itemWindow.SetActive(true);
        diceButton.SetActive(true);
        diceWindow.SetActive(false);

        player[0] = GameObject.Find("PlayerOne").GetComponent<Character>();
        player[1] = GameObject.Find("PlayerTwo").GetComponent<Character>();

        if (charCode >= 0)
        {
            myChar = player[charCode];
            myStatus = myChar.GetComponent<CharacterStatus>();
        }

        if (turnNum < 1)
            FuncHelper.SetPlace(0, 0);

        diceFunc.Initialize();
        GetPlayerPlace();
        MoveLock();
    }

    void Update()
    {
        if (myChar == null && charCode >= 0)
            SetChar();

        if (!ready || !gameSet)
            return;

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
    }

    void OnGUI()
    {
        if (myStatus != null)
        {
            HpText.text = string.Format("HP: {0} / {1}", myStatus.Hp % 1000, myStatus.Hp / 1000);
            AtkText.text = "ATK: " + myStatus.Atk;
            DefText.text = "DEF: " + myStatus.Def;
            GoldText.text = myStatus.Gold + " Gold";
        }

        debugText.text = string.Format("{0}, {1}", charCode, turnNum);
    }

    void SetChar()
    {
        myChar = player[charCode];
        myStatus = myChar.GetComponent<CharacterStatus>();
        myStatus.StatusInitialize();
    }

    public void SavePlayerStatus()
    {
        client.SaveStatus(myStatus.MaxHp, myStatus.CurHp, myStatus.Atk, myStatus.Def, myStatus.Gold, charCode);
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
        if (turnNum % 2 != charCode || !ready || gameSet) // 자기 턴이 아닐 경우, 게임 시작 전, 게임 끝날 경우
            return;

        if (CheckRolling() || !canRoll)
            return;

        turnReady = false;
        canRoll = false;
        DiceBasic.canChange = false;
        Dice.Clear();
        itemWindow.SetActive(false);
        diceButton.SetActive(false);
        diceWindow.SetActive(true);

        if (Character.itemOn == (int)ItemName.DiceAdd)
            diceNum = 3;

        string[] a = galleryDie.Split('-');
        Dice.Roll(diceNum + a[0], galleryDie, spawnPoint.transform.position, Force());

        AudioManager.Instance.PlayUISound(SoundType.dicethrow);
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
    
    // 배치된 주사위대로 진행하는 코드 (Play 버튼)
    public void DicePlay()
    {
        if (turnNum % 2 != charCode) // 자기 턴이 아닐 경우
            return;

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
        diceWindow.SetActive(false);
        client.ChangeTurn(); // turn = !turn; turnNum++;

        // 아이템 효과 초기화
        Character.itemOn = -1;
        diceNum = 2;
    }

    // 주사위 작동
    void DiceUsing(int func, int val)
    {
        switch (func)
        {
            case 0:
                myStatus.Hp = val;
                break;
            case 1:
                myStatus.Atk = val;
                break;
            case 2:
                myStatus.Def = val;
                break;
            case 3:
                client.CharMove(val);
                break;
            case 4:
                if(val > 3)
                    myStatus.Gold = 1; // 4눈 이상 -> 1원
                else
                    myStatus.Gold = 5 - val; // 1눈->4원, 3눈->2원
                break;
            case 5:
                moveLocked = false;
                MoveLock();
                break;
        }
    }

    public void PlayerMove(int val)
    {
        int pNum = (!turn) ? 0 : 1; // false턴: p1, true턴: p2

        if (val == -1) // 아이템으로 상대 캐릭터를 1칸 뒤로 보낼 때의 코드
        {
            player[1 - pNum].BackMove();
            return;
        }

        AudioManager.Instance.PlayUISound(SoundType.walking);
        player[pNum].GetMove(val);
    }

    void MoveLock()
    {
        if (moveLocked)
            moveLimit.SetActive(true);
        else
            moveLimit.SetActive(false);
    }
}
