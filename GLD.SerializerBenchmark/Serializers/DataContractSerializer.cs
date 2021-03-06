///
/// See here http://azure.microsoft.com/en-us/documentation/articles/hdinsight-dotnet-avro-serialization/
/// >PM Install-Package MsgPack.Cli
/// 

using System;
using System.IO;
using System.Runtime.Serialization;

namespace GLD.SerializerBenchmark
{
    internal class DataContractSerializerSerializer : ISerDeser
    {
        private static  DataContractSerializer _serializer = null;


        public DataContractSerializerSerializer(Type t)
        {
            _serializer = new DataContractSerializer(t);
        }

        #region ISerDeser Members

        public string Serialize<T>(object person)
        {
            using (var stream = new MemoryStream())
            {
                _serializer.WriteObject(stream, (T)person);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public T Deserialize<T>(string serialized)
        {
            var b = Convert.FromBase64String(serialized);
            using (var stream = new MemoryStream(b))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return (T) _serializer.ReadObject(stream);
            }
        }

        #endregion
    }
}