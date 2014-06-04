module("TM WebServices - User (no privs needed)");

test("Check WebServices setup", function() 
	{			
		ok(TM.WebServices.Helper.invokeWebService	, "TM.WebServices.Helper.invokeWebService");
		ok(TM.WebServices.WS_Users.createUser		, "TM.WebServices.WS_Users.createUser");
		ok(TM.WebServices.WS_Users.login			, "TM.WebServices.WS_Users.login");
		ok(TM.WebServices.WS_Users.getUsers			, "TM.WebServices.WS_Users.getUsers");		
		ok(QUnit.done								, "QUnit.done was there");				
		ok(QUnit.randomString						, "QUnit.randomString was there");
	});


asyncTest("getTime", function() 
	{
		TM.WebServices.WS_Utils.getTime(start)
	});
	
asyncTest("CreateUser", function() 
	{
		var username 	 = QUnit.randomString();
		var password     = QUnit.randomString();
		var email 		 = QUnit.randomString();
		var firstname	 = QUnit.randomString();
		var lastname 	 = QUnit.randomString();
		var note 		 = QUnit.randomString();
		
		var callback = function(newUserId)
			{				
				ok(newUserId > 0 , "new user id ok: " + newUserId);
				start();
			};
		notEqual(username, "qunit_" , "random username created");			
		TM.WebServices.WS_Users.createUser(username, password, email, firstname, lastname, note, callback);
	});

	
asyncTest("login - fail", function() 	
	{
		var username 	 = QUnit.randomString();
		var password = QUnit.randomString();
		
		var callback = function(sessionId)
			{				
				equal(sessionId, TM.Const.emptyGuid, "sessionId was an Empty GUI");
				start();
			};		
		TM.WebServices.WS_Users.login(username, password,callback);
	});
	
asyncTest("login - ok", function() 	
	{
		stop();
		var username 	 = QUnit.randomString();
		var password	 = QUnit.randomString();
		
		var login_callback = function(sessionId)
			{					
				notEqual(sessionId, TM.Const.emptyGuid, "sessionId was NOT an empty GUI");
				start();
			};		
		
		var createUser_callback = function(newUserId)
			{						
				ok(newUserId > 0 , "new user id ok: " + newUserId);
				//equal(sessionId, TM.Const.emptyGuid, "sessionId was an Empty GUI")
				TM.WebServices.WS_Users.login(username, password,login_callback);
				start();
			};		
			
		TM.WebServices.WS_Users.createUser(username, password, "", "", "", "", createUser_callback);
		
	});	

asyncTest("get currentUser details", function() 	
	{
		stop();
		var username 	 = QUnit.randomString();
		var password	 = QUnit.randomString();
		
		var currentUser_callback = function(userDetails)
			{								
				ok(userDetails, "got userDetails");
				equal(userDetails.UserName, username, "UserName & username matched");				
				ok 	 (userDetails.UserId > 0 , 		  "UserId was > 0");				
				start();
			};
			
		var login_callback = function(sessionId)
			{					
				notEqual(sessionId, TM.Const.emptyGuid, "sessionId was NOT an empty GUI");
				TM.WebServices.WS_Users.currentUser(currentUser_callback);
			};		
		
		var createUser_callback = function(newUserId)
			{						
				ok(newUserId > 0 , "new user id ok: " + newUserId);
				//equal(sessionId, TM.Const.emptyGuid, "sessionId was an Empty GUI")
				TM.WebServices.WS_Users.login(username, password,login_callback);
				start();
			};		
			
		TM.WebServices.WS_Users.createUser(username, password, "", "", "", "", createUser_callback);
	});	
