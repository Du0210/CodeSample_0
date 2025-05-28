using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float parallaxFactor = 0.1f; // 카메라 이동에 대한 배경 반응도 (0.0 ~ 1.0)
    [SerializeField] private float autoScrollSpeed = 0.02f; // 시간 기반
    [SerializeField] Material[] Materials;

    [SerializeField] private Transform Camera;
    private Vector2[] _offsets;
    private float _lastCamX;

    private void Awake()
    {
        _offsets = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };
        for (int i = 0; i < Materials.Length; i++)
            _offsets[i] = Materials[i].mainTextureOffset;
        _lastCamX = Camera.position.x;
    }

    private void LateUpdate()
    {
        // 카메라 이동량 계산
        float deltaCamX = Camera.position.x - _lastCamX;
        _lastCamX = Camera.position.x;
        Vector3 pos = transform.position;
        pos.x = _lastCamX;

        // 스크롤 조합: 카메라 기반 + 시간 기반
        for (int i = 0; i < Materials.Length; i++)
        {
            _offsets[i].x += deltaCamX * parallaxFactor;
            _offsets[i].x += autoScrollSpeed * Time.deltaTime;

            Materials[i].mainTextureOffset = _offsets[i];
        }
        transform.position = pos;
    }
}
