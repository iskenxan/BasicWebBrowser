Public Class Form1

    Dim BookmarksData(100) As BookmarksDataSet.TableRow 'this array holds the values of bookmarks
    Dim HowManyInArray As Integer = 0 'this shows how many elements are in the array


    'Make the webrowser navigate  to the typed adress when enter jey is pressed
    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles UrlTextBox.KeyDown
        Dim urlPrefix As String = "https://"
        Dim url As String = UrlTextBox.Text
        If (e.KeyCode = Keys.Return) Then

            If ((url.Contains(urlPrefix)) = False) Then
                url = urlPrefix + url
            End If
            WebBrowser1.Navigate(url)

            e.SuppressKeyPress = True
        End If
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'BookmarksDataSet.Table' table. You can move, or remove it, as needed.
        Me.TableTableAdapter.Fill(Me.BookmarksDataSet.Table)
        'TODO: This line of code loads data into the 'Database1DataSet3.Booksmarks' table. You can move, or remove it, as needed.
    End Sub

    Private Sub HomeButton_Click(sender As Object, e As EventArgs) Handles HomeButton.Click
        WebBrowser1.GoHome()
        UrlTextBox.Text = "https://www.nsuok.edu/"
    End Sub

    Private Sub BackButton_Click(sender As Object, e As EventArgs) Handles BackButton.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub ForwardButton_Click(sender As Object, e As EventArgs) Handles ForwardButton.Click
        WebBrowser1.GoForward()
    End Sub

    Private Sub BookMarkButton_Click(sender As Object, e As EventArgs) Handles BookMarkButton.Click
        'Adds a new bookmark to the database 
        Dim newBookmarkRow As BookmarksDataSet.TableRow
        newBookmarkRow = BookmarksDataSet.Table.NewTableRow()
        newBookmarkRow.Name = DisplayDialogBox()
        If String.IsNullOrEmpty(newBookmarkRow.Name) = False Then
            newBookmarkRow._Date = Date.Now
            newBookmarkRow.Url = WebBrowser1.Url.ToString
            BookmarksData(HowManyInArray) = newBookmarkRow
            HowManyInArray = HowManyInArray + 1

            BookmarksDataSet.Table.AddTableRow(newBookmarkRow)
            Me.TableBindingSource.EndEdit()
            Me.TableTableAdapter.Update(BookmarksDataSet.Table)

            'Also saves the bookmark in the text file
            Dim writer As New Writer
            writer.WriteData(newBookmarkRow.Name + "     " + newBookmarkRow.Url)
        End If
    End Sub



    'Creates  custom dialog box for Bookmarks input and returns the value of the input
    Private Function DisplayDialogBox() As String
        Dim promt As Form = New Form()
        promt.Width = 400
        promt.Height = 150
        promt.ControlBox = False

        Dim label As Label = New Label()
        label.Text = "Enter the name of your bookmark"
        label.Width = 200
        label.Height = 30
        label.Location = New Point(100, 0)
        Dim text As New TextBox
        text.Name = "Textbox1"
        text.Size = New Size(200, 20)
        text.Location = New Point(100, 40)

        Dim confirmation As Button = New Button()
        confirmation.Text = "OK"
        Dim cancel As Button = New Button()
        cancel.Text = "Cancel"
        cancel.Location = New Point(180, 80)
        AddHandler confirmation.Click, Sub()
                                           promt.Close()
                                       End Sub
        confirmation.Location = New Point(100, 80)
        AddHandler cancel.Click, Sub()
                                     text.Text = ""
                                     promt.Close()
                                 End Sub
        promt.Controls.Add(label)
        promt.Controls.Add(text)
        promt.Controls.Add(confirmation)
        promt.Controls.Add(cancel)
        promt.ShowDialog()
    
        Return text.Text
    End Function



    'Displays the list of all bookmarks
    Private Sub BookmarksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BookmarksToolStripMenuItem.Click
        Dim bookmarks As New Form()
        bookmarks.Width = 500
        bookmarks.Height = 450
        Dim list As New ListBox
        list.Size = New Size(450, 350)
        list.Location = New Point(10, 10)
        Dim bookmarkslist As New List(Of String)
        bookmarkslist = (From row In BookmarksDataSet.Table.Rows Select col1 = row(0).ToString + "---" + row(2).ToString).ToList()

        list.DataSource = bookmarkslist

        Dim button As New Button
        button.Text = "Open"
        AddHandler button.Click, Sub()
                                     Dim item As String = list.SelectedValue
                                     Dim url As List(Of String) = item.Split("---").ToList()
                                     url.RemoveAll(Function(str) String.IsNullOrEmpty(str))
                                     WebBrowser1.Navigate(url(1))
                                     bookmarks.Close()

                                 End Sub
        button.Location = New Point(210, 360)
        bookmarks.Controls.Add(list)
        bookmarks.Controls.Add(button)
        bookmarks.ShowDialog()
    End Sub


    'updates the Url adress line once the web browser navigates to the new URl user types in
    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        UrlTextBox.Text = WebBrowser1.Url.ToString
    End Sub


    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Environment.Exit(0)

    End Sub
End Class
