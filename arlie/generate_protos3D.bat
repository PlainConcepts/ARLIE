setlocal

@rem enter this directory
cd /d %~dp0

python -m grpc_tools.protoc -I . --python_out=. --grpc_python_out=. ./rlenv/envs/lunar_lander/protos/Lunar3D.proto

endlocal