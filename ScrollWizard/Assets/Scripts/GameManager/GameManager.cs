using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct BlockData
{
    public GameObject obj;
    public BlockInfo info;
    public bool isOnCharacter;

    public void Init(GameObject obj)
    {
        this.obj = obj;
        info = obj.GetComponent<BlockInfo>();
        isOnCharacter = false;
    }
}

public struct SkillInfo
{
    public string code;
    public int count;

    public void Init(string code, int count)
    {
        this.code = code;
        this.count = count;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject cursorObj; // 커서 위치에 있는 오브젝트
    public GameObject selectObj; // 클릭한 오브젝트
    public GameObject cloneObj; // 소환수가 나올 위치를 보여주는 환영 오브젝트

    public GameObject characterPrefab; // 적 프리팹
    public GameObject bigCharacterPrefab; // 큰 적 프리팹

    public List<Character> turnList; // 진행될 턴 순서

    public SkillInfo[] skillInfoArr; // 플레이어 보유 스킬 정보

    // 0 1 2 3 | 4 5 6 7
    // 3 2 1 0 | 0 1 2 3
    public GameObject[] blockObj; // 캐릭터가 움직일 8칸의 위치
    public BlockData[] blockDataArr; // 8칸의 정보
    public List<GameObject> characterObj; // 현재 생성된 캐릭터들

    public string activeSkillcode; // 버튼을 누른 스킬의 코드값
    public int summonSkillCount; // 사용하지 않은 소환수 스킬의 개수

    public enum BattleState { START, PLAYER, SUMMON, ENEMY, WIN, LOSE }
    public BattleState state;

    private void Awake()
    {
        instance = this;
        cursorObj = null;
        cloneObj = null;
        activeSkillcode = null;
        summonSkillCount = 5;
        turnList = new List<Character>();
        skillInfoArr = new SkillInfo[6];
        blockDataArr = new BlockData[8];
        characterObj = new List<GameObject>();

        for (int i = 0; i < blockDataArr.Length; i++)
            blockDataArr[i].Init(blockObj[i]);

        PlayerPrefs.SetInt("pos", 0);
        PlayerPrefs.SetString("skill code", "FI_0001/IC_0001/NA_0001");
        PlayerPrefs.SetString("skill count", "10/10/10");

        string[] tmpStr = PlayerPrefs.GetString("skill code").Split('/');
        int[] tmpInt = ConvertIntArray(PlayerPrefs.GetString("skill count"), '/');

        for (int i = 0; i < tmpStr.Length; i++)
        {
            skillInfoArr[i].Init(tmpStr[i], tmpInt[i]);
            
            if (tmpStr[i][0].Equals('S'))
            {
                summonSkillCount++;
            }
        }
    }

    private void Start()
    {
        state = BattleState.START;        

        StartCoroutine(ZeroTurn());
    }

    private void Update()
    {
        CheckCursorOn();

        if (cloneObj != null)
        {
            MoveClone();

            if (isObjClick())
                CreateSummon("S1_0001");
        }
    }

    private void TurnCalculation()
    {
        List<Character> survival = new List<Character>();

        for (int i = 0; i < characterObj.Count; i++)
        {
            if (characterObj[i].activeSelf)
            {
                Character tmp = characterObj[i].GetComponent<Character>();
                survival.Add(tmp);
            }
        }

        survival.Sort((c1, c2) =>
        {
            int result = c1.curData.spd.CompareTo(c2.curData.spd);
            if (result == 0) // 현재 속도 같으면 기본 속도로 정렬
            {
                result = c1.startData.spd.CompareTo(c2.startData.spd);

                if (result == 0) // 기본 속도도 같으면 우선순위로 정렬
                {
                    result = c1.priority.CompareTo(c2.priority);
                }
            }
            return result;
        });

        turnList.Clear();
        turnList.AddRange(survival);
        // 순서바 다시 그리기

    }

    private void NextTurn()
    {
        if (turnList[0].code.Equals("player"))
        {
            PlayerTurn();
        }
        else if (turnList[0].code[0].Equals('S'))
        {
            SummonTurn();
        }
        else
        {
            EnemyTurn();
        }
    }

    private void CreateEnemy()
    {
        // DB에서 적 배치 가져와서 데이터 저장

    }

    private void CreateSummon(string code)
    {
        Destroy(cloneObj);
        cloneObj = null;
        MakeCharacter(selectObj, code);

        selectObj = null;
        activeSkillcode = null;
        summonSkillCount--;
    }

    public void PushSkillButton()
    {
        activeSkillcode = "S0_0001";

        if (activeSkillcode[0].Equals('S'))
            CreateClone();
    }

    public void PushNextTurnButton()
    {
        if (state == BattleState.START)
        {
            summonSkillCount = 0;
            return;
        }

        NextTurn();
    }

    private bool isEndZeroTurn()
    {
        if (summonSkillCount == 0) // 모든 소환수 스킬을 사용했거나 필드가 꽉 차있으면 소환 턴 종료
            return true;

        if (isAllyFieldFull())
            return true;

        return false;
    }

    private bool isAllyFieldFull()
    {
        int count = 0;

        for (int i = 0; i < 4; i++)
        {
            if (blockDataArr[i].isOnCharacter)
                count++;
        }

        if (count == 4)
            return true;

        return false;
    }

    private IEnumerator ZeroTurn()
    {
        // 미리 지정된 위치에 플레이어 생성
        int idx = PlayerPrefs.GetInt("pos");
        MakeCharacter(blockObj[idx], "Player");

        // 소환수 스킬 사용        
        // 턴 넘기기 버튼 사용하거나 사용할 수 있는 소환수 스킬이 더 없으면 다음 턴으로
        yield return new WaitUntil(() => isEndZeroTurn());

        Debug.Log("소환턴 종료");

        for (int i = 0; i < blockObj.Length; i++)
        {
            BoxCollider2D collider2D = blockObj[i].GetComponent<BoxCollider2D>();

            if (!collider2D.enabled)
                collider2D.enabled = true;
        }

        //CreateEnemy();
        //TurnCalculation();
        //NextTurn();
    }

    private void MakeCharacter(GameObject obj, string code)
    {
        for (int i = 0; i < 4; i++)
        {
            if (obj == blockDataArr[i].obj)
            {
                blockDataArr[i].isOnCharacter = true;
                blockDataArr[i].info.CreateCharacter(characterPrefab);
                characterObj.Add(blockDataArr[i].info.linkedObj);
                blockDataArr[i].info.character.Init(code);
                break;
            }
        }
    }

    private void MoveClone()
    {
        if (cursorObj == null)
            return;

        if (cloneObj == null)
            return;

        for (int i = 0; i < 4; i++)
        {
            if (cursorObj == blockDataArr[i].obj)
            {
                if (blockDataArr[i].isOnCharacter)
                {
                    return;
                }
            }
        }

        if (!cloneObj.activeSelf)
            cloneObj.SetActive(true);

        cloneObj.transform.position = cursorObj.transform.position;
    }

    public Sprite ally;
    public Sprite enemy;
    public Sprite player;

    private void CreateClone()
    {
        cloneObj = Instantiate(characterPrefab);
        Character character = cloneObj.GetComponent<Character>();
        character.Translucence();
        SpriteRenderer spriteRenderer = cloneObj.GetComponent<SpriteRenderer>();
        // 소환수 스프라이트 넣기
        spriteRenderer.sprite = ally;
        cloneObj.SetActive(false);
    }

    private void PlayerTurn()
    {

    }

    private void SummonTurn()
    {

    }

    private void EnemyTurn()
    {

    }

    public SkillData GetSkillData(string code)
    {
        SkillData skill = FileHandler.LoadSO<SkillData>("SkillData", code);
        return skill;
    }

    public CharacterData GetCharacterData(string code)
    {
        CharacterData character = FileHandler.LoadSO<CharacterData>("CharacterData", code);
        return character;
    }

    private bool isObjClick()
    {
        if (Input.GetMouseButtonDown(0) && cursorObj != null)
        {
            selectObj = cursorObj;
            return true;
        }

        return false;
    }

    private void CheckCursorOn()
    {
        Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero);

        if (hit.collider != null)
        {
            cursorObj = hit.collider.gameObject;
        }
        else
        {
            cursorObj = null;
        }
    }

    private int[] ConvertIntArray(string input, char op)
    {
        string[] parts = input.Split(op);
        int[] numbers = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            if (int.TryParse(parts[i], out int number))
            {
                numbers[i] = number;
            }
        }

        return numbers;
    }
}