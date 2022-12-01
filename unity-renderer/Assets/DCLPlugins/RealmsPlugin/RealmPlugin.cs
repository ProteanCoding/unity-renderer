using DCL;
using DCLPlugins.RealmsPlugin;
using Decentraland.Bff;
using System.Collections.Generic;
using System.Linq;
using Variables.RealmsInfo;

namespace DCLPlugins.RealmPlugin
{
    /// <summary>
    /// Contains and triggers the realm modifiers when a new realm has been entered. This is triggered by setting a new AboutConfiguration
    /// </summary>
    public class RealmPlugin : IPlugin
    {
        private BaseVariable<AboutResponse.Types.AboutConfiguration> realmAboutConfiguration => DataStore.i.realm.playerRealmAboutConfiguration;
        private List<RealmModel> currentCatalystRealmList;

        internal List<IRealmModifier> realmsModifiers;

        public RealmPlugin(DataStore dataStore)
        {
            realmsModifiers = new List<IRealmModifier>
            {
                new RealmBlockerModifier(dataStore.worldBlockers),
                new RealmMinimapModifier(dataStore.HUDs),
                new RealmsSkyboxModifier(dataStore.skyboxConfig),
            };

            realmAboutConfiguration.OnChange += RealmChanged;
        }

        private void RealmChanged(AboutResponse.Types.AboutConfiguration current, AboutResponse.Types.AboutConfiguration _)
        {
            bool isWorld = current.ScenesUrn.Any()
                           && string.IsNullOrEmpty(current.CityLoaderContentServer);

            realmsModifiers.ForEach(rm => rm.OnEnteredRealm(isWorld, current));
        }

        public void Dispose()
        {
            realmsModifiers.ForEach(rm => rm.Dispose());
            realmAboutConfiguration.OnChange -= RealmChanged;
        }
    }
}