﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Commands.Test.WAPackIaaS.WebClient
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Commands.Utilities.WAPackIaaS.DataContract;
    using Microsoft.WindowsAzure.Commands.Utilities.WAPackIaaS.WebClient;
    using System;

    [TestClass]
    public class JsonHelperTests
    {
        [TestMethod]
        [TestCategory("WAPackIaaS")]
        public void SerializeDeserializeVirtualMachine()
        {
            var helper = new JsonHelpers<VirtualMachine>();


            var toSerialize = new VirtualMachine
                {
                    ID = Guid.NewGuid(),
                    StampId = Guid.NewGuid(),
                    CreationTime = DateTime.Now,
                    PerfDiskBytesRead = 500,
                    Name = @"This is a test 这是一个测试 㐀㐁㐂㐃㐄㐅"
                };

            var serialized = helper.Serialize(toSerialize);

            var vmList = helper.Deserialize(serialized);
            Assert.IsNotNull(vmList);
            Assert.AreEqual(vmList.Count, 1);
            Assert.IsInstanceOfType(vmList[0], typeof (VirtualMachine));

            Assert.AreEqual(toSerialize.ID, vmList[0].ID);
            Assert.AreEqual(toSerialize.StampId, vmList[0].StampId);
            Assert.AreEqual(toSerialize.CreationTime, vmList[0].CreationTime);
            Assert.AreEqual(toSerialize.PerfDiskBytesRead, vmList[0].PerfDiskBytesRead);
            Assert.AreEqual(toSerialize.Name, vmList[0].Name);
        }
    }
}
