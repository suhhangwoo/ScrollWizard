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

        // Ray가 어떤 Collider와 충돌했는지 확인
        if (hit.collider != null)
        {
            // 충돌한 Collider의 GameObject 가져오기
            selectObj = hit.collider.gameObject;
        }
        else
        {
            selectObj = null;
        }
    }
}
