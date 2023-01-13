// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: decentraland/sdk/components/avatar_attach.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace DCL.ECSComponents {

  /// <summary>Holder for reflection information generated from decentraland/sdk/components/avatar_attach.proto</summary>
  public static partial class AvatarAttachReflection {

    #region Descriptor
    /// <summary>File descriptor for decentraland/sdk/components/avatar_attach.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AvatarAttachReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci9kZWNlbnRyYWxhbmQvc2RrL2NvbXBvbmVudHMvYXZhdGFyX2F0dGFjaC5w",
            "cm90bxIbZGVjZW50cmFsYW5kLnNkay5jb21wb25lbnRzIoMBCg5QQkF2YXRh",
            "ckF0dGFjaBIWCglhdmF0YXJfaWQYASABKAlIAIgBARJLCg9hbmNob3JfcG9p",
            "bnRfaWQYAiABKA4yMi5kZWNlbnRyYWxhbmQuc2RrLmNvbXBvbmVudHMuQXZh",
            "dGFyQW5jaG9yUG9pbnRUeXBlQgwKCl9hdmF0YXJfaWQqZgoVQXZhdGFyQW5j",
            "aG9yUG9pbnRUeXBlEhEKDUFBUFRfUE9TSVRJT04QABIRCg1BQVBUX05BTUVf",
            "VEFHEAESEgoOQUFQVF9MRUZUX0hBTkQQAhITCg9BQVBUX1JJR0hUX0hBTkQQ",
            "A0IUqgIRRENMLkVDU0NvbXBvbmVudHNiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::DCL.ECSComponents.AvatarAnchorPointType), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::DCL.ECSComponents.PBAvatarAttach), global::DCL.ECSComponents.PBAvatarAttach.Parser, new[]{ "AvatarId", "AnchorPointId" }, new[]{ "AvatarId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum AvatarAnchorPointType {
    [pbr::OriginalName("AAPT_POSITION")] AaptPosition = 0,
    [pbr::OriginalName("AAPT_NAME_TAG")] AaptNameTag = 1,
    [pbr::OriginalName("AAPT_LEFT_HAND")] AaptLeftHand = 2,
    [pbr::OriginalName("AAPT_RIGHT_HAND")] AaptRightHand = 3,
  }

  #endregion

  #region Messages
  public sealed partial class PBAvatarAttach : pb::IMessage<PBAvatarAttach>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<PBAvatarAttach> _parser = new pb::MessageParser<PBAvatarAttach>(() => new PBAvatarAttach());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<PBAvatarAttach> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::DCL.ECSComponents.AvatarAttachReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PBAvatarAttach() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PBAvatarAttach(PBAvatarAttach other) : this() {
      avatarId_ = other.avatarId_;
      anchorPointId_ = other.anchorPointId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PBAvatarAttach Clone() {
      return new PBAvatarAttach(this);
    }

    /// <summary>Field number for the "avatar_id" field.</summary>
    public const int AvatarIdFieldNumber = 1;
    private string avatarId_;
    /// <summary>
    /// default's to current player avatar id if not provided
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string AvatarId {
      get { return avatarId_ ?? ""; }
      set {
        avatarId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "avatar_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasAvatarId {
      get { return avatarId_ != null; }
    }
    /// <summary>Clears the value of the "avatar_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearAvatarId() {
      avatarId_ = null;
    }

    /// <summary>Field number for the "anchor_point_id" field.</summary>
    public const int AnchorPointIdFieldNumber = 2;
    private global::DCL.ECSComponents.AvatarAnchorPointType anchorPointId_ = global::DCL.ECSComponents.AvatarAnchorPointType.AaptPosition;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::DCL.ECSComponents.AvatarAnchorPointType AnchorPointId {
      get { return anchorPointId_; }
      set {
        anchorPointId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as PBAvatarAttach);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(PBAvatarAttach other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AvatarId != other.AvatarId) return false;
      if (AnchorPointId != other.AnchorPointId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasAvatarId) hash ^= AvatarId.GetHashCode();
      if (AnchorPointId != global::DCL.ECSComponents.AvatarAnchorPointType.AaptPosition) hash ^= AnchorPointId.GetHashCode();
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
      if (HasAvatarId) {
        output.WriteRawTag(10);
        output.WriteString(AvatarId);
      }
      if (AnchorPointId != global::DCL.ECSComponents.AvatarAnchorPointType.AaptPosition) {
        output.WriteRawTag(16);
        output.WriteEnum((int) AnchorPointId);
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
      if (HasAvatarId) {
        output.WriteRawTag(10);
        output.WriteString(AvatarId);
      }
      if (AnchorPointId != global::DCL.ECSComponents.AvatarAnchorPointType.AaptPosition) {
        output.WriteRawTag(16);
        output.WriteEnum((int) AnchorPointId);
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
      if (HasAvatarId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AvatarId);
      }
      if (AnchorPointId != global::DCL.ECSComponents.AvatarAnchorPointType.AaptPosition) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AnchorPointId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(PBAvatarAttach other) {
      if (other == null) {
        return;
      }
      if (other.HasAvatarId) {
        AvatarId = other.AvatarId;
      }
      if (other.AnchorPointId != global::DCL.ECSComponents.AvatarAnchorPointType.AaptPosition) {
        AnchorPointId = other.AnchorPointId;
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
            AvatarId = input.ReadString();
            break;
          }
          case 16: {
            AnchorPointId = (global::DCL.ECSComponents.AvatarAnchorPointType) input.ReadEnum();
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
            AvatarId = input.ReadString();
            break;
          }
          case 16: {
            AnchorPointId = (global::DCL.ECSComponents.AvatarAnchorPointType) input.ReadEnum();
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