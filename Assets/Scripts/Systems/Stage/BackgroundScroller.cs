using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float parallaxFactor = 0.1f; // ī�޶� �̵��� ���� ��� ������ (0.0 ~ 1.0)
    [SerializeField] private float autoScrollSpeed = 0.02f; // �ð� ���
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
        // ī�޶� �̵��� ���
        float deltaCamX = Camera.position.x - _lastCamX;
        _lastCamX = Camera.position.x;
        Vector3 pos = transform.position;
        pos.x = _lastCamX;

        // ��ũ�� ����: ī�޶� ��� + �ð� ���
        for (int i = 0; i < Materials.Length; i++)
        {
            _offsets[i].x += deltaCamX * parallaxFactor;
            _offsets[i].x += autoScrollSpeed * Time.deltaTime;

            Materials[i].mainTextureOffset = _offsets[i];
        }
        transform.position = pos;
    }
}
