using UnityEngine;
using UnityEngine.UI;

namespace SJ.UI
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