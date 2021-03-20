Imports RestSharp
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading
Module Module1
    Dim Cookies As New CookieContainer()
    Dim TrueCount As Integer = 0
    Dim FalseCount As Integer = 0
    Dim StopDelete As Boolean = False
    Dim Username_Direct As String
    ' Demon  || await || INSTAGRAM || @_824 ||
    Sub Main()
        Console.Title = $"Delete Direct - Error{FalseCount} -"
        Console.SetWindowSize(50, 10)
        Console.WriteLine("Demon  || await || INSTAGRAM || @_824 ||")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("Enter Username : ")
        Dim username As String = Console.ReadLine
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("Enter Password : ")
        Dim password As String = Console.ReadLine
        Login(username, password)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Seconds only")
        Console.Write("Enter Sleep : ")
        Dim Sleep As Integer = Console.ReadLine
        While Not StopDelete
            GetID()
            Thread.Sleep(TimeSpan.FromSeconds(Sleep))
        End While
    End Sub
    Function GetID() As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest("direct_v2/inbox/?visual_message_return_type=unseen&thread_message_limit=10&persistentBadging=true&limit=20", Method.GET)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android"
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        Dim id As String = Regex.Match(Response, """thread_id"": ""(.*?)""").Groups(1).Value
        If Response.Contains("thread_id") Then
            Dim inbox As String = Regex.Match(Response, """inbox"": {(.*?)}").Groups(1).Value
            Username_Direct = Regex.Match(inbox, """username"": ""(.*?)""").Groups(1).Value
            Return Delete_direct(id)
        ElseIf Not Response.Contains("thread_id") Then
            StopDelete = True
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("your direct is empty")
            Console.ReadLine()
        ElseIf Response.Contains("wait") Then
            FalseCount += 1
            Console.Title = $"Delete Direct - Error{FalseCount} -"
        End If
        Return Response
    End Function
    Function Delete_direct(id As String) As String
        Thread.Sleep(100)
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"direct_v2/threads/{id}/hide/", Method.POST)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android"
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("ok") Then
            TrueCount += 1
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write(TrueCount.ToString)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("] - Done : ")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("@" + Username_Direct)
            Console.WriteLine()
        End If
        Return Response
    End Function
    ' Demon  || await || INSTAGRAM || @_824 ||
    Function Login(user As String, pass As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest("accounts/login/", Method.POST)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android"
        RestClient.CookieContainer = Cookies
        RestRequest.AddCookie("mid", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddCookie("csrftoken", "YVjsmppWJc6ylI1luYyrMOPTlfIfGXva")
        RestRequest.AddCookie("X-MID", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddParameter("", $"device_id={Guid.NewGuid}&username={user}&password={pass}&login_attempt_count=0", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("-")
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("] - logged : ")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("@" + user)
            Console.ReadLine()
        ElseIf Response.Contains("challenge") Then
            Dim Challenge As String = Regex.Match(Response, """api_path"":""(.*?)"",").Groups(1).Value
            Return SendEmail(Challenge)
        Else
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("error")
            Console.ReadLine()
        End If
        Return Response
    End Function
    Function SendEmail(url As String) As String
        MsgBox(url)
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android"
        RestClient.CookieContainer = Cookies
        RestRequest.AddCookie("mid", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddCookie("csrftoken", "YVjsmppWJc6ylI1luYyrMOPTlfIfGXva")
        RestRequest.AddCookie("X-MID", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddParameter("", "choice=1", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("security_code") Then
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("Put The Code : ")
            Dim Code As String = Console.ReadLine
            Return Security_code(url, Code)
        Else
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("error")
            Console.ReadLine()
        End If
        Return Response
    End Function
    Function Security_code(url As String, Codetext As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest(url, Method.POST)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android"
        RestClient.CookieContainer = Cookies
        RestRequest.AddCookie("mid", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddCookie("csrftoken", "YVjsmppWJc6ylI1luYyrMOPTlfIfGXva")
        RestRequest.AddCookie("X-MID", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddParameter("", $"security_code={Codetext}", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Console.ForegroundColor = ConsoleColor.Green
            Dim user As String = Regex.Match(Response, """username"": ""(.*?)""").Groups(1).Value
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("-")
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("] - logged : ")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("@" + user)
            Console.ReadLine()
        Else
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("error")
            Console.ReadLine()
        End If
        Return Response
    End Function
End Module


' Demon  || await || INSTAGRAM || @_824 ||
