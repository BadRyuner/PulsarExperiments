# PulsarExperiments

An example of the implementation of scary things. (Scary - because the code is scary, very scary)

# Examples
- Melee Weapon (with pseudo-api)
- Custom Gun (just with custom prefab)
- Space Drone (just code & prefab)

# Used

[ThunderKit](https://github.com/PassivePicasso/ThunderKit) - game dll import & asset build script
[PML](https://github.com/PULSAR-Modders/pulsar-mod-loader) - modding api

# Build Unity Bundle

In Unity Editor:
1) You need to select the path to the game exe in the ThunderKit settings and click Import.  There may be errors in AssemblyCsharp if PML is installed.  If there is an error, delete pml dll from Pulsar/Packages/PulsarLostColony/PulsarModLoader.dll, or try importing the dll from a clean version of the game.
2) Select Pipe file in Assets and change path to your mods directory
3) Click Execute in Pipe file
4) 👍

Possible problems:
After restarting unity, errors may appear due to missing scripts.  Just delete the Pulsar/Packages/PulsarLostColony folder and click Import again in ThunderKit.
