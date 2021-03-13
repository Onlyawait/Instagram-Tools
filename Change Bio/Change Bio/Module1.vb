Imports RestSharp
Imports System.Text.RegularExpressions, System.Threading
Module Module1
    Dim Cookies As New Net.CookieContainer
    Dim user, pass As String
    Dim Text_Bio As String() = IO.File.ReadAllLines("Bio.txt")
    ' Demon  ||  await || INSTAGRAM || @_824
    Sub Main()
        Console.WriteLine(" Demon  ||  await || INSTAGRAM || @_824 ")
        Console.Title = "Change bio by Await IG : @_824"
        Console.SetWindowSize(50, 10)
        user = Write("Username")
        pass = Write("Password")
        NewLogin(user, pass)
        Console.WriteLine("Minutes only")
        Dim Sleep As Integer = Write("Sleep")
        While 1
            For Each TextBio As String In Text_Bio
                Set_Bio(TextBio)
                Thread.Sleep(TimeSpan.FromMinutes(Sleep))
            Next
        End While
    End Sub
    Function Write(titl As String, Optional titl2 As String = Nothing, Optional Color As ConsoleColor = ConsoleColor.White, Optional Bool As Boolean = False)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write("+")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"] - {titl} : ")
        If Bool = False Then
            Console.ForegroundColor = Color
            Console.Write(titl2)
            Return Console.ReadLine
        Else
            Console.ForegroundColor = Color
            Console.Write(titl2)
            Console.WriteLine()
        End If
        Return Nothing
    End Function
    Function Set_Bio(NewBio As String)
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest("/accounts/set_biography/", Method.POST)
        RestClient.UserAgent = "Instagram 100.0.0.17.129 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 161478664)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"raw_text={NewBio}", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("""status"":""ok""") Then
            Write("Done", NewBio, ConsoleColor.Green, True)
        Else
            Write("Error", $"@{user}", ConsoleColor.Red, True)
        End If
        Return Response
    End Function
    ' Demon  ||  await || INSTAGRAM || @_824
    Function NewLogin(user As String, pass As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest("/accounts/login/", Method.POST)
        RestClient.UserAgent = "Instagram 100.0.0.17.129 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 161478664)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"username={user}&password={pass}&device_id={Guid.NewGuid}&login_attempt_count=0", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Write("logged", $"@{user}", ConsoleColor.Green)
        ElseIf Response.Contains("challenge_required") Then
            Dim url As String = Regex.Match(Response, """api_path"":""(.*?)""").Groups(1).Value
            Return SendEmail(url)
        Else
            Write("Error", $"@{user}", ConsoleColor.Red)
        End If
        Return Response
    End Function
    Function SendEmail(url As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.UserAgent = "Instagram 100.0.0.17.129 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 161478664)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", "choice=1", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("security_code") Then
            Dim Code As String = Write("Put the Code")
            Return Security_code(url, Code)
        Else
            Write("Error", $"@{user}", ConsoleColor.Red)
        End If
        Return Response
    End Function
    Function Security_code(url As String, Codetext As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.UserAgent = "Instagram 100.0.0.17.129 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 161478664)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"security_code={Codetext}", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Write("logged", $"@{user}", ConsoleColor.Green)
        Else
            Write("Error", $"@{user}", ConsoleColor.Red)
        End If
        Return Response
    End Function
End Module
' Demon  ||  await || INSTAGRAM || @_824
