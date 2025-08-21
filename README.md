# MGL.SharePoint.Membership

This project implements a simple REST service that is deployed via a WSP package to your local SharePoint farm. It offers methods to:
- Check if a user is in a SharePoint group, either directly or through Active Directory (AD) group membership (recursively)
- Resolve AD group members
- Resolve SharePoint group members

The web service is optimized for speed by querying the `tokengroups` attribute to efficiently determine AD group memberships.

## Features
- Fast REST service for SharePoint and AD group membership queries
- Membership service interface and implementation
- Active Directory user and domain account management
- SharePoint integration (layouts, ISAPI, and package support)

## Requirements
- .NET Framework 4.8
- SharePoint 2019 or later (not tested with older versions)

## Installation
1. Clone this repository.
2. Open the solution in Visual Studio.
3. Build the solution to restore dependencies and compile the project.

## Usage
- Deploy the generated WSP package to your SharePoint farm.
- Use the provided REST endpoints for group membership and resolution operations.

## Project Structure
- `Membership/` - Core membership and Active Directory logic
- `ISAPI/` - Service endpoints
- `Layouts/` - JavaScript and SharePoint layout resources
- `Package/` - SharePoint package definitions
- `Properties/` - Assembly info and metadata

## License
Copyright © 2024. All rights reserved.

---
For more information, see the source code and comments within each file.