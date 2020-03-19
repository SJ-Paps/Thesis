using UnityEngine;
using UnityEngine.UI;

namespace SJ.Menu
{
    [RequireComponent(typeof(Text))]
    public class BuildVersion : SJMonoBehaviour
    {
        protected override void SJAwake()
        {
            GetComponent<Text>().text = "v" + UnityEngine.Application.version;
        }
    }
}