using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jam-o-Drum Debugger. A debug label will be showed in the center of the screen.
/// </summary>
[RequireComponent(typeof(JamoDrum))]
public class JamoDrumDebugInfo : MonoBehaviour
{
    /// <summary>
    /// Show debug information in Unity editor mode only.
    /// </summary>
    [SerializeField]
    private bool showInEditorOnly = true;

    private JamoDrum jodrum;

    private int[] spinDelta = new int[4];
    private int[] hits = new int[4];
    private int[] releases = new int[4];

    private void Start()
    {
        jodrum = GetComponent<JamoDrum>();
        Debug.Assert(jodrum != null);

        jodrum.AddHitEvent(OnDrumHit);
        jodrum.AddSpinEvent(OnDrumSpin);
        jodrum.AddReleaseEvent(OnDrumRelease);
    }
    
    private void Update()
    {
        //
    }

    private void OnDrumHit(int ctrlId)
    {
        hits[ctrlId - 1] += 1;
    }

    private void OnDrumRelease(int ctrlId)
    {
        releases[ctrlId - 1] += 1;
    }

    private void OnDrumSpin(int ctrlId, int delta)
    {
        spinDelta[ctrlId - 1] += delta;
    }

    private void OnGUI()
    {
        if (!showInEditorOnly)
            return;

        // We cannot get data from OnLateUpdate because it is cleared by the script
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.red;

        int width = Screen.width;
        int height = Screen.height;

        Vector2 center = new Vector2(width / 2.0f, height / 2.0f);
        Vector2 lineSize = new Vector2(200f, 40f);

        for (int i = 1; i <= 4; ++i)
        {
            GUI.Label(
                new Rect(center + new Vector2(-lineSize.x / 2f, lineSize.y * (i - 3)), lineSize),
                new GUIContent() { text = string.Format("Drum {0}: Hit: {1,3:D} Spin： {2,3:D}", i, hits[i - 1], spinDelta[i - 1]) },
                style
            );
        }
    }
}
