# Mosfet.NeteaseMusic
[![.NET](https://github.com/billtsui/Mosfet.NeteaseMusic/actions/workflows/dotnet.yml/badge.svg)](https://github.com/billtsui/Mosfet.NeteaseMusic/actions/workflows/dotnet.yml) ![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg) ![.NET Version](https://img.shields.io/badge/.NET-10.0.9-%23512bd4.svg?&logoColor=white) ![Windows 11](https://custom-icon-badges.demolab.com/badge/Windows_11-0078D6?logo=windows11) ![macOS 15](https://img.shields.io/badge/macOS_15-%231F161F?logo=apple&logoColor=fff")




This is a tool that converts files with the .ncm extension to the .flac extension.

## Usage
```bash 
GoldenCudgel -p "your_directory" -t 4 
```

> -p, Required. Directory of .ncm files.

> -t, Number of threads,include 1/2/4/8. Default is 1 and more than 20 songs enable multi-threading.

The program will create a new folder named "convert" in the same directory as the .ncm directory, and the converted .flac files will be saved in this folder.
