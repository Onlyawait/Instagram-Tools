Imports RestSharp
Imports System.Threading, System.IO, System.Text.RegularExpressions, System.Net
Module Module1
    Dim Cookies As New CookieContainer
    Dim user, pass, UsernameID As String
    Dim ErrorCount As Integer = 0
    Dim DoneCount As Integer = 0
    Dim StopRquest As Boolean = False
    ' Demon  ||  await || INSTAGRAM || @_824
    Sub Main()
        Console.WriteLine(" Demon  ||  await || INSTAGRAM || @_824 ")
        Console.Title = $"- Archive Post - Error [{ErrorCount}] -"
        Console.SetWindowSize(50, 10)
        user = Write("Username")
        pass = Write("Password")
        NewLogin(user, pass)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Seconds only")
        Dim Sleep As Integer = Write("Sleep")
        While Not StopRquest
            GetID_FormPost(UsernameID)
            Thread.Sleep(TimeSpan.FromSeconds(Sleep))
        End While
        Console.ReadLine()
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
            ElseIf titl = "Successfully" Then
                Console.ForegroundColor = ConsoleColor.Green
            Else
                Console.ForegroundColor = ConsoleColor.White
            End If
            Console.Write(username)
        End If
        Return Console.ReadLine
    End Function
    Sub Write2(titl As String, Optional titl2 As String = Nothing, Optional Count As Integer = 0)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write(Count.ToString)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"] - {titl} : ")
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write(titl2)
        Console.WriteLine()
    End Sub
    Function GetID_FormPost(usernameID As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"feed/user/{usernameID}/?exclude_comment=true&only_fetch_first_carousel_media=false", Method.GET)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("id") Then
            Dim PostID As String = Regex.Match(Response, """id"":""(.*?)""").Groups(1).Value
            Return Archive_Post(PostID)
        ElseIf Response.Contains("wait") Then
            ErrorCount += 1
            Console.Title = $"- Archive Post - Error [{ErrorCount}] -"
        End If
        If Not Response.Contains("id") Then
            StopRquest = True
            Write("All posts are archived", "Successfully")
        End If
        Return Response
    End Function
    Function Archive_Post(PostID As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"media/{PostID}/only_me/", Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.CookieContainer = Cookies
        RestRequest.AddParameter("", $"media_id={PostID}&_csrftoken=missing&_uid={UsernameID}&_uuid={Guid.NewGuid}", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("""status"":""ok""") Then
            DoneCount += 1
            Write2("Post Archive", "only_me", DoneCount)
        ElseIf Response.Contains("wait") Then
            ErrorCount += 1
            Console.Title = $"- Archive Post - Error [{ErrorCount}] -"
        End If
        Return Response
    End Function
    ' Demon  ||  await || INSTAGRAM || @_824
    Function NewLogin(user As String, pass As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest("accounts/login/", Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddCookie("mid", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddCookie("csrftoken", "YVjsmppWJc6ylI1luYyrMOPTlfIfGXva")
        RestClient.CookieContainer = Cookies
        RestRequest.AddParameter("", $"username={user}&password={pass}&_csrftoken=YVjsmppWJc6ylI1luYyrMOPTlfIfGXva&device_id={Guid.NewGuid}&login_attempt_count=0", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            UsernameID = Regex.Match(Response, """pk"":(.*?),").Groups(1).Value
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
        RestRequest.AddCookie("mid", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddCookie("csrftoken", "YVjsmppWJc6ylI1luYyrMOPTlfIfGXva")
        RestClient.CookieContainer = Cookies
        RestRequest.AddParameter("", "choice=1", ParameterType.RequestBody)
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
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddCookie("mid", "YBFqywABAAE0ZL2yz_x6XtohlZPN")
        RestRequest.AddCookie("csrftoken", "YVjsmppWJc6ylI1luYyrMOPTlfIfGXva")
        RestClient.CookieContainer = Cookies
        RestRequest.AddParameter("", $"security_code={Codetext}", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Write("logged", $"@{user}")
        Else
            Write("Error", $"@{user}")
        End If
        Return Response
    End Function
End Module
