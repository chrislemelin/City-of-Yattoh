using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.Env;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePropertyManager : MonoBehaviour {

    private static ScenePropertyManager instance;
    public static ScenePropertyManager Instance
    {
        get {

            return instance;
        }
    }

    private Dictionary<CharacterType, GameObject> boardEntityCharacters = new Dictionary<CharacterType, GameObject>();
    public Dictionary<CharacterType, GameObject> BoardEntityCharacters
    {
        get { return new Dictionary<CharacterType, GameObject>(boardEntityCharacters); }
    }

    public Dictionary<CharacterType, CharContainer> typeToContainer = new Dictionary<CharacterType, CharContainer>();
    public Dictionary<CharacterType, CharContainer> TypeToContainer
    {
        get { return new Dictionary<CharacterType, CharContainer>(typeToContainer); }
    }

    [SerializeField]
    private List<GameObject> boardEntities;


    void Awake()
    {
        MakeDictionary();
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        initCharacters2();
    }

    private void MakeDictionary()
    {
        foreach (GameObject character in boardEntities)
        {
            if (character.GetComponent<CharacterBoardEntity>() != null)
            {
                CharacterType type = character.GetComponent<CharacterBoardEntity>().CharcaterType;
                boardEntityCharacters.Add(type, character);
                typeToContainer.Add(type, character.GetComponent<CharContainer>());
                character.GetComponent<CharContainer>().Init(character.GetComponent<CharacterBoardEntity>());
                character.GetComponent<CharacterBoardEntity>().PartialInit();
            }
        }
    }


   // public Dictionary<Position, CharacterType> characters;
    public List<CharacterType> characters = new List<CharacterType>() {CharacterType.PlayerBongani, CharacterType.PlayerTisha,
        CharacterType.PlayerJaz, CharacterType.PlayerDadi};

    public List<Tuple<CharacterType, Ka>> characters2 = new List<Tuple<CharacterType, Ka>>() {};

    private void  initCharacters2()
    {
        characters2.Add(new Tuple<CharacterType, Ka>(CharacterType.PlayerBongani, new Ka(typeToContainer[CharacterType.PlayerLesidi])));
        characters2.Add(new Tuple<CharacterType, Ka>(CharacterType.PlayerAmare, null));
        characters2.Add(new Tuple<CharacterType, Ka>(CharacterType.PlayerJaz, null));
        characters2.Add(new Tuple<CharacterType, Ka>(CharacterType.PlayerTisha, null));
    }

}
