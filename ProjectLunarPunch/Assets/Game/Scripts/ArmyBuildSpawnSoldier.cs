using UnityEngine;
using System.Collections;

public class ArmyBuildSpawnSoldier : MonoBehaviour
{

    public GameObject soldierTest;

    public GameObject[] spawnLoc;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnCharacter()
    {
        Instantiate(soldierTest, spawnLoc[0].transform.position, spawnLoc[0].transform.rotation);
    }


}
