#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDKSettings : ScriptableObject
{
    [SerializeField]
    private bool isEnableLog;
    public bool GetIsEnableLog => isEnableLog;

    [SerializeField]
    private bool isTestAds;
    public bool GetIsTestAds => isTestAds;

    [SerializeField]
    private string facebookID;
    public string GetFacebookID => facebookID;

    [SerializeField]
    private string applovinSDKKey;
    public string GetApplovinSDKKey => applovinSDKKey;

    [SerializeField]
    private string appsFlyerKey;                                                                                                                  
    public string GetAppsFlyerKey => appsFlyerKey;

    [SerializeField]
    private string oneSignalAppID;
    public string GetOneSignalAppID => oneSignalAppID;

    [SerializeField]
    private string appleAppID;
    public string GetAppleAppID => appleAppID;

    [SerializeField]
    private string ironSource_AppKey;
    public string IronSourceAppKey => ironSource_AppKey;

    [SerializeField]
    private int banner_Placement_Size;
    public int Banner_Placement_Size => banner_Placement_Size;

    [SerializeField]
    private int interstitial_Placement_Size;
    public int Interstitial_Placement_Size => interstitial_Placement_Size;


    [SerializeField]
    private int reward_Placement_Size;
    public int Reward_Placement_Size => reward_Placement_Size;



    [SerializeField]
    private SDKPlatformSettings admobAppID;
    public SDKPlatformSettings GetAdmobAppID => admobAppID;

    [SerializeField]
    private SDKPlatformSettings[] admobNativeID;
    public SDKPlatformSettings[] GetAdmobNativeID => admobNativeID;

    [SerializeField]
    private SDKPlatformSettings[] maxBannerID;
    public SDKPlatformSettings[] GetMaxBannerID => maxBannerID;

    [SerializeField]
    private SDKPlatformSettings[] maxInterstitialID;
    public SDKPlatformSettings[] GetMaxInterstitialID => maxInterstitialID;

    [SerializeField]
    private SDKPlatformSettings[] maxRewardID;
    public SDKPlatformSettings[] GetMaxRewardID => maxRewardID;

    [SerializeField]
    public SDKPlatformSettings[] ironSourceBannerID;
    public SDKPlatformSettings[] GetIronSourceBannerID => ironSourceBannerID;

    [SerializeField]
    public SDKPlatformSettings[] ironSourceRewardID;
    public SDKPlatformSettings[] GetIronSourceRewardID => ironSourceRewardID;

    [SerializeField]
    public SDKPlatformSettings[] ironSourceInterstitialID;
    public SDKPlatformSettings[] GetIronSourceInterstitialID => ironSourceInterstitialID;

#if UNITY_EDITOR

    private static string settingPath = "/SDK/Resources/";

    [UnityEditor.MenuItem("Tools/SDKs/CreateSDKSetting")]
    private static void CreateSettings()
    {
        CheckFolder();

        string scriptablePath = string.Format("Assets/{0}/SDKSettings.asset", settingPath);

        SDKSettings preSDKSettings = (SDKSettings)UnityEditor.AssetDatabase.LoadAssetAtPath(scriptablePath, typeof(SDKSettings));

        SDKSettings sdkSetting = CreateInstance<SDKSettings>();
        UnityEditor.AssetDatabase.CreateAsset(sdkSetting, scriptablePath);

        SetCommonInfo(sdkSetting, preSDKSettings);
        CreateBanner(sdkSetting, preSDKSettings, sdkSetting.banner_Placement_Size);
        CreateInterstitial(sdkSetting, preSDKSettings, sdkSetting.interstitial_Placement_Size);
        CreateReward(sdkSetting, preSDKSettings, sdkSetting.reward_Placement_Size);
        CreateNative(sdkSetting, preSDKSettings);

        UnityEditor.Selection.activeObject = sdkSetting.admobAppID;
        UnityEditor.AssetDatabase.ImportAsset(scriptablePath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.EditorUtility.SetDirty(sdkSetting);
        UnityEditor.Selection.activeObject = sdkSetting;
    }

    private static void SetCommonInfo(SDKSettings current, SDKSettings prev)
    {
        if (prev != null)
        {
            current.isEnableLog = prev.isEnableLog;
            current.isTestAds = prev.isTestAds;
            current.facebookID = prev.facebookID;
            current.applovinSDKKey = prev.applovinSDKKey;
            current.appsFlyerKey = prev.appsFlyerKey;
            current.oneSignalAppID = prev.oneSignalAppID;
            current.appleAppID = prev.appleAppID;
            current.ironSource_AppKey = prev.ironSource_AppKey;
            current.banner_Placement_Size = prev.banner_Placement_Size;
            current.interstitial_Placement_Size = prev.interstitial_Placement_Size;
            current.reward_Placement_Size = prev.reward_Placement_Size;
        }

        SDKPlatformSettings platformSettings = CreateInstance<SDKPlatformSettings>();
        platformSettings.name = "AdmobAppID";
        UnityEditor.AssetDatabase.AddObjectToAsset(platformSettings, current);
        current.admobAppID = platformSettings;
        if (prev?.admobAppID != null)
        {
            current.admobAppID.ID_iOS = prev.admobAppID.ID_iOS;
            current.admobAppID.ID_AOS = prev.admobAppID.ID_AOS;
        }
    }

    private static void CreateBanner(SDKSettings current, SDKSettings prev, int size)
    {
        current.maxBannerID = new SDKPlatformSettings[size];
        current.ironSourceBannerID = new SDKPlatformSettings[size];
        
        foreach(var item in System.Enum.GetValues(typeof(AD_SDK_TYPE)))
        {
            switch (item)
            {
                case AD_SDK_TYPE.MAX:
                    current.maxBannerID = GenerateSettings("Banner", AD_SDK_TYPE.MAX.ToString(), size, current, prev?.maxBannerID);
                    break;
                case AD_SDK_TYPE.IRONSOURCE:
                    current.ironSourceBannerID = GenerateSettings("Banner", AD_SDK_TYPE.IRONSOURCE.ToString(), size, current, prev?.ironSourceBannerID);
                    break;
            }
        }
    }

    private static void CreateInterstitial(SDKSettings current, SDKSettings prev, int size)
    {
        current.maxInterstitialID = new SDKPlatformSettings[size];
        current.ironSourceInterstitialID = new SDKPlatformSettings[size];

        foreach (var item in System.Enum.GetValues(typeof(AD_SDK_TYPE)))
        {
            switch (item)
            {
                case AD_SDK_TYPE.MAX:
                    current.maxInterstitialID = GenerateSettings("Interstitial", AD_SDK_TYPE.MAX.ToString(), size, current, prev?.maxInterstitialID);
                    break;
                case AD_SDK_TYPE.IRONSOURCE:
                    current.ironSourceInterstitialID = GenerateSettings("Interstitial", AD_SDK_TYPE.IRONSOURCE.ToString(), size, current, prev?.ironSourceInterstitialID);
                    break;
            }
        }
    }

    private static void CreateReward(SDKSettings current, SDKSettings prev, int size)
    {
        current.maxRewardID = new SDKPlatformSettings[size];
        current.ironSourceRewardID = new SDKPlatformSettings[size];

        foreach (var item in System.Enum.GetValues(typeof(AD_SDK_TYPE)))
        {
            switch (item)
            {
                case AD_SDK_TYPE.MAX:
                        current.maxRewardID = GenerateSettings("Reward", AD_SDK_TYPE.MAX.ToString(), size, current, prev?.maxRewardID);
                    break;
                case AD_SDK_TYPE.IRONSOURCE:
                        current.ironSourceRewardID = GenerateSettings("Reward", AD_SDK_TYPE.IRONSOURCE.ToString(), size, current, prev?.ironSourceRewardID);
                    break;
            }
        }
    }

    private static SDKPlatformSettings[] GenerateSettings(string format, string type, int size, SDKSettings current, SDKPlatformSettings[] copyDatas)
    {
        var settings = new SDKPlatformSettings[size];
        for (int i = 0; i < size; i++)
        {
            SDKPlatformSettings platformSettings = CreateInstance<SDKPlatformSettings>();
            platformSettings.name = string.Format("{0}_{1}_{2}", format, type, i);
            CopyPlatformSetting(platformSettings, copyDatas, i);
            settings[i] = platformSettings;
            UnityEditor.AssetDatabase.AddObjectToAsset(platformSettings, current);
        }
        return settings;
    }

    private static void CreateNative(SDKSettings current, SDKSettings prev)
    {
        current.admobNativeID = new SDKPlatformSettings[(int)NATIVE_TYPE.MAX];
        for (int i = 0, count = (int)NATIVE_TYPE.MAX; i < count; i++)
        {
            SDKPlatformSettings platformSettings = CreateInstance<SDKPlatformSettings>();
            platformSettings.name = string.Format("Native_{0}", (NATIVE_TYPE)i);
            current.admobNativeID[i] = platformSettings;
            CopyPlatformSetting(platformSettings, prev?.admobNativeID, i);
            UnityEditor.AssetDatabase.AddObjectToAsset(platformSettings, current);
        }
    }

    private static void CopyPlatformSetting(SDKPlatformSettings target, SDKPlatformSettings[] copyDatas, int index)
    {
        if (copyDatas == null || copyDatas.Length <= index || copyDatas[index] == null)
        {
            return;
        }

        target.Copy(copyDatas[index]);
    }

    /// <summary>
    /// ScriptableObject 생성할 폴더 확인(없으면 생성)
    /// </summary>
    private static void CheckFolder()
    {
        string folderPath = string.Format("{0}/{1}", Application.dataPath, settingPath);
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }
    }
#endif
}
