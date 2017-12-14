using Placeholdernamespace.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Placeholdernamespace.Common.Utils
{
    public class OnClickRedirectGeneral : OnClickRedirect
    {
        [SerializeField]
        public MonoBehaviour target;

        public override IClickable GetTarget()
        {
            return (IClickable)target;
        }
    }
}
