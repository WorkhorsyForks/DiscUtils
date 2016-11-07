//
// Copyright (c) 2008-2011, Kenneth Bell
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
using DiscUtils.Internal;

namespace DiscUtils.Udf
{
    internal abstract class PartitionMap : IByteArraySerializable
    {
        public byte Type;

        public abstract int Size { get; }

        public int ReadFrom(byte[] buffer, int offset)
        {
            Type = buffer[offset];
            return Parse(buffer, offset);
        }

        public void WriteTo(byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }

        public static PartitionMap CreateFrom(byte[] buffer, int offset)
        {
            PartitionMap result = null;

            byte type = buffer[offset];
            if (type == 1)
            {
                result = new Type1PartitionMap();
            }
            else if (type == 2)
            {
                EntityIdentifier id = Utilities.ToStruct<UdfEntityIdentifier>(buffer, offset + 4);
                switch (id.Identifier)
                {
                    case "*UDF Virtual Partition":
                        result = new VirtualPartitionMap();
                        break;
                    case "*UDF Sparable Partition":
                        result = new SparablePartitionMap();
                        break;
                    case "*UDF Metadata Partition":
                        result = new MetadataPartitionMap();
                        break;
                    default:
                        throw new InvalidDataException("Unrecognized partition map entity id: " + id);
                }
            }

            if (result != null)
            {
                result.ReadFrom(buffer, offset);
            }

            return result;
        }

        protected abstract int Parse(byte[] buffer, int offset);
    }
}