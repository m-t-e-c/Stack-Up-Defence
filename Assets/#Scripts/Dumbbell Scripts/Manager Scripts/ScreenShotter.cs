using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class ScreenShotter : MonoBehaviour
{
#if UNITY_EDITOR
    int ssCount;
    int startHeight;
    int startWidth;
    
    void Start()
    {
        startHeight = Screen.height;
        startWidth = Screen.width;
        ssCount = PlayerPrefs.GetInt("SSCount",0);
    }

    public void TakeSS()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine(AppleStoreScreenShots());
        }
    }

    IEnumerator AppleStoreScreenShots()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(TakeScreenShotSize(29));
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(TakeScreenShotSize(30));
        yield return new WaitForSecondsRealtime(1.5f); 
        StartCoroutine(TakeScreenShotSize(31));
        yield return new WaitForSecondsRealtime(1.5f);
        ssCount++; 
        PlayerPrefs.SetInt("SSCount",ssCount);
        PlayerPrefs.Save();
        Time.timeScale = 1;
    }

    IEnumerator TakeScreenShotSize(int index)
    {
        if(index > 0) SetSize(index);
        yield return new WaitForSecondsRealtime(1f);
        string ssName = Screen.width + "x" + Screen.height + "_" + Application.productName + "_" + ssCount.ToString() + "_" +  "ScreenShot.png";
        Debug.Log("Screenshot taken : " + ssName + " / " + index);
        ScreenCapture.CaptureScreenshot(ssName);  // 2048x2732
    }

    public static void SetSize(int index)
    {
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        selectedSizeIndexProp.SetValue(gvWnd, index, null);
    }

    // Update is called once per frame
    void Update()
    {
        TakeSS();
    }
#endif
}
