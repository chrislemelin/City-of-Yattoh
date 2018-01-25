using UnityEngine;
using System.Collections;
using Placeholdernamespace.Common.Interfaces;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Interaction;
using UnityEngine.EventSystems;

namespace Placeholdernamespace.Common.Utils
{
    public class BoardEntityEventRedirect : OnClickRedirect, IPointerClickHandler, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        CharacterBoardEntity characterBoardEntity;

        public override IClickable GetTarget()
        {
            return ((IClickable)characterBoardEntity.GetTile().GetComponentInChildren<PathOnClick>());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //characterBoardEntity.GetTile().GetComponentInChildren<OutlineOnHover>().OnMousUp();
            characterBoardEntity.GetTile().GetComponentInChildren<PathOnClick>().OnMouseUpHelper();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
            characterBoardEntity.GetTile().GetComponentInChildren<OutlineOnHover>().OnMouseEnter();
            characterBoardEntity.GetTile().GetComponentInChildren<PathOnClick>().OnMouseEnterHelper();
        }

        public void OnMouseOver()
        {
            //characterBoardEntity.GetTile().GetComponentInChildren<PathOnClick>().OnMouseOver();
            //characterBoardEntity.GetTile().GetComponentInChildren<OutlineOnHover>().OnMouseOver();


        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //CharacterBoardEntity c = GetComponentInParent<CharacterBoardEntity>();
            characterBoardEntity.GetTile().GetComponentInChildren<OutlineOnHover>().OnMouseExit();
            characterBoardEntity.GetTile().GetComponentInChildren<PathOnClick>().OnMouseExit();
        }
    }
}