using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public void PushSkillButton1()
    {
        Debug.Log("skiil 1 : " + GameManager.Instance.skillInfoArr[0].code);
        GameManager.Instance.activeSkillcode = GameManager.Instance.skillInfoArr[0].code;

        if (GameManager.Instance.activeSkillcode.Contains("SU"))
            StartCoroutine(CloneManager.Instance.CreateClone());
    }

    public void PushSkillButton2()
    {
        Debug.Log("skiil 2 : " + GameManager.Instance.skillInfoArr[1].code);
        GameManager.Instance.activeSkillcode = GameManager.Instance.skillInfoArr[1].code;

        if (GameManager.Instance.activeSkillcode.Contains("SU"))
            StartCoroutine(CloneManager.Instance.CreateClone());
    }

    public void PushSkillButton3()
    {
        Debug.Log("skiil 3 : " + GameManager.Instance.skillInfoArr[2].code);
        GameManager.Instance.activeSkillcode = GameManager.Instance.skillInfoArr[2].code;

        if (GameManager.Instance.activeSkillcode.Contains("SU"))
            StartCoroutine(CloneManager.Instance.CreateClone());
    }

    public void PushSkillButton4()
    {
        Debug.Log("skiil 4 : " + GameManager.Instance.skillInfoArr[3].code);
        GameManager.Instance.activeSkillcode = GameManager.Instance.skillInfoArr[3].code;

        if (GameManager.Instance.activeSkillcode.Contains("SU"))
            StartCoroutine(CloneManager.Instance.CreateClone());
    }

    public void PushSkillButton5()
    {
        Debug.Log("skiil 5 : " + GameManager.Instance.skillInfoArr[4].code);
        GameManager.Instance.activeSkillcode = GameManager.Instance.skillInfoArr[4].code;

        if (GameManager.Instance.activeSkillcode.Contains("SU"))
            StartCoroutine(CloneManager.Instance.CreateClone());
    }

    public void PushSkillButton6()
    {
        Debug.Log("skiil 6 : " + GameManager.Instance.skillInfoArr[5].code);
        GameManager.Instance.activeSkillcode = GameManager.Instance.skillInfoArr[5].code;

        if (GameManager.Instance.activeSkillcode.Contains("SU"))
            StartCoroutine(CloneManager.Instance.CreateClone());
    }

    public void PushInventoryButton()
    {

    }

    public void PushMoveButton()
    {

    }

    public void PushNextTurnButton()
    {
        if (GameManager.Instance.state == GameManager.BattleState.START)
        {
            GameManager.Instance.summonSkillCount = 0;
            return;
        }

        GameManager.Instance.NextTurn();
    }
}
