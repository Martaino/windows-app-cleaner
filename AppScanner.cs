using System.Diagnostics;

class AppScanner
{
    public static async Task<List<App>> GetInstalledAppsAsync()
    {
        // Find all the apps on the user pc and save it into apps list

        // get the info for powershell
        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        // list arguments being used in powershell
        info.ArgumentList.Add("-NoProfile");
        info.ArgumentList.Add("-NonInteractive");
        info.ArgumentList.Add("-Command");
        info.ArgumentList.Add("Get-AppxPackage -ErrorAction Stop | Select-Object -ExpandProperty Name");

        // Start powershell and save all apps
        try
        {
            // start powershell
            using Process process = Process.Start(info) ?? throw new Exception("Could not start Powershell.");

            // get the output and the error
            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            string output = await outputTask;
            string error = await errorTask;

            // If scan didn't work then print errors, else get the info and save it into apps list
            if (process.ExitCode != 0)
            {
                System.Console.WriteLine($"Scan Failed: {error}");
                return [];
            }
            else
            {
                List<App> apps = [];

                List<string> lines = output
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(app => app.Trim())
                    .Where(app => app.Length > 0)
                    .ToList();

                foreach (string name in lines)
                {

                    apps.Add(new App
                    {
                        Name = name,
                        PackageName = name,
                        Selected = false
                    });
                }

                Console.WriteLine($"Found {apps.Count} packages:");

                return apps;
            }            
        }
        catch(Exception err)
        {
            System.Console.WriteLine($"Error: {err.Message}");
            return [];
        }
    }
}
