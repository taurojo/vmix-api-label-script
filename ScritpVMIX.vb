' --- Arturo del Pino
' --- Script that captures the active video from any VMIX
' --- and retrieves information to display on a label
' --- Designed for on-set viewing

    ' --- Define variables to capture XML
Dim apiUrl As String = "http://10.207.10.235:8088/api"
Dim webClient As New WebClient()
Dim xmlString As String = ""

  

    ' --- Define variables

Dim position As Integer
Dim positionTC As String
Dim duration As Integer
Dim durationTC As String
Dim remainTC As String
 

    ' --- Prepare the barprogress

Dim numeromaxCaracteres As Integer = 124
Dim caracteresBarra As Integer
 

    ' --- Default bar colors

Dim colorFondo As String = "#40B0FF" ' name="barra.Fill.Color"
Dim colorFondoBarra As String ="#0043C8C2" 'name="FondoBarra.Fill.Color">


    ' --- Bar colors at 15 seconds

Dim tiempoPrimero As Integer = 15
Dim colorFondoPrimero As String ="#a88927" 'name="barra.Fill.Color">
Dim colorFondoBarraPrimero As String ="#594813" 'name="FondoBarra.Fill.Color">

 

    ' --- Bar colors at 10 seconds

Dim tiempoUltimo As Integer = 10
Dim colorFondoUltimo  As String ="#b83f32" 'name="barra.Fill.Color">
Dim colorFondoBarraUltimo  As String ="#70261e" 'name="FondoBarra.Fill.Color">

 

  ' --- Launch the loop, include a sleep to pause it
 

Do While true


    xmlString = webClient.DownloadString(apiUrl)
    Dim xmlDoc As New XmlDocument()
    xmlDoc.LoadXml(xmlString)
    Dim activeNode As XmlNode = xmlDoc.SelectSingleNode("//active")
    Dim activeValue As String = activeNode.InnerText



        ' --- Retrieve the active line's 

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

    Dim tsremain  As TimeSpan = TimeSpan.FromMilliseconds(duration - position)
    Dim mydateremain  As DateTime = New DateTime(tsremain .Ticks)
    remainTC = mydateremain .ToString("HH:mm:ss")

        'Mark the text:

    API.Function("SetText",Input:="TCPLAY_realizacion",SelectedName:="NOMBRE.Text" ,Value:=""+ shortTitle + "")
    API.Function("SetText",Input:="TCPLAY_realizacion",SelectedName:="DURACIONES.Text" ,Value:="" + positionTC +" / " + remainTC +" / " + durationTC + "")

 

        'Preparamos la barra. Dots in Arial 20

    caracteresBarra = (position * numeromaxCaracteres)  / duration
    Dim caracteresaImprimirBarra  As New String("."c, caracteresBarra)
    API.Function("SetText",Input:="TCPLAY_realizacion",SelectedName:="barraTexto.Text" ,Value:="" + caracteresaImprimirBarra  + "")

 

        'Define bar colors.

        'Default color

    API.Function("SetColor",Input:="TCPLAY_realizacion",SelectedName:="barra.Fill.Color",Value:=""+ colorFondo +"")
    API.Function("SetColor",Input:="TCPLAY_realizacion",SelectedName:="FondoBarra.Fill.Color",Value:=""+ colorFondoBarra +"")

 
        ' Color for the 15 seconds remain

    If tsremain.TotalSeconds < tiempoPrimero Then
        API.Function("SetColor",Input:="TCPLAY_realizacion",SelectedName:="barra.Fill.Color",Value:=""+ colorFondoPrimero +"")
        API.Function("SetColor",Input:="TCPLAY_realizacion",SelectedName:="FondoBarra.Fill.Color",Value:=""+ colorFondoBarraPrimero +"")
    End If

 

        ' Color for the 15 seconds remain

    If tsremain.TotalSeconds < tiempoUltimo Then
        API.Function("SetColor",Input:="TCPLAY_realizacion",SelectedName:="barra.Fill.Color",Value:=""+ colorFondoUltimo +"")
        API.Function("SetColor",Input:="TCPLAY_realizacion",SelectedName:="FondoBarra.Fill.Color",Value:=""+ colorFondoBarraUltimo +"")
    End If

                                            
End If

Sleep(50)

loop                  
