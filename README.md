# EnvJsonConv
Converts ASP.NET Core configs: .env to .json and vice versa

## Usage
```powershell
.\EnvJsonConv.exe -i <input_file> -o <output_file> [--add-prefix <prefix_>] [--remove-prefix <prefix_>]
```

e.g.
```powershell
.\EnvJsonConv.exe -i launchSettings.json -o Development.env
.\EnvJsonConv.exe -i Development.env -o launchSettings.json
```
