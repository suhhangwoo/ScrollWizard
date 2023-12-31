using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public struct CloneInfo
{
    public GameObject obj;
    public bool isBig;

    public void Init()
    {
        obj = null;
        isBig = false;
    }
}

public class CloneManager : Singleton<CloneManager>
{
    public CloneInfo[] cloneArr; // ��ȯ���� ���� ��ġ�� �����ִ� ȯ��
    private string summonCode;

    private void Start()
    {
        summonCode = string.Empty;
        cloneArr = new CloneInfo[3];

        for (int i = 0; i < cloneArr.Length; i++)
            cloneArr[i].Init();
    }

    private void Update()
    {
        if (cloneArr[0].obj != null)
        {
            MoveClone();

            if (GameManager.Instance.isObjClick())
                GameManager.Instance.CreateSummon(summonCode);
        }
    }

    private void MoveClone()
    {
        if (GameManager.Instance.cursorObj == null)
        {
            ChangeCloneActive(cloneArr.Length, false);
            return;
        }

        BlockInfo blockInfo = GameManager.Instance.cursorObj.GetComponent<BlockInfo>();
        int idx = blockInfo.index;

        if (GameManager.Instance.blockDataArr[idx].isOnCharacter)
        {
            ChangeCloneActive(cloneArr.Length, false);
            return;
        }

        int count = 0;

        for (int i = 0; i < cloneArr.Length; i++)
        {
            if (cloneArr[i].obj == null)
                break;

            count++;
        }

        int tmp = 0;

        if (count == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                if (GameManager.Instance.blockDataArr[i].isOnCharacter)
                    continue;

                cloneArr[tmp++].obj.transform.position = GameManager.Instance.blockDataArr[i].obj.transform.position;
            }
        }
        else if (count == 2)
        {
            if (idx == 3)
            {
                for (int i = 3; i >= 0; i--)
                {
                    if (tmp == count)
                        break;

                    if (GameManager.Instance.blockDataArr[i].isOnCharacter)
                        continue;

                    cloneArr[tmp++].obj.transform.position = GameManager.Instance.blockDataArr[i].obj.transform.position;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    idx %= 4;

                    if (GameManager.Instance.blockDataArr[idx++].isOnCharacter)
                        continue;

                    cloneArr[tmp++].obj.transform.position = GameManager.Instance.blockDataArr[idx - 1].obj.transform.position;
                }
            }
        }
        else
        {
            Vector2 v = GameManager.Instance.cursorObj.transform.position;

            if (cloneArr[0].isBig)
            {
                if (idx == 3)
                {
                    if (GameManager.Instance.blockDataArr[idx - 1].isOnCharacter)
                        return;

                    v.x = (v.x + GameManager.Instance.blockDataArr[idx - 1].obj.transform.position.x) / 2;
                }
                else
                {
                    if (GameManager.Instance.blockDataArr[idx + 1].isOnCharacter)
                        return;

                    v.x = (v.x + GameManager.Instance.blockDataArr[idx + 1].obj.transform.position.x) / 2;
                }
            }

            cloneArr[0].obj.transform.position = v;
        }

        ChangeCloneActive(count, true);
    }

    public IEnumerator CreateClone()
    {        
        // clone �����ؼ� �������ϰ�
        cloneArr[0].obj = Instantiate(GameManager.Instance.characterPrefab);
        Character character = cloneArr[0].obj.GetComponent<Character>();
        character.Translucence();

        // ��ȯ�� ��ų���� ���� ��ȯ������ ����        
        DataManager.Instance.LoadSkillData(GameManager.Instance.activeSkillcode);

        yield return new WaitUntil(() => GameManager.Instance.isAssetNull());

        SkillData skillData = DataManager.Instance.Obj as SkillData;
        string num = string.Empty;

        for (int i = 0; i < skillData.KeywordList.Count; i++)
        {
            if (skillData.KeywordList[i].Contains("��ȯ"))
            {
                summonCode = skillData.KeywordList[i].Replace("��ȯ", string.Empty);
            }
            else if (skillData.KeywordList[i].Contains("����"))
            {
                num = skillData.KeywordList[i].Replace("����", string.Empty);
            }
        }

        if (int.TryParse(num, out int n))
        {
            for (int i = 1; i < n; i++)
            {
                cloneArr[i].obj = Instantiate(GameManager.Instance.characterPrefab);
                character = cloneArr[i].obj.GetComponent<Character>();
                character.Translucence();
            }
        }

        DataManager.Instance.Obj = null;

        DataManager.Instance.LoadCharacterData(summonCode);

        yield return new WaitUntil(() => GameManager.Instance.isAssetNull());

        CharacterData characterData = DataManager.Instance.Obj as CharacterData;

        if (characterData.Size == 2)
            cloneArr[0].isBig = true;

        // ��ȯ�� ��������Ʈ �ֱ�
        SpriteRenderer spriteRenderer = null;

        for (int i = 0; i < cloneArr.Length; i++)
        {
            if (cloneArr[i].obj == null)
                break;

            spriteRenderer = cloneArr[i].obj.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = characterData.Sprite[(int)SpriteKind.IDLE];
            cloneArr[i].obj.SetActive(false);
        }

        DataManager.Instance.Obj = null;
    }

    /// <summary>
    /// CloneArr �ʱ�ȭ
    /// </summary>
    /// <returns>�ʱ�ȭ�� Clone�� ����</returns>
    public int ResetCloneArr()
    {
        int count = 0;

        for (int i = 0; i < cloneArr.Length; i++)
        {
            if (cloneArr[i].obj == null)
                break;

            Destroy(cloneArr[i].obj);
            cloneArr[i].Init();
            count++;
        }

        return count;
    }

    private void ChangeCloneActive(int count, bool flag)
    {
        for (int i = 0; i < count; i++)
        {
            if (cloneArr[i].obj == null)
                return;

            if (flag)
            {
                if (!cloneArr[i].obj.activeSelf)
                    cloneArr[i].obj.SetActive(true);
            }
            else
            {
                if (cloneArr[i].obj.activeSelf)
                    cloneArr[i].obj.SetActive(false);
            }
        }        
    }
}
