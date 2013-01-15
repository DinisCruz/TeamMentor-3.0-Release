﻿using TeamMentor.CoreLib;

namespace TeamMentor.Website
{
    public class TM_Customizations
    {
		public TMConfig			tmConfig;
	    public GoogleAnalytics	googleAnalytics;

	    public static void AppInitialize()
	    {		 
		    new TM_Customizations().CustomizeTMConfig();			
	    }

	    public TM_Customizations()
	    {
			tmConfig = TMConfig.Current;
			googleAnalytics =  GoogleAnalytics.Current;
	    }

	    public void CustomizeTMConfig()
	    {	
	    	//General TM Configuration
			tmConfig.ShowContentToAnonymousUsers	= true;

			//Google Analytics
		    googleAnalytics.AccountID				= "UA-37594728-3";			
			googleAnalytics.Enabled					= true;
			googleAnalytics.LogWebServicesCalls		= true;
	    }
    }
}