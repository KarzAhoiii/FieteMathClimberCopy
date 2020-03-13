using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMC_RadioButtonController : MonoBehaviour
{

    [Range (0.0f, 0.5f)] public float height;
    [Range (0.0f, 0.5f)] public float transitionTime;
    public Color checkedColor;
    public Color uncheckedColor;
    public TextMesh resultIsRangeOfNumberText;

    public List<FMC_RadioButton> radioButtons;
    public FMC_RadioButton currentlyCheckedButton { get; private set; }

    private void Awake ()
    {
        if (radioButtons.Count > 0)
        {
            foreach (FMC_RadioButton b in radioButtons)
                b.initialise(this);

            radioButtons[0].checkButton(false);
        } 
    }

    public void radioButtonPressed (FMC_RadioButton pressedButton)
    {
        currentlyCheckedButton = pressedButton;
        if (resultIsRangeOfNumberText)
            resultIsRangeOfNumberText.text = "=" + pressedButton.text;

        foreach (FMC_RadioButton b in radioButtons)
        {
            if (b != currentlyCheckedButton)
            {
                b.uncheckButton(false);
            }
        }
    }

}
