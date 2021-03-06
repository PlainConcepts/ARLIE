// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Lunar.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace RLEnvs.Lunar2D {
  public static partial class LunarService
  {
    static readonly string __ServiceName = "LunarService";

    static readonly grpc::Marshaller<global::RLEnvs.Lunar2D.ServiceMessage> __Marshaller_ServiceMessage = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar2D.ServiceMessage.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar2D.DimResult> __Marshaller_DimResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar2D.DimResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar2D.Action> __Marshaller_Action = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar2D.Action.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar2D.ActionResult> __Marshaller_ActionResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar2D.ActionResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar2D.Observation> __Marshaller_Observation = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar2D.Observation.Parser.ParseFrom);

    static readonly grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.DimResult> __Method_GetActionDim = new grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.DimResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetActionDim",
        __Marshaller_ServiceMessage,
        __Marshaller_DimResult);

    static readonly grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.DimResult> __Method_GetObservationDim = new grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.DimResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetObservationDim",
        __Marshaller_ServiceMessage,
        __Marshaller_DimResult);

    static readonly grpc::Method<global::RLEnvs.Lunar2D.Action, global::RLEnvs.Lunar2D.ActionResult> __Method_PerformAction = new grpc::Method<global::RLEnvs.Lunar2D.Action, global::RLEnvs.Lunar2D.ActionResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PerformAction",
        __Marshaller_Action,
        __Marshaller_ActionResult);

    static readonly grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.ServiceMessage> __Method_Render = new grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.ServiceMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Render",
        __Marshaller_ServiceMessage,
        __Marshaller_ServiceMessage);

    static readonly grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.Observation> __Method_Reset = new grpc::Method<global::RLEnvs.Lunar2D.ServiceMessage, global::RLEnvs.Lunar2D.Observation>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Reset",
        __Marshaller_ServiceMessage,
        __Marshaller_Observation);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::RLEnvs.Lunar2D.LunarReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of LunarService</summary>
    public abstract partial class LunarServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar2D.DimResult> GetActionDim(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar2D.DimResult> GetObservationDim(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar2D.ActionResult> PerformAction(global::RLEnvs.Lunar2D.Action request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar2D.ServiceMessage> Render(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar2D.Observation> Reset(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for LunarService</summary>
    public partial class LunarServiceClient : grpc::ClientBase<LunarServiceClient>
    {
      /// <summary>Creates a new client for LunarService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public LunarServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for LunarService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public LunarServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected LunarServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected LunarServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::RLEnvs.Lunar2D.DimResult GetActionDim(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetActionDim(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar2D.DimResult GetActionDim(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetActionDim, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.DimResult> GetActionDimAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetActionDimAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.DimResult> GetActionDimAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetActionDim, null, options, request);
      }
      public virtual global::RLEnvs.Lunar2D.DimResult GetObservationDim(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetObservationDim(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar2D.DimResult GetObservationDim(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetObservationDim, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.DimResult> GetObservationDimAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetObservationDimAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.DimResult> GetObservationDimAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetObservationDim, null, options, request);
      }
      public virtual global::RLEnvs.Lunar2D.ActionResult PerformAction(global::RLEnvs.Lunar2D.Action request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PerformAction(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar2D.ActionResult PerformAction(global::RLEnvs.Lunar2D.Action request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PerformAction, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.ActionResult> PerformActionAsync(global::RLEnvs.Lunar2D.Action request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PerformActionAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.ActionResult> PerformActionAsync(global::RLEnvs.Lunar2D.Action request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PerformAction, null, options, request);
      }
      public virtual global::RLEnvs.Lunar2D.ServiceMessage Render(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Render(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar2D.ServiceMessage Render(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Render, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.ServiceMessage> RenderAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RenderAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.ServiceMessage> RenderAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Render, null, options, request);
      }
      public virtual global::RLEnvs.Lunar2D.Observation Reset(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Reset(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar2D.Observation Reset(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Reset, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.Observation> ResetAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ResetAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar2D.Observation> ResetAsync(global::RLEnvs.Lunar2D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Reset, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override LunarServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new LunarServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(LunarServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetActionDim, serviceImpl.GetActionDim)
          .AddMethod(__Method_GetObservationDim, serviceImpl.GetObservationDim)
          .AddMethod(__Method_PerformAction, serviceImpl.PerformAction)
          .AddMethod(__Method_Render, serviceImpl.Render)
          .AddMethod(__Method_Reset, serviceImpl.Reset).Build();
    }

  }
}
#endregion
