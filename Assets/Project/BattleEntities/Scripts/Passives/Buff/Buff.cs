using Placeholdernamespace.Battle.Calculator;
using Placeholdernamespace.Battle.Env;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Buff : Passive
    {
        protected const int STACKS_HIDDEN = int.MaxValue;
        protected int stacks = STACKS_HIDDEN;
        private Func<Passive, bool> remove;

        public Buff(int stacks = STACKS_HIDDEN) : base()
        {
            this.stacks = stacks;
        }

        public override void StartTurn()
        {
            if(stacks != STACKS_HIDDEN)
            {
                PopStack(1);
            }
        }

        public void Init(Func<Passive, bool> remove)
        {
            this.remove = remove;
        }

        protected void Remove()
        {
            if(remove != null)
            {
                remove(this);
            }
        }

        public void PopStack(int value = 1)
        {
            Remove();
            
        }

        public void AddStack(int value  = 1)
        {
            if(stacks != STACKS_HIDDEN)
                stacks = stacks + value;
        }

        public void PopAll()
        {
            PopStack(stacks);
        }

        public override string GetDescription()
        {
            string descriptionReturn = GetDescriptionHelper();
            string descriptionExtra = GetDescriptionExtra();
            if (descriptionExtra != "")
            {
                descriptionReturn += "\n" + descriptionExtra;
            }
            return descriptionReturn;
        }

        /// <summary>
        /// override for function based description
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDescriptionHelper()
        {
            return description;
        }

        private string GetDescriptionExtra()
        {
            string returnString = "STACKS: "+stacks;
            return returnString;
        }


    }
}
