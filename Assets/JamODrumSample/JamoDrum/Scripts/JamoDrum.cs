//Jam-o-Drum unity interface
//Orignal version by Bryan Maher (27-Jan-2012)
//Updated by Andrew Roxby 2/29/12
//Updated by Yikai Han 10/3/19 to upgrade to Unity 2019.2

using System.Threading;
using UnityEngine;
using ETC.Platforms;

// Update input prior to game logic input
[DefaultExecutionOrder(-100)]
public class JamoDrum : MonoBehaviour
{
    private enum UpdateMode
    {
        OnUpdate, OnFixedUpdate
    }

    [SerializeField]
    [Tooltip("Choose when the JamoDrum input will be updated")]
    private UpdateMode updateMode = UpdateMode.OnUpdate;

    [SerializeField]
    [Tooltip("Choose if mouse cursoe will be hidden on startup")]
    private bool hideMouse = true;

    [Header("Debug")]
    [Tooltip("Show JamoDrumClient log. It will not take effect after your game starts.")]
    [SerializeField]
    private bool showDebugLog = false;

    [Header("Read Only")]
    public int[] spinDelta = new int[4];
    public int[] hit = new int[4];
    public int[] release = new int[4];

    // Buffer for input sync
    private int[] spinDeltaBuffer = new int[4];
    private int[] hitBuffer = new int[4];
    private int[] releaseBuffer = new int[4];

    private static JamoDrumClient jod = null;

    private ETC.Platforms.HitEventHandler hitEvents;
    private ETC.Platforms.SpinEventHandler spinEvents;
    private ETC.Platforms.HitEventHandler releaseEvents;

    private void Start()
    {
        JamoDrumClient.ShowDebugMessage = showDebugLog;

        if (hideMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (jod == null) jod = JamoDrumClient.Instance;

        jod.Hit += HandleJodHit;
        jod.Spin += HandleJodSpin;
        jod.Release += HandleJodRelease;

        hitEvents += DummyMethodPreventsEmptyCallbacks;
        spinEvents += DummyMethodPreventsEmptyCallbacks;
        releaseEvents += DummyMethodPreventsEmptyCallbacks;
    }

    private void OnDestroy()
    {
        jod.Hit -= HandleJodHit;
        jod.Spin -= HandleJodSpin;
        jod.Release -= HandleJodRelease;
    }

    private void DummyMethodPreventsEmptyCallbacks(int controllerID)
    {
        //
    }

    private void DummyMethodPreventsEmptyCallbacks(int controllerID, int delta)
    {
        //
    }

    /// <summary>
    /// This is the code that is run when a pad is hit.
    /// </summary>
    /// <param name="controllerID">
    /// This is a number from 1 to 4 indicating which controller
    /// is sending the message.
    /// </param>
    private void HandleJodHit(int controllerID)
    {
        Interlocked.Increment(ref hitBuffer[controllerID - 1]);
    }

    private void HandleJodRelease(int controllerID)
    {
        Interlocked.Increment(ref releaseBuffer[controllerID - 1]);
    }

    /// <summary>
    /// This is the code that runs when any one of the
    /// spinners is rotated.
    /// </summary>
    /// <param name="controllerID">
    /// This is a number from 1 to 4 indicating which controller
    /// is sending the message.
    /// </param>
    /// <param name="delta">
    /// This is how much the spinner changed since the last event.
    /// The value is directly from the underlying mouse hardware,
    /// there is no "unit" for this number.  That is, it does not
    /// represent degrees of rotation, or millimeters of movement,
    /// it's just a relative number.
    /// In the future, we may want to have a calibration routine
    /// which determines how many delta values there are per
    /// rotation of the spinner.  This would allow us to convert
    /// the number to degrees of rotation.
    /// </param>
    private void HandleJodSpin(int controllerID, int delta)
    {
        Interlocked.Add(ref spinDeltaBuffer[controllerID - 1], delta);
    }

    public void AddHitEvent(HitEventHandler func)
    {
        hitEvents += func;
    }

    public void AddSpinEvent(SpinEventHandler func)
    {
        spinEvents += func;
    }

    public void AddReleaseEvent(HitEventHandler func)
    {
        releaseEvents += func;
    }

    public void InjectHit(int controllerID)
    {
        HandleJodHit(controllerID);
    }

    public void InjectSpin(int controllerID, int delta)
    {
        HandleJodSpin(controllerID, delta);
    }

    public void InjectRelease(int controllerID)
    {
        HandleJodRelease(controllerID);
    }

    private void FixedUpdate()
    {
        if (updateMode == UpdateMode.OnFixedUpdate)
        {
            UpdateInput();
        }
    }

    private void Update()
    {
        if (updateMode == UpdateMode.OnUpdate)
        {
            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        // Copy buffer data
        for (int i = 0; i < 4; i++)
        {
            spinDelta[i] = spinDeltaBuffer[i];
            hit[i] = hitBuffer[i];
            release[i] = releaseBuffer[i];
        }

        // Clear buffer data
        for (int i = 0; i < 4; i++)
        {
            Interlocked.Add(ref spinDeltaBuffer[i], -spinDelta[i]);
            Interlocked.Add(ref hitBuffer[i], -hit[i]);
            Interlocked.Add(ref releaseBuffer[i], -release[i]);
        }

        // Invoke event callbacks
        for (int i = 0; i < 4; i++)
        {
            for (int n = 0; n < hit[i]; n++)
            {
                hitEvents(i + 1);
            }
            for (int n = 0; n < release[i]; n++)
            {
                releaseEvents(i + 1);
            }
            if (spinDelta[i] != 0) spinEvents(i + 1, spinDelta[i]);
        }
    }
}
