Imports System.IO, System.Threading, System.Net
Imports System.Text.RegularExpressions
Imports RestSharp
Module Module1
    Dim Cookies As New CookieContainer
    Dim user, pass As String
    Dim AcceptCount As Integer = 0
    Dim UnAcceptCount As Integer = 0
    Dim CheckCount As Integer = 0
    ' Demon  ||  await || INSTAGRAM || @_824
    Sub Main()
        Console.Title = $"- Auto Accept v2.1 [{CheckCount}] -"
        Console.SetWindowSize(50, 10)
        Console.WriteLine("Demon  || await || INSTAGRAM || @_824 ||")
        Console.WriteLine()
        Console.Title = "Login with Secure Code"
        Console.SetWindowSize(50, 10)
        user = Write("Username")
        pass = Write("Password")
        NewLogin(user, pass)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Seconds only")
        Dim Sleep As Integer = Write("Sleep")
        While 1
            Get_usernameID()
            Thread.Sleep(TimeSpan.FromSeconds(Sleep))
        End While
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
            ElseIf titl = "Set Private" Then
                Console.ForegroundColor = ConsoleColor.Green
            Else
                Console.ForegroundColor = ConsoleColor.White
            End If
            Console.Write(username)
        End If
        Return Console.ReadLine
    End Function
    Sub Write2(titl As String, Optional username As String = Nothing, Optional Count As Integer = 0, Optional Count2 As Integer = 0)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        If Count = 0 Then
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("+")
        ElseIf Not Count2 = 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.Write(Count2.ToString)
        Else
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write(Count.ToString)
        End If
        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"] - {titl} : ")
        If Not username = Nothing Then
            If titl = "UnAccept" Then
                Console.ForegroundColor = ConsoleColor.Red
            ElseIf titl = "Accept" Then
                Console.ForegroundColor = ConsoleColor.Green
            Else
                Console.ForegroundColor = ConsoleColor.White
            End If
            Console.Write(username)
            Console.WriteLine()
        End If
    End Sub
    ' Demon  ||  await || INSTAGRAM || @_824
    Function Get_usernameID()
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest("friendships/pending/?rank_mutual=0", Method.GET)
        RestClient.UserAgent = "Instagram 130.0.0.15.120 (iPhone9,4; iOS 13_4; en_SA@calendar=gregorian; ar-SA; scale=2.61; 1080x1920; 199860337) AppleWebKit/420"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Not Response.Contains("""users"":[],") Then
            Dim username As String = Regex.Match(Response, """username"":""(.*?)""").Groups(1).Value
            Dim usernameID As String = Regex.Match(Response, """pk"":(.*?),").Groups(1).Value
            Return Accept(usernameID, username)
        ElseIf Response.Contains("wait") Then
            UnAcceptCount += 1
            Write2("Error", "", UnAcceptCount)
        Else
            CheckCount += 1
            Console.Title = $"- Auto Accept v2.1 [{CheckCount}] -"
        End If
        Return Response
    End Function
    Function Accept(usernameID As String, username As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"friendships/approve/{usernameID}/", Method.POST)
        RestClient.UserAgent = "Instagram 130.0.0.15.120 (iPhone9,4; iOS 13_4; en_SA@calendar=gregorian; ar-SA; scale=2.61; 1080x1920; 199860337) AppleWebKit/420"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("""status"":""ok""") Then
            AcceptCount += 1
            Write2("Accept", $"@{username}", AcceptCount)
        ElseIf Response.Contains("wait") Then
            UnAcceptCount += 1
            Write2("UnAccept", $"@{username}", UnAcceptCount)
        End If
        Return Response
    End Function
    ' Demon  ||  await || INSTAGRAM || @_824
    Function NewLogin(user As String, pass As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest("/accounts/login/", Method.POST)
        RestClient.UserAgent = "Instagram 130.0.0.15.120 (iPhone9,4; iOS 13_4; en_SA@calendar=gregorian; ar-SA; scale=2.61; 1080x1920; 199860337) AppleWebKit/420"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", $"username={user}&password={pass}&device_id={Guid.NewGuid}&login_attempt_count=0", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Write("logged", $"@{user}")
            If Response.Contains("""is_private"":false") Then
                Return SetPrivate()
            End If
        ElseIf Response.Contains("challenge_required") Then
            Dim url As String = Regex.Match(Response, """api_path"":""(.*?)""").Groups(1).Value
            Return SendEmail(url)
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
    Function SetPrivate() As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest("accounts/set_private/", Method.POST)
        RestClient.UserAgent = "Instagram 130.0.0.15.120 (iPhone9,4; iOS 13_4; en_SA@calendar=gregorian; ar-SA; scale=2.61; 1080x1920; 199860337) AppleWebKit/420"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", "is_private=true", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("""status"":""ok""") Then
            Write("Set Private", "True")
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
    Function SendEmail(url As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.UserAgent = "Instagram 130.0.0.15.120 (iPhone9,4; iOS 13_4; en_SA@calendar=gregorian; ar-SA; scale=2.61; 1080x1920; 199860337) AppleWebKit/420"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", "choice=1", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("security_code") Then
            Dim Code As String = Write("Put the Code", "")
            Return Security_code(url, Code)
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
    Function Security_code(url As String, Codetext As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.UserAgent = "Instagram 130.0.0.15.120 (iPhone9,4; iOS 13_4; en_SA@calendar=gregorian; ar-SA; scale=2.61; 1080x1920; 199860337) AppleWebKit/420"
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
End Module
' Demon  ||  await || INSTAGRAM || @_824
