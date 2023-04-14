# Dragonfly.Net #

A collection of .Net Helpers/Models created by [Heather Floyd](https://www.HeatherFloyd.com).

## Installation ##
[![Nuget Downloads](https://buildstats.info/nuget/Dragonfly.Net)](https://www.nuget.org/packages/Dragonfly.Net/)

    PM > Install-Package Dragonfly.Net
    
## Features & Usage : Models ###

### StatusMessage

An object used for collecting and reporting information about code operations - a great way to return more detailed information from your custom functions and APIs. You can assign any Exceptions, as well as nest statuses. Explore all the properties and methods for details.


*Example Usage:*

	public StatusMessage GetLocalFilesInfo(out List<FileInfo> FilesList)
    {
        FilesList = new List<FileInfo>();
        var returnStatus = new StatusMessage(true);
        returnStatus.ObjectName = "GetLocalFilesInfo";

        IEnumerable<FileInfo> files;
        var statusGetListOfFiles = GetListOfFiles(out files);
        returnStatus.InnerStatuses.Add(statusGetListOfFiles);

        if (files.Any())
        {
            foreach (var fileInfo in files)
            {
                StatusMessage readStatus = new StatusMessage(true);
                readStatus.RunningFunctionName = "GetLocalFilesInfo";
                try
                {
                    FilesList.Add(fileInfo);
                }
                catch (Exception e)
                {
                    readStatus.Success = false;
                    readStatus.Message = $"GetLocalFilesInfo: Failure getting file '{fileInfo.FullName}'.";
                    readStatus.SetRelatedException(e);
                }
                returnStatus.InnerStatuses.Add(readStatus);
            }
        }

        return returnStatus;
    }