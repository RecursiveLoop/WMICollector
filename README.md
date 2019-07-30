# ARAMS - Application Readiness Assessment for Microsoft Workloads

ARAMS is an application that helps customers to optimise their Microsoft workloads on AWS by providing:

- Actual Resource Consumption (ARC) logging using an agent

- License optimisation by recommending BYOL vs license-included deployment, depending on the application

## Problem

Migration discovery tools like TSO Logic collect a set of performance data for right-sizing, but noticeably miss out the ability to capture memory and disk I/O usage.

There is also no tool that exists which provides the ability to recommend licensing 



## Solution

The WMICollector tool is used to capture performance log information, and can be installed as a Windows Service. It logs the following statistics to a CSV file every second:

- CPU Usage
- Free Physical Memory
- Free Virtual Memory
- Max Disk Writes Per Second in Bytes
- Max Disk Reads Per Second in Bytes
- Network Read/Write Bytes

The output can then be used for right-sizing.

Installing the application as a Windows Service avoids premature termination due to users logging off, and allows the lifecycle of the application to be managed in the Windows Services console. 

## Cross Platform Compatibility

As WMI is currently Windows-only, this application doesn't work on Linux or OSX just yet. However, the instrumentation code has been developed using dependency injection, so other OSes can be supported easily in future.