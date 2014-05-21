using System;
using FluentSharp.CoreLib;

namespace TeamMentor.CoreLib
{
    public static class TmConfig_ExtensionMethods
    {
        public static string tmConfig_Location(this TM_UserData userData)
        {
            return (userData.notNull())
                        ? userData.Path_UserData.pathCombine(TMConsts.TM_CONFIG_FILENAME)
                        : null;
        }

        public static TM_UserData tmConfig_Load(this TM_UserData userData)
        {
            var userConfigFile = userData.tmConfig_Location();
            if (userConfigFile.fileExists())
            {
                var newConfig = userConfigFile.load<TMConfig>();    // to check that the new TMConfig is not corrupted
                if (newConfig.isNull())
                {
                    "[handleUserDataConfigActions] failed to load config file from: {0}".error(userConfigFile);
                    return null;
                }
                else
                {
                    TMConfig.Current = newConfig;
                    return userData;
                }
            }
            //    userData.tmConfig_Save(); // (legacy): if the TMConfig.config doesn't exist or failed to load, save it with the current TMConfig.Current
            //TMConfig.Current.SaveTMConfig(); 
            return userData;
        }

        public static TMConfig tmConfig_Reload(this TM_UserData userData)
        {
            TMConfig.Current = userData.tmConfig_Location().fileExists()
                                    ? userData.tmConfig_Location().load<TMConfig>()
                                    : new TMConfig();
            return TMConfig.Current;
        }

        public static bool tmConfig_Save(this TM_UserData userData)
        {
            var tmConfig = TMConfig.Current;
            var location = userData.tmConfig_Location();
            return  (tmConfig.notNull() && location.valid())
                        ? tmConfig.saveAs(location)
                        : false;
        }

        public static bool tmConfig_SetCurrent(this TM_UserData userData, TMConfig tmConfig)
        {
            if (userData.isNull() || tmConfig.isNull())
                return false;
            TMConfig.Current = tmConfig;
            return userData.tmConfig_Save();            
        }

        public static DateTime  currentExpirationDate(this TMConfig tmConfig)
        {
            return (tmConfig.TMSecurity.EvalAccounts_Enabled)
                       ? DateTime.Now.AddDays(tmConfig.TMSecurity.EvalAccounts_Days)
                       : default(DateTime);
        }

        public static bool  newAccountsEnabled(this TMConfig tmConfig)
        {
            if (tmConfig.notNull() && tmConfig.TMSecurity.notNull())
                return tmConfig.TMSecurity.NewAccounts_Enabled;
            return false;
        }

        public static bool  emailAdminOnNewUsers(this TMConfig tmConfig)
        {
            if (tmConfig.notNull() && tmConfig.TMSecurity.notNull())
                return tmConfig.TMSecurity.EmailAdmin_On_NewUsers;
            return false;
        }
        public static bool  windowsAuth(this TMConfig tmConfig)
        {
            if (tmConfig.notNull() && tmConfig.TMSecurity.notNull())
                return tmConfig.WindowsAuthentication.Enabled;
            return false;
        }
        
    }
}