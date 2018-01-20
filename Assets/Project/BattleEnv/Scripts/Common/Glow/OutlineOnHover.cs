using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.Battle.Interaction;
using Placeholdernamespace.Battle.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Placeholdernamespace.Battle.Interaction
{
    public class OutlineOnHover : MonoBehaviour
    {

        public Color onHoverColor;
        private GlowManager glowManager;
        private Tile tile;
        private PathOnClick pathOnClick;
        private TileSelectionManager pathSelectManager;

        public static bool disabled = false;

        private bool IsActive
        {
            get { return !pathSelectManager.IsActive(); }
        }

        void Start()
        {
            glowManager = GetComponent<GlowManager>();
            tile = GetComponentInParent<Tile>();
            pathOnClick = GetComponent<PathOnClick>();
        }

        public void Init(TileSelectionManager pathSelectManager)
        {
            this.pathSelectManager = pathSelectManager;
        }

        public void OnMouseEnter()
        {
            if (!UIHoverListener.isUIOverride)
            {
                if (tile.BoardEntity != null && tile.BoardEntity.GetComponentInChildren<GlowManager>() != null)
                {
                    tile.BoardEntity.Hover();
                    tile.BoardEntity.GetComponentInChildren<GlowManager>().TurnOn(this, onHoverColor, onlyAddIfStackEmpty: true, putOnColorStack: false);
                }
                glowManager.TurnOn(this, onHoverColor, onlyAddIfStackEmpty: true, putOnColorStack: false);
            }
        }

        public void OnMouseExit()
        {
            if (tile.BoardEntity != null && tile.BoardEntity.GetComponentInChildren<GlowManager>() != null)
            {
                tile.BoardEntity.ExitHover();
                tile.BoardEntity.GetComponentInChildren<GlowManager>().TurnOff(this, false);
            }
            glowManager.TurnOff(this, false);
        }
    }
}
