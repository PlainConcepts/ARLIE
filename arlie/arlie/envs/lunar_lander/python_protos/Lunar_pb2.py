# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: Lunar.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='Lunar.proto',
  package='',
  syntax='proto3',
  serialized_options=None,
  serialized_pb=_b('\n\x0bLunar.proto\"\x8f\x01\n\x0bObservation\x12\x0c\n\x04posX\x18\x01 \x01(\x02\x12\x0c\n\x04posY\x18\x02 \x01(\x02\x12\x0c\n\x04velX\x18\x03 \x01(\x02\x12\x0c\n\x04velY\x18\x04 \x01(\x02\x12\r\n\x05\x61ngle\x18\x05 \x01(\x02\x12\x0e\n\x06\x61ngVel\x18\x06 \x01(\x02\x12\x13\n\x0bleftContact\x18\x07 \x01(\x02\x12\x14\n\x0crightContact\x18\x08 \x01(\x02\"a\n\x06\x41\x63tion\x12$\n\x0c\x45ngineAction\x18\x01 \x01(\x0e\x32\x0e.Action.Engine\"1\n\x06\x45ngine\x12\x08\n\x04NONE\x10\x00\x12\x08\n\x04LEFT\x10\x01\x12\x08\n\x04MAIN\x10\x02\x12\t\n\x05RIGHT\x10\x03\"O\n\x0c\x41\x63tionResult\x12!\n\x0bobservation\x18\x01 \x01(\x0b\x32\x0c.Observation\x12\x0e\n\x06reward\x18\x02 \x01(\x02\x12\x0c\n\x04\x64one\x18\x03 \x01(\x08\"\x1a\n\tDimResult\x12\r\n\x05value\x18\x01 \x01(\x05\"\x10\n\x0eServiceMessage2\xea\x01\n\x0cLunarService\x12+\n\x0cGetActionDim\x12\x0f.ServiceMessage\x1a\n.DimResult\x12\x30\n\x11GetObservationDim\x12\x0f.ServiceMessage\x1a\n.DimResult\x12\'\n\rPerformAction\x12\x07.Action\x1a\r.ActionResult\x12*\n\x06Render\x12\x0f.ServiceMessage\x1a\x0f.ServiceMessage\x12&\n\x05Reset\x12\x0f.ServiceMessage\x1a\x0c.Observationb\x06proto3')
)



_ACTION_ENGINE = _descriptor.EnumDescriptor(
  name='Engine',
  full_name='Action.Engine',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='NONE', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='LEFT', index=1, number=1,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='MAIN', index=2, number=2,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='RIGHT', index=3, number=3,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=209,
  serialized_end=258,
)
_sym_db.RegisterEnumDescriptor(_ACTION_ENGINE)


_OBSERVATION = _descriptor.Descriptor(
  name='Observation',
  full_name='Observation',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='posX', full_name='Observation.posX', index=0,
      number=1, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='posY', full_name='Observation.posY', index=1,
      number=2, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='velX', full_name='Observation.velX', index=2,
      number=3, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='velY', full_name='Observation.velY', index=3,
      number=4, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='angle', full_name='Observation.angle', index=4,
      number=5, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='angVel', full_name='Observation.angVel', index=5,
      number=6, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='leftContact', full_name='Observation.leftContact', index=6,
      number=7, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='rightContact', full_name='Observation.rightContact', index=7,
      number=8, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=16,
  serialized_end=159,
)


_ACTION = _descriptor.Descriptor(
  name='Action',
  full_name='Action',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='EngineAction', full_name='Action.EngineAction', index=0,
      number=1, type=14, cpp_type=8, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
    _ACTION_ENGINE,
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=161,
  serialized_end=258,
)


_ACTIONRESULT = _descriptor.Descriptor(
  name='ActionResult',
  full_name='ActionResult',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='observation', full_name='ActionResult.observation', index=0,
      number=1, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='reward', full_name='ActionResult.reward', index=1,
      number=2, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='done', full_name='ActionResult.done', index=2,
      number=3, type=8, cpp_type=7, label=1,
      has_default_value=False, default_value=False,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=260,
  serialized_end=339,
)


