using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class FMC_StoryModeBoxLayout : MonoBehaviour
{

    public GameObject background;
    public GameObject overlay;
    public GameObject backButton;
    public SkeletonAnimation spine;
    public Text welcomingText;
    public Canvas coinsAndLevels;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private float totalBoxHeight;

    public void setLayout () {
    
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        //float scale = 1.0f;
        //if (Camera.main.aspect > 0.6f)
        //    scale = 1.3f;

        if (background) {
            background.transform.localScale = new Vector3(cameraWidth, background.transform.localScale.y, 1.0f);
            totalBoxHeight = background.transform.localScale.y;
        }

        if (overlay)
            overlay.transform.localScale = new Vector3(cameraWidth, overlay.transform.localScale.y, 1.0f);

        if (spine)
            spine.gameObject.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.37f), spine.gameObject.transform.position.y, 0.0f);

        if (backButton)
            backButton.transform.position = new Vector3((cameraPosition.x - (cameraWidth * 0.5f)) + (backButton.transform.localScale.x * 1.2f), (cameraPosition.y + (cameraHeight * 0.5f)) - (backButton.transform.localScale.y * 1.1f), 0.0f);

    }

    public void renewStatisticsData() {
    	print(FMC_GameDataController.instance.getCurrentPlayerName());
        if (FMC_GameDataController.instance) {
            welcomingText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][0] + " " + FMC_GameDataController.instance.getCurrentPlayerName() /*+ ", " + FMC_GameDataController.instance.getCurrentPlayerAge()*/;
        } else {
            welcomingText.text = "Hallo";
        }
    }
}