using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.Instances;
using Placeholdernamespace.Battle.Entities.Kas;
using Placeholdernamespace.Battle.Env;
using Placeholdernamespace.CharacterSelection;
using System;
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

    public Dictionary<Position, CharacterType> Enemies = new Dictionary<Position, CharacterType>() {
        { new Position(9, 9), CharacterType.EnemyTank },
       // { new Position(4, 0), CharacterType.EnemyRanged },
        //{ new Position(3, 1), CharacterType.EnemyBalanced },
        { new Position(4, 4), CharacterType.EnemyBalanced }
        //{ new Position(9, 8), CharacterType.EnemySpeedy},
        //{ new Position(9, 7), CharacterType.EnemySpeedy}
    };

    [SerializeField]
    public bool testing = false;

    [SerializeField]
    public CharacterBoardEntity testingPlayer;

    [SerializeField]
    public CharacterBoardEntity testingEnemy;

    private Dictionary<CharacterType, GameObject> boardEntityCharacters = new Dictionary<CharacterType, GameObject>();
    public Dictionary<CharacterType, GameObject> BoardEntityCharacters
    {
        get { return new Dictionary<CharacterType, GameObject>(boardEntityCharacters); }
    }

    public Dictionary<CharacterType, CharacterBoardEntity> typeToBE = new Dictionary<CharacterType, CharacterBoardEntity>();
    public Dictionary<CharacterType, CharacterBoardEntity> TypeToBE
    {
        get { return new Dictionary<CharacterType, CharacterBoardEntity>(typeToBE); }
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
        Init();
    }

    public void Init()
    {
        MakeDictionary();
        instance = this;
        if (!testing)
        {
            InitCharacters();
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            InitTestingCharacters();           
        }
    

    }

    private void MakeDictionary()
    {
        foreach (GameObject character in boardEntities)
        {
            if (character.GetComponent<CharacterBoardEntity>() != null)
            {
                CharacterType type = character.GetComponent<CharacterBoardEntity>().CharcaterType;
                boardEntityCharacters.Add(type, character);
                typeToBE.Add(type, character.GetComponent<CharacterBoardEntity>());
                typeToContainer.Add(type, character.GetComponent<CharContainer>());
                character.GetComponent<CharContainer>().Init(character.GetComponent<CharacterBoardEntity>());
                character.GetComponent<CharacterBoardEntity>().PartialInit();
            }
        }
    }

    private void InitTestingCharacters()
    {
        SetCharacterParty(new List<Tuple<CharacterBoardEntity, Ka>>()
        {
            new Tuple<CharacterBoardEntity, Ka>(testingPlayer, null )
        });
        Enemies = new Dictionary<Position, CharacterType>()
        {
            { new Position(1,0), CharacterType.TestingEnemy }
        };
    }

   // public Dictionary<Position, CharacterType> characters;
    public List<CharacterType> characters = new List<CharacterType>() {CharacterType.PlayerBongani, CharacterType.PlayerTisha,
        CharacterType.PlayerJaz, CharacterType.PlayerDadi};

    public event Action updatedParty;

    private List<Tuple<CharacterBoardEntity, Ka>> characterParty = new List<Tuple<CharacterBoardEntity, Ka>>() {};
    public List<Tuple<CharacterBoardEntity, Ka>> GetCharacterParty()
    {
        return characterParty;
    }
    public void SetCharacterParty(List<Tuple<CharacterBoardEntity, Ka>> newParty)
    {
        characterParty = newParty;
        if(updatedParty != null)
        {
            updatedParty();
        }
    }

    public HashSet<CharacterBoardEntity> GetUsedCharacters()
    {
        HashSet<CharacterBoardEntity> returnSet = new HashSet<CharacterBoardEntity>();
        foreach (Tuple <CharacterBoardEntity, Ka> tup in characterParty)
        {
            returnSet.Add(tup.first);
            if(tup.second != null)
            {
                returnSet.Add(typeToBE[tup.second.CharacterType]);
            }
        }
        return returnSet;

    }

    private void  InitCharacters()
    {
        SetCharacterParty(PremadeParties.PresetPartyOne());
    }

}
