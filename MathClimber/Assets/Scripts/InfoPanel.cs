using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

	
    public GameObject button;
    public GameObject label;
    public Text textLabel;
    
	private bool isOpen = false;
	
	public void toggle (bool visible) {
    
        if (visible) {
            label.SetActive(true);
            button.SetActive(false);
        } else {
            label.SetActive(false);
            button.SetActive(true);
        }
        isOpen = visible;
    }
    
    public void setLabel (string labelText) {
        textLabel.text = labelText;
    }
    
    public void activate () {
        if (!isOpen) {
            label.SetActive(false);
            button.SetActive(true);
        } else {
            label.SetActive(true);
            button.SetActive(false);
        }
    }
    
    public void deactivate () {
        label.SetActive(false);
        button.SetActive(false);
    }
}
