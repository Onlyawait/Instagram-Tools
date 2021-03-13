Imports System.Net.Http, System.Net, System.Text, System.Text.RegularExpressions
Imports System.IO, System.Threading
Module Module1
    Dim Cookie As New CookieContainer()
    Dim Stop1 As Boolean = False
    Dim user, pass As String
    Dim Sleep As Integer = 0
    Dim Count As Integer = 0
    Sub Main()
        Console.Title = "UnSave Posts by await | Insta | @_824 "
        Console.SetWindowSize(45, 10)
        Started.GetAwaiter().GetResult()
    End Sub
    Async Function Started() As Task
        user = Write("username")
        pass = Write("password")
        Await NewLogin(user, pass)
        Sleep = Write("Enter Sleep")
        While Not Stop1
            Await GetPostId()
            Thread.Sleep(TimeSpan.FromSeconds(Sleep))
        End While
        Console.ReadLine()
    End Function
    Async Function GetPostId() As Task
        Dim Handler As New HttpClientHandler()
        Dim baseAddress = New Uri("https://i.instagram.com/api/v1/feed/saved/")
        Handler.CookieContainer = Cookie
        Handler.UseCookies = True
        Using HttpClient As New HttpClient(Handler)
            HttpClient.BaseAddress = baseAddress

            HttpClient.DefaultRequestHeaders.Add("user-agent", "Instagram 100.0.0.17.129 Android (25/7.1.2; 240dpi; 720x1280; Asus; ASUS_Z01QD; ASUS_Z01QD; intel; en_US; 161478664)")
            HttpClient.DefaultRequestHeaders.Host = "i.instagram.com"
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en;q=1")
            Dim Response As HttpResponseMessage : Dim Respon As String
            Try
                Response = Await HttpClient.GetAsync(baseAddress)
                Respon = Await Response.Content.ReadAsStringAsync()
            Catch ex As WebException
                Dim Stream As New IO.StreamReader(ex.Response.GetResponseStream())
                Respon = Stream.ReadToEnd()
            End Try
            If Respon.Contains("id") Then
                Dim PostId As String = Regex.Match(Respon, """id"":""(.*?)"",").Groups(1).Value
                Dim UsernamePost As String = Regex.Match(Respon, """username"":""(.*?)"",").Groups(1).Value
                Await UnSave_Post(PostId, UsernamePost)
            ElseIf Not Respon.Contains("id") Then
                Stop1 = True
                Write("UnSave ALL Post", "Enter To Closed", ConsoleColor.Green, True)
            Else
                Write("Error", $"@{user}", ConsoleColor.Red)
            End If
        End Using
    End Function
    Async Function UnSave_Post(PostId As String, UsernamePost As String) As Task
        Dim Handler As New HttpClientHandler()
        Dim baseAddress = New Uri($"https://i.instagram.com/api/v1/media/{PostId}/unsave/")
        Handler.CookieContainer = Cookie
        Handler.UseCookies = True
        Using HttpClient As New HttpClient(Handler)
            HttpClient.BaseAddress = baseAddress
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Instagram 100.0.0.17.129 Android (25/7.1.2; 240dpi; 720x1280; Asus; ASUS_Z01QD; ASUS_Z01QD; intel; en_US; 161478664)")
            HttpClient.DefaultRequestHeaders.Host = "i.instagram.com"
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en;q=1")
            Dim Content As New StringContent($"media_id={PostId}", Encoding.UTF8, "application/x-www-form-urlencoded")
            Dim Response As HttpResponseMessage : Dim Respon As String
            Try
                Response = Await HttpClient.PostAsync(baseAddress, Content)
                Respon = Await Response.Content.ReadAsStringAsync()
            Catch ex As WebException
                Dim Stream As New IO.StreamReader(ex.Response.GetResponseStream())
                Respon = Stream.ReadToEnd()
            End Try
            If Respon.Contains("ok") Then
                Count += 1
                Write("UnSave", "@" + UsernamePost, ConsoleColor.Green, True, Count)
            Else
                Write("Error", $"@{user}", ConsoleColor.Red, True)
            End If
        End Using
    End Function
    Async Function NewLogin(User As String, Pass As String) As Task
        Dim Handler As New HttpClientHandler()
        Dim baseAddress = New Uri("https://i.instagram.com/api/v1/accounts/login/")
        Handler.CookieContainer = Cookie
        Using HttpClient As New HttpClient(Handler)
            HttpClient.BaseAddress = baseAddress
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Instagram 100.0.0.17.129 Android (25/7.1.2; 240dpi; 720x1280; Asus; ASUS_Z01QD; ASUS_Z01QD; intel; en_US; 161478664)")
            HttpClient.DefaultRequestHeaders.Host = "i.instagram.com"
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en;q=1")
            Dim Content As New StringContent($"username={User}&password={Pass}&device_id={Guid.NewGuid()}&login_attempt_count=0", Encoding.UTF8, "application/x-www-form-urlencoded")
            Dim Response As HttpResponseMessage : Dim Respon As String
            Try
                Response = Await HttpClient.PostAsync(baseAddress, Content)
                Respon = Await Response.Content.ReadAsStringAsync()
            Catch ex As WebException
                Dim Stream As New IO.StreamReader(ex.Response.GetResponseStream())
                Respon = Stream.ReadToEnd()
            End Try
            If Respon.Contains("logged_in_user") Then
                Write("logged", $"@{User}", ConsoleColor.Green, False)
            ElseIf Respon.Contains("api_path") Then
                Dim api As String = Regex.Match(Respon, """api_path"":""(.*?)""").Groups(1).Value
                Await SendEmail(api)
            Else
                Write("Error", $"@{User}", ConsoleColor.Red, False)
            End If
        End Using
    End Function
    Async Function SendEmail(API As String) As Task
        Dim Handler As New HttpClientHandler()
        Dim baseAddress = New Uri($"https://i.instagram.com/api/v1{API}")
        Handler.CookieContainer = Cookie
        Handler.UseCookies = True
        Using HttpClient As New HttpClient(Handler)
            HttpClient.BaseAddress = baseAddress
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Instagram 100.0.0.17.129 Android (25/7.1.2; 240dpi; 720x1280; Asus; ASUS_Z01QD; ASUS_Z01QD; intel; en_US; 161478664)")
            HttpClient.DefaultRequestHeaders.Host = "i.instagram.com"
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en;q=1")
            Dim Content As New StringContent("choice=1", Encoding.UTF8, "application/x-www-form-urlencoded")
            Dim Response As HttpResponseMessage : Dim Respon As String
            Try
                Response = Await HttpClient.PostAsync(baseAddress, Content)
                Respon = Await Response.Content.ReadAsStringAsync()
            Catch ex As WebException
                Dim Stream As New StreamReader(ex.Response.GetResponseStream())
                Respon = Stream.ReadToEnd()
            End Try
            If Respon.Contains("security_code") Then
                Dim Code As String = Write("Put the Code",, ConsoleColor.White, False)
                Await Security_code(API, Code)
            Else
                Write("Error", $"@{user}", ConsoleColor.Red, False)
            End If
        End Using
    End Function
    Async Function Security_code(API As String, Codetext As String) As Task
        Dim Handler As New HttpClientHandler()
        Dim baseAddress = New Uri($"https://i.instagram.com/api/v1{API}")
        Handler.CookieContainer = Cookie
        Handler.UseCookies = True
        Using HttpClient As New HttpClient(Handler)
            HttpClient.BaseAddress = baseAddress
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Instagram 100.0.0.17.129 Android (25/7.1.2; 240dpi; 720x1280; Asus; ASUS_Z01QD; ASUS_Z01QD; intel; en_US; 161478664)")
            HttpClient.DefaultRequestHeaders.Host = "i.instagram.com"
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en;q=1")
            Dim Content As New StringContent($"security_code={Codetext}", Encoding.UTF8, "application/x-www-form-urlencoded")
            Dim Response As HttpResponseMessage : Dim Respon As String
            Try
                Response = Await HttpClient.PostAsync(baseAddress, Content)
                Respon = Await Response.Content.ReadAsStringAsync()
            Catch ex As WebException
                Dim Stream As New IO.StreamReader(ex.Response.GetResponseStream())
                Respon = Stream.ReadToEnd()
            End Try
            If Respon.Contains("logged_in_user") Then
                Write("logged", $"@{user}", ConsoleColor.Green, False)
                Console.Read()
            Else
                Write("Error", $"@{user}", ConsoleColor.Red, False)
                Console.Read()
            End If
        End Using
    End Function
    Function Write(titl As String, Optional titl2 As String = Nothing, Optional Color As ConsoleColor = ConsoleColor.White, Optional Bool As Boolean = False, Optional Count As Integer = 0)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        If Count = 0 Then
            Console.Write("+")
        Else
            Console.Write(Count)
        End If
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
End Module
