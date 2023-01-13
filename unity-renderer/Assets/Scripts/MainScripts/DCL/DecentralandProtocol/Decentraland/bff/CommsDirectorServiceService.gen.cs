// AUTOGENERATED, DO NOT EDIT
// Type definitions for server implementations of ports.
// package: decentraland.bff
// file: decentraland/bff/comms_director_service.proto
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using rpc_csharp.protocol;
using rpc_csharp;
using Google.Protobuf.WellKnownTypes;
namespace Decentraland.Bff {
public interface ICommsDirectorService<Context>
{

  UniTask<Empty> SendHeartbeat(Heartbeat request, Context context, CancellationToken ct);

  IUniTaskAsyncEnumerable<WorldCommand> GetCommsCommands(Empty request, Context context);

}

public static class CommsDirectorServiceCodeGen
{
  public const string ServiceName = "CommsDirectorService";

  public static void RegisterService<Context>(RpcServerPort<Context> port, ICommsDirectorService<Context> service)
  {
    var result = new ServerModuleDefinition<Context>();
      
    result.definition.Add("SendHeartbeat", async (payload, context, ct) => { var res = await service.SendHeartbeat(Heartbeat.Parser.ParseFrom(payload), context, ct); return res?.ToByteString(); });
    result.serverStreamDefinition.Add("GetCommsCommands", (payload, context) => { return ProtocolHelpers.SerializeMessageEnumerator<WorldCommand>(service.GetCommsCommands(Empty.Parser.ParseFrom(payload), context)); });

    port.RegisterModule(ServiceName, (port) => UniTask.FromResult(result));
  }
}
}