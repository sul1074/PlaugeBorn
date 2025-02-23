using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // Cinemachine 카메라의 Transform
    [SerializeField] private float parallaxFactor = 0.1f; // 달이 카메라보다 느리게 따라오는 효과 (선택적)

    void Start()
    {
        // Cinemachine Virtual Camera의 Transform을 자동으로 찾음
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // 기본 메인 카메라 사용
        }
    }

    void Update()
    {
        // 카메라 위치에 따라 배경 이동
        Vector3 newPosition = cameraTransform.position;
        newPosition.z = transform.position.z; // Z축은 고정 (배경 깊이 유지)
        transform.position = newPosition * parallaxFactor + transform.position * (1 - parallaxFactor);
    }
}