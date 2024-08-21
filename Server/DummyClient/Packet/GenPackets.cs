using ServerCore;
using System;
using System.Text;
using System.Collections.Generic;

public enum PacketID
{
    S_BroadcastEnterGame = 1, 
    C_LeaveGame = 2, 
    S_BroadcastLeaveGame = 3, 
    S_PlayerList = 4, 
    C_Move = 5, 
    S_BroadcastMove = 6, 
    
}

public interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}

public class S_BroadcastEnterGame : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        this.playerId = BitConverter.ToInt32(s.Slice(offset, s.Length - offset));
		offset += sizeof(int);
		
		this.posX = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
		this.posY = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
		this.posZ = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.S_BroadcastEnterGame);
        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.playerId);           
		offset += sizeof(int);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posX);           
		offset += sizeof(float);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posY);           
		offset += sizeof(float);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posZ);           
		offset += sizeof(float);
		

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {
            return null;
        }
                
        return SendBufferHelper.Close(offset); 
    }
}
public class C_LeaveGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_LeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.C_LeaveGame);
        offset += sizeof(ushort);

        

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {
            return null;
        }
                
        return SendBufferHelper.Close(offset); 
    }
}
public class S_BroadcastLeaveGame : IPacket
{
    public int playerId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveGame; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        this.playerId = BitConverter.ToInt32(s.Slice(offset, s.Length - offset));
		offset += sizeof(int);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.S_BroadcastLeaveGame);
        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.playerId);           
		offset += sizeof(int);
		

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {
            return null;
        }
                
        return SendBufferHelper.Close(offset); 
    }
}
public class S_PlayerList : IPacket
{
    public class Player
	{
	    public bool isSelf;
		public int playerId;
		public float posX;
		public float posY;
		public float posZ;
	
	    public void Read(ReadOnlySpan<byte> s, ref ushort offset)
	    {
	        this.isSelf = BitConverter.ToBoolean(s.Slice(offset, s.Length - offset));
			offset += sizeof(bool);
			
			this.playerId = BitConverter.ToInt32(s.Slice(offset, s.Length - offset));
			offset += sizeof(int);
			
			this.posX = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
			offset += sizeof(float);
			
			this.posY = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
			offset += sizeof(float);
			
			this.posZ = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
			offset += sizeof(float);
			
	    }
	
	    public bool Write(Span<byte> s, ref ushort offset)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.isSelf);           
			offset += sizeof(bool);
			
			success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.playerId);           
			offset += sizeof(int);
			
			success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posX);           
			offset += sizeof(float);
			
			success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posY);           
			offset += sizeof(float);
			
			success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posZ);           
			offset += sizeof(float);
			
	        return true;
	    }
	}
	public List<Player> players = new();

    public ushort Protocol { get { return (ushort)PacketID.S_PlayerList; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(s.Slice(offset, s.Length - offset));
		offset += sizeof(ushort);
		for (int i = 0; i < playerLen; i++)
		{
		    Player player = new();
		    player.Read(s, ref offset);
		    players.Add(player);
		}
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.S_PlayerList);
        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)this.players.Count);
		offset += sizeof(ushort);
		foreach (Player player in this.players)
		{
		    success &= player.Write(s, ref offset);
		}
		

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {
            return null;
        }
                
        return SendBufferHelper.Close(offset); 
    }
}
public class C_Move : IPacket
{
    public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.C_Move; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        this.posX = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
		this.posY = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
		this.posZ = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.C_Move);
        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posX);           
		offset += sizeof(float);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posY);           
		offset += sizeof(float);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posZ);           
		offset += sizeof(float);
		

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {
            return null;
        }
                
        return SendBufferHelper.Close(offset); 
    }
}
public class S_BroadcastMove : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastMove; } }

    public void Read(ArraySegment<byte> segment)
    {
        ushort offset = sizeof(ushort) + sizeof(ushort);

        ReadOnlySpan<byte> s = new(segment.Array, segment.Offset, segment.Count);

        this.playerId = BitConverter.ToInt32(s.Slice(offset, s.Length - offset));
		offset += sizeof(int);
		
		this.posX = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
		this.posY = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
		this.posZ = BitConverter.ToSingle(s.Slice(offset, s.Length - offset));
		offset += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(65535);
        ushort offset = 0;
        bool success = true;

        Span<byte> s = new(segment.Array, segment.Offset, segment.Count);

        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), (ushort)PacketID.S_BroadcastMove);
        offset += sizeof(ushort);

        success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.playerId);           
		offset += sizeof(int);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posX);           
		offset += sizeof(float);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posY);           
		offset += sizeof(float);
		
		success &= BitConverter.TryWriteBytes(s.Slice(offset, s.Length - offset), this.posZ);           
		offset += sizeof(float);
		

        success &= BitConverter.TryWriteBytes(s, offset);

        if (success == false)
        {
            return null;
        }
                
        return SendBufferHelper.Close(offset); 
    }
}

