﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace TeamMentor.CoreLib
{
    //********  Session Management (& Login)]
    public partial class TM_WebServices
    {
        [WebMethod(EnableSession = true)]	public Guid Login(string username, string password)      		    {   return tmAuthentication.sessionID = userData.login(username,password);      }  			
        [WebMethod(EnableSession = true)]	public Guid Logout()      										    {	return tmAuthentication.logout();                                           }	
        [WebMethod(EnableSession = true)]	public Guid Current_SessionID()										{	return tmAuthentication.sessionID;                                          }
        [WebMethod(EnableSession = true)]	public TMUser Current_User()										{	return tmAuthentication.currentUser;                                        }
        [WebMethod(EnableSession = true)]	public List<string> GetCurrentUserRoles()							{	return tmAuthentication.sessionID.session_UserRoles().toStringList();       }
        [WebMethod(EnableSession = true)]	public bool SetCurrentUserPassword(string password)			        {	return userData.setCurrentUserPassword(tmAuthentication , password);        }        
    }
}
