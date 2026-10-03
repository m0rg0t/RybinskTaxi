# Portable source regressions

## Fix
Compute the search results before constructing the All filter so its count matches the current query, instead of always showing the old result count. Reuse that result list in selection handling and tolerate missing title/description fields and a null query.

## Run
With .NET SDK 10.0.401 installed:

    dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release

This is a dependency-free net10.0 console harness with C# 5 syntax checking. Its project links the actual production files, without copying their logic. NuGet sources are cleared. The SDK runs the tests only; the historical application is not retargeted.

## Coverage
Real catalog and search page code with small UI/navigation stand-ins: repeated queries, no matches, case-insensitive title search, null fields and null-query reset.

## Original failure
The same tests were run against original source at [cf2c52b4d5f5a2eddaa099009f79e16ddc159f09](https://github.com/m0rg0t/RybinskTaxi/tree/cf2c52b4d5f5a2eddaa099009f79e16ddc159f09) and exited nonzero; the repaired source exits zero.

    Unhandled exception. System.Exception: Search count is stale

## Boundaries
The stand-ins in Portable.Tests/Stubs.cs only supply unavailable platform types; they do not prove platform compatibility or simulate the app UI. No real service/user data, credentials, notifications, payments, publishing or deployment are used.

XAML selection dispatch and visual states are not validated on Windows. The only existing All filter is preserved.

The full historical Windows/Phone build and device runtime remain unverified. Original project targets, package manifests, vendor binaries and assets are unchanged. No compatible modern replacement for the old platform-specific dependencies has been established, so no speculative package upgrade is included.
