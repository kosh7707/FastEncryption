using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    internal class PacketFormat
    {
        // {0} 패킷 등록
        public static string managerFormat =
@"using ServerCore;
using System;
using System.Text;
using System.Collections.Generic;

public class PacketManager
{{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance {{  get {{ return _instance; }} }}
    #endregion

    PacketManager()
    {{
        Register();
    }}

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new();
    public void Register()
    {{
{0}
    }}

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {{
        ushort offset = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        offset += 2;

        ushort packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
        offset += 2;

        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(packetId, out func))
        {{
            IPacket packet = func.Invoke(session, buffer);

            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else 
                HandlePacket(session, packet);
        }}
    }}

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {{
        T pkt = new();
        pkt.Read(buffer);
        return pkt;
    }}

    public void HandlePacket(PacketSession session, IPacket packet)
    {{
        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }}
}}";
        
        // {0} 패킷 이름
        public static string managerRegisterFormat =
@"        _makeFunc.Add((ushort)PacketID.{0}, MakePacket<{0}>);
        _handler.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);
";

        // {0} 패킷 이름/번호 목록
        // {1} 패킷 목록
        public static string fileFormat =
@"using ServerCore;
using System;
using System.Text;
using System.Collections.Generic;

public enum PacketID
{{
    {0}
}}

public interface IPacket
{{
	ushort Protocol {{ get; }}
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}}

{1}
";

        // {0} 패킷 이름
        // {1} 패킷 번호
        public static string packetEnumFormat = 
@"{0} = {1}, 
    ";


        // {0} 패킷 이름
        // {1} 멤버 변수
        // {2} 멤버 변수 Read
        // {3} 멤버 변수 Write
        public static string packetFormat =
@"public class {0} : IPacket
{{
    {1}

    public ushort Protocol {{ get {{ return (ushort)PacketID.{0}; }} }}

    public void Read(ArraySegment<byte> segment)
    {{
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.{0});
        offset += sizeof(ushort);

        {3}

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {{
            return null;
        }}
                
        return SendBufferHelper.Close(offset); 
    }}
}}
";
        // {0} 변수 형식
        // {1} 변수 이름
        public static string memberFormat =
@"public {0} {1};";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        // {2} 멤버 변수
        // {3} 멤버 변수 Read
        // {4} 멤버 변수 Write
        public static string memberListFormat =
@"public class {0}
{{
    {2}

    public void Read(ReadOnlySpan<byte> s, ref ushort offset)
    {{
        {3}
    }}

    public bool Write(Span<byte> s, ref ushort offset)
    {{
        bool success = true;
        {4}
        return true;
    }}
}}
public List<{0}> {1}s = new();";

        // {0} 변수 이름
        // {1} To~ 변수 형식
        // {2} 변수 형식
        public static string readFormat =
@"this.{0} = BitConverter.{1}(s.Slice(offset, s.Length - offset));
offset += sizeof({2});
";

        // {0} 변수 이름
        // {1} 변수 형식
        public static string readByteFormat =
@"this.{0} = ({1})segment.Array[segment.Offset + offset];
offset += sizeof({1});
";

        // {0} 변수 이름
        public static string readStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(s.Slice(offset, s.Length - offset));
offset += sizeof(ushort);

this.{0} = Encoding.Unicode.GetString(s.Slice(offset, {0}Len));
offset += {0}Len;
";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        public static string readListFormat =
@"this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(s.Slice(offset, s.Length - offset));
offset += sizeof(ushort);
for (int i = 0; i < {1}Len; i++)
{{
    {0} {1} = new();
    {1}.Read(s, ref offset);
    {1}s.Add({1});
}}
";

        // {0} 변수 이름
        // {1} 변수 형식
        public static string writeFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.{0});           
offset += sizeof({1});
";

        // {0} 변수 이름
        // {1} 변수 형식
        public static string writeByteFormat =
@"segment.Array[segment.Offset + offset] = (byte)this.{0};
offset += sizeof({1});";

        // {0} 변수 이름
        public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, segment.Array, segment.Offset + sizeof(ushort) + offset);
success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), {0}Len);
offset += sizeof(ushort);
offset += {0}Len;
";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        public static string writeListFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)this.{1}s.Count);
offset += sizeof(ushort);
foreach ({0} {1} in this.{1}s)
{{
    success &= {1}.Write(s, ref offset);
}}
";
    }


}
