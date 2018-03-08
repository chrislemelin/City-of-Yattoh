using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Dialouge
{
    public enum DialougeCharacterType
    {
        PlayerBongani, PlayerAmare, PlayerDadi, PlayerLesidi, PlayerJaz
    }

    public enum DialogueCharacterExpression
    {
        Normal, Angry, Sad
    }

    public class DialogueCharacter : MonoBehaviour
    {
        [SerializeField]
        public string name;
        public string Name
        {
            get { return name; }
        }

        [SerializeField]
        private Sprite defaultExpression;

        [SerializeField]
        private List<Sprite> expressionSprites;

        [SerializeField]
        private List<DialogueCharacterExpression> expressionTypes;

        private Dictionary<DialogueCharacterExpression, Sprite> expressionToSprite = new Dictionary<DialogueCharacterExpression, Sprite>();

        private void Start()
        {
            for(int a = 0; a < expressionSprites.Count && a < expressionTypes.Count; a++)
            {
                expressionToSprite.Add(expressionTypes[a], expressionSprites[a]);
            }
        }

        public Sprite GetSprite(DialogueCharacterExpression expression = DialogueCharacterExpression.Normal)
        {
            if(expressionToSprite.ContainsKey(expression))
            {
                return expressionToSprite[expression];
            }
            else
            {
                return defaultExpression;
            }
        }

    }
}
