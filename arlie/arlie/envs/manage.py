import os
import platform
import zipfile
from arlie.__about__ import __version__, __uri__
from pathlib import Path
from urllib.request import urlopen
from tempfile import NamedTemporaryFile


def _envs_dir(base_dir):
    envs = next(os.walk(base_dir))[1]
    try:
        envs.remove("__pycache__")
    except ValueError:
        pass

    return envs


def _download_extract_zip(url, path):
    """
    Download a ZIP file and extract its contents in path
    """
    with urlopen(url) as zipresp, NamedTemporaryFile() as tfile:
        tfile.write(zipresp.read())
        tfile.seek(0)
        zfile = zipfile.ZipFile(tfile)
        zfile.extractall(path)


def _load_envs(base_dir, envs):
    module_prefix = ".".join(__name__.split(".")[:-1]) + "."
    env_modules = [module_prefix + e for e in envs]
    for modname in env_modules:
        __import__(modname)


def check_envs():
    base_dir = Path(os.path.dirname(os.path.abspath(__file__)))

    envs = _envs_dir(base_dir)

    if not envs:
        tag = "v" + __version__
        operative_system = "windows"
        if platform.system() == "Linux":
            operative_system = "linux"

        print("Installing wave environments...")
        url = (
            __uri__ + "/releases/download/" + tag + "/envs-" + operative_system + ".zip"
        )
        _download_extract_zip(url, base_dir)
        print("..install complete.")

        if platform.system() == "Linux":
            import stat

            for name in base_dir.rglob("*.exe"):
                st = os.stat(name)
                os.chmod(name, st.st_mode | stat.S_IEXEC)

        envs = _envs_dir(base_dir)

    _load_envs(base_dir, envs)
