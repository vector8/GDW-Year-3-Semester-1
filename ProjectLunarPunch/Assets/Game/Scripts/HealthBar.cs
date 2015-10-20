using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Unit target;
    public RectTransform bar;

    // Update is called once per frame
    void Update()
    {
        float width = (target.hp / target.maxHp) * 100f;
        if (width < 0f)
        {
            width = 0;
        }
        bar.sizeDelta = new Vector2(width, bar.sizeDelta.y);

        gameObject.SetActive(!target.isDead());
    }
}
