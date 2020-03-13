using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class FMC_OneTimesOneSettingsLayout : MonoBehaviour
{

    public GameObject background;
    public GameObject backButton;
    public List<FMC_CheckButton> allCheckButtons;
    public Text auswahlText;
    public Transform infoButton;

    private int autoCheckButton = 0;
    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private void Awake ()
    {
        setLayout();
    }

    private void OnEnable ()
    {
        int autoCheck = 0;
        if (allCheckButtons.Count > autoCheckButton)
            autoCheck = autoCheckButton;

        Debug.LogWarning("Auto Check: " + autoCheckButton);

        for (int i = 0; i < allCheckButtons.Count; i++)
        {
            if (i == autoCheck)
                allCheckButtons[i].checkButton(false);
            else
                allCheckButtons[i].uncheckButton(false);
        }
        for (int i = 0; i < allCheckButtons.Count; i++)
        {
            if (i == autoCheck)
                allCheckButtons[i].checkButton(false);
            else
                allCheckButtons[i].uncheckButton(false);
        }
    }

    public void setAutoCheckButton (FMC_Settings_Input.allInformation row)
    {
        if (row == FMC_Settings_Input.allInformation.oxo_1)
            autoCheckButton = 0;
        else if (row == FMC_Settings_Input.allInformation.oxo_2)
            autoCheckButton = 1;
        else if (row == FMC_Settings_Input.allInformation.oxo_3)
            autoCheckButton = 2;
        else if (row == FMC_Settings_Input.allInformation.oxo_4)
            autoCheckButton = 3;
        else if (row == FMC_Settings_Input.allInformation.oxo_5)
            autoCheckButton = 4;
        else if (row == FMC_Settings_Input.allInformation.oxo_6)
            autoCheckButton = 5;
        else if (row == FMC_Settings_Input.allInformation.oxo_7)
            autoCheckButton = 6;
        else if (row == FMC_Settings_Input.allInformation.oxo_8)
            autoCheckButton = 7;
        else if (row == FMC_Settings_Input.allInformation.oxo_9)
            autoCheckButton = 8;
        else if (row == FMC_Settings_Input.allInformation.oxo_10)
            autoCheckButton = 9;
        else
        {
            Debug.LogWarning("Could not find the right button.");
            autoCheckButton = 0;
        }
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
        //setCheckButtons(allCheckButtons);

    }

    public void createButtons()
    {
        createCheckButtons(allCheckButtons);
    }

    public void destroyButtons()
    {
        destroyCheckButtons(allCheckButtons);
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
        float buttonWidth = (cameraWidth * 0.8f) / 3.2f;

        for (int i = 0; i < checkButtons.Count; i++)
        {
            //checkButtons[i].gameObject.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (i * buttonWidth) + (buttonWidth * 0.5f) + (cameraWidth * 0.1f), checkButtons[i].gameObject.transform.position.y, checkButtons[i].gameObject.transform.position.z);

            if (i % 3 == 0)
                checkButtons[i].gameObject.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (buttonWidth * 0.5f) + (cameraWidth * 0.1f), checkButtons[i].gameObject.transform.position.y, checkButtons[i].gameObject.transform.position.z);
            else if (i % 3 == 1)
                checkButtons[i].gameObject.transform.position = new Vector3(0.0f, checkButtons[i].gameObject.transform.position.y, checkButtons[i].gameObject.transform.position.z);
            else if (i % 3 == 2)
                checkButtons[i].gameObject.transform.position = new Vector3(cameraPosition.x + (cameraWidth * 0.5f) - (buttonWidth * 0.5f) - (cameraWidth * 0.1f), checkButtons[i].gameObject.transform.position.y, checkButtons[i].gameObject.transform.position.z);

            //if (i / 3 < 1.0f)
             //   checkButtons[i].gameObject.transform.position = new Vector3();


            //if (checkButtons[i].faceSpriteRenderer)
            //    checkButtons[i].faceSpriteRenderer.size = new Vector2(buttonWidth, checkButtons[i].faceSpriteRenderer.size.y);
            //if (checkButtons[i].boxCollider)
            //    checkButtons[i].boxCollider.size = checkButtons[i].faceSpriteRenderer.size;
            //if (checkButtons[i].backgroundSpriteRenderer)
            //    checkButtons[i].backgroundSpriteRenderer.size = checkButtons[i].faceSpriteRenderer.size;
            //if (checkButtons[i].edgeSpriteRenderer)
            //    checkButtons[i].edgeSpriteRenderer.size = checkButtons[i].faceSpriteRenderer.size;
        }
    }

    public void close ()
    {
        gameObject.SetActive(false);
    }

    public void loadGameScene ()
    {
        Debug.Log("Load Game Scene Here");
        FLS_LoadingScreen.instance.loadScene("MathLadder");
        //gameObject.SetActive(false);
    }
}

#if UNITY_EDITOR 
[CustomEditor(typeof(FMC_OneTimesOneSettingsLayout))]
public class FMC_OneTimesOneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FMC_OneTimesOneSettingsLayout myScript = (FMC_OneTimesOneSettingsLayout)target;
        if (GUILayout.Button("Set layout"))
        {
            myScript.setLayout();
        }
        if (GUILayout.Button("Create Buttons"))
        {
            myScript.createButtons();
        }
        if (GUILayout.Button("Destroy Buttons"))
        {
            myScript.destroyButtons();
        }
    }
}
#endif