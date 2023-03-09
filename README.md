# VMIX VIDEO INFO SCRIPT PLAY


This is a VB.NET script that captures information from a vMix software video mixer through its API and displays it in a label, including the active input's title, its position, duration, and remaining time in HH:mm:ss format, and a progress bar that indicates the elapsed time of the input.

The script uses the WebClient class to retrieve an XML string from the vMix API, which is parsed using the XmlDocument class. The script then extracts the required information from the XML and updates the label and progress bar using vMix API commands.

The progress bar is displayed as a string of dots whose length is proportional to the elapsed time of the input, calculated as the ratio of the input's position and duration to a maximum number of characters.

The script also defines default and alternative colors for the progress bar based on the remaining time of the input, with a yellow color for the last 15 seconds and a red color for the last 10 seconds.

The script runs in an infinite loop with a sleep time of 50 milliseconds between iterations to avoid overloading the system.In case of working outside the localhost, please increase to 200 milliseconds or whatever you consider according to your network.

Note that this script requires a running instance of vMix software with the API enabled on port 8088. Also, the vMix API documentation and function reference should be consulted for more information on the available API commands and parameters.

## Key parts of the script
The script starts by defining the vMix API URL and creating a WebClient object to access it. Then, several variables are defined to store information about the active video clip, including position, duration, and remaining time in timecode format.

The script also defines the colors of the progress bar at different points in the video clip (15 seconds and 10 seconds remaining). Then, an infinite loop is started that downloads the vMix API XML and parses it to obtain information about the active video source.

If the XML is valid, the script extracts information about the active video clip and uses it to update the content of the label and progress bar. Finally, the script changes the colors of the progress bar based on the remaining time in the video clip.

The loop is paused for 50 milliseconds before repeating. If vMix is not running locally  it is advised to increase the pause time to 200 milliseconds when running on a network.

## Usage
This script is useful for displaying useful information about a video clip in a live production, and can be further customized to fit the specific needs of each project.

To use the script, simply copy the code into a VB script VMIX. And add, the GT Tittler (TCPLAY.gtzip) to proyect whit the title: "TCPLAY". Or changue that title in the script.
Then, run the project and the video information should start displaying on the label and progress bar.


_Change Vmix IP read API:_
```sh
Dim apiUrl As String = "http://localhost:8088/API"
```
