using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Placeholdernamespace.Battle.Entities;
using Placeholdernamespace.Battle.Entities.AttributeStats;
using Placeholdernamespace.Battle.UI;
using Placeholdernamespace.Battle.Interaction;

namespace Placeholdernamespace.Battle.Managers
{
    public class TurnManager : MonoBehaviour
    {

        public GUIText display;

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

        public void startGame()
        {
            NextTurn();
        }

        public void init(BoardEntitySelector boardEntitySelector)
        {
            this.boardEntitySelector = boardEntitySelector;
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
            if (turnQueue.Count == 0)
            {
                ReCalcQueue();
            }
            boardEntitySelector.setSelectedBoardEntity(null);
            currentBoardEntity = turnQueue[0];
            turnQueue.RemoveAt(0);
            UpdateGui();
            currentBoardEntity.StartMyTurn();

        }

        public void ClearBoardEnities()
        {
            enities.Clear();
            ReCalcQueue();
        }

        private void UpdateGui()
        {
            string newText = "";
            foreach (BoardEntity entity in turnQueue)
            {
                newText += entity.name + '\n';
            }
            if (display != null)
            {
                display.text = newText;
            }
        }

        private void ReCalcQueue()
        {
            turnQueue.Clear();
            turnQueue.AddRange(enities);
            turnQueue.OrderBy(x => x.Stats.GetStatInstance().GetStat(StatType.Speed));
            UpdateGui();
        }
    }
}
