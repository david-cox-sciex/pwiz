﻿/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2018 University of Washington - Seattle, WA
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pwiz.Skyline.Alerts;
using pwiz.Skyline.Controls.Graphs;
using pwiz.Skyline.FileUI;
using pwiz.Skyline.Model.ElementLocators;
using pwiz.Skyline.Model.Results.RemoteApi;
using pwiz.Skyline.ToolsUI;
using pwiz.SkylineTestUtil;

namespace pwiz.SkylineTestConnected
{
    [TestClass]
    public class UnifiFunctionalTest : AbstractFunctionalTestEx
    {
        [TestMethod]
        public void TestUnifi()
        {
            if (!UnifiTestUtil.EnableUnifiTests)
            {
                return;
            }
            TestFilesZip = @"TestConnected\UnifiFunctionalTest.zip";
            RunFunctionalTest();
        }

        protected override void DoTest()
        {
            RunUI(()=>SkylineWindow.OpenFile(TestFilesDir.GetTestPath("test.sky")));
            var askDecoysDlg = ShowDialog<MultiButtonMsgDlg>(SkylineWindow.ImportResults);
            var importResultsDlg = ShowDialog<ImportResultsDlg>(askDecoysDlg.ClickNo);
            var openDataSourceDialog = ShowDialog<OpenDataSourceDialog>(importResultsDlg.OkDialog);
            var editAccountDlg = ShowDialog<EditRemoteAccountDlg>(() => openDataSourceDialog.CurrentDirectory = RemoteUrl.EMPTY);
            RunUI(()=>editAccountDlg.SetRemoteAccount(UnifiTestUtil.GetTestAccount()));
            OkDialog(editAccountDlg, editAccountDlg.OkDialog);
            OpenFile(openDataSourceDialog, "Company");
            OpenFile(openDataSourceDialog, "Demo Department");
            OpenFile(openDataSourceDialog, "Peptides");
            OpenFile(openDataSourceDialog, "Hi3_ClpB_MSe_01");
            var lockMassDlg = WaitForOpenForm<ImportResultsLockMassDlg>();
            OkDialog(lockMassDlg, lockMassDlg.OkDialog);
            WaitForDocumentLoaded();
            RunUI(() => SkylineWindow.SelectElement(ElementRefs.FromObjectReference(ElementLocator.Parse("Molecule:/sp|P0A6A8|ACP_ECOLI/ITTVQAAIDYINGHQA"))));
            ClickChromatogram(4.0, 3.25);
            GraphFullScan graphFullScan = FindOpenForm<GraphFullScan>();
            Assert.IsNotNull(graphFullScan);
        }

        private void OpenFile(OpenDataSourceDialog openDataSourceDialog, string name)
        {
            WaitForConditionUI(() => openDataSourceDialog.ListItemNames.Contains(name));
            RunUI(()=>
            {
                openDataSourceDialog.SelectFile(name);
                openDataSourceDialog.Open();
            });
            
        }
    }
}
