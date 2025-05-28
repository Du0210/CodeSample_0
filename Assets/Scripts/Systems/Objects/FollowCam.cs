using HDU.Managers;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;           // 따라갈 대상
    public Vector3 offset = new Vector3(0, 0, -10); // 상대 위치 오프셋
    public float followSpeed = 1f;     // 따라가는 속도 (Lerp 계수)
    private bool _isUpdate = false;

    private void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnFollowCamUpdate, (bool b) => SetState(b));
    }
    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnFollowCamUpdate, (bool b) => SetState(b));
    }

    private void SetState(bool isUpdate)
    {
        _isUpdate = isUpdate;
    }

    void LateUpdate()
    {
        if (target == null || !_isUpdate) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
