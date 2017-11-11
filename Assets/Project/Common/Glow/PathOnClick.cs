using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Battle.Interaction
{
    public class PathOnClick : MonoBehaviour, IClickable
    {

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

        public void OnMouseDown()
        {
            if (tile.BoardEntity != null && !pathSelectManager.SelectedBoardEntity == tile.BoardEntity)
            {
                tile.BoardEntity.OnSelect();
            }
            else
            {
                pathSelectManager.TileClicked(this);
            }
        }

        public void OnMouseEnter()
        {
            if (pathSelectManager.SelectedBoardEntity != null)
            {
                pathSelectManager.TileHover(this);
            }
        }


    }
}