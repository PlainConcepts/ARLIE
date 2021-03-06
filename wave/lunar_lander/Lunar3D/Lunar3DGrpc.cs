// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Lunar3D.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace RLEnvs.Lunar3D {
  public static partial class Lunar3DService
  {
    static readonly string __ServiceName = "Lunar3DService";

    static readonly grpc::Marshaller<global::RLEnvs.Lunar3D.ServiceMessage> __Marshaller_ServiceMessage = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar3D.ServiceMessage.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar3D.DimResult> __Marshaller_DimResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar3D.DimResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar3D.Action> __Marshaller_Action = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar3D.Action.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar3D.ActionResult> __Marshaller_ActionResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar3D.ActionResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RLEnvs.Lunar3D.Observation> __Marshaller_Observation = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RLEnvs.Lunar3D.Observation.Parser.ParseFrom);

    static readonly grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.DimResult> __Method_GetActionDim = new grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.DimResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetActionDim",
        __Marshaller_ServiceMessage,
        __Marshaller_DimResult);

    static readonly grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.DimResult> __Method_GetObservationDim = new grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.DimResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetObservationDim",
        __Marshaller_ServiceMessage,
        __Marshaller_DimResult);

    static readonly grpc::Method<global::RLEnvs.Lunar3D.Action, global::RLEnvs.Lunar3D.ActionResult> __Method_PerformAction = new grpc::Method<global::RLEnvs.Lunar3D.Action, global::RLEnvs.Lunar3D.ActionResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PerformAction",
        __Marshaller_Action,
        __Marshaller_ActionResult);

    static readonly grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.ServiceMessage> __Method_Render = new grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.ServiceMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Render",
        __Marshaller_ServiceMessage,
        __Marshaller_ServiceMessage);

    static readonly grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.Observation> __Method_Reset = new grpc::Method<global::RLEnvs.Lunar3D.ServiceMessage, global::RLEnvs.Lunar3D.Observation>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Reset",
        __Marshaller_ServiceMessage,
        __Marshaller_Observation);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::RLEnvs.Lunar3D.Lunar3DReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Lunar3DService</summary>
    public abstract partial class Lunar3DServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar3D.DimResult> GetActionDim(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar3D.DimResult> GetObservationDim(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar3D.ActionResult> PerformAction(global::RLEnvs.Lunar3D.Action request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar3D.ServiceMessage> Render(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RLEnvs.Lunar3D.Observation> Reset(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Lunar3DService</summary>
    public partial class Lunar3DServiceClient : grpc::ClientBase<Lunar3DServiceClient>
    {
      /// <summary>Creates a new client for Lunar3DService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public Lunar3DServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Lunar3DService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public Lunar3DServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected Lunar3DServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected Lunar3DServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::RLEnvs.Lunar3D.DimResult GetActionDim(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetActionDim(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar3D.DimResult GetActionDim(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetActionDim, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.DimResult> GetActionDimAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetActionDimAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.DimResult> GetActionDimAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetActionDim, null, options, request);
      }
      public virtual global::RLEnvs.Lunar3D.DimResult GetObservationDim(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetObservationDim(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar3D.DimResult GetObservationDim(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetObservationDim, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.DimResult> GetObservationDimAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetObservationDimAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.DimResult> GetObservationDimAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetObservationDim, null, options, request);
      }
      public virtual global::RLEnvs.Lunar3D.ActionResult PerformAction(global::RLEnvs.Lunar3D.Action request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PerformAction(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar3D.ActionResult PerformAction(global::RLEnvs.Lunar3D.Action request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PerformAction, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.ActionResult> PerformActionAsync(global::RLEnvs.Lunar3D.Action request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PerformActionAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.ActionResult> PerformActionAsync(global::RLEnvs.Lunar3D.Action request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PerformAction, null, options, request);
      }
      public virtual global::RLEnvs.Lunar3D.ServiceMessage Render(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Render(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar3D.ServiceMessage Render(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Render, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.ServiceMessage> RenderAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RenderAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.ServiceMessage> RenderAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Render, null, options, request);
      }
      public virtual global::RLEnvs.Lunar3D.Observation Reset(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Reset(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RLEnvs.Lunar3D.Observation Reset(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Reset, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.Observation> ResetAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ResetAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RLEnvs.Lunar3D.Observation> ResetAsync(global::RLEnvs.Lunar3D.ServiceMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Reset, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override Lunar3DServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new Lunar3DServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(Lunar3DServiceBase serviceImpl)
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
