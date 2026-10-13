using System.Diagnostics;
using System.Text.Json;

class AppScanner
{
    public static async Task<List<App>> GetInstalledAppsAsync()
    {
        // PowerShell script to retrieve applications
        string script = """
                # Get application in start menu
                $startApps = @(Get-StartApps -ErrorAction Stop)

                # Get all installed AppX packages
                $apps = @(

                    Get-AppxPackage -ErrorAction Stop | ForEach-Object {
                        $package = $_

                        # Find the readable display name
                        $displayName = $startApps | 
                            Where-Object {
                                $_.AppID -like "$($package.PackageFamilyName)!*"
                            } | 
                            Select-Object -First 1 -ExpandProperty Name

                        # If the display name is empty then fall back to the package name
                        if ([string]::IsNullOrWhiteSpace($displayName)) {
                            $displayName = $package.Name
                        }

                        # Create an object with both the names to convert to JSON
                        [PSCustomObject]@{
                            Name = $displayName
                            PackageName =$package.Name
                        }
                    }
                )
                
                # Convert apps array into JSON
                ConvertTo-Json -InputObject $apps -Depth 3 -Compress
            """;

        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        info.ArgumentList.Add("-NoProfile");
        info.ArgumentList.Add("-NonInteractive");
        info.ArgumentList.Add("-Command");
        info.ArgumentList.Add(script);

        try
        {
            using Process process = Process.Start(info)
                ?? throw new Exception("Could not start PowerShell.");

            Task<string> outputTask =
                process.StandardOutput.ReadToEndAsync();

            Task<string> errorTask =
                process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            string output = await outputTask;
            string error = await errorTask;

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Scan failed: {error}");
                return [];
            }

            // Convert JSON into C# App objects
            List<App> apps =
                JsonSerializer.Deserialize<List<App>>(output) ?? [];

            return apps;
        }
        catch (Exception err)
        {
            Console.WriteLine($"Error: {err.Message}");
            return [];
        }
    }
}

