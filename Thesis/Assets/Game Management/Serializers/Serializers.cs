using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SJ.Save;

namespace SJ
{
    public static class Serializers
    {
        private static ISaveSerializer saveSerializer;

        public static ISaveSerializer GetSaveSerializer()
        {
            if(saveSerializer == null)
            {
                saveSerializer = new JsonSaveSerializer();
            }

            return saveSerializer;
        }
    }
}
