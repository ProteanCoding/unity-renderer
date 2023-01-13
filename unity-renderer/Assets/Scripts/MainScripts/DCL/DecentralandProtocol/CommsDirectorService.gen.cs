// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: decentraland/bff/comms_director_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Decentraland.Bff {

  /// <summary>Holder for reflection information generated from decentraland/bff/comms_director_service.proto</summary>
  public static partial class CommsDirectorServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for decentraland/bff/comms_director_service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommsDirectorServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci1kZWNlbnRyYWxhbmQvYmZmL2NvbW1zX2RpcmVjdG9yX3NlcnZpY2UucHJv",
            "dG8SEGRlY2VudHJhbGFuZC5iZmYaG2dvb2dsZS9wcm90b2J1Zi9lbXB0eS5w",
            "cm90bxohZGVjZW50cmFsYW5kL2NvbW1vbi92ZWN0b3JzLnByb3RvIlkKDFdv",
            "cmxkQ29tbWFuZBI+ChFzZXRfY29tbXNfYWRhcHRlchgBIAEoCzIhLmRlY2Vu",
            "dHJhbGFuZC5iZmYuU2V0Q29tbXNBZGFwdGVySABCCQoHY29tbWFuZCIsCg9T",
            "ZXRDb21tc0FkYXB0ZXISGQoRY29ubmVjdGlvbl9zdHJpbmcYASABKAkiaAoJ",
            "SGVhcnRiZWF0Ei8KCHBvc2l0aW9uGAEgASgLMh0uZGVjZW50cmFsYW5kLmNv",
            "bW1vbi5Qb3NpdGlvbhIZCgxkZXNpcmVkX3Jvb20YAiABKAlIAIgBAUIPCg1f",
            "ZGVzaXJlZF9yb29tMq4BChRDb21tc0RpcmVjdG9yU2VydmljZRJGCg1TZW5k",
            "SGVhcnRiZWF0EhsuZGVjZW50cmFsYW5kLmJmZi5IZWFydGJlYXQaFi5nb29n",
            "bGUucHJvdG9idWYuRW1wdHkiABJOChBHZXRDb21tc0NvbW1hbmRzEhYuZ29v",
            "Z2xlLnByb3RvYnVmLkVtcHR5Gh4uZGVjZW50cmFsYW5kLmJmZi5Xb3JsZENv",
            "bW1hbmQiADABYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, global::Decentraland.Common.VectorsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Decentraland.Bff.WorldCommand), global::Decentraland.Bff.WorldCommand.Parser, new[]{ "SetCommsAdapter" }, new[]{ "Command" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Decentraland.Bff.SetCommsAdapter), global::Decentraland.Bff.SetCommsAdapter.Parser, new[]{ "ConnectionString" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Decentraland.Bff.Heartbeat), global::Decentraland.Bff.Heartbeat.Parser, new[]{ "Position", "DesiredRoom" }, new[]{ "DesiredRoom" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class WorldCommand : pb::IMessage<WorldCommand>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<WorldCommand> _parser = new pb::MessageParser<WorldCommand>(() => new WorldCommand());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<WorldCommand> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Decentraland.Bff.CommsDirectorServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public WorldCommand() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public WorldCommand(WorldCommand other) : this() {
      switch (other.CommandCase) {
        case CommandOneofCase.SetCommsAdapter:
          SetCommsAdapter = other.SetCommsAdapter.Clone();
          break;
      }

      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public WorldCommand Clone() {
      return new WorldCommand(this);
    }

    /// <summary>Field number for the "set_comms_adapter" field.</summary>
    public const int SetCommsAdapterFieldNumber = 1;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Decentraland.Bff.SetCommsAdapter SetCommsAdapter {
      get { return commandCase_ == CommandOneofCase.SetCommsAdapter ? (global::Decentraland.Bff.SetCommsAdapter) command_ : null; }
      set {
        command_ = value;
        commandCase_ = value == null ? CommandOneofCase.None : CommandOneofCase.SetCommsAdapter;
      }
    }

    private object command_;
    /// <summary>Enum of possible cases for the "command" oneof.</summary>
    public enum CommandOneofCase {
      None = 0,
      SetCommsAdapter = 1,
    }
    private CommandOneofCase commandCase_ = CommandOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CommandOneofCase CommandCase {
      get { return commandCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearCommand() {
      commandCase_ = CommandOneofCase.None;
      command_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as WorldCommand);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(WorldCommand other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(SetCommsAdapter, other.SetCommsAdapter)) return false;
      if (CommandCase != other.CommandCase) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (commandCase_ == CommandOneofCase.SetCommsAdapter) hash ^= SetCommsAdapter.GetHashCode();
      hash ^= (int) commandCase_;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (commandCase_ == CommandOneofCase.SetCommsAdapter) {
        output.WriteRawTag(10);
        output.WriteMessage(SetCommsAdapter);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (commandCase_ == CommandOneofCase.SetCommsAdapter) {
        output.WriteRawTag(10);
        output.WriteMessage(SetCommsAdapter);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (commandCase_ == CommandOneofCase.SetCommsAdapter) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SetCommsAdapter);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(WorldCommand other) {
      if (other == null) {
        return;
      }
      switch (other.CommandCase) {
        case CommandOneofCase.SetCommsAdapter:
          if (SetCommsAdapter == null) {
            SetCommsAdapter = new global::Decentraland.Bff.SetCommsAdapter();
          }
          SetCommsAdapter.MergeFrom(other.SetCommsAdapter);
          break;
      }

      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            global::Decentraland.Bff.SetCommsAdapter subBuilder = new global::Decentraland.Bff.SetCommsAdapter();
            if (commandCase_ == CommandOneofCase.SetCommsAdapter) {
              subBuilder.MergeFrom(SetCommsAdapter);
            }
            input.ReadMessage(subBuilder);
            SetCommsAdapter = subBuilder;
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            global::Decentraland.Bff.SetCommsAdapter subBuilder = new global::Decentraland.Bff.SetCommsAdapter();
            if (commandCase_ == CommandOneofCase.SetCommsAdapter) {
              subBuilder.MergeFrom(SetCommsAdapter);
            }
            input.ReadMessage(subBuilder);
            SetCommsAdapter = subBuilder;
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class SetCommsAdapter : pb::IMessage<SetCommsAdapter>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SetCommsAdapter> _parser = new pb::MessageParser<SetCommsAdapter>(() => new SetCommsAdapter());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<SetCommsAdapter> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Decentraland.Bff.CommsDirectorServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SetCommsAdapter() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SetCommsAdapter(SetCommsAdapter other) : this() {
      connectionString_ = other.connectionString_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SetCommsAdapter Clone() {
      return new SetCommsAdapter(this);
    }

    /// <summary>Field number for the "connection_string" field.</summary>
    public const int ConnectionStringFieldNumber = 1;
    private string connectionString_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ConnectionString {
      get { return connectionString_; }
      set {
        connectionString_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as SetCommsAdapter);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(SetCommsAdapter other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ConnectionString != other.ConnectionString) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ConnectionString.Length != 0) hash ^= ConnectionString.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ConnectionString.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(ConnectionString);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ConnectionString.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(ConnectionString);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (ConnectionString.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ConnectionString);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(SetCommsAdapter other) {
      if (other == null) {
        return;
      }
      if (other.ConnectionString.Length != 0) {
        ConnectionString = other.ConnectionString;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ConnectionString = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            ConnectionString = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class Heartbeat : pb::IMessage<Heartbeat>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Heartbeat> _parser = new pb::MessageParser<Heartbeat>(() => new Heartbeat());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<Heartbeat> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Decentraland.Bff.CommsDirectorServiceReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Heartbeat() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Heartbeat(Heartbeat other) : this() {
      position_ = other.position_ != null ? other.position_.Clone() : null;
      desiredRoom_ = other.desiredRoom_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Heartbeat Clone() {
      return new Heartbeat(this);
    }

    /// <summary>Field number for the "position" field.</summary>
    public const int PositionFieldNumber = 1;
    private global::Decentraland.Common.Position position_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Decentraland.Common.Position Position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    /// <summary>Field number for the "desired_room" field.</summary>
    public const int DesiredRoomFieldNumber = 2;
    private string desiredRoom_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string DesiredRoom {
      get { return desiredRoom_ ?? ""; }
      set {
        desiredRoom_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "desired_room" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasDesiredRoom {
      get { return desiredRoom_ != null; }
    }
    /// <summary>Clears the value of the "desired_room" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearDesiredRoom() {
      desiredRoom_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as Heartbeat);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(Heartbeat other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Position, other.Position)) return false;
      if (DesiredRoom != other.DesiredRoom) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (position_ != null) hash ^= Position.GetHashCode();
      if (HasDesiredRoom) hash ^= DesiredRoom.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (position_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Position);
      }
      if (HasDesiredRoom) {
        output.WriteRawTag(18);
        output.WriteString(DesiredRoom);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (position_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Position);
      }
      if (HasDesiredRoom) {
        output.WriteRawTag(18);
        output.WriteString(DesiredRoom);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (position_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Position);
      }
      if (HasDesiredRoom) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DesiredRoom);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(Heartbeat other) {
      if (other == null) {
        return;
      }
      if (other.position_ != null) {
        if (position_ == null) {
          Position = new global::Decentraland.Common.Position();
        }
        Position.MergeFrom(other.Position);
      }
      if (other.HasDesiredRoom) {
        DesiredRoom = other.DesiredRoom;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (position_ == null) {
              Position = new global::Decentraland.Common.Position();
            }
            input.ReadMessage(Position);
            break;
          }
          case 18: {
            DesiredRoom = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (position_ == null) {
              Position = new global::Decentraland.Common.Position();
            }
            input.ReadMessage(Position);
            break;
          }
          case 18: {
            DesiredRoom = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code