﻿//
// Copyright (c) 2008-2009, Kenneth Bell
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Security.AccessControl;
using NUnit.Framework;

namespace DiscUtils.Registry
{
    [TestFixture]
    public class RegistryHiveTest
    {
        [Test]
        public void Create()
        {
            MemoryStream ms = new MemoryStream();
            RegistryHive hive = RegistryHive.Create(ms);
            Assert.IsNull(hive.Root.Parent);
            Assert.AreEqual(0, hive.Root.ValueCount);
            Assert.AreEqual(0, hive.Root.SubKeyCount);
            Assert.IsNotNull(hive.Root.SubKeys);
            Assert.AreEqual("O:BAG:BAD:PAI(A;;KA;;;SY)(A;CI;KA;;;BA)", hive.Root.GetAccessControl().GetSecurityDescriptorSddlForm(AccessControlSections.All));
            Assert.AreEqual(RegistryKeyFlags.Root | RegistryKeyFlags.Normal, hive.Root.Flags);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null()
        {
            RegistryHive hive = RegistryHive.Create((Stream)null);
        }

    }
}
