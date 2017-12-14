using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.Entities.Passives
{
    public abstract class Buff : Passive
    {
        private Action<Buff> remove;
        protected int stacks = 1;

        public Buff (Action<Buff> remove) : base()
        {
            this.remove = remove;
        }


        protected void PopStack(int value = 1)
        {
            stacks = stacks - value;
            if (stacks <= 0)
            {
                remove(this);
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

        
    }
}
