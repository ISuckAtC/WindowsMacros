This is a project for writing low latency high precision macros using WinAPI.

This project is made with dotnet core 3.1, and you will need to download it if you want to build the project.
Link: https://dotnet.microsoft.com/en-us/download/dotnet/3.1

To run the project, use the command "dotnet run", and if you want an exe to run for later, you can build one with the command "dotnet publish"



The following methods are relevant to writing your macros:

    GetKey(WINKEY) - This will check if the given key is currently held down, and returns true if it is
    GetKeyDown(WINKEY) - This will return true ONCE when the key is pressed (this is reccomended for hotkeys)
    GetKeyUp(WINKEY) - This will return true ONCE when the key is released

    The WINKEY parameter is a Windows virtual key, these can be accessed through the WinVirtualKey enum
    Example GetKey(WinVirtualKey.VK_ONE)


    GetMousePosition() - This will return a point which contains the X and Y coordinate of the mouse
    There is also GetCursorPos, which is the actual DLL import, but GetMousePosition is a bit more user friendly

    Wait(TIME IN MILLISECONDS) - This will wait a certain amount of time before continuing, remember that most games wont 
    register the same button being pressed multiple times in one frame, so use this to delay your key presses so the game 
    has a chance to see them (For example a delay of 17ms is about equivalent to a single frame in game)

    SendKey(SCANKEY) - This will input the given key

    The SCANKEY parameter is a hardware scan code, these can be accessed through the ScanCodes enum
    Example SendKey(ScanCodes.A)

    ClickMouse() - This clicks the left mouse down
    ReleaseMouse() - This releases the left mouse button


    RefreshKeys() - This must be at the end of the loop to check if previous held keys are released (so that KeyDown works properly)

    To exit the loop (which also ends the program), just set the "exit" bool to true
    Example exit = true;



In Program.cs, in the Main method, you can write your own macros, and there will be an example there for how to write them.