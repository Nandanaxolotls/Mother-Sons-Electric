using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class StepManagerCBT : MonoBehaviour
{
    [Header("CB 30")]
    public CB30onMachine cB30OnMachine;
    public VideoPlaybackManager videoPlaybackManager; // Assign in Inspector

    [Header("CB 35 SnapPoints")]
    public List<GameObject> CB35SnapPointsToActivate;

    [Header("CB 35")]
    public CB35onMachine cB35OnMachine;
    public VideoPlaybackManager2 videoPlaybackManager2;

    [Header("CB 40 SnapPoints")]
    public List<GameObject> CB40SnapPointsToActivate;

    [Header("Activate Conveyor Belt")]
    public CB40onMachine cB40OnMachine;
    // public GameObject ActivatePositions;
    public RandomActivationManager randomActivationManager;

    public AllObjectDestroy allObjectDestroy;

    public TutorialDestroyed tutorialDestroyed;

    [Header("CB 30 Parts to Highlight")]
    public List<StepWiseHighlighter> CB30parts; // Now a list
    public List<StepWiseHighlighter> CB30partsOnMachine;
    public List<StepWiseHighlighter> CB35partsOnMachine;
    public StepWiseHighlighter CB40Part;
    public GameObject CB40PartPrefab;
    public SnapAndRotateTutorial SnapAndRotateTutorial;
    public bool DoneRotation = false;
    public StepWiseHighlighter childpart;
    public H11BulbSnapping h11BulbSnapping;
    public GameObject Magnifyingsphere;
    public GameObject MagnifyingPoint;
    public bool MagnifyingsphereActivated = false;
    public MagnifyingTutorial magnifyingTutorial;
    public GameObject SphereOnMachine;
    public TagRestrictedSnapZone tagRestrictedSnapZone;

    [Header("Arrow")]
    public GameObject Arrow1;
    public GameObject Arrow2;
    public GameObject Arrow3;

    [Space]
    public GameObject NextButton;
    public TMP_Text subTitletxt;
    public string levelName;

    [Header("Input")]
    public InputActionProperty selectAction;
    private bool hasActivated = false;
    private bool isHoveredButton1 = false;
    private bool isHoveredButton2 = false;
    public bool video1canPlay = false;
    public bool video2canPlay = false;

    [Header("Door")]
    public Transform doorObject;             // Door transform to move
    public Transform doorObject2;
    public float moveSpeed = 1f;              // Movement speed
    public float targetX = 1f;                // X position when door is closed
    public float targetX2 = 1f;

    private Vector3 originalPosition;
    private Vector3 originalPosition2;

    void Start()
    {
      //  totalScore = int.Parse(GameManager.Instance.levelDatas[0].TotalScore.ToString());
        levelName = GameManager.Instance.levelDatas[0].LevelName;
        if (CB30parts != null && CB30parts.Count > 0)
        {
            foreach (var part in CB30parts)
            {
                if (part != null)
                    part.Highlight();
            }
        }
        if (doorObject != null)
            originalPosition = doorObject.position; // Save starting position
        if (doorObject2 != null)
            originalPosition2 = doorObject2.position; // Save starting position
        Arrow1.SetActive(true);
        cB30OnMachine.CB30PlayVideo += StartButton;
        cB35OnMachine.CB35PlayVideo += StartButton2;
        // Disable all objects to activate initially
        foreach (var obj in CB35SnapPointsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach (var obj in CB40SnapPointsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        videoPlaybackManager.CB30VideoPlayed += CB35SnapsActivate;
        videoPlaybackManager2.CB40SnapsActivate += CB40SnapsActivate;
        cB40OnMachine.CB40Done += ActivateCB40PartPrefab;
        allObjectDestroy.AllObjectChecked += LevelCompleted;
        SnapAndRotateTutorial.FirstSnapped += FirstSnapAndRotated;
        h11BulbSnapping.H11BulbSnapped += StartMagnifying;
        magnifyingTutorial.defectChecked += MagnifyingDone;
        tagRestrictedSnapZone.Startmachine += MachineCanBeStart;
        tutorialDestroyed.TutorialObjectChecked += ActivateRandomScript;
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 0, subTitletxt); // welcome to the game 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(0, 1, subTitletxt,3f)); // Pick two part from box using both hands and put on cb30 machine

        }
        //   ActivatePositions.SetActive(false);
    }
    public void OnHoverEnteredButton1(HoverEnterEventArgs args) => isHoveredButton1 = true;
    public void OnHoverExitedButton1(HoverExitEventArgs args) => isHoveredButton1 = false;
    public void OnHoverEnteredButton2(HoverEnterEventArgs args) => isHoveredButton2 = true;
    public void OnHoverExitedButton2(HoverExitEventArgs args) => isHoveredButton2 = false;

    private void Update()
    {
        if (selectAction.action.WasPressedThisFrame())
        {
            if (isHoveredButton1 && !hasActivated && video1canPlay)
            {
                doorclose1();
                hasActivated = true;
            }
            else if (isHoveredButton2 && hasActivated && video2canPlay)
            {
               doorclose2();
            }
        }
    }

    public void StartButton()
    {
        video1canPlay = true;
        {
            SoundManager.instance.PlayVoiceOver(0, 2, subTitletxt); //  Press the stick in right using right hand trigger button of controller to start the machine process
        }
    }
    public void doorclose1()
    {
        if (doorObject != null)
            StartCoroutine(MoveDoor(new Vector3(targetX, doorObject.position.y, doorObject.position.z), () =>
            {
                // When movement finishes ? play video
                PlayCB30video();
            }));
    }

    private IEnumerator MoveDoor(Vector3 targetPos, System.Action onComplete)
    {
        while (Vector3.Distance(doorObject.position, targetPos) > 0.01f)
        {
            doorObject.position = Vector3.MoveTowards(doorObject.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        doorObject.position = targetPos;

        if (onComplete != null)
            onComplete.Invoke();
    }
    public void MoveDoorBack()
    {
        if (doorObject != null)
            StartCoroutine(MoveDoor(originalPosition, null));
    }
    public void PlayCB30video()
    {
        if (videoPlaybackManager != null)
        {
            videoPlaybackManager.StartVideoWithCountdown();
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 3, subTitletxt); // wait for the machine process to complete

        }
    }

    public void CB35SnapsActivate()
    {
        MoveDoorBack();
        foreach (var obj in CB35SnapPointsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        if (CB30partsOnMachine != null && CB30partsOnMachine.Count > 0)
        {
            foreach (var part in CB30partsOnMachine)
            {
                if (part != null)
                    part.Highlight();
            }
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 4, subTitletxt); // Pick parts from CB30 machine and put them on CB35 machine

        }
    }

    public void StartButton2()
    {
        video2canPlay = true;
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 5, subTitletxt); // Press the stick in right using right hand trigger button of controller to start the machine process

        }
    }
    public void doorclose2()
    {
        if (doorObject2 != null)
            StartCoroutine(MoveDoor2(new Vector3(targetX2, doorObject2.position.y, doorObject2.position.z), () =>
            {
                // When movement finishes ? play video
                PlayCB35video();
            }));
    }

    private IEnumerator MoveDoor2(Vector3 targetPos, System.Action onComplete)
    {
        while (Vector3.Distance(doorObject2.position, targetPos) > 0.01f)
        {
            doorObject2.position = Vector3.MoveTowards(doorObject2.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        doorObject2.position = targetPos;

        if (onComplete != null)
            onComplete.Invoke();
    }
    public void MoveDoorBack2()
    {
        if (doorObject2 != null)
            StartCoroutine(MoveDoor2(originalPosition2, null));
    }

    public void PlayCB35video()
    {
        if (videoPlaybackManager2 != null)
        {
            videoPlaybackManager2.StartVideoWithCountdown();
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 6, subTitletxt); // wait for the machine process to complete

        }
    }

    public void CB40SnapsActivate()
    {
        MoveDoorBack2();
        foreach (var obj in CB40SnapPointsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        if (CB35partsOnMachine != null && CB35partsOnMachine.Count > 0)
        {
            foreach (var part in CB35partsOnMachine)
            {
                if (part != null)
                    part.Highlight();
            }
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 7, subTitletxt); // Pick parts from cb35 machine and put them on cb40 machine

        }
        if (Arrow2 != null)
        {
            Arrow2.SetActive(true);
        }
    }

    public void FirstSnapAndRotated()
    {

        if(!DoneRotation)
        {
            DoneRotation = true;
        }
        if (childpart != null)
        {
            childpart.Highlight();
        }

        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 9, subTitletxt); // pick cb40 and childpart from bin and attach childpart to the highlighted part of the cb40

        }
        Debug.Log("done rotation");
    }

    public void StartMagnifying()
    {
        if (Magnifyingsphere != null && !MagnifyingsphereActivated)
        {
            Magnifyingsphere.SetActive(true);
            MagnifyingsphereActivated = true;
        }
        if(MagnifyingPoint != null)
        {
            MagnifyingPoint.SetActive(true);
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 10, subTitletxt); // check for the defect on the part

        }
    }

    public void MagnifyingDone()
    {
        SphereOnMachine.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0,11, subTitletxt); // put cb40 part on machine 3 for fixing childpart with the main part

        }
    }

    public void MachineCanBeStart()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 12, subTitletxt); // press green button with left hand 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(0,13, subTitletxt, 3f)); // after process completes , place the defected part in defected bin and good part in good bin

        }
    }

    public void ActivateCB40PartPrefab()
    {
        if (CB40PartPrefab != null)
        { 
            CB40PartPrefab.SetActive(true);
        }
        if (CB40Part != null) 
        { 
            CB40Part.Highlight(); 
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 8, subTitletxt); // pick highlighted CB40 part and put it on machine 2 

        }
        if(Arrow3 != null)
        {
            Arrow3.SetActive(true);
        }
        // randomActivationManager.StartActivating();
        // ActivatePositions.SetActive(true);
    }

    public void ActivateRandomScript()
    {
        randomActivationManager.StartActivating();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 14, subTitletxt); // do this same process with all other parts 

        }
    }

    public void LevelCompleted()
    {
        Debug.Log("Level completed");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(0, 15, subTitletxt); // Congratulations Level Completed , now go near screen and press next button 

        }
        if(NextButton != null)
        {  
            NextButton.SetActive(true);
        }
    }

    public void Nextlevel( string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
