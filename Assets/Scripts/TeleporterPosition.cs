using UnityEngine;
using System.Collections;

public class TeleporterPosition : MonoBehaviour {

	private bool isAdjusted = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isAdjusted && (transform.rotation.x >= 90 || transform.rotation.x <= -90 || transform.rotation.z >= 90 || transform.rotation.z <= 90)) {
			transform.localEulerAngles = new Vector3(0, transform.rotation.y, 0);
			isAdjusted = true;
		}
	}
}
