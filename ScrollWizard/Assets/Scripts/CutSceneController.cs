using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct BlockUnit
{
	public GameObject obj;
	public SpriteRenderer sr;
	public GameObject hitObj;
	public SpriteRenderer hitSr;
}


public class CutSceneController : Singleton<CutSceneController>
{
	private BlockUnit[] arrDefaultBlockUnit = null;
	private BlockUnit[] arrCutScaneLeftUnit = null;
	private BlockUnit[] arrCutScaneRightUnit = null;

	[SerializeField]
	private float Interval;
	[SerializeField]
	private float Duration;
	[SerializeField]
	private float TargetScaleRatio;

	public void Initialize()
	{
		arrDefaultBlockUnit = new BlockUnit[8];
		for (int i = 0; i < arrDefaultBlockUnit.Length; i++)
		{
			arrDefaultBlockUnit[i].obj = new GameObject($"Sprite{i}");
			arrDefaultBlockUnit[i].obj.transform.parent = gameObject.transform;
			arrDefaultBlockUnit[i].sr = arrDefaultBlockUnit[i].obj.AddComponent<SpriteRenderer>();
			arrDefaultBlockUnit[i].hitObj = new GameObject("HitEffect");
			arrDefaultBlockUnit[i].hitObj.transform.transform.parent = arrDefaultBlockUnit[i].obj.transform;
			arrDefaultBlockUnit[i].hitSr = arrDefaultBlockUnit[i].hitObj.AddComponent<SpriteRenderer>();
		}
		ReSet();
	}

	private void ReSet()
	{
		if (arrDefaultBlockUnit == null)
			return;

		for (int i = 0; i < arrDefaultBlockUnit.Length; i++)
		{
			arrDefaultBlockUnit[i].obj.transform.position = Vector3.zero;
			arrDefaultBlockUnit[i].obj.transform.localScale = Vector3.one * 2;
			arrDefaultBlockUnit[i].sr.sprite = null;
			arrDefaultBlockUnit[i].hitObj.transform.position = Vector3.zero;
			arrDefaultBlockUnit[i].hitObj.transform.localScale = Vector3.one * 0.5f;
			arrDefaultBlockUnit[i].hitSr.sprite = null;
		}
	}

	public void StartCutScene(Sprite[] arrLeft, Sprite[] arrRight, Sprite[] hitLeft, Sprite[] hitRight)
	{
		if (arrLeft == null || arrRight == null || hitLeft == null || hitRight == null|| arrDefaultBlockUnit == null)
			return;
		if (arrLeft.Length != 4 || arrRight.Length != 4 || hitLeft.Length != 4 || hitRight.Length != 4)
			return;

		int leftCnt = 0;
		int rightCnt = 0;

		for (int i = 0; i < arrLeft.Length; i++)
		{
			if (arrLeft[i] != null) { leftCnt++; }
		}
		for (int i = 0; i < arrRight.Length; i++)
		{
			if (arrRight[i] != null) { rightCnt++; }
		}
		arrCutScaneLeftUnit = new BlockUnit[leftCnt];
		arrCutScaneRightUnit = new BlockUnit[rightCnt];
		leftCnt = 0; 
		rightCnt = 0;
		for(int i = 0; i < arrLeft.Length; i++)
		{
			if (arrLeft[i] != null) 
			{
				arrCutScaneLeftUnit[leftCnt] = arrDefaultBlockUnit[leftCnt];
				arrCutScaneLeftUnit[leftCnt].sr.sprite = arrLeft[i];
				if (hitLeft[i] != null)
					arrCutScaneLeftUnit[leftCnt].hitSr.sprite = hitLeft[i];
				leftCnt++;
			}
		}
		for (int i = 0; i < arrRight.Length; i++)
		{
			if (arrRight[i] != null)
			{
				arrCutScaneRightUnit[rightCnt] = arrDefaultBlockUnit[leftCnt + rightCnt];
				arrCutScaneRightUnit[rightCnt].sr.sprite = arrRight[i];
				if (hitRight[i] != null)
					arrCutScaneRightUnit[rightCnt].hitSr.sprite = hitRight[i];
				rightCnt++;
			}
		}
		for(int i = 0; i < arrCutScaneLeftUnit.Length; i++) 
		{
			arrCutScaneLeftUnit[i].sr.sortingOrder = i * 2;
			arrCutScaneLeftUnit[i].hitSr.sortingOrder = i * 2 + 1;
		}

		int j = 0;
		for (int i = arrCutScaneRightUnit.Length - 1; i >= 0 ; i--)
		{
			arrCutScaneRightUnit[i].sr.sortingOrder = j * 2;
			arrCutScaneRightUnit[i].hitSr.sortingOrder = j * 2 + 1;
			j++;
		}
		StartCoroutine(CutScene());
	}

	private IEnumerator CutScene()
	{
		for(int i = 0; i < arrCutScaneLeftUnit.Length; i++)
		{
			arrCutScaneLeftUnit[i].obj.transform.position = new Vector3((-Interval * arrCutScaneLeftUnit.Length) +  Interval * i, 0, 0);
		}

		for (int i = 0; i < arrCutScaneRightUnit.Length; i++)
		{
			arrCutScaneRightUnit[i].obj.transform.position = new Vector3((Interval * i) + Interval, 0, 0);
		}


		float elapsedTime = 0f;
		Vector3 startScale = arrCutScaneLeftUnit[0].obj.transform.localScale;
		while (elapsedTime < Duration)
		{
			for (int i = 0; i < arrCutScaneLeftUnit.Length; i++)
			{
				arrCutScaneLeftUnit[i].obj.transform.localScale = Vector3.Lerp(startScale, startScale * TargetScaleRatio, elapsedTime / Duration);
			}
			for (int i = 0; i < arrCutScaneRightUnit.Length; i++)
			{
				arrCutScaneRightUnit[i].obj.transform.localScale = Vector3.Lerp(startScale, startScale * TargetScaleRatio, elapsedTime / Duration);
			}
			elapsedTime += Time.deltaTime;
			yield return null; // 한 프레임 대기
		}
		ReSet();
	}
}
