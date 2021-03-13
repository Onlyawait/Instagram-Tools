Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions
Imports RestSharp
' Demon  ||  await || Scold || INSTAGRAM || @_824 || @5yx5
'https://i.instagram.com/api/v1/accounts/set_phone_and_name/
Module Module1
    Dim Cookies As New Net.CookieContainer
    Dim user, pass As String
    Sub Main()
        Console.WriteLine(" Demon  ||  await || INSTAGRAM || @_824 ")
        Console.Title = "Login with Secure Code"
        Console.SetWindowSize(50, 10)
        user = Write("Username")
        pass = Write("Password")
        NewLogin(user, pass)
        Dim NewName As String = InputBox("Set New Name")
        Change_Name(NewName)
    End Sub
    Function Write(titl As String, Optional username As String = Nothing)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write("+")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"] - {titl} : ")
        If Not username = Nothing Then
            If titl = "Error" Then
                Console.ForegroundColor = ConsoleColor.Red
            ElseIf titl = "logged" Then
                Console.ForegroundColor = ConsoleColor.Green
            Else
                Console.ForegroundColor = ConsoleColor.White
            End If
            Console.Write(username)
        End If
        Return Console.ReadLine
    End Function
    Function Change_Name(NewName As String)
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest("/accounts/set_phone_and_name/", Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"first_name={NewName}", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("ok") Then
            Write("changed To", NewName)
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
    ' Demon  ||  await || Scold || INSTAGRAM || @_824 || @5yx5
    Function NewLogin(user As String, pass As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest("/accounts/login/", Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"username={user}&password={pass}&device_id={Guid.NewGuid}&login_attempt_count=0", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Write("logged", $"@{user}")
        ElseIf Response.Contains("challenge_required") Then
            Dim url As String = Regex.Match(Response, """api_path"":""(.*?)""").Groups(1).Value
            Return SendEmail(url)
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
    Function SendEmail(url As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", "choice=1", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("security_code") Then
            Dim Code As String = Write("Put the Code", "")
            Return Security_code(url, Code)
        Else
            Write("Error")
        End If
        Return Response
    End Function
    Function Security_code(url As String, Codetext As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"security_code={Codetext}", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Write("logged", $"@{user}")
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
    ' Demon  ||  await || Scold || INSTAGRAM || @_824 || @5yx5
End Module
