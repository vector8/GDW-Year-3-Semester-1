using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour
{
    public GameObject linePrefab;

    private bool drawingLine = false;
    private Vector3 lineStart, lineStartScreenSpace;
    private LineRenderer line;

    private const int NUM_LINE_VERTICES = 20;

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
                    lineStart = getMouseWorldPosition();
                    lineStartScreenSpace = Input.mousePosition;
                    //lineStart = hit.transform.position;
                    if(line != null)
                    {
                        DestroyImmediate(line.gameObject);
                    }
                    GameObject lineObject = Instantiate<GameObject>(linePrefab);
                    line = lineObject.GetComponent<LineRenderer>();
                }
            }
        }
        else if (!Input.GetMouseButton(0) && drawingLine)
        {
            drawingLine = false;
        }
        

        if(drawingLine)
        {
            //Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(lineStart);
            //Vector3 worldEndPos = Camera.main.ScreenToWorldPoint(getMouseWorldPosition());
            Vector3 worldEndPos = getMouseWorldPosition();

            Vector3 anchor1 = Camera.main.ScreenToWorldPoint(new Vector3(lineStartScreenSpace.x + (7f / 16f) * (Input.mousePosition.x - lineStartScreenSpace.x), Camera.main.pixelHeight * 3f / 4f, Camera.main.nearClipPlane + 10f));
            Vector3 anchor2 = Camera.main.ScreenToWorldPoint(new Vector3(lineStartScreenSpace.x + (9f / 16f) * (Input.mousePosition.x - lineStartScreenSpace.x), Camera.main.pixelHeight * 3f / 4f, Camera.main.nearClipPlane + 10f));

            //Vector3 anchor1 = new Vector3(-2.5f, 1f, 13f);
            //Vector3 anchor2 = new Vector3(2.5f, 1f, 13f);

            for (int i = 0; i < NUM_LINE_VERTICES; i++)
            {
                Vector3 pos = new Vector3();

                // set pos based on bezier function of startpos, endpos, and two anchor points at centre of screen.
                float u = (float)i / (float)(NUM_LINE_VERTICES - 1);

                pos = bezier(u, lineStart, anchor1, anchor2, worldEndPos);

                line.SetPosition(i, pos);
            }
        }
    }

    private Vector3 bezier(float u, Vector3 p0,  Vector3 c0,  Vector3 c1,  Vector3 p1)
    {
        return (u * u * u) * (-p0 + 3f * c0 - 3f * c1 + p1) +
            (u * u) * (3f * p0 - 6f * c0 + 3f * c1) +
            (u) * (-3f * p0 + 3f * c0) +
            p0;

        //return (1f - u) * (1f - u) * (1f - u) * p0 + 3f * (1f - u) * (1f - u) * u * c0 + 3f * (1 - u) * u * u * c1 + u * u * u * p1;
    }

    private Vector3 getMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 10f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    public void lockIn()
    {
        if(line != null)
        {
            DestroyImmediate(line.gameObject);
        }
    }
}
