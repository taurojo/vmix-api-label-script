' --- Arturo del Pino
' --- Script that captures the active video from any VMIX
' --- and retrieves information to display on a label
' --- Designed for on-set viewing

' --- Define variables to capture XML
Dim apiUrl As String = "http://localhost:8088/API"
Dim webClient As New WebClient()
Dim xmlString As String = ""

' --- Define variables
Dim position As Integer
Dim positionTC As String
Dim duration As Integer
Dim durationTC As String
Dim remainTC As String

' --- Prepare the bar
Dim maxCharacters As Integer = 124
Dim barCharacters As Integer

' --- Default bar colors
Dim backgroundColor As String = "#40B0FF" ' name="barra.Fill.Color"
Dim barBackgroundColor As String ="#0043C8C2" 'name="FondoBarra.Fill.Color">

' --- Bar colors at 15 seconds
Dim firstTime As Integer = 15
Dim firstBackgroundColor As String ="#a88927" 'name="barra.Fill.Color">
Dim firstBarBackgroundColor As String ="#594813" 'name="FondoBarra.Fill.Color">

' --- Bar colors at 10 seconds
Dim lastTime As Integer = 10
Dim lastBackgroundColor As String ="#b83f32" 'name="barra.Fill.Color">
Dim lastBarBackgroundColor As String ="#70261e" 'name="FondoBarra.Fill.Color">

' --- Launch the loop, include a sleep to pause it

Do While true

xmlString = webClient.DownloadString(apiUrl)
Dim xmlDoc As New XmlDocument()

' --- Launch the loop, include a sleep to pause it
If xmlDoc.TryParse(xmlString, Nothing) Then
' The XML is valid
xmlDoc.LoadXml(xmlString)

Dim activeNode As XmlNode = xmlDoc.SelectSingleNode("//active")
Dim activeValue As String = activeNode.InnerText

' --- Retrieve the active line's Title values
Dim inputNode As Xml.XmlNode = xmlDoc.SelectSingleNode("/vmix/inputs/input[@number='" +activeValue + "']")
If inputNode IsNot Nothing Then

' -- Title
Dim shortTitle As String = inputNode.Attributes("shortTitle").Value

' -- Durations
position = Integer.Parse(inputNode.Attributes("position").Value)
duration = Integer.Parse( inputNode.Attributes("duration").Value)

' -- Convert from seconds to TC

' position
Dim tspostion As TimeSpan = TimeSpan.FromMilliseconds(position)
Dim mydate As DateTime = New DateTime(tspostion.Ticks)
positionTC = mydate.ToString("HH:mm:ss")

' duration
Dim ts As TimeSpan = TimeSpan.FromMilliseconds(duration )
Dim mydatepostion As DateTime = New DateTime(ts.Ticks)
durationTC = mydatepostion.ToString("HH:mm:ss")

' remain
Dim tsremain As TimeSpan = TimeSpan.FromMilliseconds(duration - position)
Dim mydateremain As DateTime = New DateTime(tsremain .Ticks)
remainTC = mydateremain .ToString("HH:mm:ss")

'Mark the text:
API.Function("SetText",Input:="TCPLAY",SelectedName:="NOMBRE.Text" ,Value:=""+ shortTitle + "")
API.Function("SetText",Input:="TCPLAY",SelectedName:="DURACIONES.Text" ,Value:="" + positionTC +" / " + remainTC +" / " + durationTC + "")

'Prepare the bar. Arial 20

charactersInBar = (position * maxCharacters) / duration
Dim charactersToPrintBar As New String("."c, charactersInBar)
API.Function("SetText", Input:="TCPLAY", SelectedName:="barText.Text", Value:="" + charactersToPrintBar + "")

'Define bar colors.

' Default color
API.Function("SetColor", Input:="TCPLAY", SelectedName:="bar.Fill.Color", Value:=""+ backgroundColor + "")
API.Function("SetColor", Input:="TCPLAY", SelectedName:="BarBackground.Fill.Color", Value:=""+ barBackgroundColor + "")

' Color for the first section
If tsremain.TotalSeconds < firstSectionTime Then
    API.Function("SetColor", Input:="TCPLAY", SelectedName:="bar.Fill.Color", Value:=""+ firstSectionBackgroundColor +"")
    API.Function("SetColor", Input:="TCPLAY", SelectedName:="BarBackground.Fill.Color", Value:=""+ firstSectionBarBackgroundColor +"")
End If

' Color for the last section
If tsremain.TotalSeconds < lastSectionTime Then
    API.Function("SetColor", Input:="TCPLAY", SelectedName:="bar.Fill.Color", Value:=""+ lastSectionBackgroundColor +"")
    API.Function("SetColor", Input:="TCPLAY", SelectedName:="BarBackground.Fill.Color", Value:=""+ lastSectionBarBackgroundColor +"")
End If

End If
Sleep(50)

Else
    ' Invalid XML
    Console.WriteLine("Invalid XML.")
End If

Loop
