using UnityEngine;
using TMPro;
using HDU.Managers;
using HDU.Define;
using UnityEngine.UI;

public class StageTile : MonoBehaviour, IClickable
{
    [SerializeField] private PolygonCollider2D Collider;
    [SerializeField] private TextMeshPro StageText;
    [SerializeField] private int StageIndex;
    [SerializeField] private Sprite[] StarSprites;
    [SerializeField] private SpriteRenderer SRSlot;
    [SerializeField] private SpriteRenderer[] ImgStar;
    [SerializeField] private CoreDefine.EStageType StageType;
    [SerializeField] private Color[] ActiveColors;
    private int _stageScore;
    
    public void OnClick2D()
    {
        if (Managers.Stage.StageData.StageIndexList[(int)CoreDefine.EStageType.First] < StageIndex)
            return;
        Managers.Event.InvokeEvent(CoreDefine.EEventType.OnOpenLobbyType, CoreDefine.ELobbySceneType.Deck);
        Managers.Event.InvokeEvent(CoreDefine.EEventType.OnClickStageTile, StageType, StageIndex);
        Managers.Stage.StageIndex = StageIndex;
    }

    public void OnEnable()
    {
        SetUI();
    }

    private void SetUI()
    {
        _stageScore = Managers.Stage.GetStageScore(StageType, StageIndex - 1);
        for (int i = 0; i < ImgStar.Length; i++)
            ImgStar[i].sprite = _stageScore >= i + 1 ? StarSprites[1] : StarSprites[0];
        SRSlot.color = (Managers.Stage.StageData.StageIndexList[(int)CoreDefine.EStageType.First] < StageIndex) ? ActiveColors[0] : ActiveColors[1];
    }
}
