# ARLIE Packaging

Wave environments are installed from the arlie module itself, as they are too big to be stored in PyPI.
The environments are stored in the GitHub releases.

After wave packaging, create the wheel package and upload to PyPI.
Then create a zip file with the environments folders for windows (envs-windows.zip) and linux (envs-linux.zip).
Finally create a new release in GitHub with the tag vX.Y.Z uploading those two files
