import os
import platform
import zipfile
from arlie.__about__ import __version__, __uri__
from pathlib import Path
from urllib.request import urlopen
from tempfile import NamedTemporaryFile


def _download_extract_zip(url, path):
    """
    Download a ZIP file and extract its contents in path
    """
    with urlopen(url) as zipresp, NamedTemporaryFile() as tfile:
        tfile.write(zipresp.read())
        tfile.seek(0)
        zfile = zipfile.ZipFile(tfile)
        zfile.extractall(path)


def check_envs():
    base_dir = Path(os.path.dirname(os.path.abspath(__file__)))

    envs = next(os.walk(base_dir))[1]
    try:
        envs.remove("__pycache__")
    except ValueError:
        pass

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


if __name__ == "__main__":
    check_envs()
