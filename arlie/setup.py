from setuptools import setup, find_packages
import sys
import os.path

sys.path.insert(0, os.path.join(os.path.dirname(__file__), "arlie"))

about = {}
with open("arlie/__about__.py") as fp:
    exec(fp.read(), about)

setup(
    name=about["__title__"],
    version=about["__version__"],
    description=about["__summary__"],
    long_description=about["__summary__"],
    author=about["__author__"],
    author_email=about["__email__"],
    license=about["__license__"],
    classifiers=[
        "Programming Language :: Python :: 3.7",
        "License :: OSI Approved :: MIT License",
        "Operating System :: Microsoft :: Windows",
        "Operating System :: POSIX :: Linux",
    ],
    project_urls={"Source": about["__uri__"]},
    packages=find_packages(exclude=["arlie.envs.*"]),
    zip_safe=False,
    install_requires=[
        "grpcio",
        "grpcio-tools",
        "googleapis-common-protos",
        "protobuf",
        "pip>=18.1",
        "gym",
        "keyboard",
    ],
    python_requires=">=3.7",
)
