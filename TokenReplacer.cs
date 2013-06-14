using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace avt.Redirect
{
    public class TokenReplacer
    {
        public static string Tokenize(string content)
        {
            return Tokenize(content, null, null, false, true);
        }

        public static bool IsMyTokensInstalled()
        {
            Tokenize("");
            return (HttpRuntime.Cache.Get("avt.MyTokens2.Installed") as string) == "yes";
        }

        static public string Tokenize(string strContent, DotNetNuke.Entities.Modules.ModuleInfo modInfo, UserInfo user, bool forceDebug, bool bRevertToDnn)
        {
            string cacheKey_Installed = "avt.MyTokens2.Installed";
            string cacheKey_MethodReplace = "avt.MyTokens2.MethodReplace";

            string bMyTokensInstalled = "no";
            System.Reflection.MethodInfo methodReplace = null;

            bool bDebug = forceDebug;
            if (!bDebug) {
                try { bDebug = DotNetNuke.Common.Globals.IsEditMode(); } catch { }
            }

            lock (typeof(DotNetNuke.Services.Tokens.TokenReplace)) {
                // first, determine if MyTokens is installed
                if (HttpRuntime.Cache.Get(cacheKey_Installed) == null) {

                    // check again, maybe current thread was locked by another which did all the work
                    if (HttpRuntime.Cache.Get(cacheKey_Installed) == null) {

                        // it's not in cache, let's determine if it's installed
                        try {
                            Type myTokensRepl = DotNetNuke.Framework.Reflection.CreateType("avt.MyTokens.MyTokensReplacer", true);
                            if (myTokensRepl == null)
                                throw new Exception(); // handled in catch

                            bMyTokensInstalled = "yes";

                            // we now know MyTokens is installed, get ReplaceTokensAll methods
                            methodReplace = myTokensRepl.GetMethod(
                                "ReplaceTokensAll",
                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                                null,
                                System.Reflection.CallingConventions.Any,
                                new Type[] { 
                                typeof(string), 
                                typeof(DotNetNuke.Entities.Users.UserInfo), 
                                typeof(bool),
                                typeof(DotNetNuke.Entities.Modules.ModuleInfo)
                            },
                                null
                            );

                            if (methodReplace == null) {
                                // this shouldn't really happen, we know MyTokens is installed
                                throw new Exception();
                            }

                        } catch {
                            bMyTokensInstalled = "no";
                        }

                        // cache values so next time the funciton is called the reflection logic is skipped
                        HttpRuntime.Cache.Insert(cacheKey_Installed, bMyTokensInstalled);
                        if (bMyTokensInstalled == "yes") {
                            HttpRuntime.Cache.Insert(cacheKey_MethodReplace, methodReplace);
                        }
                    }
                }
            }

            bMyTokensInstalled = HttpRuntime.Cache.Get(cacheKey_Installed).ToString();
            if (bMyTokensInstalled == "yes") {
                methodReplace = (System.Reflection.MethodInfo)HttpRuntime.Cache.Get(cacheKey_MethodReplace);
                if (methodReplace == null) {
                    HttpRuntime.Cache.Remove(cacheKey_Installed);
                    return Tokenize(strContent, modInfo, user, forceDebug, bRevertToDnn);
                }
            } else {
                // if it's not installed return string or tokenize with DNN replacer
                if (!bRevertToDnn) {
                    return strContent;
                } else {
                    DotNetNuke.Services.Tokens.TokenReplace dnnTknRepl = new DotNetNuke.Services.Tokens.TokenReplace();
                    dnnTknRepl.AccessingUser = user ?? DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();
                    dnnTknRepl.DebugMessages = bDebug;
                    if (modInfo != null)
                        dnnTknRepl.ModuleInfo = modInfo;

                    // MyTokens is not installed, execution ends here
                    return dnnTknRepl.ReplaceEnvironmentTokens(strContent);
                }
            }

            // we have MyTokens installed, proceed to token replacement
            return (string)methodReplace.Invoke(
                null,
                new object[] {
                    strContent,
                    user ?? DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo(),
                    bDebug,
                    modInfo
                }
            );

        }

    }
}
