using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using static Dreamteck.Splines.SplineSampleModifier;
using DG.Tweening.Plugins.Core.PathCore;

#if UNITY_IOS

using UnityEditor.iOS.Xcode;
public class IosBuildAdapter : MonoBehaviour
{
    public const string REPLACEMENT_IOS_PLIST_FILE_PATH = "/../PlistReplacement/Info_replace.plist";

    public static string trackingUsageDescriptionText = "This identifier will be used to deliver personalized ads to you.";

    public static string[] SUPERSONIC_AD_NETWORK_IDs =
    {
        "v9wttpbfk9.skadnetwork",
        "n38lu8286q.skadnetwork"

    };

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = path + "/Info.plist";

            //ReplaceInfoPlist(path + REPLACEMENT_IOS_PLIST_FILE_PATH, plistPath);

            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            //// Get root
            PlistElementDict rootDict = plist.root;

            //// Add NSUserTrackingUSageDescription Key
            rootDict["NSUserTrackingUsageDescription"] = new PlistElementString(trackingUsageDescriptionText);

            AddSKAdNetworkItems(rootDict, SUPERSONIC_AD_NETWORK_IDs);

            // var ser = (rootDict["NSUserTrackingUsageDescription"] = new PlistElementArray()) as PlistElementArray;
            //var ser = (rootDict["NSUserTrackingUsageDescription"] = new PlistElementString()) as PlistElementString;
            // ser.values.Add(new PlistElementString(trackingUsageDescriptionText));



            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }

    static void ReplaceInfoPlist(string myPlistFile, string originalPlistPath)
    {
        string myPlistText = System.IO.File.ReadAllText(myPlistFile);
        File.WriteAllText(originalPlistPath, myPlistText);
    }
    static void AddSKAdNetworkItems(PlistElementDict rootDict, string[] adNetworkIDs)
    {
        rootDict["SKAdNetworkItems"] = new PlistElementArray();
        for(int i=0; i < adNetworkIDs.Length; i++)
        {
            PlistElementDict currentIdentifier = new PlistElementDict();
            currentIdentifier["SKAdNetworkIdentifier"] = new PlistElementString(adNetworkIDs[i]);
            var arr = rootDict["SKAdNetworkItems"] as PlistElementArray;
            arr.values.Add(currentIdentifier);
        }
    }
}
#endif