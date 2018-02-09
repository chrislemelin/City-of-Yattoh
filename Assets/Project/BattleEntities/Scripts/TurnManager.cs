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

        public TextMeshProUGUI display;

        public delegate void NewTurnHandler(object sender, EventArgs e);
        public event NewTurnHandler OnNewTurn;


        private static BoardEntity currentBoardEntity;
        public static BoardEntity CurrentBoardEntity
        {
            get { return currentBoardEntity; }
        }

        private static List<BoardEntity> enities = new List<BoardEntity>();
        public static List<BoardEntity> Entities
        {
            get { return new List<BoardEntity>(enities); }
        }

        //private List<BoardEntity> enities = new List<BoardEntity>();
        private List<BoardEntity> turnQueue = new List<BoardEntity>();
        private HashSet<BoardEntity> alreadyTakenTurn = new HashSet<BoardEntity>();
        private int queueLength = 5;
        private Profile profile;
        private BoardEntitySelector boardEntitySelector;
        private TileSelectionManager tileSelectionManager;

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

        public void AddBoardEntity(BoardEntity boardEntity)
        {
            enities.Add(boardEntity);
            ReCalcQueue();
        }

        public void RemoveBoardEntity(BoardEntity boardEntity)
        {
            enities.Remove(boardEntity);
            ReCalcQueue();
        }

        public void NextTurnHelper()
        {
            if(currentBoardEntity != null)
            {
                ((CharacterBoardEntity)currentBoardEntity).EndMyTurn();
            }
        }

        public void NextTurn()
        {
            ReCalcQueue();
            PathOnClick.pause = true;
            tileSelectionManager.CancelSelection();
            currentBoardEntity = null;
            boardEntitySelector.setSelectedBoardEntity(null);
            
            BoardEntity tempcurrentBoardEntity = turnQueue[0];
            alreadyTakenTurn.Add(tempcurrentBoardEntity);
            turnQueue.RemoveAt(0);
            
            CenterText.Instance.DisplayMessage(tempcurrentBoardEntity.Name + "'s Turn", () => {
                currentBoardEntity = tempcurrentBoardEntity;
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

        public void ClearBoardEnities()
        {
            enities.Clear();
            ReCalcQueue();
        }

        private void UpdateGui()
        {
            string newText = "";
            newText += currentBoardEntity.Name;

            List<BoardEntity> displayList = turnQueue;
            foreach (BoardEntity entity in displayList)
            {
                newText += " -> " + entity.Name;
            }
            if (display != null)
            {
                display.text = newText;
            }
        }

        public void ReCalcQueue()
        {
            List<BoardEntity> firstListentities = ReCalcQueueHelper();
            firstListentities.RemoveAll((a) => alreadyTakenTurn.Contains(a));
            SetFirst(firstListentities);
            
            if(firstListentities.Count == 0)
            {
                alreadyTakenTurn.Clear();
                firstListentities = ReCalcQueueHelper();
            }

            List<BoardEntity> secondListentities = ReCalcQueueHelper();
            secondListentities.RemoveAll((a) => !alreadyTakenTurn.Contains(a));
            SetFirst(secondListentities);

            firstListentities.AddRange(secondListentities);
            turnQueue = firstListentities;
        }

        public void SetFirst(List<BoardEntity> list)
        {
            for(int a = 0; a < list.Count; a++)
            {
                BoardEntity b = list[a];
                if(b is CharacterBoardEntity)
                {
                    bool first = false;
                    foreach(Passive p in ((CharacterBoardEntity)b).Passives)
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

        private List<BoardEntity> ReCalcQueueHelper()
        {
            List<BoardEntity> returnQueue = new List<BoardEntity>();
            returnQueue.AddRange(enities);
            // this should order it so that speed determines the turn order with movement as a tiebreaker
            returnQueue = returnQueue.OrderBy(x => {
                return -(x.Stats.GetNonMuttableStat(StatType.Speed).Value)
                - (.01 * x.Stats.GetNonMuttableStat(StatType.Movement).Value);
            }).ToList();
            return returnQueue;
        }

        private List<BoardEntity> QueueDisplayHelper()
        {
            List<BoardEntity> returnQueue = new List<BoardEntity>();
            while(returnQueue.Count < queueLength)
            {
                if(turnQueue.Count > returnQueue.Count)
                {
                    returnQueue.AddRange(turnQueue);
                }
                else
                {
                    List<BoardEntity> helper = ReCalcQueueHelper();
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
