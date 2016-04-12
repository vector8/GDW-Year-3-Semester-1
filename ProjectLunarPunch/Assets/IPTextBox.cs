using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IPTextBox : MonoBehaviour {

	public void TextChange()
    {
        NetworkWrapper.serverIP = this.GetComponentInChildren<Text>().text;
    }
}
