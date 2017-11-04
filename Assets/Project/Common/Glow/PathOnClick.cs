using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathOnClick : MonoBehaviour, IClickable{

    public Color BoardEntityOnSelectColor;
    public Color TileOnSelectColor;


    private Tile tile;
    public Tile Tile
    {
        get { return tile; }
    }

    private GlowManager glowManager;
    public GlowManager GlowManager
    {
        get{return glowManager; }
    }

    private ColorEffectManager colorEffectManager;
    public ColorEffectManager ColorEffectManager
    {
        get { return colorEffectManager; }
    }

    public static bool active = true;

    private bool isHighlighted;
    public bool IsHighlighted
    {
        get { return isHighlighted; }
    }

    private static List<PathOnClick> clickedPath = new List<PathOnClick>();
    public static List<PathOnClick> ClickedPath
    {
        get { return clickedPath; }
    } 

    public void Start()
    {
        colorEffectManager = GetComponent<ColorEffectManager>();
        glowManager = GetComponent<GlowManager>();
        tile = GetComponentInParent<Tile>();
    }

    public void OnMouseDown()
    {
        PathSelectManager.Instance.TileSelected(this);   
    }

    public void OnMouseEnter()
    {
        if (PathSelectManager.Instance.SelectedTile != null)
        {
            PathSelectManager.Instance.TileHover(this);
        }
    }


}
