﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using TeamMentor.CoreLib;

namespace TeamMentor.UnitTests.CoreLib
{
	[TestFixture]
	public class Test_Users_UserActivities
	{
		public UserActivities userActivities;		

		[SetUp]
		public void Setup()
		{
			userActivities = UserActivities.Current;
			userActivities.reset();
		}

		[Test]
		public void CheckSetup()
		{
			Assert.IsNotNull(userActivities);
			Assert.IsNotNull(userActivities.ActivitiesLog);
			Assert.IsEmpty(userActivities.ActivitiesLog);
		}

		[Test]
		public void AddAnotherActivity()
		{
			var name = "Test Name";
			var details = "Test Details";
			name.logActivity(details);
			Assert.AreEqual(1, userActivities.ActivitiesLog.size());
		}
	}
}