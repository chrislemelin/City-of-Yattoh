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

        private List<BoardEntity> enities = new List<BoardEntity>();
        private List<BoardEntity> turnQueue = new List<BoardEntity>();
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
            this.boardEntitySelector = boardEntitySelector;
            this.tileSelectionManager = tileSelectionManager;
        }

        public void AddBoardEntity(BoardEntity boardEntity)
        {
            enities.Add(boardEntity);
            ReCalcQueue();
            //NextTurn();
        }

        public void RemoveBoardEntity(BoardEntity boardEntity)
        {
            enities.Remove(boardEntity);
            ReCalcQueue();
        }

        public void NextTurn()
        {
            tileSelectionManager.CancelSelection();
            if (turnQueue.Count == 0)
            {
                ReCalcQueue();
            }
            boardEntitySelector.setSelectedBoardEntity(null);
            currentBoardEntity = null;
            BoardEntity tempcurrentBoardEntity = turnQueue[0];          
            turnQueue.RemoveAt(0);
            
            CenterText.Instance.DisplayMessage(tempcurrentBoardEntity.Name + "'s Turn", () => {
                tempcurrentBoardEntity.StartMyTurn();
                currentBoardEntity = tempcurrentBoardEntity;
                UpdateGui();
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

            List<BoardEntity> displayList = QueueDisplayHelper();
            foreach (BoardEntity entity in displayList)
            {
                newText += " -> " + entity.Name;
            }
            if (display != null)
            {
                display.text = newText;
            }
        }

        private void ReCalcQueue()
        {
            turnQueue = ReCalcQueueHelper();
        }

        private List<BoardEntity> ReCalcQueueHelper()
        {
            List<BoardEntity> returnQueue = new List<BoardEntity>();
            returnQueue.AddRange(enities);
            returnQueue.OrderBy(x => { return -(x.Stats.GetStatInstance().GetStat(StatType.Speed).Value +
                (.01 * x.Stats.GetNonMuttableStat(StatType.Movement).Value)); });
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
