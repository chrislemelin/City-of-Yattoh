using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Buff : Passive
    {
        private Func<Passive, bool> remove;
        protected int stacks = 1;

        public Buff () : base()
        {
        }

        public override void StartTurn()
        {
            PopStack(1);
        }

        public void addRemoveAction(Func<Passive, bool> remove)
        {
            this.remove = remove;
        }

        protected void PopStack(int value = 1)
        {
            stacks = stacks - value;
            if (stacks <= 0)
            {
                if(remove != null)
                {
                    remove(this);
                }
            }
        }

        protected void AddStack(int value  = 1)
        {
            stacks = stacks + value;
        }

        protected void PopAll()
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
