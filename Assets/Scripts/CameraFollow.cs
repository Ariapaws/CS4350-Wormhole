using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;

	void LateUpdate(){
		transform.position = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z);
	}
}
