using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Msg : Singleton<Msg>
{
    [SerializeField] Color colorDefault;
    [SerializeField] UnityEngine.UI.Image obj;
    [SerializeField] TMPro.TMP_Text txtMsg;

    private Color colorDisable;
    private Color colorEnable;

    private void Start()
    {
        colorEnable = Color.black;
        colorDisable = Color.black;
        colorEnable.a = 0.8f;
        colorDisable.a = 0f;

        txtMsg.CrossFadeAlpha(0, 0, false);
        obj.color = colorDisable;
    }

    private IEnumerator IEFadeTxt()
    {
        txtMsg.CrossFadeAlpha(1, 0.8f, false); // Text come 
        obj.color = colorEnable;
        yield return new WaitForSeconds(2);
        obj.color = colorDisable;
        txtMsg.CrossFadeAlpha(0, 0.8f, false); // Text fade (Gone)
    }


    public void DisplayMsg(string msg, Color color)
    {
        StopAllCoroutines();

        txtMsg.color = color;
        txtMsg.text = msg;
        StartCoroutine(IEFadeTxt());
    }

    public void DisplayMsg(string msg)
    {
        DisplayMsg(msg, colorDefault);
    }
}