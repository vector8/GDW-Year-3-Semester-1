using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour
{
    private bool drawingLine = false;
    private Vector2 lineStart;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name.StartsWith("Unit"))
                {
                    drawingLine = true;
                    lineStart = Input.mousePosition;
                }
            }
        }
        else if (!Input.GetMouseButton(0) && drawingLine)
        {
            drawingLine = false;
        }
        

        if(drawingLine)
        {
            
        }
    }
}
