using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Interaction
{
    public class PathOnClick : MonoBehaviour, IClickable
    {
        public static bool pause = false;
        private TileSelectionManager pathSelectManager;
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
            get { return glowManager; }
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

        public void Init(TileSelectionManager pathSelectManager)
        {
            this.pathSelectManager = pathSelectManager;
        }

        public void Start()
        {
            colorEffectManager = GetComponent<ColorEffectManager>();
            glowManager = GetComponent<GlowManager>();
            tile = GetComponentInParent<Tile>();
        }

        public void OnMouseUp()
        {
            if (!pause && !UIHoverListener.isUIOverride)
            {
                // pass the select on to the board entity
                if (tile.BoardEntity != null && !pathSelectManager.IsActive())
                {
                    tile.BoardEntity.OnSelect();
                }
                // pass the select onto the tile
                else
                {
                    pathSelectManager.TileClicked(this);
                }
            }
        }

        public void OnMouseEnter()
        {
            if (!pause && !UIHoverListener.isUIOverride)
            {
                if (pathSelectManager.IsActive())
                {
                    pathSelectManager.TileHover(this);
                }
            }
        }

        public void OnMouseExit()
        {
            if (!pause && !UIHoverListener.isUIOverride)
            {
                if (pathSelectManager.IsActive())
                {
                    pathSelectManager.ClearTileHover();
                    
                }
            }
        }


    }
}