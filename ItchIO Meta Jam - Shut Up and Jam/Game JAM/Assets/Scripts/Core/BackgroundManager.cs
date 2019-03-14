using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject BackgroundObject;
    public Camera CameraObject;

    public GameObject ScreenBackground;

    public void Initialize()
    {
        BackgroundObject.SetActive(true);
        CameraObject.backgroundColor = Color.black;
        ResetBackgrounds();
    }

    public void ShowScreenBackground()
    {
        ResetBackgrounds();
        CameraObject.backgroundColor = new Color(167.0f / 255.0f, 167.0f / 255.0f, 167.0f / 255.0f, 1.0f);
        ScreenBackground.SetActive(true);
    }

    public void ShowBlackBackground()
    {
        ResetBackgrounds();
        CameraObject.backgroundColor = Color.black;
    }

    private void ResetBackgrounds()
    {
        ScreenBackground.SetActive(false);
    }
}
