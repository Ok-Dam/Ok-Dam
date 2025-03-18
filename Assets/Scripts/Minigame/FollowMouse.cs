using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowMouse : MonoBehaviour
{
    private bool followMouse = false;

    // 외부에서 FollowMouse를 활성화/비활성화할 수 있게 설정
    public void SetFollowMouse(bool follow)
    {
        followMouse = follow;
    }

    void Update()
    {
        if (followMouse)
        {
            // UI 요소가 아닌 게임 오브젝트만 마우스를 따라가게 처리
            if (GetComponent<RectTransform>() == null) // UI 요소가 아닐 경우에만 처리
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0; // z값을 0으로 설정 (2D 게임에서 Z축은 고정)
                transform.position = mousePos;
            }
        }
    }
}
