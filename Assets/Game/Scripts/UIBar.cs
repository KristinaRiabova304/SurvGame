using UnityEngine;

// Простая полоска (HP / XP): ширина fill = значение 0..1.
public class UIBar : MonoBehaviour
{
    [SerializeField] RectTransform fill;

    public void Set(float value01)
    {
        fill.anchorMax = new Vector2(Mathf.Clamp01(value01), 1f);
    }
}
