using Placeholdernamespace.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common.Utils
{
    public abstract class OnClickRedirect : MonoBehaviour
    {

        public abstract IClickable GetTarget();

    }
}
