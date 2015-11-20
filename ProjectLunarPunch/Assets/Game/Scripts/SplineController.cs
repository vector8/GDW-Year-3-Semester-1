using UnityEngine;
using System.Collections;

public class SplineController : MonoBehaviour
{

    public GameObject nodeA;
    public GameObject nodeB;
    public GameObject nodeC;
    public GameObject nodeD;
    public float speed = 0;

    Vector3 Catmull(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u)
    {
        return 0.5f * (u * u * u * (-a + 3f * b - 3f * c + d) +
                       u * u * (2f * a - 5f * b + 4f * c - d) +
                       u * (-a + c) + 
                       2f * b);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
