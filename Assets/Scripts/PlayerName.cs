using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerName : MonoBehaviour {

    public Text nameTag;
    public Camera cameraToLookAt;

    //Allow the text to keep facing you
    void update()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }

    [PunRPC]
    public void updateName(string name)
    {
        nameTag.text = name;
    }
}
