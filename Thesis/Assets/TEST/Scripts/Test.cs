using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ref GameConfiguration config = ref GameConfigurationCareTaker.GetConfiguration();

        Debug.Log(config.generalVolume);

        config.generalVolume = 0.5f;

        var saveTask = GameConfigurationCareTaker.SaveConfigurationAsync();

        while(saveTask.IsCompleted == false)
        {

        }

        var loadTask = GameConfigurationCareTaker.LoadConfigurationAsync();

        while(loadTask.IsCompleted == false)
        {

        }

        config = ref GameConfigurationCareTaker.GetConfiguration();

        Debug.Log(config.generalVolume);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
