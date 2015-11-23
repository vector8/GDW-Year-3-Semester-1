using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Unit target;
    public GameObject bar;

    void Start()
    {
        target = GetComponentInParent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = (target.hp / target.maxHp) * 130f;
        if (width < 0f)
        {
            width = 0;
        }

        bar.transform.localScale = new Vector3(width, bar.transform.localScale.y, bar.transform.localScale.z);

        Vector3 lookAtPosition = Camera.main.transform.position;
        lookAtPosition.x = transform.position.x;
        transform.LookAt(lookAtPosition);
    }
}
