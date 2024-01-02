using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

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

public class GameManager : Singleton<GameManager>
{
    public GameObject cursorObj; // 커서 위치에 있는 오브젝트
    public GameObject selectObj; // 클릭한 오브젝트

    public GameObject characterPrefab; // 적 프리팹

    public List<Character> turnList; // 진행될 턴 순서

    public SkillInfo[] skillInfoArr; // 플레이어 보유 스킬 정보

    // 0 1 2 3 | 4 5 6 7
    // 3 2 1 0 | 0 1 2 3
    public GameObject[] blockObj; // 캐릭터가 움직일 8칸의 위치
    public BlockData[] blockDataArr; // 8칸의 정보
    public List<GameObject> characterObj; // 현재 생성된 캐릭터들

    public string activeSkillcode; // 버튼을 누른 스킬의 코드값
    public int summonSkillCount; // 사용하지 않은 소환수 스킬의 개수

    public enum SpriteKind { IDLE, SINGLE, MULTI, BUFF, SUMMON, MOVE, HIT }
    public enum BattleState { START, PLAYER, SUMMON, ENEMY, WIN, LOSE }
    public BattleState state;

    private void Start()
    {
        cursorObj = null;
        activeSkillcode = string.Empty;
        summonSkillCount = 0;

        turnList = new List<Character>();
        skillInfoArr = new SkillInfo[6];
        blockDataArr = new BlockData[8];
        characterObj = new List<GameObject>();
        
        state = BattleState.START;

        for (int i = 0; i < blockDataArr.Length; i++)
            blockDataArr[i].Init(blockObj[i]);

        PlayerPrefs.SetInt("pos", 2);
        PlayerPrefs.SetInt("chapter", 1);
        PlayerPrefs.SetString("skill code", "SU_0001/SU_0002/FI_0001/IC_0001/NA_0001/NO_0001");
        PlayerPrefs.SetString("skill count", "10/10/10/10/10/10");

        string[] tmpStr = PlayerPrefs.GetString("skill code").Split('/');
        int[] tmpInt = DataManager.Instance.ConvertIntArray(PlayerPrefs.GetString("skill count"), '/');

        for (int i = 0; i < tmpStr.Length; i++)
        {
            skillInfoArr[i].Init(tmpStr[i], tmpInt[i]);

            if (tmpStr[i][0].Equals('S'))
            {
                summonSkillCount++;
            }
        }

        StartCoroutine(ZeroTurn());
    }

    private void Update()
    {
        CheckCursorOn();
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

    public void NextTurn()
    {
        if (turnList[0].code.Equals("Player"))
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

    public void CreateSummon(string code)
    {
        BlockInfo blockInfo = selectObj.GetComponent<BlockInfo>();
        int idx = blockInfo.index;

        if (blockDataArr[idx].isOnCharacter)
        {
            selectObj = null;
            return;
        }

        int count = CloneManager.Instance.ResetCloneArr();
        List<GameObject> list = new List<GameObject>();

        if (count == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                if (blockDataArr[i].isOnCharacter)
                    continue;

                list.Add(blockDataArr[i].obj);
            }
        }
        else if (count == 2)
        {
            if (idx == 3)
            {
                for (int i = 3; i >= 0; i--)
                {
                    if (list.Count == count)
                        break;

                    if (blockDataArr[i].isOnCharacter)
                        continue;

                    list.Add(blockDataArr[i].obj);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    idx %= 4;

                    if (list.Count == count)
                        break;

                    if (blockDataArr[idx++].isOnCharacter)
                        continue;

                    list.Add(blockDataArr[idx].obj);
                }
            }
        }

        if (list.Count > 1)
        {
            StartCoroutine(MakeCharacter(list, code));
        }
        else
        {
            StartCoroutine(MakeCharacter(selectObj, code));
        }

        selectObj = null;
        activeSkillcode = string.Empty;
        summonSkillCount--;

        // 2칸 소환수 소환했다면 다른 2칸 소환수 버튼 비활성화
    }

    private int AllyFieldCount()
    {
        int count = 0;

        for (int i = 0; i < 4; i++)
        {
            if (blockDataArr[i].isOnCharacter)
                count++;
        }

        return count;
    }

    private IEnumerator ZeroTurn()
    {
        // 미리 지정된 위치에 플레이어 생성
        int idx = PlayerPrefs.GetInt("pos");
        StartCoroutine(MakeCharacter(blockObj[idx], "Player"));

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

    /// <summary>
    /// block 위치에 캐릭터 생성
    /// </summary>
    /// <param name="block">2칸 몬스터면 왼쪽 블럭 입력</param>
    /// <param name="code">캐릭터 코드</param>
    /// <returns></returns>
    private IEnumerator MakeCharacter(GameObject block, string code)
    {
        DataManager.Instance.LoadCharacterData(code);

        yield return new WaitUntil(() => isAssetNull());

        CharacterData characterData = DataManager.Instance.obj as CharacterData;
        BlockInfo blockInfo = block.GetComponent<BlockInfo>();
        int idx = blockInfo.index;

        if (characterData.Size == 2)
        {
            if (idx == 3 || idx == 7 || blockDataArr[idx + 1].isOnCharacter)
            {
                blockInfo.CreateCharacter(characterPrefab, blockDataArr[idx - 1].obj);
                blockDataArr[idx - 1].isOnCharacter = true;
            }
            else
            {
                blockInfo.CreateCharacter(characterPrefab, blockDataArr[idx + 1].obj);
                blockDataArr[idx + 1].isOnCharacter = true;
            }
        }
        else
        {
            blockInfo.CreateCharacter(characterPrefab);
        }

        blockDataArr[idx].isOnCharacter = true;
        characterObj.Add(blockInfo.linkedObj);
        blockInfo.character.Init(characterData);

        DataManager.Instance.obj = null;
    }

    private IEnumerator MakeCharacter(List<GameObject> blockList, string code)
    {
        DataManager.Instance.LoadCharacterData(code);

        yield return new WaitUntil(() => isAssetNull());

        CharacterData characterData = DataManager.Instance.obj as CharacterData;

        BlockInfo blockInfo = null;
        int idx = 0;

        for (int i = 0; i < blockList.Count; i++)
        {
            blockInfo = blockList[i].GetComponent<BlockInfo>();
            idx = blockInfo.index;

            if (characterData.Size == 2)
            {
                if (idx == 3 || idx == 7)
                {
                    blockInfo.CreateCharacter(characterPrefab, blockDataArr[idx - 1].obj);
                    blockDataArr[idx - 1].isOnCharacter = true;
                }
                else
                {
                    blockInfo.CreateCharacter(characterPrefab, blockDataArr[idx + 1].obj);
                    blockDataArr[idx + 1].isOnCharacter = true;
                }
            }
            else
            {
                blockInfo.CreateCharacter(characterPrefab);
            }

            blockDataArr[idx].isOnCharacter = true;
            characterObj.Add(blockInfo.linkedObj);
            blockInfo.character.Init(characterData);
        }        

        DataManager.Instance.obj = null;
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

    public bool isEndZeroTurn()
    {
        if (summonSkillCount == 0) // 모든 소환수 스킬을 사용했거나 필드가 꽉 차있으면 소환 턴 종료
            return true;

        if (AllyFieldCount() == 4)
            return true;

        return false;
    }

    public bool isAssetNull()
    {
        return DataManager.Instance.obj != null;
    }

    public bool isObjClick()
    {
        if (Input.GetMouseButtonDown(0) && cursorObj != null)
        {
            selectObj = cursorObj;
            return true;
        }

        return false;
    }

    public void CheckCursorOn()
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
}