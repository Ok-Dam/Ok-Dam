using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Scene 전환을 위한 네임스페이스

 // Scene 전환을 위한 네임스페이스

public class ClickEffect : MonoBehaviour
{
    public GameObject clickEffectPrefab;  // 에셋에서 가져온 파티클 효과 프리팹
    public float effectDuration = 5f;     // 이펙트가 지속될 시간 (초)
    private GameObject effect;            // 생성된 파티클 효과를 저장할 변수

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭 (왼쪽 마우스 버튼)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 레이 발사
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // 물체에 충돌한 경우
            {
                // 클릭한 위치에 파티클 시스템 생성
                effect = Instantiate(clickEffectPrefab, hit.point, Quaternion.identity);

                // 5초 후에 이펙트를 삭제
                Destroy(effect, effectDuration); // effectDuration 시간 후에 삭제
            }
        }
        if (effect != null && !effect.activeInHierarchy)
        {
            SceneManager.LoadScene("Gudle_End");  // "End"라는 씬을 로드
        }
    }

    // 5초 후 자동으로 End 씬으로 전환
    private void Start()
    {
        // Invoke를 Start()에서 제거하고, 파티클 시스템이 사라질 때 씬 전환을 처리할 예정
    }

    // 파티클이 사라지면 End 씬으로 전환
 
}
