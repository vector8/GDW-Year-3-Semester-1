using UnityEngine;
using System.Collections;

public class ArmyBuildSpawnSoldier : MonoBehaviour
{

    public GameObject soldierTest;
    public int counter = 0;

    public GameObject[] spawnLoc;

    bool isButtonPressed = false;

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
        Instantiate(soldierTest, spawnLoc[counter].transform.position, spawnLoc[counter].transform.rotation);
        counter++;
    }


}
