using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerMenu : MonoBehaviour {

    Text[] currentGamesText = new Text[7];
    Button[] currentGamesButton = new Button[7];

    // Use this for initialization
	void Start () {
        currentGamesText[0] = this.transform.Find("Game1Text").gameObject.GetComponent<Text>();
        currentGamesText[1] = this.transform.Find("Game2Text").gameObject.GetComponent<Text>();
        currentGamesText[2] = this.transform.Find("Game3Text").gameObject.GetComponent<Text>();
        currentGamesText[3] = this.transform.Find("Game4Text").gameObject.GetComponent<Text>();
        currentGamesText[4] = this.transform.Find("Game5Text").gameObject.GetComponent<Text>();
        currentGamesText[5] = this.transform.Find("Game6Text").gameObject.GetComponent<Text>();
        currentGamesText[6] = this.transform.Find("Game7Text").gameObject.GetComponent<Text>();

        currentGamesButton[0] = this.transform.Find("Game1Join").gameObject.GetComponent<Button>();
        currentGamesButton[1] = this.transform.Find("Game2Join").gameObject.GetComponent<Button>();
        currentGamesButton[2] = this.transform.Find("Game3Join").gameObject.GetComponent<Button>();
        currentGamesButton[3] = this.transform.Find("Game4Join").gameObject.GetComponent<Button>();
        currentGamesButton[4] = this.transform.Find("Game5Join").gameObject.GetComponent<Button>();
        currentGamesButton[5] = this.transform.Find("Game6Join").gameObject.GetComponent<Button>();
        currentGamesButton[6] = this.transform.Find("Game7Join").gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frWSame
	void Update () {

	}
}
