using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ProfileMenu : SJMonoBehaviour
{
    [SerializeField]
    private ProfileInfoItem profileInfoItemPrefab;

    [SerializeField]
    private Transform layoutObject;

    private List<ProfileInfoItem> items;
    
    protected override void SJAwake()
    {
        items = new List<ProfileInfoItem>();
    }

    protected override void SJOnEnable()
    {
        CoroutineManager.GetInstance().StartCoroutine(WaitLoadProfiles());
    }

    protected override void SJOnDisable()
    {
        for(int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
            items.RemoveAt(i);
            i--;
        }
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
                items.Add(instance);
                instance.SetInfo(profiles[i]);
                instance.onSelectRequest += OnSelectProfile;
                instance.onDeleteRequest += OnDeleteProfile;
            }
        }
        
    }

    private void OnSelectProfile(ProfileInfoItem item)
    {
        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

        gameConfiguration.lastProfile = item.ProfileData.name;

        GameConfigurationCareTaker.SaveConfiguration();

        GameManager.GetInstance().BeginSessionWithProfile(item.ProfileData);
    }

    private void OnDeleteProfile(ProfileInfoItem item)
    {
        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

        if(gameConfiguration.lastProfile == item.ProfileData.name)
        {
            gameConfiguration.lastProfile = null;

            GameConfigurationCareTaker.SaveConfiguration();
        }

        ProfileCareTaker.DeleteProfile(item.ProfileData.name);
        items.Remove(item);
        Destroy(item.gameObject);
    }

}
