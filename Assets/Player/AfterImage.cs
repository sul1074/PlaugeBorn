using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField] private GameObject afterimagePrefab; // 잔상 프리팹
    private float afterimageInterval = 0.03f; // 잔상 생성 간격
    private float fadeDuration = 0.1f; // 잔상이 사라지는 시간
    private bool isGhosting = false;

    public void StartGhosting()
    {
        if (!isGhosting)
        {
            isGhosting = true;
            StartCoroutine(CreateAfterimage());
        }
    }

    public void StopGhosting()
    {
        isGhosting = false;
    }

    private IEnumerator CreateAfterimage()
    {
        while (isGhosting)
        {
            GameObject ghost = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            ghost.transform.localScale = transform.localScale; // 크기 유지

            SpriteRenderer[] ghostRenderers = ghost.GetComponentsInChildren<SpriteRenderer>();
            SpriteRenderer[] playerRenderers = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < ghostRenderers.Length; i++)
            {
                ghostRenderers[i].sprite = playerRenderers[i].sprite; // 현재 스프라이트 복사
                ghostRenderers[i].color = new Color(0, 0, 0, 0.5f); // 반투명 적용
                ghostRenderers[i].sortingOrder = playerRenderers[i].sortingOrder - 1; // 플레이어보다 뒤로 설정 (근데 안되는 것 같음)
            }

            Destroy(ghost, fadeDuration); // fadeDuration 이후 삭제
            yield return new WaitForSeconds(afterimageInterval);
        }
    }
}
