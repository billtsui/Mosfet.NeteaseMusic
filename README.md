# GoldenCudgel
[![.NET](https://github.com/billtsui/SunWukong/actions/workflows/dotnet.yml/badge.svg)](https://github.com/billtsui/SunWukong/actions/workflows/dotnet.yml)  ![GitHub License](https://img.shields.io/github/license/billtsui/SunWuKong?label=License&color=%2354C64F) ![Static Badge](https://img.shields.io/badge/Version-Net%2010.0-%2354C64F) ![Static Badge](https://img.shields.io/badge/Platform-macOS%2015%2CWindows%2011-%2354C64F)



This is a tool that converts files with the .ncm extension to the .flac extension.

## Usage
```bash 
GoldenCudgel -p "your_directory" -t 4 
```

> -p, Required. Directory of .ncm files.

> -t, Number of threads,include 1/2/4/8. Default is 1 and more than 20 songs enable multi-threading.

The program will create a new folder named "convert" in the same directory as the .ncm directory, and the converted .flac files will be saved in this folder.
