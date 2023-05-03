using System.Management;

namespace WindowsConfiguration;

public class ComputerInfo
{
    public string? OsName { get; set; }

    public string? OsVersion { get; set; }

    public string? ProcessorName { get; set; }

    public string? GraphicsCardName { get; set; }

    public string? TotalRam { get; set; }

    public static ComputerInfo? GetSystemInfo()
    {
        try
        {
            var systemInfo = new ComputerInfo();

            // Get Operating System Information
            var managementObjectSearcher = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem");

            var queryCollection = managementObjectSearcher.Get();

            foreach (var queryObj in queryCollection)
            {
                systemInfo.OsName = queryObj["Caption"].ToString();
                systemInfo.OsVersion = queryObj["Version"].ToString();
            }

            // Get Processor Name
            managementObjectSearcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");

            queryCollection = managementObjectSearcher.Get();

            foreach (var queryObj in queryCollection)
            {
                systemInfo.ProcessorName = queryObj["Name"].ToString();
            }

            // Get Graphics Card Name
            managementObjectSearcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");

            queryCollection = managementObjectSearcher.Get();

            foreach (var queryObj in queryCollection)
            {
                systemInfo.GraphicsCardName = queryObj["Name"].ToString();
            }

            // Get Total Ram
            managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");

            double ramSize = 0;
            foreach (ManagementObject queryObj in managementObjectSearcher.Get())
            {
                double size = Convert.ToDouble(queryObj["Capacity"]);
                ramSize += size;
            }

            ramSize /= (1024 * 1024 * 1024);

            systemInfo.TotalRam = ramSize.ToString("#,##0.00") + " GB";

            return systemInfo;
        }
        catch
        {
            return null;
        }
    }
}