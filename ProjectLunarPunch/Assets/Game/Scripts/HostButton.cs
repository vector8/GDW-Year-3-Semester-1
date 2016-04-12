using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HostButton : MonoBehaviour {

    public GameObject HostText;
    public GameObject HostIP;

    public void HostOnClick()
    {
        if (this.GetComponentInChildren<Text>().text == "HOST")
        {
            this.GetComponentInChildren<Text>().text = "CLIENT";
            NetworkWrapper.isHost = false;
            HostText.SetActive(true);
            HostIP.SetActive(true);
        }
        else if (this.GetComponentInChildren<Text>().text == "CLIENT")
        {
            this.GetComponentInChildren<Text>().text = "HOST";
            NetworkWrapper.isHost = true;
            HostText.SetActive(false);
            HostIP.SetActive(false);
        }
    }
}
