
// AUTOGENERATED, DO NOT EDIT
// Type definitions for server implementations of ports.
// package: decentraland.renderer.kernel_services
// file: decentraland/renderer/kernel_services/friends_kernel.proto
using Cysharp.Threading.Tasks;
using rpc_csharp;

namespace Decentraland.Renderer.KernelServices {
public interface IClientFriendsKernelService
{
  UniTask<GetFriendshipStatusResponse> GetFriendshipStatus(GetFriendshipStatusRequest request);
}

public class ClientFriendsKernelService : IClientFriendsKernelService
{
  private readonly RpcClientModule module;

  public ClientFriendsKernelService(RpcClientModule module)
  {
      this.module = module;
  }

  
  public UniTask<GetFriendshipStatusResponse> GetFriendshipStatus(GetFriendshipStatusRequest request)
  {
      return module.CallUnaryProcedure<GetFriendshipStatusResponse>("GetFriendshipStatus", request);
  }

}
}