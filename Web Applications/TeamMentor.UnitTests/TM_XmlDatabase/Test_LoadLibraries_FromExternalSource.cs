﻿using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using TeamMentor.CoreLib;

namespace TeamMentor.UnitTests.TM_XmlDatabase
{
    [TestFixture]
    public class Test_LoadLibraries_FromExternalSource : TM_XmlDatabase_InMemory
    {
        [Test]
        public void DownloadAndInstallLibraryFromZip()
        {
            /*tmXmlDatabase.Path_XmlLibraries = "_TempXmlLibraries".tempDir();
            var pathToGitHubZipBall = "https://github.com/TeamMentor/Library_Top_Vulnerabilities/zipball/master";
            var tmLibraries_Before = tmXmlDatabase.tmLibraries();
            var result = tmXmlDatabase.xmlDB_Libraries_ImportFromZip(pathToGitHubZipBall, "");
            
            var tmLibraries_After = tmXmlDatabase.tmLibraries();            
            Assert.IsEmpty(tmLibraries_Before, "No Libraries should be there before install");
            Assert.IsTrue(result , "xmlDB_Libraries_ImportFromZip");
            */

            var tmLibraries_Before = tmXmlDatabase.tmLibraries();            

            Install_LibraryFromZip_TopVulns();
            Install_LibraryFromZip_TopVulns();          //2nd time should skip install

            Assert.IsEmpty(tmLibraries_Before, "No Libraries should be there before install");
            Assert.IsNotEmpty(tmXmlDatabase.tmLibraries(), "After install, no Libraries");
            Assert.IsNotEmpty(tmXmlDatabase.tmViews(), "After install, no Views");
            //Assert.IsNotEmpty(tmXmlDatabase.tmFolders(), "After install, no Folders");
            Assert.IsNotEmpty(tmXmlDatabase.tmGuidanceItems(), "After install, no Articles");

            Install_LibraryFromZip_OWASP();

            Assert.AreEqual  (2, tmXmlDatabase.tmLibraries().size() , "After OWASP install, there should be 2");
            Assert.IsNotEmpty(tmXmlDatabase.tmViews(), "After OWASP install, no Views");
            Assert.IsNotEmpty(tmXmlDatabase.tmFolders(), "After OWASPinstall, no Folders");
            Assert.IsNotEmpty(tmXmlDatabase.tmGuidanceItems(), "After OWASP install, no Articles");
        }
        
    }
}