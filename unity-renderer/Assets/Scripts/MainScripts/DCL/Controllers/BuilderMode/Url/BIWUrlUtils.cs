using System.Collections;
using System.Collections.Generic;
using DCL.Configuration;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class BIWUrlUtils
{
    public static string GetUrlSceneObjectContent() { return BIWSettings.BASE_URL_SCENE_OBJECT_CONTENT.Replace("{ENV}", GetEnvBase()); }

    public static string GetBuilderAPIBaseUrl()
    {
        return GetResolvedEnviromentUrl(BIWSettings.BASE_URL_BUILDER_API);
    }

    public static string GetBuilderProjecThumbnailUrl(string projectId, string filename)
    {
        string resolvedUrl = GetResolvedEnviromentUrl(BIWSettings.BASE_URL_BUILDER_PROJECT_THUMBNAIL);
        resolvedUrl = resolvedUrl.Replace("{id}", projectId) +filename;
        return resolvedUrl;
    }

    public static string GetUrlCatalog(string ethAddress)
    {
        string paramToAdd = "default";
        if (!string.IsNullOrEmpty(ethAddress))
            paramToAdd = ethAddress;
        return GetResolvedEnviromentUrl(BIWSettings.BASE_URL_CATALOG) + paramToAdd;
    }

    public static string GetUrlAssetPackContent() { return GetResolvedEnviromentUrl(BIWSettings.BASE_URL_ASSETS_PACK_CONTENT); }

    private static string GetResolvedEnviromentUrl(string url)
    {
        return url.Replace("{ENV}", GetEnvBase());
    }
    
    private static bool IsMainnet()
    {
        return KernelConfig.i.Get().network == "mainnet";
    }
    
    private static string GetEnvBase()
    {
        if (IsMainnet())
            return "org";

        return "io";
    }
}