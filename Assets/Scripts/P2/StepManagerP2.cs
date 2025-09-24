using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StepManagerP2 : MonoBehaviour
{
    [Header("MainObjects")]
    [Header("Machine1")]
    public GameObject ChipSnapPointObject; // object which has chip snapping script
    public ChipToDrawerSnapPoint chipToDrawerSnap; // after chip snapped then it send message
    public GameObject DoorScriptObject;
    public DrawerDoor drawerDoor;
    public XRGrabInteractable ChipInDrawer;
    [Header("Machine2")]
    public GameObject ChipOnCheckerSnapPointObject; //object which has chip snapping script on checker
    public ChipToChipCheckerSnapPoint chipTocheckerSnap;
    public GameObject ChipCheckerHandleScript;
    public CheckerHandle checkerHandle; // chip checker moving handle script
    public XRGrabInteractable BackCoverMain;
    public GameObject SphereObjectUnlockButton;
    public XRGrabInteractable UnlockButtonGrab; //Unlock button grabbable
    public UnlockButtonSnapPoint unlockButtonSnapPoint;
    public GameObject UnlockSnapPoint; // snappoint script attached object
    public XRGrabInteractable LockButtonGrab; // //Lock button grabbable
    public GameObject SphereObjectLockButton;
    public LockButtonSnapPoint lockButtonSnapPoint;
    public GameObject LockSnapPoint; //  snappoint script attached object
    public XRGrabInteractable RubberCoverGrab;
    public GameObject SphereObjectRubber;
    public GameObject RubberSnapPoint; //  snappoint script attached object
    public RubberSnapPoint rubberSnapPoint;
    public XRGrabInteractable ChipGrab;
    public GameObject SphereObjectChip;
    public GameObject ChipSnapPoint; //  snappoint script attached object
    public ChipToBackCoverSnapPoint chipTobackCoverSnapPoint;
    public GameObject SphereObjectCoverOnPunching;
    public GameObject PunchingMachineSnapPoint;  //  snappoint script attached object
    public PunchingMachineSnapPoint punchingMachineSnapPoint;
    public XRGrabInteractable BatteryPlacerGrab;
    public GameObject SphereObjectBatteryPlacer;
    public GameObject BatteryPlacerSnapPoint;
    public BatteryCoverSnapPoint batteryCoverSnapPoint;
    public PunchingMachine punchingMachine;
    public XRGrabInteractable BackCoverOnPunchingMachine;
    public GameObject PressingMachineScriptObject; // Punching script attached to object
    public XRGrabInteractable BatteryGrab;
    public GameObject BatterySnapPoint; // snappoint script attached object
    public GameObject SphereObjectBattery;
    public BatterySnapPoint batterySnapPoint;
    [Header("Machine3")]
    public GameObject SphereObjectBackCoverOnAssembly;
    public GameObject BackCoverSnapPointOnAssembly;
    public BackCoverOnAssembly backCoverOnAssembly;
    public XRGrabInteractable FrontCoverGrab;
    public GameObject FrontCoverOnAssemblySnapPoint; // snappoint script attached object
    public GameObject ButtonScriptObject;
    public GreenButtonP2 greenButtonP2;
    public XRGrabInteractable FrontCoverFromAssemblyGrab;
    public FrontCoverOnAssembly frontCoverOnAssembly;
    public GameObject SphereObjectFrontCoverOnBackCover;
    public GameObject FrontOnCoverSnapPoint; //snappoint script attached object
    public FrontOnBackSnapPoint frontOnBackSnapPoint;


    [Header("Highlighter")]
    [Header("Machine1")]
    public StepWiseHighlighter ChipInStartHighlight; //picking chip from table
    public GameObject Arrow1;
    public GameObject SphereObjectChipOnDrawer;
    public StepWiseHighlighter SphereChipOnDrawer; // sphere chip which highlights chip snapping position
    public GameObject Arrow2;
    public GameObject Tooltip1;
    public GameObject Tooltip2;
    public GameObject Arrow3;
    [Header("Machine2")]
    public GameObject Arrow4;
    public StepWiseHighlighter SphereChipOnChecker; // sphere chip which highlights chip snapping position on checker
    public GameObject SphereObjectChipOnChecker;
    public GameObject Tooltip3;
    public GameObject Arrow5;
    public StepWiseHighlighter SphereBackCoverMain;
    public GameObject Arrow6;
    public StepWiseHighlighter UnlockButton;
    public StepWiseHighlighter SphereUnlockHighlighter;
    public GameObject Arrow7;
    public StepWiseHighlighter LockButton;
    public StepWiseHighlighter SphereLockHighlighter;
    public GameObject Arrow8;
    public StepWiseHighlighter SphereRubberCoverHighlight;
    public StepWiseHighlighter SphereRubberHighlight;
    public GameObject Tooltip4;
    public GameObject Arrow9;
    public StepWiseHighlighter ChipOnCheckerHighlight;
    public StepWiseHighlighter SphereChipHighlight;
    public GameObject Arrow10;
    public StepWiseHighlighter SphereCoverOnPunchingHighlight;
    public GameObject Arrow11;
    public StepWiseHighlighter BatteryPlacerHighlight;
    public StepWiseHighlighter SphereHighlightBatteryPlacer;
    public GameObject Arrow12;
    public GameObject Tooltip5;
    public GameObject Arrow13;
    public StepWiseHighlighter BatteryHighlighter;
    public GameObject Arrow14;
    public StepWiseHighlighter SphereBatteryHighlighter;
    [Header("Machine3")]
    public StepWiseHighlighter SphereBackCover;
    public GameObject Tooltip6;
    public GameObject Arrow15;
    public StepWiseHighlighter FrontCoverHighlight;
    public GameObject Arrow16;
    public GameObject Tooltip7;
    public GameObject Arrow17;
    public StepWiseHighlighter FrontCoverOnAssemblyHighlight;
    public StepWiseHighlighter SphereFrontCoverOnBackCoverHighlight;
    public GameObject Tooltip8;


    [Header("Display1")]
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;

    void Start()
    {
        ChipInStartHighlight.Highlight();
        chipToDrawerSnap.Chipsnapped += ChipSnapped;
        drawerDoor.onReachedOriginal += DoorOpened;
        chipTocheckerSnap.ChipsnappedToChecker += ChipSnappedToChecker;
        checkerHandle.onReachedDesired += CheckerClosed;
        unlockButtonSnapPoint.UnlockSnapped += UnlockButtonSnapped;
        lockButtonSnapPoint.LockSnapped += LockButtonSnapped;
        rubberSnapPoint.RubberSnapped += RubberSnapped;
        checkerHandle.onReachedOriginal += CheckerOpened;
        chipTobackCoverSnapPoint.ChipsnappedToBackCover += ChipSnappedToCover;
        punchingMachineSnapPoint.CoversnappedToPunching += CoverSnappedToPunching;
        batteryCoverSnapPoint.BatteryPlacerSnapped += BatteryPlacerSnapped;
        punchingMachine.onReachedOriginal += PunchingDone;
        batterySnapPoint.BatterySnapped += BatterySnapped;
        backCoverOnAssembly.BackOnMachineSnapped += BackSnappedToAssembly;
        frontCoverOnAssembly.FrontOnMachineSnapped += FrontCoverSnapped;
        greenButtonP2.CameraChecked += CameraChecked;
        frontOnBackSnapPoint.FrontOnBackSnapped += FrontSnappedOnBack;
    }
    //Machine1
    private bool grabbingDone = false;
    public void ChipGrabbed()
    {
        if (!grabbingDone)
        {
            Arrow1.SetActive(false);
            SphereObjectChipOnDrawer.SetActive(true);
            SphereChipOnDrawer.Highlight();
            Arrow2.SetActive(true);
            ChipSnapPointObject.SetActive(true);
            grabbingDone = true;
        }

    }
    public void ChipSnapped()
    {
        Arrow2.SetActive(false);
        SphereObjectChipOnDrawer.SetActive(false);
        Tooltip1.SetActive(true);
        DoorScriptObject.SetActive(true);
    }
    public void DoorClosed()
    {
        Tooltip1.SetActive(false);
        StartCoroutine(DoorDisplay());

    }

    public IEnumerator DoorDisplay()
    {
        Button1.SetActive(true);
        yield return new WaitForSeconds(4);
        Button1.SetActive(false);
        Button2.SetActive(true);
        drawerDoor.Unlock();
        Tooltip2.SetActive(true);
    }
    public void DoorOpened()
    {
        Tooltip2.SetActive(false);
        ChipInDrawer.enabled = true;
        Arrow3.SetActive(true);
    }
    //Machine2
    public void ChipGrabbedFromDrawer()
    {
        Arrow3.SetActive(false);
        Arrow4.SetActive(true);
        ChipOnCheckerSnapPointObject.SetActive(true);
        SphereObjectChipOnChecker.SetActive(true);
        SphereChipOnChecker.Highlight();
    }
    public void ChipSnappedToChecker()
    {
        Arrow4.SetActive(false);
        SphereObjectChipOnChecker.SetActive(false);
        Tooltip3.SetActive(true);
        ChipCheckerHandleScript.SetActive(true);
    }
    public void CheckerClosed()
    {
        Tooltip3.SetActive(false);
        Arrow5.SetActive(true);
        BackCoverMain.enabled = true;
        SphereBackCoverMain.Highlight();
        StartCoroutine(DoorDisplay2());
    }
    public IEnumerator DoorDisplay2()
    {
        Button3.SetActive(true);
        yield return new WaitForSeconds(10);
        Button3.SetActive(false);
        Button4.SetActive(true);
    }
    public void BackCoverGrabbed()
    {
        Arrow5.SetActive(false);
        Arrow6.SetActive(true); // arrow for unlock button showing
        UnlockButton.Highlight();
        UnlockButtonGrab.enabled = true;

    }

    public void UnlockButtonGrabbed()
    {
        Arrow6.SetActive(false);
        SphereObjectUnlockButton.SetActive(true);
        SphereUnlockHighlighter.Highlight();
        UnlockSnapPoint.SetActive(true);
    }
    public void UnlockButtonSnapped()
    {
        SphereObjectUnlockButton.SetActive(false);
        Arrow7.SetActive(true);
        LockButtonGrab.enabled = true;

    }
    public void LockButtonGrabbed()
    {
        Arrow7.SetActive(false);
        SphereObjectLockButton.SetActive(true);
        SphereLockHighlighter.Highlight();
        LockSnapPoint.SetActive(true);
    }
    public void LockButtonSnapped()
    {
        SphereObjectLockButton.SetActive(false);
        Arrow8.SetActive(true);
        SphereRubberCoverHighlight.Highlight();
        RubberCoverGrab.enabled = true;
    }
    public void RubberCoverGrabbed()
    {
        Arrow8.SetActive(false);
        SphereObjectRubber.SetActive(true);
        SphereRubberHighlight.Highlight();
        RubberSnapPoint.SetActive(true);
    }
    public void RubberSnapped()
    {
        SphereObjectRubber.SetActive(false);
        checkerHandle.Unlock();
        Tooltip4.SetActive(true);
    }
    public void CheckerOpened()
    {
        Tooltip4.SetActive(false);
        checkerHandle.PermanantlyLock();
        ChipOnCheckerHighlight.Highlight();
        Arrow9.SetActive(true);
        ChipGrab.enabled = true;
    }
    public void ChipGrabbedFromChecker()
    {
        Arrow9.SetActive(false);
        SphereObjectChip.SetActive(true);
        SphereChipHighlight.Highlight();
        ChipSnapPoint.SetActive(true);

    }
    public void ChipSnappedToCover()
    {
        SphereObjectChip.SetActive(false);
        Arrow10.SetActive(true);
        SphereObjectCoverOnPunching.SetActive(true);
        SphereCoverOnPunchingHighlight.Highlight();
        PunchingMachineSnapPoint.SetActive(true);
    }
    public void CoverSnappedToPunching()
    {
        Arrow10.SetActive(false);
        SphereObjectCoverOnPunching.SetActive(false);
        Arrow11.SetActive(true);
        BatteryPlacerGrab.enabled = true;
        BatteryPlacerHighlight.Highlight();
    }
    public void BatteryCoverGrabbed()
    {
        Arrow11.SetActive(false);
        SphereObjectBatteryPlacer.SetActive(true);
        SphereHighlightBatteryPlacer.Highlight();
        Arrow12.SetActive(true);
        BatteryPlacerSnapPoint.SetActive(true);

    }
    public void BatteryPlacerSnapped()
    {
        Arrow12.SetActive(false);
        SphereObjectBatteryPlacer.SetActive(false);
        Tooltip5.SetActive(true);
        PressingMachineScriptObject.SetActive(true);
    }
    public void PunchingDone()
    {
        Debug.Log("PunchingDone ");
        BackCoverOnPunchingMachine.enabled = true;
        Arrow13.SetActive(true);
    }
    public void ChipGrabbedFromPunching()
    {
        Arrow13.SetActive(false);
        BatteryGrab.enabled = true;
        BatteryHighlighter.Highlight();
        Arrow14.SetActive(true);
    }
    public void BatteryGrabbed()
    {
        Arrow14.SetActive(false);
        BatterySnapPoint.SetActive(true);
        SphereObjectBattery.SetActive(true);
        SphereBatteryHighlighter.Highlight();

    }
    public void BatterySnapped()
    {
        SphereObjectBattery.SetActive(false);
        SphereObjectBackCoverOnAssembly.SetActive(true);
        BackCoverSnapPointOnAssembly.SetActive(true);
        SphereBackCover.Highlight();
        Tooltip6.SetActive(true);
    }
    public void BackSnappedToAssembly()
    {
        SphereObjectBackCoverOnAssembly.SetActive(false);
        Tooltip6.SetActive(false);
        FrontCoverGrab.enabled = true;
        Arrow15.SetActive(true);
        FrontCoverHighlight.Highlight();
    }
    public void FrontCoverGrabbed()
    {
        Arrow15.SetActive(false);
        Arrow16.SetActive(true);
        FrontCoverOnAssemblySnapPoint.SetActive(true);
    }
    public void FrontCoverSnapped()
    {
        Arrow16.SetActive(false);
        Tooltip7.SetActive(true);
        ButtonScriptObject.SetActive(true);
        greenButtonP2.FrontCoverSnapped();
    }
    public void CameraChecked()
    {
        Arrow17.SetActive(true);
        FrontCoverOnAssemblyHighlight.Highlight();
        FrontCoverFromAssemblyGrab.enabled = true;
    }
    public void FrontCoverGrabbedFromAssembly()
    {
        Arrow17.SetActive(false);
        SphereObjectFrontCoverOnBackCover.SetActive(true);
        SphereFrontCoverOnBackCoverHighlight.Highlight();
        FrontOnCoverSnapPoint.SetActive(true);
    }
    public void FrontSnappedOnBack()
    {
        SphereObjectFrontCoverOnBackCover.SetActive(false);
        Tooltip8.SetActive(true);
    }


}
 