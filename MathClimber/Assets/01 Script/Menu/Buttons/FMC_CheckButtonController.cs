using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMC_CheckButtonController : MonoBehaviour
{

    [Range(0.0f, 0.5f)] public float height;
    [Range(0.0f, 0.5f)] public float transitionTime;
    public Color checkedColor;
    public Color uncheckedColor;

    public List<FMC_CheckButton> checkButtons;

    public List<FMC_CheckButton> currentlyCheckedButtons { get; private set; }

    private void Awake()
    {
        currentlyCheckedButtons = new List<FMC_CheckButton>();
        if (checkButtons.Count > 0)
        {
            foreach (FMC_CheckButton b in checkButtons)
                b.initialise(this);
        }
    }

    public void checkButtonChecked (FMC_CheckButton pressedButton)
    {
        if (!currentlyCheckedButtons.Contains(pressedButton))
        {
            currentlyCheckedButtons.Add(pressedButton);
        }
    }
    
    public void checkButtonUnchecked (FMC_CheckButton pressedButton)
    {
        if (currentlyCheckedButtons.Contains(pressedButton))
        {
            currentlyCheckedButtons.Remove(pressedButton);
        }
    }

}
