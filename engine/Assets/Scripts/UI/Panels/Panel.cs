using UnityEngine;
using System.Collections;

namespace Synthesis.UI.Panels
{
    public class Panel : MonoBehaviour
    {
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