_DIMRESULT = _descriptor.Descriptor(
  name='DimResult',
  full_name='DimResult',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='value', full_name='DimResult.value', index=0,
      number=1, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=341,
  serialized_end=367,
)


_SERVICEMESSAGE = _descriptor.Descriptor(
  name='ServiceMessage',
  full_name='ServiceMessage',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=369,
  serialized_end=385,
)

_ACTION.fields_by_name['EngineAction'].enum_type = _ACTION_ENGINE
_ACTION_ENGINE.containing_type = _ACTION
_ACTIONRESULT.fields_by_name['observation'].message_type = _OBSERVATION
DESCRIPTOR.message_types_by_name['Observation'] = _OBSERVATION
DESCRIPTOR.message_types_by_name['Action'] = _ACTION
DESCRIPTOR.message_types_by_name['ActionResult'] = _ACTIONRESULT
DESCRIPTOR.message_types_by_name['DimResult'] = _DIMRESULT
DESCRIPTOR.message_types_by_name['ServiceMessage'] = _SERVICEMESSAGE
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

Observation = _reflection.GeneratedProtocolMessageType('Observation', (_message.Message,), dict(
  DESCRIPTOR = _OBSERVATION,
  __module__ = 'Lunar_pb2'
  # @@protoc_insertion_point(class_scope:Observation)
  ))
_sym_db.RegisterMessage(Observation)

Action = _reflection.GeneratedProtocolMessageType('Action', (_message.Message,), dict(
  DESCRIPTOR = _ACTION,
  __module__ = 'Lunar_pb2'
  # @@protoc_insertion_point(class_scope:Action)
  ))
_sym_db.RegisterMessage(Action)

ActionResult = _reflection.GeneratedProtocolMessageType('ActionResult', (_message.Message,), dict(
  DESCRIPTOR = _ACTIONRESULT,
  __module__ = 'Lunar_pb2'
  # @@protoc_insertion_point(class_scope:ActionResult)
  ))
_sym_db.RegisterMessage(ActionResult)

DimResult = _reflection.GeneratedProtocolMessageType('DimResult', (_message.Message,), dict(
  DESCRIPTOR = _DIMRESULT,
  __module__ = 'Lunar_pb2'
  # @@protoc_insertion_point(class_scope:DimResult)
  ))
_sym_db.RegisterMessage(DimResult)

ServiceMessage = _reflection.GeneratedProtocolMessageType('ServiceMessage', (_message.Message,), dict(
  DESCRIPTOR = _SERVICEMESSAGE,
  __module__ = 'Lunar_pb2'
  # @@protoc_insertion_point(class_scope:ServiceMessage)
  ))
_sym_db.RegisterMessage(ServiceMessage)



_LUNARSERVICE = _descriptor.ServiceDescriptor(
  name='LunarService',
  full_name='LunarService',
  file=DESCRIPTOR,
  index=0,
  serialized_options=None,
  serialized_start=388,
  serialized_end=622,
  methods=[
  _descriptor.MethodDescriptor(
    name='GetActionDim',
    full_name='LunarService.GetActionDim',
    index=0,
    containing_service=None,
    input_type=_SERVICEMESSAGE,
    output_type=_DIMRESULT,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='GetObservationDim',
    full_name='LunarService.GetObservationDim',
    index=1,
    containing_service=None,
    input_type=_SERVICEMESSAGE,
    output_type=_DIMRESULT,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='PerformAction',
    full_name='LunarService.PerformAction',
    index=2,
    containing_service=None,
    input_type=_ACTION,
    output_type=_ACTIONRESULT,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='Render',
    full_name='LunarService.Render',
    index=3,
    containing_service=None,
    input_type=_SERVICEMESSAGE,
    output_type=_SERVICEMESSAGE,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='Reset',
    full_name='LunarService.Reset',
    index=4,
    containing_service=None,
    input_type=_SERVICEMESSAGE,
    output_type=_OBSERVATION,
    serialized_options=None,
  ),
])
_sym_db.RegisterServiceDescriptor(_LUNARSERVICE)

DESCRIPTOR.services_by_name['LunarService'] = _LUNARSERVICE

# @@protoc_insertion_point(module_scope)
