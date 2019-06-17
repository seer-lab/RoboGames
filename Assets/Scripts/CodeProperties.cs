using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds data regarding spacing, sizes and positioning of various code objects in the game.
/// </summary>
public class CodeProperties 
{
    public float initialLineY = 3f;
    public float initialLineX = -4.47f;
    // Spacing between lines.
    public float linespacing = 0.825f;
    // Offset for line spacing.
    public float lineOffset = -0.3f;
    // Used for Bugs, prizes, and resizing the play area
    public float levelLineRatio = 0.55f;
    public float bugXshift = 1.12444f;
    public float fontwidth = 0.15f;
    public float bugsize = 1f;
    public float bugscale = 1.5f;
    public float textscale = 1.75f;
    public float losstime;
    public float lossdelay = 3f;
    public float leveldelay = 2f;
    public float startNextLevelTimeDelay = 0f;
    public int totalNumberOfTools = stateLib.NUMBER_OF_TOOLS;
    public Vector3 defaultPosition = new Vector3(0, 0, 0);
    public Vector3 defaultLocalScale = new Vector3(0, 0, 0);
}
