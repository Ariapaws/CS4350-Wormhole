using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class ChatScript : MonoBehaviour {

	// Use this for initialization
	private bool isActive = false;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			Debug.Log(GetComponent<UnityEngine.UI.InputField>().isFocused);
			if (isActive == true) {
				GetComponent<UnityEngine.UI.Image>().enabled = false;
				foreach (UnityEngine.UI.Text text in GetComponentsInChildren<UnityEngine.UI.Text>()) {
					text.enabled = false;
				}
				isActive = false;
			} else {
				GetComponent<UnityEngine.UI.InputField>().text = "";
				GetComponent<UnityEngine.UI.InputField>().enabled = true;
				GetComponent<UnityEngine.UI.Image>().enabled = true;
				foreach (UnityEngine.UI.Text text in GetComponentsInChildren<UnityEngine.UI.Text>()) {
					text.enabled = true;
				}
				
				GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
				EventSystem.current.SetSelectedGameObject(GetComponent<UnityEngine.UI.InputField>().gameObject, null);
				GetComponent<UnityEngine.UI.InputField>().OnPointerClick(new PointerEventData(EventSystem.current));
				isActive = true;
			}
		}
	}
}
