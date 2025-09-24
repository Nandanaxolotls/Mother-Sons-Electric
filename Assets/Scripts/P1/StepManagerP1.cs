using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StepManagerP1 : MonoBehaviour
{
    [Header("Machine1")]
    public ChipToCover chipToCoverSnappoint;
    public GameObject CoverToPuncherSnapPoint;// SnapPoint
    public CoverToPuncher coverToPuncher;
    public SideHandle sideHandle; //Side Handle Send Invoke
    public GameObject SideHandle; // GameObject
    public PuncherHandle PuncherHandle; // script calling
    public GameObject PunchersHandle; // GameObject
    public XRGrabInteractable CoverAfterPunched;
    [Header("Machine2")]
    public GameObject SolderingSnapPoint;
    public CoverToSoldering coverToSolderingSnapPoint;
    public SolderingMachine solderingMachine;
    public XRGrabInteractable CoverAfterSoldered;
    [Header("Machine3")]
    public GameObject BlackBoxSnapPoint;
    public GameObject SphereBlackBoxSnapPoint;
    public CoverToBackBox coverToBackBox;
    public HolderMachine holderMachine;
    public Door1 door1;
    public ComputerDisplay computerDisplay;
    public GameObject DisplayCanvas;
    public XRGrabInteractable CoverInBlackBox;
    [Header("Machine4")]
    public GameObject UpperSnappingPoint;
    public UpperCoverToMain UpperSnappingScript; // script calling after snapped
    public GameObject UpperOnPuncherSnapPoint; // upper on puncher snappoint
    public UpperToPuncher UpperToPuncherSnapped;
    public GameObject SideHandle2;
    public GameObject Handle2;
    public SideHandle2 sideHandle2;
    public PuncherHandle2 puncherHandle2;
    public XRGrabInteractable UpperCoverAfterPunched;
    [Header("Machine5")]
    public UpperToBlackBox upperToBlackBox; // SnapPoint for snapping upper to black box2
    public GameObject UpperSnapPoint; // snappoint object
    public GameObject wireSnappingGameObject; //snappoint gameobject
    public WireSnapping wireSnapping;
    public XRGrabInteractable WireGrabbing;
    public GameObject ButtonTrigger;
    public BoxHandle boxHandle;
    public Door2 door2;
    public ComputerDisplay2 computerDisplay2;
    public GameObject DisplayCanvas2;
    public XRGrabInteractable CoverInBlackBox2;
    [Header("Machine6")]
    public GameObject StickerPressingSnapPoint;
    public StickerPressingSnapPoint StickerPressing;
    public GameObject PressingTrigger; // pressing machine mechainsm game object 
    public StickerPressing stickerPressingTrigger; //Pressing script calling after closing
    public GameObject Sticker;
    public GameObject StickerSnapPoint;
    public stickerSnapPoint stickerSnapPoint;
    public GameObject GreenButtonScriptObject;
    public GreenButton greenButton;
    public XRGrabInteractable Marker;
    public GameObject MarkingPoint;
    public Marking marking;
    public XRGrabInteractable UpperAfterStickerAttached;
    public GameObject FinalStandSnapPoint;
    public FinalStandSnapPoint FinalStandScript;

    [Header("Highlights")]
    [Header("Machine1")]
    public StepWiseHighlighter CoverHighlighter;
    public GameObject Arrow1;
    public StepWiseHighlighter ChipHighlighter;
    public GameObject Arrow2;
    public GameObject SphereOnCover;
    public GameObject Arrow3;
    public GameObject SphereOnPuncher;
    public StepWiseHighlighter CoverOnPuncher;
    public GameObject Tooltip; // side handle highlighter
    public GameObject Tooltip2; // Puncher highlighter
    public GameObject Tooltip3; // Puncher highlighter close
    public GameObject Tooltip4; // Side Handle CLose (original position)
    public StepWiseHighlighter CoverOnPuncherPick;
    [Header("Machine2")]
    public GameObject Arrow4;
    public GameObject SphereOnSoldering; // sphere which highlights the soldering area for snapping cover 
    public StepWiseHighlighter SolderingSnappointHighlight;
    public GameObject Tooltip5;
    public StepWiseHighlighter CoverAfterSolderedHighlight;
    [Header("Machine3")]
    public GameObject Arrow5;
    public StepWiseHighlighter CoverOnBlackBox;
    public GameObject Tooltip6;
    public GameObject Arrow6;
    public GameObject Tooltip7;
    public GameObject Tooltip8;
    public GameObject Tooltip9;
    public StepWiseHighlighter CoverBlackBoxHighlighter;
    public GameObject Tooltip10;
    [Header("Machine4")]
    public GameObject Arrow7;
    public GameObject Arrow8;
    // public StepWiseHighlighter UpperHighlightSphere;
    public GameObject UpperCoverSphere;
    public GameObject Arrow9;
    public GameObject SnapHighlighterInPuncher; // Snappoint highlighter on puncher
    public StepWiseHighlighter UpperOnPuncherHighlighter;
    public GameObject Tooltip11;
    public GameObject Tooltip12;
    public GameObject Tooltip13;
    public GameObject Tooltip14;
    public StepWiseHighlighter UpperCoverOnPuncherPick2;
    [Header("Machine5")]
    public GameObject Arrow10;
    public GameObject SphereUpperOnBoxHighlighter2; // highlighter object for placing upper on black box 2
    public StepWiseHighlighter UpperOnBoxHighlighter2;
    public GameObject Tooltip15;
    public StepWiseHighlighter wirePlugHighlight;
    public GameObject WireSphere;
    public GameObject Tooltip16;
    public GameObject Arrow11;
    public GameObject Tooltip17;
    public GameObject Tooltip18;
    public GameObject Tooltip19;
    public GameObject Tooltip20;
    public StepWiseHighlighter CoverBlackBox2Highlighter;
    public GameObject Arrow12;
    public GameObject Tooltip21;
    public GameObject Arrow13;
    public GameObject Tooltip22;
    public GameObject Tooltip23;
    public GameObject Tooltip24;
    public GameObject Arrow14;
    public StepWiseHighlighter UpperAfterSticker;
    public GameObject Arrow15;
    public GameObject Arrow16;





    void Start()
    {
        CoverHighlighter.Highlight();
        chipToCoverSnappoint.Chipsnapped += ChipSnappedToCover;
        coverToPuncher.Coversnapped += CoverSnappedToPuncher;
        sideHandle.onReachedDesired += SideHandlePlaced;
        PuncherHandle.onReachedOriginal += PunchingDone;
        sideHandle.onReachedOriginal += SideHandleToOriginal;
        SolderingSnapPoint.SetActive(false);
        coverToSolderingSnapPoint.CoversnappedToSoldering += CoverSnappedToSolder;
        solderingMachine.enabled = false;
        solderingMachine.onProcessComplete += SolderingDone;
        coverToBackBox.CoversnappedToBlackBox += CoverSnappedToBlackBox;
        holderMachine.onReachedDesired += BlackBoxHolded;
        door1.Door1ReachedDesired += DoorClosed;
        // computerDisplay.enabled = false;
        door1.Door1ReachedOriginal += DoorOpened;
        holderMachine.onReachedOriginal += BlackBoxHolderOpened;
        UpperSnappingScript.UpperCoverSnappedToCover += UpperSnappedToCover;
        UpperToPuncherSnapped.UpperCoverSnappedToPuncher += UpperSnappedToPuncher;
        sideHandle2.onReachedDesired += SideHandlePlaced2;
        puncherHandle2.onReachedOriginal += PunchingDone2;
        sideHandle2.onReachedOriginal += SideHandleToOriginal2;
        upperToBlackBox.CoversnappedToBlackBox += UpperSnappedToBox;
        wireSnapping.WireSnapped += WireDoneSnapping;
        boxHandle.onReachedDesired += BoxHandleHolded;
        door2.Door2ReachedDesired += DoorClosed2;
        door2.Door2ReachedOriginal += DoorOpened2;
        boxHandle.onReachedOriginal += BoxHandleReleased;
        StickerPressing.UpperCoverOnPressing += UpperCoverSnappedToPressing;
        stickerSnapPoint.Stickersnapped += StickerSnapped;
        stickerPressingTrigger.onReachedDesired += StickerPressingClosed;
        greenButton.ButtonPressed += GreenButtonPressed;
        stickerPressingTrigger.onReachedOriginal += StickerPressingOpened;
        marking.MarkingDone += MarkingDone;
        FinalStandScript.DoneLevel += DoneFinal;





    }
    //Machine 1
    public void CoverGrabbed()
    {
        Debug.Log("Cover Grabbed");
        ChipHighlighter.Highlight();
        Arrow1.SetActive(false);
        Arrow2.SetActive(true);

    }

    public void ChipGrabbed()
    {
        Debug.Log("Chip Grabbed");
        Arrow2.SetActive(false);
        SphereOnCover.SetActive(true);
    }

    public void ChipSnappedToCover()
    {
        Debug.Log("Chip Snapped");
        CoverToPuncherSnapPoint.SetActive(true);
        SphereOnCover.SetActive(false);
        Arrow3.SetActive(true);
        SphereOnPuncher.SetActive(true);
        CoverOnPuncher.Highlight();

    }
    public void CoverSnappedToPuncher()
    {
        Debug.Log("Cover Snapped To Puncher");
        SideHandle.SetActive(true);
        CoverAfterPunched.enabled = false;
        Arrow3.SetActive(false);
        SphereOnPuncher.SetActive(false);
        Tooltip.SetActive(true);

    }
    public void SideHandlePlaced()
    {
        Debug.Log("Side Handle Placed");
        PunchersHandle.SetActive(true);
        Tooltip.SetActive(false);
        Tooltip2.SetActive(true);
    }
    public void PunchingDone()
    {
        Debug.Log("PunchingDone");
        sideHandle.Unlock();
        Tooltip3.SetActive(false);
        Tooltip4.SetActive(true);

    }
    public void SideHandleToOriginal()
    {
        Debug.Log("Side Handle To Original");
        sideHandle.PermanantlyLock();
        Tooltip4.SetActive(false);
        CoverAfterPunched.enabled = true;
        SolderingSnapPoint.SetActive(true);
        CoverOnPuncherPick.Highlight();

    }
    public void CoverGrabbedFromPuncher() // cover grabbed from puncher
    {
        SphereOnSoldering.SetActive(true);
        Arrow4.SetActive(true);
        SolderingSnappointHighlight.Highlight();
    }

    // Machine 2
    public void CoverSnappedToSolder() // cover snapped to soldering machine
    {
        Arrow4.SetActive(false);
        SphereOnSoldering.SetActive(false);
        Tooltip5.SetActive(true);
        solderingMachine.enabled = true;

    }

    public void SolderingDone()
    {
        CoverAfterSoldered.enabled = true;
        CoverAfterSolderedHighlight.Highlight();
    }

    public void CoverGrabbedAfterSoldering()
    {
        BlackBoxSnapPoint.SetActive(true);
        Arrow5.SetActive(true);
        SphereBlackBoxSnapPoint.SetActive(true);
        CoverOnBlackBox.Highlight();
    }

    //Machine 3
    public void CoverSnappedToBlackBox()
    {
        Arrow5.SetActive(false);
        SphereBlackBoxSnapPoint.SetActive(false);
        Tooltip6.SetActive(true);
    }

    public void BlackBoxHolded() // holder holded cover in black box
    {
        Tooltip6.SetActive(false);
        Tooltip7.SetActive(true);
        Arrow6.SetActive(true);

    }
    public void DoorClosed()
    {
        Debug.Log("Door closed");
        computerDisplay.enabled = true;
        Tooltip8.SetActive(true);
        DisplayCanvas.SetActive(true);

    }
    public void DoorOpened()
    {
        door1.PermanantlyLock();
        Tooltip9.SetActive(true);
        holderMachine.Unlock();
        Tooltip10.SetActive(false);

    }
    public void BlackBoxHolderOpened() // holder opened of black box
    {
        holderMachine.PermanantlyLock();
        CoverInBlackBox.enabled = true;
        CoverBlackBoxHighlighter.Highlight();
        Tooltip9.SetActive(false);

    }
    private bool grabbed = false;
    public void CoverGrabbedFromBlackBox() // cover grabbed from black box after process completes
    {
        if (!grabbed)
        {
            Debug.Log("pakad liya");
            Arrow7.SetActive(true);
            grabbed = true;
        }
    }

    public void MagnifyingChecked()
    {
        Arrow7.SetActive(false);
        Arrow8.SetActive(true); // pick coverupper from bin
    }
    private bool grabbed2 = false;
    public void UpperCoverBlackBox() // Upper cover grabbed from bin
    {
        if (!grabbed2)
        {
            Arrow8.SetActive(false);
            grabbed2 = true;
            UpperSnappingPoint.SetActive(true);
            UpperCoverSphere.SetActive(true);
            //UpperHighlightSphere.Highlight();
        }
    }

    public void UpperSnappedToCover()
    {
        UpperCoverSphere.SetActive(false);
        Arrow9.SetActive(true);
        SnapHighlighterInPuncher.SetActive(true);
        UpperOnPuncherHighlighter.Highlight();
        UpperOnPuncherSnapPoint.SetActive(true);
    }

    public void UpperSnappedToPuncher()
    {
        Arrow9.SetActive(false);
        SnapHighlighterInPuncher.SetActive(false);
        Tooltip11.SetActive(true);
        SideHandle2.SetActive(true);

    }
    public void SideHandlePlaced2()
    {
        Handle2.SetActive(true);
        Tooltip11.SetActive(false);
        Tooltip12.SetActive(true);
    }
    public void PunchingDone2()
    {
        sideHandle2.Unlock();
        Tooltip13.SetActive(false);
        Tooltip14.SetActive(true);
    }

    public void SideHandleToOriginal2()
    {
        Debug.Log("Side Handle To Original");
        sideHandle2.PermanantlyLock();
        Tooltip14.SetActive(false);
        UpperCoverAfterPunched.enabled = true;
        // SolderingSnapPoint.SetActive(true);
        UpperCoverOnPuncherPick2.Highlight();

    }
    private bool grabbed3 = false;
    public void UpperGrabbedFromPuncher2() // upper cover grabbed from puncher 2
    {
        if (!grabbed3)
        {
            grabbed3 = true;
            Arrow10.SetActive(true);
            SphereUpperOnBoxHighlighter2.SetActive(true);
            UpperOnBoxHighlighter2.Highlight();
            UpperSnapPoint.SetActive(true);
        }
    }

    public void UpperSnappedToBox()
    {
        Arrow10.SetActive(false);
        SphereUpperOnBoxHighlighter2.SetActive(false);
        Tooltip15.SetActive(true);
        wirePlugHighlight.Highlight();
        WireGrabbing.enabled = true;

    }

    public void WireGrabbed()
    {
        Tooltip15.SetActive(false);
        WireSphere.SetActive(true);
        wireSnappingGameObject.SetActive(true);
    }

    public void WireDoneSnapping()
    {
        WireSphere.SetActive(false);
        ButtonTrigger.SetActive(true);
        Tooltip16.SetActive(true);

    }
    public void BoxHandleHolded()
    {
        Tooltip16.SetActive(false);
        Arrow11.SetActive(true);
        Tooltip17.SetActive(true);
        door2.enabled = true;

    }
    public void DoorClosed2()
    {
        Debug.Log("Door closed");
        computerDisplay2.enabled = true;
        Tooltip18.SetActive(true);
        DisplayCanvas2.SetActive(true);

    }
    public void DoorOpened2()
    {
        door2.PermanantlyLock();
        boxHandle.Unlock();
        Tooltip19.SetActive(false);
        Tooltip20.SetActive(true); // holder of black box2 opening tooltip
    }
    public void BoxHandleReleased()
    {
        boxHandle.PermanantlyLock();
        Tooltip20.SetActive(false);
        CoverInBlackBox2.enabled = true;
        CoverBlackBox2Highlighter.Highlight();

    }
    public void UpperGrabbedFromBox2()
    {
        StickerPressingSnapPoint.SetActive(true);
        Arrow12.SetActive(true);
    }
    public void UpperCoverSnappedToPressing()
    {
        Arrow12.SetActive(false);
        Tooltip21.SetActive(true); //take sticker
        Sticker.SetActive(true);
    }
    public void StickerGrabbed()
    {
        Tooltip21.SetActive(false);
        Arrow13.SetActive(true);
        StickerSnapPoint.SetActive(true);
    }
    public void StickerSnapped()
    {
        Arrow13.SetActive(false);
        PressingTrigger.SetActive(true);
        Tooltip22.SetActive(true);
    }

    public void StickerPressingClosed()
    {
        Tooltip22.SetActive(false);
        Tooltip23.SetActive(true);
        GreenButtonScriptObject.SetActive(true);
    }
    public void GreenButtonPressed()
    {
        Tooltip23.SetActive(false);
        stickerPressingTrigger.Unlock();
        Tooltip24.SetActive(true);
    }
    public void StickerPressingOpened()
    {
        Tooltip24.SetActive(false);
        stickerPressingTrigger.PermanantlyLock();
        Arrow14.SetActive(true); // for marker
        Marker.enabled = true;

    }
    public void MarkerGrabbed()
    {
        Arrow14.SetActive(false); // for marker
        Arrow15.SetActive(true);
        MarkingPoint.SetActive(true);
    }
    public void MarkingDone()
    {
        Arrow15.SetActive(false);
        UpperAfterSticker.Highlight();
        UpperAfterStickerAttached.enabled = true;
    }
    public void UpperGrabbedAfterSticker()
    {
       Arrow16.SetActive(true); //final position arrow 
       FinalStandSnapPoint.SetActive(true);
    }
    public void DoneFinal()
    {
        Arrow16.SetActive(false);
    }


}
