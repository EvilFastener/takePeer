# WireGuard Peer Viewer

A tool for collecting and browsing WireGuard peer information.

The project consists of two parts:

1. a C#/.NET console application that retrieves peer information from WireGuard and stores it in a JSON file,
2. a browser-based interface for viewing, searching and sorting the collected data.

and extracts information about WireGuard peers, including:

peer ID,
preshared key,
endpoint,
allowed IP addresses,
latest handshake,
transfer statistics.

The collected data is saved to a peers.json file.

If the file already exists, the application reads the existing data and appends newly collected peer information.

Web interface

The web interface allows the user to:

load one or multiple JSON files,
browse WireGuard peer information,
search peers by ID or endpoint,
sort peers by ID,
sort peers by endpoint,
view allowed IP addresses,
expand and hide handshake history.
Technologies
C#
.NET 8
System.Text.Json
HTML
CSS
JavaScript
WireGuard
Project Structure
TakePeers/
    C# application for collecting WireGuard peer data

takePeer/
    web interface for displaying and browsing JSON data
Running the Data Collector

The application requires WireGuard to be installed and wg.exe to be available on the system.

Because access to WireGuard information may require elevated permissions, the application should be run with administrator privileges.

After running the application, the collected peer information is saved as:

peers.json

on the user's desktop.

Viewing the Data

Open the HTML interface in a web browser and select the generated peers.json file.

The interface will display the peer information and allow searching and sorting the records.

Author

Patryk Sitek
