# WMICollector
Collects system performance information and logs it into a file for analysis.



## Problem

Migration discovery tools like TSO Logic collect a set of performance data for right-sizing, but noticeably miss out the ability to capture memory and disk I/O usage.



## Solution

WMICollector can be installed as a Windows Service, and logs the following statistics to a CSV file every second:

- CPU Usage
- Free Physical Memory
- Free Virtual Memory
- Disk Writes Per Second in Bytes
- Disk Reads Per Second in Bytes

The output can then be used for right-sizing.

Installing the application as a Windows Service avoids premature termination due to users logging off, and allows the lifecycle of the application to be managed in the Windows Services console.

## Cross Platform Compatibility

As WMI is currently Windows-only, this application doesn't work on Linux or OSX just yet. However, the instrumentation code has been developed using dependency injection, so other OSes can be supported easily in future.