using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Kas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.CharacterSelection
{
    public class PremadeParties : MonoBehaviour
    {

        public static List<Tuple<CharacterBoardEntity, Ka>> PresetPartyOne()
        {
            Ka AmareKa = new Ka(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerAmare]);
            AmareKa.AddPassive(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerAmare].Passive);
            Ka TishaKa = new Ka(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerTisha]);
            TishaKa.AddPassive(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerTisha].Passive);

            return new List<Tuple<CharacterBoardEntity, Ka>>()
            {
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerLesidi],AmareKa),
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerDadi],TishaKa),
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerJaz],null),
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerBongani],null),
            };
        }

        public static List<Tuple<CharacterBoardEntity, Ka>> PresetPartyTwo()
        { 
            Ka DadiKa = new Ka(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerDadi]);
            DadiKa.AddPassive(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerDadi].Passive);
            Ka BonganiKa = new Ka(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerBongani]);
            BonganiKa.AddPassive(ScenePropertyManager.Instance.typeToContainer[CharacterType.PlayerBongani].Passive);

            return new List<Tuple<CharacterBoardEntity, Ka>>()
            {
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerLesidi],null),
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerJaz], DadiKa),
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerTisha], BonganiKa),
                new Tuple<CharacterBoardEntity, Ka>(ScenePropertyManager.Instance.typeToBE[CharacterType.PlayerAmare],null),
            };
        }

        [SerializeField]
        private CharacterView2 characterView2;

        public void PresetOne()
        {
            ScenePropertyManager.Instance.SetCharacterParty(PresetPartyOne());
            characterView2.Clear();
        }

        public void PresetTwo()
        {
            ScenePropertyManager.Instance.SetCharacterParty(PresetPartyTwo());
            characterView2.Clear();
        }

    }
}