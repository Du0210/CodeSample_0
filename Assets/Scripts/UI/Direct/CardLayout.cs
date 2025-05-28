using System.Collections.Generic;
using UnityEngine;

public class CardLayout : MonoBehaviour 
{
    public List<RectTransform> cards = new();   // 정렬할 카드 목록
    public float spacing = 100f;
    public float maxRotation = 30f;

    public void ApplyFanLayout()
    {
        int count = cards.Count;
        if (count == 0) return;

        float angleStep = count > 1 ? maxRotation * 2f / (count - 1) : 0f;
        float xStep = count > 1 ? spacing : 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = -maxRotation + angleStep * i;
            float x = -spacing * (count - 1) / 2f + xStep * i;

            var card = cards[i];
            card.anchoredPosition = new Vector2(x, 0f);  // y는 고정
            card.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
