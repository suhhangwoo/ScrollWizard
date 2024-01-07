using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public struct BlockUnit
{
	public GameObject obj;
	public SpriteRenderer sr;
	public GameObject hitObj;
	public SpriteRenderer hitSr;
}

public enum CutState
{
	None, //공격이 아님
	Left,
	Right
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
	[SerializeField]
	private float Distance;

	private bool rightEmpty;
	private bool leftEmpty;
	private CutState state;
	private bool[] realDie;
	private Vector3 startScale;
	public void Initialize()
	{
		arrDefaultBlockUnit = new BlockUnit[8];
		rightEmpty = false;
		leftEmpty = false;
		realDie = new bool[4];
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
		rightEmpty = false;
		leftEmpty = false;
		for (int i = 0; i < arrDefaultBlockUnit.Length; i++)
		{
			arrDefaultBlockUnit[i].obj.transform.position = Vector3.zero;
			arrDefaultBlockUnit[i].obj.transform.localScale = Vector3.one * 2;
			arrDefaultBlockUnit[i].sr.sprite = null;
			arrDefaultBlockUnit[i].hitObj.transform.position = Vector3.zero;
			arrDefaultBlockUnit[i].hitObj.transform.localScale = Vector3.one * 0.5f;
			arrDefaultBlockUnit[i].hitSr.sprite = null;
			arrDefaultBlockUnit[i].sr.color = Color.white;
		}
		for(int i = 0; i < realDie.Length; i++)
		{
			realDie[i] = false;
		}
		startScale = arrDefaultBlockUnit[0].obj.transform.localScale;
	}

	public void AreaAttack(Sprite[] arrLeft, Sprite[] arrRight, Sprite hit, bool[] die)
	{
		Sprite[] nullsptites = new Sprite[4];
		Sprite[] hitSprites= new Sprite[4];
		for (int i = 0; i < hitSprites.Length; i++)
		{
			hitSprites[i] = hit;
		}
		StartCutScene(arrLeft, arrRight, nullsptites, hitSprites, die, CutState.Right);
	}

	public void AreaAttackEnemy(Sprite[] arrLeft, Sprite[] arrRight, Sprite hit, bool[] die)
	{
		Sprite[] nullsptites = new Sprite[4];
		Sprite[] hitSprites = new Sprite[4];
		for (int i = 0; i < hitSprites.Length; i++)
		{
			hitSprites[i] = hit;
		}
		StartCutScene(arrLeft, arrRight, hitSprites, nullsptites, die, CutState.Left);
	}


