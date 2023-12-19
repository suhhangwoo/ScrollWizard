using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject cursorObj; // 커서 위치에 있는 오브젝트
    public GameObject selectObj; // 클릭한 오브젝트
    public GameObject characterPrefab; // 적 프리팹
    public GameObject bigCharacterPrefab; // 큰 적 프리팹
    public List<Character> turnList; // 진행될 턴 순서

    // 0 1 2 3 | 4 5 6 7
    // 3 2 1 0 | 0 1 2 3
    public GameObject[] posObj; // 캐릭터가 움직일 8칸의 위치
    public List<GameObject> characterObj;

    public string activeSkillcode;

    public enum BattleState { START, PLAYER, SUMMON, ENEMY, WIN, LOSE }
    public BattleState state;

    void Awake()
    {
        instance = this;
        cursorObj = null;
        cloneObj = null;
        activeSkillcode = null;
        turnList = new List<Character>();
        characterObj = new List<GameObject>();

        PlayerPrefs.SetInt("pos", 0);
    }

    void Start()
    {
        state = BattleState.START;

        StartCoroutine(ZeroTurn());
    }

    void Update()
    {
        CheckCursorOn();

        if (cloneObj != null)
        {
            MoveClone();

            if (isObjClick())
                CreateSummon("SU_0001");
        }
    }
    
    void TurnCalculation()
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

    void NextTurn()
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

    void CreateEnemy()
    {
        // DB에서 적 배치 가져와서 데이터 저장

    }

    int summonSkillCount = 1;

    void CreateSummon(string code)
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

    bool isEndZeroTurn()
    {
        if (summonSkillCount == 0)
            return true;

        return false;
    }

    IEnumerator ZeroTurn()
    {
        // 미리 지정된 위치에 플레이어 생성
        int idx = PlayerPrefs.GetInt("pos");
        MakeCharacter(posObj[idx], "Player");

        // 소환수 스킬 사용        
        // 턴 넘기기 버튼 사용하거나 사용할 수 있는 소환수 스킬이 더 없으면 다음 턴으로
        yield return new WaitUntil(() => isEndZeroTurn());

        for (int i = 0; i < posObj.Length; i++)
        {
            BoxCollider2D collider2D = posObj[i].GetComponent<BoxCollider2D>();

            if (!collider2D.enabled)
                collider2D.enabled = true;
        }

        //CreateEnemy();
        //TurnCalculation();
        //NextTurn();
    }

    public GameObject cloneObj;

    void MakeCharacter(GameObject obj, string code)
    {
        SpawnPos spawn = obj.GetComponent<SpawnPos>();
        spawn.CreateCharacter(characterPrefab);
        characterObj.Add(spawn.linkedObj);
        spawn.character.Init(code);
    }

    void MoveClone()
    {
        if (cursorObj == null)
            return;

        if (cloneObj == null)
            return;

        SpawnPos spawn = cursorObj.GetComponent<SpawnPos>();

        if (spawn.character != null)
            return;

        SpriteRenderer spriteRenderer = cloneObj.GetComponent<SpriteRenderer>();

        // 소환수 스프라이트 넣기
        if (spriteRenderer.sprite == null)
            spriteRenderer.sprite = s;

        cloneObj.transform.position = cursorObj.transform.position;
    }

    public Sprite s;

    void CreateClone()
    {
        cloneObj = Instantiate(characterPrefab);
        Character character = cloneObj.GetComponent<Character>();
        character.Translucence();
    }

    void PlayerTurn()
    {

    }

    void SummonTurn()
    {

    }

    void EnemyTurn()
    {

    }

    bool isObjClick()
    {
        if (Input.GetMouseButtonDown(0) && cursorObj != null)
        {
            selectObj = cursorObj;
            return true;
        }

        return false;
    }

    void CheckCursorOn()
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

    int[] ConvertIntArray(string input)
    {
        string[] parts = input.Split('/');
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