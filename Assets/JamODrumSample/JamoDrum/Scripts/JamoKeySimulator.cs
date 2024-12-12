//Exmaple Jam-o-Drum simulator
//Andrew Roxby 2/29/12
//Yikai Han 10/3/19

using UnityEngine;

/// <summary>
/// Jam-o-Drum Simulator.
/// Note it only works for scripts that are using AddHitEvent and AddSpinEvent.
/// </summary>
[RequireComponent(typeof(JamoDrum))]
public class JamoKeySimulator : MonoBehaviour
{
    // Keys to simulate left spin
    private KeyCode[] spinLeft = { KeyCode.A, KeyCode.Keypad7, KeyCode.Keypad4, KeyCode.Keypad1 };

    // Keys to simulate right spin
    private KeyCode[] spinRight = { KeyCode.D, KeyCode.Keypad9, KeyCode.Keypad6, KeyCode.Keypad3 };

    // Keys to simulate hit
    private KeyCode[] hit = { KeyCode.S, KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad2 };

    // ALT KEY BINDINGS
    // Keys to simulate left spin
    // private KeyCode[] spinLeft = { KeyCode.Q, KeyCode.A, KeyCode.Z, KeyCode.I };
    // // Keys to simulate right spin
    // private KeyCode[] spinRight = { KeyCode.E, KeyCode.D, KeyCode.C, KeyCode.P };
    // // Keys to simulate hit
    // private KeyCode[] hit = { KeyCode.W, KeyCode.S, KeyCode.X, KeyCode.O };




    // Simulated spin delta per frame
    public int spinsPerFrame = 3;

    // Jam-o-Drum instance.
    private JamoDrum jamoDrum;

    private void Start()
    {
        jamoDrum = GetComponent<JamoDrum>();
        if (jamoDrum == null)
        {
            Debug.LogError("JamoKey script has to be attached on the object with JamoDrum object!");
        }
    }

    private void Update()
    {
        if (jamoDrum == null) return;

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(spinLeft[i])) jamoDrum.InjectSpin(i + 1, -spinsPerFrame);
            if (Input.GetKey(spinRight[i])) jamoDrum.InjectSpin(i + 1, spinsPerFrame);
            if (Input.GetKeyDown(hit[i])) jamoDrum.InjectHit(i + 1);
            if (Input.GetKeyUp(hit[i])) jamoDrum.InjectRelease(i + 1);
        }
    }
}
