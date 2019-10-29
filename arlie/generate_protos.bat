setlocal

@rem enter this directory
cd /d %~dp0

python -m grpc_tools.protoc -I../protos --python_out=./rlenv/envs/lunar_lander/python_protos --grpc_python_out=./rlenv/envs/lunar_lander/python_protos ../protos/Lunar.proto

endlocal