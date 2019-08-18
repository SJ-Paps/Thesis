using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ProfileMenu : MonoBehaviour
{
    [SerializeField]
    private ProfileInfoItem profileInfoItemPrefab;

    [SerializeField]
    private Transform layoutObject;
    
    void Awake()
    {
        CoroutineManager.GetInstance().StartCoroutine(WaitLoadProfiles());
    }

    private IEnumerator WaitLoadProfiles()
    {
        Task<ProfileData[]> taskProfiles = ProfileCareTaker.GetAllProfilesAsync();

        while(taskProfiles.IsCompleted == false)
        {
            yield return null;
        }

        ProfileData[] profiles = taskProfiles.Result;

        if(profiles != null && profiles.Length > 0)
        {
            for (int i = 0; i < profiles.Length; i++)
            {
                ProfileInfoItem instance = Instantiate(profileInfoItemPrefab, layoutObject);
                instance.SetInfo(profiles[i]);
                instance.onChoose += OnChooseProfile;
            }
        }
        
    }

    private void OnChooseProfile(ProfileInfoItem item)
    {
        GameManager.GetInstance().BeginSessionWithProfile(item.ProfileData);
    }
}
