using System;
using System.Management;
using System.Collections.Generic;
using VirtualBoxDiskCreator.Utility;

namespace VirtualBoxDiskCreator.Drive
{
    class DiskDrive
    {
        public string Model { get; }
        public string Name { get; }
        public double Size { get; }
        public int ID { get; }

        public DiskDrive(string model, string name, int id, double size)
        {
            Model = model;
            Name = name;
            ID = id;
            Size = Math.Round(size / Math.Pow(1024, 3), 2);
        }

        public override string ToString()
        {
            return $"{ID} - {Model} - {Size} GB";
        }

        public static DiskDrive[] GetDriveInfo()
        {
            string query = "SELECT * FROM WIN32_DISKDRIVE";
            List<DiskDrive> diskInfo = new List<DiskDrive>();

            using (ManagementObjectSearcher management = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject disk in management.Get())
                {
                    string model = Convert.ToString(disk["Model"]);
                    string name = Convert.ToString(disk["Name"]);
                    double size = Convert.ToDouble(disk["Size"]);
                    int id = Service.GetNumberFromString(name);
                    diskInfo.Add(new DiskDrive(model, name, id, size));
                }
            }

            return diskInfo.ToArray();
        }
    }
}