	public void StartCutScene(Sprite[] arrLeft, Sprite[] arrRight, Sprite[] hitLeft, Sprite[] hitRight, bool[] die, CutState state)
	{
		if (arrLeft == null || arrRight == null || hitLeft == null || hitRight == null|| arrDefaultBlockUnit == null)
			return;
		if (arrLeft.Length != 4 || arrRight.Length != 4 || hitLeft.Length != 4 || hitRight.Length != 4)
			return;
		ReSet();
		this.state = state; 
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
				if (state == CutState.Left)
					realDie[leftCnt] = die[i];
				leftCnt++;
			}
		}
		if (leftCnt == 0)
			leftEmpty = true;
		for (int i = 0; i < arrRight.Length; i++)
		{
			if (arrRight[i] != null)
			{
				arrCutScaneRightUnit[rightCnt] = arrDefaultBlockUnit[leftCnt + rightCnt];
				arrCutScaneRightUnit[rightCnt].sr.sprite = arrRight[i];
				if (hitRight[i] != null)
					arrCutScaneRightUnit[rightCnt].hitSr.sprite = hitRight[i];
				if (state == CutState.Right)
					realDie[rightCnt] = die[i];
				rightCnt++;
			}
		}
		if (rightCnt == 0)
			rightEmpty = true;
		for (int i = 0; i < arrCutScaneLeftUnit.Length; i++) 
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
		if (leftEmpty && rightEmpty)
			return;
		if (leftEmpty || rightEmpty)
		{
			BlockUnit[] blocks;
			if (leftEmpty)
				blocks = arrCutScaneRightUnit;
			else
				blocks = arrCutScaneLeftUnit;
			float distanceFromCenter;

			int centerIndex = blocks.Length / 2;
			for (int i = 0; i < blocks.Length; i++)
			{
				if (blocks.Length % 2 == 0)
				{
					distanceFromCenter = (i - centerIndex + 0.5f) * Interval;
				}
				else // 홀수 개의 스프라이트인 경우
				{
					distanceFromCenter = (i - centerIndex) * Interval;
				}
				blocks[i].obj.transform.position = new Vector3(distanceFromCenter, 0, 0);
			}
		}
		else
		{
			for (int i = 0; i < arrCutScaneLeftUnit.Length; i++)
			{
				arrCutScaneLeftUnit[i].obj.transform.position = new Vector3((-Interval * arrCutScaneLeftUnit.Length) + Interval * i, 0, 0);
			}

			for (int i = 0; i < arrCutScaneRightUnit.Length; i++)
			{
				arrCutScaneRightUnit[i].obj.transform.position = new Vector3((Interval * i) + Interval, 0, 0);
			}
		}
		StartCoroutine(CutScene());
	}

	private IEnumerator CutScene()
	{
		
		float elapsedTime = 0f;
		float leftDistance = 0.0f;
		float rightDistance = 0.0f;
		if(state == CutState.Left)
		{
			leftDistance = -Distance;
			rightDistance = Distance * 0.5f;
		}
		else if(state == CutState.Right)
		{
			leftDistance = -Distance * 0.5f;
			rightDistance = Distance;
		}

		float[] leftStartX = new float[arrCutScaneLeftUnit.Length];
		float[] rightStartX = new float[arrCutScaneRightUnit.Length];
		for(int i = 0; i < leftStartX.Length; i++)
			leftStartX[i] = arrCutScaneLeftUnit[i].obj.transform.position.x;
		for (int i = 0; i < rightStartX.Length; i++)
			rightStartX[i] = arrCutScaneRightUnit[i].obj.transform.position.x;

		while (elapsedTime < Duration)
		{
			for (int i = 0; i < arrCutScaneLeftUnit.Length; i++)
			{
				arrCutScaneLeftUnit[i].obj.transform.position = new Vector3(Mathf.Lerp(leftStartX[i], leftStartX[i]+ leftDistance, elapsedTime / Duration), 0, 0);
				arrCutScaneLeftUnit[i].obj.transform.localScale = Vector3.Lerp(startScale, startScale * TargetScaleRatio, elapsedTime / Duration);
				if(state == CutState.Left && realDie[i] && elapsedTime > Duration / 2)
				{
					Color c = arrCutScaneLeftUnit[i].sr.color;
					c.r = c.g = c.b = Mathf.Lerp(1, 0, (elapsedTime - (Duration / 2)) / (Duration - (Duration / 2)));
					arrCutScaneLeftUnit[i].sr.color = c;
				}
			}
			for (int i = 0; i < arrCutScaneRightUnit.Length; i++)
			{
				arrCutScaneRightUnit[i].obj.transform.position = new Vector3(Mathf.Lerp(rightStartX[i], rightStartX[i] + rightDistance, elapsedTime / Duration), 0, 0);
				arrCutScaneRightUnit[i].obj.transform.localScale = Vector3.Lerp(startScale, startScale * TargetScaleRatio, elapsedTime / Duration);
				if (state == CutState.Right && realDie[i] && elapsedTime > Duration / 2)
				{
					Color c = arrCutScaneRightUnit[i].sr.color;
					c.r = c.g = c.b = Mathf.Lerp(1, 0, (elapsedTime - (Duration / 2)) / (Duration - (Duration / 2)));
					arrCutScaneRightUnit[i].sr.color = c;
				}
			}
			elapsedTime += Time.deltaTime;
			yield return null; // 한 프레임 대기
		}
		ReSet();
	}
}
