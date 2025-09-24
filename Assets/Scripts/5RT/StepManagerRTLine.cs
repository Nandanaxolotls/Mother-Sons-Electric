using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerRTLine : MonoBehaviour
{
    public AllObjectDestroy AllObjectDestroy;
    public GameObject NextButton;
    //public List<GameObject> RTParts;
    //public GameObject RTsnapPoint;
    //public RTSnapPoint rTSnapPoint;

    //public VideoPlaybackManagerRTline videoPlaybackManagerRTline;

    //public GameObject RTfinalSnappoint;

  //  public RTfinalSnapPoint rTfinalSnapPoint;

    void Start()
    {
        AllObjectDestroy.AllObjectChecked += LevelCompleted;
        if(NextButton != null )
        {
            NextButton.SetActive(false);
        }
        //RTsnapPoint.SetActive(false);
        //foreach (var obj in RTParts)
        //{
        //    if (obj != null)
        //        obj.SetActive(false);
        //}
        //rTSnapPoint.RTsnapped += Playvideo;
        //videoPlaybackManagerRTline.RTVideoPlayed += VideoPlayed;
        //RTfinalSnappoint.SetActive(false);
       // rTfinalSnapPoint.RTFinalsnapped += LevelCompleted;
    }

    public void Allchecked()
    {
        //foreach (var obj in RTParts)
        //{
        //    if (obj != null)
        //        obj.SetActive(true);
        //}
        //RTsnapPoint.SetActive(true );
    }

    //public void Playvideo()
    //{
    //    if (videoPlaybackManagerRTline != null)
    //    {
    //        videoPlaybackManagerRTline.StartVideoWithCountdown();
    //    }
    //}

    //public void VideoPlayed()
    //{
    //    RTfinalSnappoint.SetActive(true);
    //}

    public void LevelCompleted()
    {
        Debug.Log("RT line Level completed");
        if (NextButton != null)
        {
            NextButton.SetActive(true);
        }
    }

    public void levelChange(string name)
    {
        SceneManager.LoadScene(name);
    }
}
