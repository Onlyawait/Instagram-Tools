Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports RestSharp
Imports System.Threading
Module Module1
    Dim Cookies As New CookieContainer
    Dim TrueCount As Integer = 0
    Dim FalseCount As Integer = 0
    Dim StopRemove As Boolean = False
    Dim Username_id As String
    Dim user, pass As String
    Dim Sleep As Integer = 0
    Sub Main()
        Console.SetWindowSize(60, 13)
        Console.Title = $"Remove follow - Error{FalseCount} -"
        Write("Remove followers", "1", , True)
        Write("Remove following", "2", , True)
        Dim Mode As String = Write("Mode")
        user = Write("username")
        pass = Write("password")
        NewLogin(user, pass)
        Sleep = Write("Sleep")
        If Mode = "1" Then
            While Not StopRemove
                Get_followers()
                Thread.Sleep(TimeSpan.FromSeconds(Sleep))
            End While
        ElseIf Mode = "2" Then
            While Not StopRemove
                Get_following()
                Thread.Sleep(TimeSpan.FromSeconds(Sleep))
            End While
        End If
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
    Function Get_followers() As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"friendships/{Username_id}/followers/?search_surface=follow_list_page&order=default&query=&enable_groups=true&rank_token={Guid.NewGuid}", Method.GET)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("pk") Then
            Dim id As String = Regex.Match(Response, """pk"":(.*?),").Groups(1).Value
            Dim Username_Followers As String = Regex.Match(Response, """username"":""(.*?)"",").Groups(1).Value
            Return Remove_followers(id, Username_Followers)
        ElseIf Not Response.Contains("username") Then
            StopRemove = True
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("All followers have been Removed")
            Console.ReadLine()
        ElseIf Response.Contains("wait") Then
            FalseCount += 1
            Console.Title = $"Remove follow - Error{FalseCount} -"
        End If
        Return Response
    End Function
    Function Remove_followers(ID As String, Username_Followers As String) As String
        Thread.Sleep(100)
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"friendships/remove_follower/{ID}/", Method.POST)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("ok") Then
            TrueCount += 1
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write(TrueCount.ToString)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("] - Removed : ")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("@" + Username_Followers)
            Console.WriteLine()
        End If
        Return Response
    End Function
    Function Get_following() As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"friendships/{Username_id}/following/?includes_hashtags=true&search_surface=follow_list_page&query=&enable_groups=true&rank_token={Guid.NewGuid}", Method.GET)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("pk") Then
            Dim id As String = Regex.Match(Response, """pk"":(.*?),").Groups(1).Value
            Dim Username_Following As String = Regex.Match(Response, """username"":""(.*?)"",").Groups(1).Value
            Return Remove_following(id, Username_Following)
        ElseIf Not Response.Contains("username") Then
            StopRemove = True
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("All following have been Removed")
            Console.ReadLine()
        ElseIf Response.Contains("wait") Then
            FalseCount += 1
            Console.Title = $"Remove follow - Error{FalseCount} -"
        End If
        Return Response
    End Function
    Function Remove_following(ID As String, Username_Following As String) As String
        Thread.Sleep(100)
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1/")
        Dim RestRequest As New RestRequest($"friendships/destroy/{ID}/", Method.POST)
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.CookieContainer = Cookies
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("ok") Then
            TrueCount += 1
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write(TrueCount.ToString)
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("] - Removed : ")
            Console.ForegroundColor = ConsoleColor.Green
            Console.Write("@" + Username_Following)
            Console.WriteLine()
        End If
        Return Response
    End Function
    Function NewLogin(user As String, pass As String) As String
        Dim RestClient As New RestClient("https://i.instagram.com/api/v1")
        Dim RestRequest As New RestRequest("/accounts/login/", Method.POST)
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestClient.CookieContainer = Cookies
        RestRequest.AddCookie("csrftoken", "z5v5MW4tZe5NOcyLJRYdHoSVpkOl52ij")
        RestRequest.AddCookie("mid", "YB83-gALAAGfStaPYB4RpcW9PWfh")
        RestRequest.AddCookie("ig_did", "FB593ABD-993A-4640-B095-34BFB0321CC")
        RestRequest.AddParameter("", $"username={user}&password={pass}&device_id={Guid.NewGuid}&login_attempt_count=0", ParameterType.RequestBody)
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Username_id = Regex.Match(Response, """pk"":(.*?),").Groups(1).Value
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
        RestClient.UserAgent = "Instagram 100.1.0.29.135 Android (25/7.1.2; 192dpi; 720x1280; google; G011A; G011A; qcom; en_US; 262886984)"
        RestClient.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded")
        RestRequest.AddParameter("", "choice=1", ParameterType.RequestBody)
        RestClient.CookieContainer = Cookies
        RestRequest.AddCookie("csrftoken", "z5v5MW4tZe5NOcyLJRYdHoSVpkOl52ij")
        RestRequest.AddCookie("mid", "YB83-gALAAGfStaPYB4RpcW9PWfh")
        RestRequest.AddCookie("ig_did", "FB593ABD-993A-4640-B095-34BFB0321CC")
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("security_code") Then
            Dim Code As String = Write("Put the Code",, ConsoleColor.White)
            Return Security_code(url, Code)
        Else
            Write("Error", $"@{user}", ConsoleColor.Red)
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
        RestRequest.AddCookie("csrftoken", "z5v5MW4tZe5NOcyLJRYdHoSVpkOl52ij")
        RestRequest.AddCookie("mid", "YB83-gALAAGfStaPYB4RpcW9PWfh")
        RestRequest.AddCookie("ig_did", "FB593ABD-993A-4640-B095-34BFB0321CC")
        Dim Response As String = RestClient.Execute(RestRequest).Content
        If Response.Contains("logged_in_user") Then
            Username_id = Regex.Match(Response, """pk"":(.*?),").Groups(1).Value
            Write("logged", $"@{user}", ConsoleColor.Green)
        Else
            Write("Error", $"@{user}", ConsoleColor.Red)
        End If
        Return Response
    End Function
End Module
