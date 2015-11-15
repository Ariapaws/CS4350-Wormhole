using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerName : MonoBehaviour {

    public Text nameTag;

    [PunRPC]
    public void updateName(string name)
    {
        nameTag.text = name;
    }
}
