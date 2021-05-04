using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Encoder = System.Drawing.Imaging.Encoder;

namespace SocketCommon
{
    public static class DataHelper
    {
        public enum DataType : byte { 
            SByte   = 0b0000_0001,
            Short   = 0b0000_0010,
            UShort  = 0b0000_0011,
            Int     = 0b0000_0100,
            UInt    = 0b0000_0101,
            Long    = 0b0000_0110,
            ULong   = 0b0000_0111,
            Float   = 0b0000_1000,
            Double  = 0b0000_1001,
            String  = 0b0000_1010,
            Boolean = 0b0000_1011,
            Byte    = 0b0000_1100,
            Array   = 0b1000_0000
        }

        /// <summary>
        /// Parse a simple value type to byte[]
        /// </summary>
        /// <typeparam name="T">The origin value type</typeparam>
        /// <param name="Data">The value to be converted</param>
        /// <returns>The converted data</returns>
        public static byte[] ParseToBytes<T>(this T Data) {
            using (var MemBuffer = new MemoryStream())
            using (BinaryWriter Binary = new BinaryWriter(MemBuffer))
            {
                switch (Data)
                {
                    case sbyte S8:
                        Binary.Write(DataType.SByte);
                        Binary.Write(S8);
                        break;
                    case short S16:
                        Binary.Write(DataType.Short);
                        Binary.Write(S16);
                        break;
                    case int S32:
                        Binary.Write(DataType.Int);
                        Binary.Write(S32);
                        break;
                    case long S64:
                        Binary.Write(DataType.Long);
                        Binary.Write(S64);
                        break;

                    case byte U8:
                        Binary.Write(DataType.Byte);
                        Binary.Write(U8);
                        break;
                    case ushort U16:
                        Binary.Write(DataType.UShort);
                        Binary.Write(U16);
                        break;
                    case uint U32:
                        Binary.Write(DataType.UInt);
                        Binary.Write(U32);
                        break;
                    case ulong U64:
                        Binary.Write(DataType.ULong);
                        Binary.Write(U64);
                        break;

                    case float Float:
                        Binary.Write(DataType.Float);
                        Binary.Write(Float);
                        break;
                    case double Double:
                        Binary.Write(DataType.Double);
                        Binary.Write(Double);
                        break;

                    case string String:
                        Binary.Write(DataType.String);
                        Binary.WriteString(String);
                        break;

                    case bool Boolean:
                        Binary.Write(DataType.Boolean);
                        Binary.Write(Boolean);
                        break;

                    case byte[] ByteArr:
                        Binary.Write(DataType.Byte | DataType.Array);
                        Binary.Write(ByteArr.Length);
                        Binary.Write(ByteArr);
                        break;

                    case string[] StringArr:
                        Binary.Write(DataType.String | DataType.Array);
                        Binary.Write(StringArr.Length);
                        foreach (var String in StringArr)
                            Binary.WriteString(String);
                        break;

                    default:
                        throw new Exception("Type not Supported");

                }
                return MemBuffer.ToArray();
            }
        }

        /// <summary>
        /// Decode a data genareted by <see cref="ParseToBytes{T}(T)"/>
        /// </summary>
        /// <param name="RawData">The Raw Data</param>
        /// <returns>The original data value and type</returns>
        public static dynamic ReadData(this byte[] RawData, out int Readed) {
            using (MemoryStream tmp = new MemoryStream(RawData)) {
                var Data = tmp.ReadData();
                Readed = (int)tmp.Position;
                return Data;
            }
        }

        /// <summary>
        /// Decode a data genareted by <see cref="ParseToBytes{T}(T)"/>
        /// </summary>
        /// <param name="Input">The Stream to Read the Data</param>
        /// <returns></returns>
        public static dynamic ReadData(this Stream Input) {

            var TypeValue = Input.ReadByte();
            if (TypeValue < 0)
                throw new Exception("Unexpected Stream End");

            BinaryReader Reader = new BinaryReader(Input);
            switch ((DataType)TypeValue)
            {
                case DataType.SByte:
                    return Reader.ReadSByte();
                case DataType.Short:
                    return Reader.ReadInt16();
                case DataType.Int:
                    return Reader.ReadInt32();
                case DataType.Long:
                    return Reader.ReadInt64();

                case DataType.Byte:
                    return Reader.ReadByte();
                case DataType.UShort:
                    return Reader.ReadUInt16();
                case DataType.UInt:
                    return Reader.ReadUInt32();
                case DataType.ULong:
                    return Reader.ReadUInt64();


                case DataType.Float:
                    return Reader.ReadSingle();
                case DataType.Double:
                    return Reader.ReadDouble();

                case DataType.String:
                    return Reader.ReadCustomString();

                case DataType.Boolean:
                    return Reader.ReadBoolean();


                case DataType.Array | DataType.Byte:
                    byte[] ArrData = new byte[Reader.ReadInt32()];
                    Reader.BaseStream.Read(ArrData, 0, ArrData.Length);
                    return ArrData;

                case DataType.Array | DataType.String:
                    string[] ArrString = new string[Reader.ReadInt32()];
                    for (int i = 0; i < ArrString.Length; i++)
                        ArrString[i] = Reader.ReadCustomString();
                    return ArrString;

                default:
                    throw new Exception("Type not Supported");
            }
        }

        public static void Write(this BinaryWriter Stream, DataType Type) {
            Stream.Write((byte)Type);
        }
        public static void WriteString(this BinaryWriter Stream, string String) {
            var StrData = Encoding.Unicode.GetBytes(String);
            Stream.Write(StrData.Length);
            Stream.Write(StrData);
        }
        public static string ReadCustomString(this BinaryReader Stream)
        {
            byte[] ArrData = new byte[Stream.ReadInt32()];
            Stream.BaseStream.Read(ArrData, 0, ArrData.Length);

            return Encoding.Unicode.GetString(ArrData);
        }

        public static byte[] Export(this Bitmap Image, long Quality = 90) {
            using (var Stream = new MemoryStream())
            using (EncoderParameters EncoderParameters = new EncoderParameters(1))
            using (EncoderParameter EncoderParameter = new EncoderParameter(Encoder.Quality, Quality))
            {
                ImageCodecInfo codecInfo = ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters.Param[0] = EncoderParameter;
                Image.Save(Stream, codecInfo, EncoderParameters);
                return Stream.ToArray();
            }
        }

        public static Image CastImage(this byte[] Data) {
            return Image.FromStream(new MemoryStream(Data));
        }
    }
}
