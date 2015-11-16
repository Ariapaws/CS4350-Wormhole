using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerName : MonoBehaviour {

    public Text nameTag;


    public Transform target;
    private Vector3 rotate180 = new Vector3(0, 180, 0);


    void Update()
    {
        if ((GameObject)PhotonNetwork.player.TagObject != null)
        {
            nameTag.transform.LookAt(((GameObject)PhotonNetwork.player.TagObject).transform);
            nameTag.transform.Rotate(rotate180);
        }
    }

    [PunRPC]
    public void updateName(string name)
    {
        nameTag.text = name;
    }
}
