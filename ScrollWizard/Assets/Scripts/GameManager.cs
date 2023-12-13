using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject selectObj;

    void Update()
    {
        Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero);

        // Ray�� � Collider�� �浹�ߴ��� Ȯ��
        if (hit.collider != null)
        {
            // �浹�� Collider�� GameObject ��������
            selectObj = hit.collider.gameObject;
        }
        else
        {
            selectObj = null;
        }
    }
}
