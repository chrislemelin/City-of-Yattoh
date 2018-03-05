using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.Battle.Interaction;
using TMPro;
using Placeholdernamespace.Battle.Entities.Passives;

namespace Placeholdernamespace.Battle.Managers
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        GameObject currentTurnPointer;
        public TextMeshProUGUI display;

        public delegate void NewTurnHandler(object sender, EventArgs e);
        private bool endState = false;

        private static CharacterBoardEntity currentBoardEntity;
        public static CharacterBoardEntity CurrentBoardEntity
        {
            get { return currentBoardEntity; }
        }

        private static List<CharacterBoardEntity> enities = new List<CharacterBoardEntity>();
        public static List<CharacterBoardEntity> Entities
        {
            get { return new List<CharacterBoardEntity>(enities); }
        }

        public static List<BoardEntity> GetBEofType(CharacterType type)
        {
            List<BoardEntity> returnList = new List<BoardEntity>();
            foreach(BoardEntity BE in Entities)
            {
               // if(BE)
            }
            return null;

        }

        private List<CharacterBoardEntity> turnQueue = new List<CharacterBoardEntity>();
        private HashSet<CharacterBoardEntity> alreadyTakenTurn = new HashSet<CharacterBoardEntity>();
        private int queueLength = 5;
        private Profile profile;
        private BoardEntitySelector boardEntitySelector;
        private TileSelectionManager tileSelectionManager;

        

        public void Awake()
        {
            enities.Clear();
        }

        public void startGame()
        {     
            NextTurn();
        }

        public void init(BoardEntitySelector boardEntitySelector, TileSelectionManager tileSelectionManager)
        {
            PathOnClick.pause = true;
            this.boardEntitySelector = boardEntitySelector;
            this.tileSelectionManager = tileSelectionManager;
        }

        public void AddBoardEntity(CharacterBoardEntity boardEntity)
        {
           enities.Add(boardEntity);
           //ReCalcQueue();
        }

        public void RemoveBoardEntity(CharacterBoardEntity boardEntity)
        {
            enities.Remove(boardEntity);
            //ReCalcQueue();
            CheckForEndState();
        }

        private void CheckForEndState()
        {
            bool win = true;
            bool lose = true;
            foreach (BoardEntity boardEntity in Entities)
            {
                if (boardEntity.Team == Team.Enemy)
                    win = false;
                if (boardEntity.Team == Team.Player)
                    lose = false;
            }
            if(win)
            {
                winHelper();
            }
            else if (lose)
            {
                loseHelper();
            }
        }

        private void winHelper()
        {
            endState = true;
            CenterText.Instance.DisplayMessage("Player Wins! press reset in top left corner to restart", null, duration: -1);
            NextTurnHelper();
        }

        private void loseHelper()
        {
            endState = true;
            CenterText.Instance.DisplayMessage("Player Loses! press reset in top left corner to restart", null, duration: -1);
            NextTurnHelper();
        }

        public void NextTurnHelper()
        {
            if(currentBoardEntity != null)
            {
                currentBoardEntity.EndMyTurn();
            }
        }

        public void NextTurn()
        {
            currentBoardEntity = null;
            boardEntitySelector.setSelectedBoardEntity(null);
            if (!endState)
            {
                ReCalcQueue();
                PathOnClick.pause = true;
                tileSelectionManager.CancelSelection();

                CharacterBoardEntity tempcurrentBoardEntity = turnQueue[0];
                alreadyTakenTurn.Add(tempcurrentBoardEntity);
                turnQueue.RemoveAt(0);

                CenterText.Instance.DisplayMessage(tempcurrentBoardEntity.Name + "'s Turn", () =>
                {
                    currentBoardEntity = tempcurrentBoardEntity;
                    SetCurrentTurnMarker((CharacterBoardEntity)tempcurrentBoardEntity);
                    PathOnClick.pause = false;
                    UpdateGui();
                    ((CharacterBoardEntity)tempcurrentBoardEntity).SetUpMyTurn();
                    if (currentBoardEntity.Team == Team.Player)
                    {
                        boardEntitySelector.setSelectedBoardEntity(currentBoardEntity);
                    }
                    tempcurrentBoardEntity.StartMyTurn();

                });
            }
           
        }

        private void SetCurrentTurnMarker(CharacterBoardEntity characterBoardEntity)
        { 
            if(characterBoardEntity != null)
            {
                currentTurnPointer.GetComponent<UIFollow>().target = characterBoardEntity.gameObject;
                currentTurnPointer.SetActive(true);
            }
 
   
        }

        public void ClearBoardEnities()
        {
            enities.Clear();
            ReCalcQueue();
        }

        private void UpdateGui()
        {
            string newText = "";
            int counter = 1;
            newText += AddOrdinal(counter++) + "  " + currentBoardEntity.Name;
            
            List<CharacterBoardEntity> displayList = turnQueue;
            foreach (CharacterBoardEntity entity in displayList)
            {
                newText += "\n"+AddOrdinal(counter++)+"  " + entity.Name;
            }
            if (display != null)
            {
                display.text = newText;
            }
        }

        //https://stackoverflow.com/questions/20156/is-there-an-easy-way-to-create-ordinals-in-c
        private string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        public void ReCalcQueue()
        {
            List<CharacterBoardEntity> firstListentities = ReCalcQueueHelper();
            firstListentities.RemoveAll((a) => alreadyTakenTurn.Contains(a));
            SetFirst(firstListentities);
            
            if(firstListentities.Count == 0)
            {
                alreadyTakenTurn.Clear();
                firstListentities = ReCalcQueueHelper();
            }

            List<CharacterBoardEntity> secondListentities = ReCalcQueueHelper();
            secondListentities.RemoveAll((a) => !alreadyTakenTurn.Contains(a));
            SetFirst(secondListentities);

            firstListentities.AddRange(secondListentities);
            turnQueue = firstListentities;
        }

        public void SetFirst(List<CharacterBoardEntity> list)
        {
            for(int a = 0; a < list.Count; a++)
            {
                CharacterBoardEntity b = list[a];

                bool first = false;
                foreach(Passive p in b.Passives)
                {
                    first = p.TurnOrderFirst(first);
                }
                if(first)
                {
                    list.Remove(b);
                    list.Insert(0, b);
                }
                
            }
        }

        public void CheckEntitiesForDeath()
        {
            foreach(CharacterBoardEntity character in Entities)
            {
                if(character.Stats.GetMutableStat(StatType.Health).Value == 0)
                {
                    character.Die();
                }
            }
        }

        private List<CharacterBoardEntity> ReCalcQueueHelper()
        {
            List<CharacterBoardEntity> returnQueue = new List<CharacterBoardEntity>();
            returnQueue.AddRange(enities);
            // this should order it so that speed determines the turn order with movement as a tiebreaker
            returnQueue = returnQueue.OrderBy(x => {
                return -(x.Stats.GetNonMuttableStat(StatType.Speed).Value)
                - (.01 * x.Stats.GetNonMuttableStat(StatType.Movement).Value);
            }).ToList();
            return returnQueue;
        }

        private List<CharacterBoardEntity> QueueDisplayHelper()
        {
            List<CharacterBoardEntity> returnQueue = new List<CharacterBoardEntity>();
            while(returnQueue.Count < queueLength)
            {
                if(turnQueue.Count > returnQueue.Count)
                {
                    returnQueue.AddRange(turnQueue);
                }
                else
                {
                    List<CharacterBoardEntity> helper = ReCalcQueueHelper();
                    returnQueue.AddRange(helper);
                }
            }
            if(returnQueue.Count > queueLength)
            {
                returnQueue.RemoveAt(queueLength);
            }
            return returnQueue;
        }
    }
}
