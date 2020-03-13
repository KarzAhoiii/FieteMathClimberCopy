using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FMC_FreestyleSettingLayout : MonoBehaviour {

    public GameObject background;
    public GameObject backButton;
    public List<FMC_RadioButton> radioButtonsRange;
    public List<FMC_CheckButton> checkButtonsNumberType;
    public List<FMC_CheckButton> checkButtonsOperation;
    public List<FMC_CheckButton> checkButtonsTaskType;
    public List<FMC_RadioButton> radioButtonsTime;
    public List<FMC_IterateButton> iterateButtons;
    public Text auswahlText;
    public Transform infoButton;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private void Awake()
    {
        setLayout();
    }

    public void OnEnable ()
    {
        FMC_Settings currentSetting = FMC_GameDataController.instance.getCurrentSettings();
        
        if (currentSetting is FMC_Settings_Freestyle) {
             disableButtons(); 
             autoClickButtons(currentSetting);
        }

    }

    private void autoClickButtons (FMC_Settings currentSetting)
    {
        // Click and unclick Range of Number Buttons

        if (currentSetting._rangeOfNumbers <= 15) 
        {
            radioButtonsRange[0].setText(currentSetting._rangeOfNumbers.ToString());
            radioButtonsRange[1].setText("20");
            radioButtonsRange[2].setText("100");
            radioButtonsRange[3].setText("1000");
            radioButtonsRange[0].checkButton(false);
        }
        else if (currentSetting._rangeOfNumbers <= 50) 
        {
            radioButtonsRange[0].setText("10");
            radioButtonsRange[1].setText(currentSetting._rangeOfNumbers.ToString());
            radioButtonsRange[2].setText("100");
            radioButtonsRange[3].setText("1000");
            radioButtonsRange[1].checkButton(false);
        }
        else if (currentSetting._rangeOfNumbers <= 500)
        {
            radioButtonsRange[0].setText("10");
            radioButtonsRange[1].setText("20");
            radioButtonsRange[2].setText(currentSetting._rangeOfNumbers.ToString());
            radioButtonsRange[3].setText("1000");
            radioButtonsRange[2].checkButton(false);
        }
        else if (currentSetting._rangeOfNumbers > 500)
        {
            radioButtonsRange[0].setText("10");
            radioButtonsRange[1].setText("20");
            radioButtonsRange[2].setText("100");
            radioButtonsRange[3].setText(currentSetting._rangeOfNumbers.ToString());
            radioButtonsRange[3].checkButton(false);
        }

        // Iterate Button Auto Setting
        iterateButtons[0].setState(currentSetting._numberTypeFront);
        iterateButtons[1].setState(currentSetting._numberTypeBack);

        // Click and Unclick Operation Buttons
        if (currentSetting._operationPlusIsPossible)
            checkButtonsOperation[0].checkButton(false);
        else
            checkButtonsOperation[0].uncheckButton(false);

        if (currentSetting._operationTimesIsPossible)
            checkButtonsOperation[1].checkButton(false);
        else
            checkButtonsOperation[1].uncheckButton(false);

        if (currentSetting._operationMinusIsPossible)
            checkButtonsOperation[2].checkButton(false);
        else
            checkButtonsOperation[2].uncheckButton(false);

        if (currentSetting._operationDividedIsPossible)
            checkButtonsOperation[3].checkButton(false);
        else
            checkButtonsOperation[3].uncheckButton(false);


        // Click and Unclick Task Type Buttons
        if (currentSetting._taskTypeGreaterIsPossible)
            checkButtonsTaskType[0].checkButton(false);
        else
            checkButtonsTaskType[0].uncheckButton(false);

        if (currentSetting._taskTypeSmallerIsPossible)
            checkButtonsTaskType[1].checkButton(false);
        else
            checkButtonsTaskType[1].uncheckButton(false);

        if (currentSetting._taskTypeSameIsPossible)
            checkButtonsTaskType[2].checkButton(false);
        else
            checkButtonsTaskType[2].uncheckButton(false);

        //if (currentSetting._taskTypeEqualsIsPossible)
        //    checkButtonsTaskType[3].checkButton(false);
        //else
        //    checkButtonsTaskType[3].uncheckButton(false);


        // Click and unclick time Specification Buttons
        if (currentSetting._timeSpecification <= 8 && currentSetting._timeSpecification > 0)
        {
            radioButtonsTime[1].checkButton(false);
            radioButtonsTime[1].setText(currentSetting._timeSpecification.ToString());
            radioButtonsTime[2].setText("15");
            radioButtonsTime[3].setText("30");
        }
        else if (currentSetting._timeSpecification <= 500 && currentSetting._timeSpecification > 0)
        {
            radioButtonsTime[2].checkButton(false);
            radioButtonsTime[1].setText("5");
            radioButtonsTime[2].setText(currentSetting._timeSpecification.ToString());
            radioButtonsTime[3].setText("30");
        }
        else if (currentSetting._timeSpecification <= 1000 && currentSetting._timeSpecification > 0)
        {
            radioButtonsTime[3].checkButton(false);
            radioButtonsTime[1].setText("5");
            radioButtonsTime[2].setText("15");
            radioButtonsTime[3].setText(currentSetting._timeSpecification.ToString());
        }
        else if (currentSetting._timeSpecification == -1)
        {
            radioButtonsTime[0].checkButton(false);
            radioButtonsTime[1].setText("5");
            radioButtonsTime[2].setText("15");
            radioButtonsTime[3].setText("30");
        }

    }
    
    
    private void disableButtons () {

        radioButtonsRange[0].disableButton();
        radioButtonsRange[1].disableButton();
        radioButtonsRange[2].disableButton();
        radioButtonsRange[3].disableButton();
        
        checkButtonsOperation[0].disableButton();
        checkButtonsOperation[1].disableButton();
        checkButtonsOperation[2].disableButton();
        checkButtonsOperation[3].disableButton();
        
        checkButtonsTaskType[0].disableButton();
        checkButtonsTaskType[1].disableButton();
        checkButtonsTaskType[2].disableButton();
        

        radioButtonsTime[0].disableButton();
        radioButtonsTime[1].disableButton();
        radioButtonsTime[2].disableButton();
        radioButtonsTime[3].disableButton();
    }

    public void setLayout()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (background)
            background.transform.localScale = new Vector3(cameraWidth, cameraHeight, 1.0f);

        if (backButton)
            backButton.transform.position = new Vector3((cameraPosition.x - (cameraWidth * 0.5f)) + (backButton.transform.localScale.x * 1.1f), (cameraPosition.y + (cameraHeight * 0.5f)) - (backButton.transform.localScale.y * 1.1f), 0.0f);

        if (auswahlText && FMC_GameDataController.instance)
            auswahlText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][20];

        if (infoButton)
            infoButton.position = new Vector3((cameraPosition.x + (cameraWidth * 0.5f)) - (infoButton.localScale.x * 1.1f), (cameraPosition.y + (cameraHeight * 0.5f)) - (infoButton.localScale.y * 1.1f), 0.0f);

        setRadioButtons(radioButtonsRange);
        setRadioButtons(radioButtonsTime);
        setCheckButtons(checkButtonsOperation);
        setCheckButtons(checkButtonsTaskType);
        setIterateButtons(iterateButtons);

    }

    private void setIterateButtons(List<FMC_IterateButton> iterateButtons)
    {
        float buttonWidth = (cameraWidth * 0.9f) * 0.75f;

        for (int i = 0; i < iterateButtons.Count; i++)
        {
            iterateButtons[i].transform.position = new Vector3(cameraPosition.x + (cameraWidth * 0.5f) - (buttonWidth * 0.5f) - (cameraWidth * 0.05f), iterateButtons[i].transform.position.y, iterateButtons[i].transform.position.z);

            if (iterateButtons[i].faceSpriteRenderer)
                iterateButtons[i].faceSpriteRenderer.size = new Vector2(buttonWidth, iterateButtons[i].faceSpriteRenderer.size.y);
            if (iterateButtons[i].boxCollider)
                iterateButtons[i].boxCollider.size = iterateButtons[i].faceSpriteRenderer.size;
            if (iterateButtons[i].backgroundSpriteRenderer)
            {
                iterateButtons[i].backgroundSpriteRenderer.transform.position = new Vector3(iterateButtons[i].transform.position.x, iterateButtons[i].backgroundSpriteRenderer.transform.position.y, iterateButtons[i].backgroundSpriteRenderer.transform.position.z);
                iterateButtons[i].backgroundSpriteRenderer.size = iterateButtons[i].faceSpriteRenderer.size;
            }
            if (iterateButtons[i].backgroundBackgroundSpriteRenderer)
            {
                iterateButtons[i].backgroundBackgroundSpriteRenderer.size = new Vector2(cameraWidth * 0.9f, iterateButtons[i].backgroundBackgroundSpriteRenderer.size.y);
                iterateButtons[i].backgroundBackgroundSpriteRenderer.transform.position = new Vector3(cameraPosition.x, iterateButtons[i].backgroundBackgroundSpriteRenderer.transform.position.y, iterateButtons[i].backgroundBackgroundSpriteRenderer.transform.position.z);
            }
            if (iterateButtons[i].numberText)
                iterateButtons[i].numberText.position = new Vector3(radioButtonsRange[0].transform.position.x, iterateButtons[i].numberText.position.y, iterateButtons[i].numberText.position.z);

            iterateButtons[i].setIapOverlaySize(new Vector2(cameraWidth, 0.8f));
        }
    }

    private void setRadioButtons(List<FMC_RadioButton> radioButtons)
    {
        float buttonWidth = (cameraWidth * 0.9f) / radioButtons.Count;

        for (int i = 0; i < radioButtons.Count; i++)
        {
            radioButtons[i].gameObject.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (i * buttonWidth) + (buttonWidth * 0.5f) + (cameraWidth * 0.05f), radioButtons[i].gameObject.transform.position.y, radioButtons[i].gameObject.transform.position.z);

            if (radioButtons[i].faceSpriteRenderer)
                radioButtons[i].faceSpriteRenderer.size = new Vector2(buttonWidth, radioButtons[i].faceSpriteRenderer.size.y);
            if (radioButtons[i].boxCollider)
                radioButtons[i].boxCollider.size = radioButtons[i].faceSpriteRenderer.size;
            if (radioButtons[i].backgroundSpriteRenderer)
                radioButtons[i].backgroundSpriteRenderer.size = radioButtons[i].faceSpriteRenderer.size;
            if (radioButtons[i].edgeSpriteRenderer)
                radioButtons[i].edgeSpriteRenderer.size = radioButtons[i].faceSpriteRenderer.size;
            radioButtons[i].setIapOverlaySize(radioButtons[i].faceSpriteRenderer.size);
        }
    }

    private void createRadioButtons(List<FMC_RadioButton> radioButtons)
    {
        for (int i = 0; i < radioButtons.Count; i++)
        {
            radioButtons[i].createButton();
        }
    }

    private void destroyRadioButtons(List<FMC_RadioButton> radioButtons)
    {
        for (int i = 0; i < radioButtons.Count; i++)
        {
            radioButtons[i].destroyButton();
        }
    }

    private void createCheckButtons(List<FMC_CheckButton> checkButtons)
    {
        for (int i = 0; i < checkButtons.Count; i++)
        {
            checkButtons[i].createButton();
        }
    }

    private void destroyCheckButtons(List<FMC_CheckButton> checkButtons)
    {
        for (int i = 0; i < checkButtons.Count; i++)
        {
            checkButtons[i].destroyButton();
        }
    }

    private void setCheckButtons(List<FMC_CheckButton> checkButtons)
    {
        float buttonSpacing = 0.1f;
        float buttonWidth = ((cameraWidth * 0.9f) / checkButtons.Count) - ((buttonSpacing / checkButtons.Count) * (checkButtons.Count - 1));

        for (int i = 0; i < checkButtons.Count; i++)
        {
            checkButtons[i].gameObject.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (i * buttonWidth) + (buttonWidth * 0.5f) + (cameraWidth * 0.05f) + (i * buttonSpacing), checkButtons[i].gameObject.transform.position.y, checkButtons[i].gameObject.transform.position.z);

            if (checkButtons[i].faceSpriteRenderer)
                checkButtons[i].faceSpriteRenderer.size = new Vector2(buttonWidth, checkButtons[i].faceSpriteRenderer.size.y);
            if (checkButtons[i].boxCollider)
                checkButtons[i].boxCollider.size = checkButtons[i].faceSpriteRenderer.size;
            if (checkButtons[i].backgroundSpriteRenderer)
                checkButtons[i].backgroundSpriteRenderer.size = checkButtons[i].faceSpriteRenderer.size;
            if (checkButtons[i].edgeSpriteRenderer)
                checkButtons[i].edgeSpriteRenderer.size = checkButtons[i].faceSpriteRenderer.size;

            checkButtons[i].setIapOverlaySize(checkButtons[i].faceSpriteRenderer.size);

        }
    }

    public void closeFreestyleSettings()
    {
        gameObject.SetActive(false);
    }

    public void loadGameScene() {
        FLS_LoadingScreen.instance.loadScene("MathLadder");
    }
}

#if UNITY_EDITOR 
[CustomEditor(typeof(FMC_FreestyleSettingLayout))]
public class PS_EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FMC_FreestyleSettingLayout myScript = (FMC_FreestyleSettingLayout)target;
        if (GUILayout.Button("Set layout"))
        {
            myScript.setLayout();
        }
        //if (GUILayout.Button("Create Buttons"))
        //{
        //    myScript.createButtons();
        //}
        //if (GUILayout.Button("Destroy Buttons"))
        //{
        //    myScript.destroyButtons();
        //}
    }
}
#endif