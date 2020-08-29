using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FiveInLine
{
    struct BOARD
    {
        public int rows;
        public int cols;
        public int start;
        public double gap;
    };

    public class User: INotifyPropertyChanged
    {
        public string No { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
        public string Player { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {

            if (this.PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }

        }
    }


    public delegate void DisplayMessageDelegate(string message);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BOARD b = new BOARD();
        private int margin = 10;
        private ChessArray myChessArray;
        private CHESSMAN myColor = CHESSMAN.NOTHING;

        Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private DisplayMessageDelegate displayMessageDelegate;

        private string rival = " ";

        private bool win = false;
        private ObservableCollection<User> UserList = new ObservableCollection<User>();


        public MainWindow()
        {
            InitializeComponent();
            displayMessageDelegate = new DisplayMessageDelegate(readAndShowMessage);
        }

        //calling functions
        private void board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            b = drawBoard(10);
            drawChessMan();
        }

        private void board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed && myColor!=CHESSMAN.NOTHING)
            {
               

                Point clicked  = e.GetPosition(GameBoard);
                int numOfColumn = (int)Math.Round(((clicked.X - b.start) / b.gap), 0);
                int numOfRow = (int)Math.Round(((clicked.Y - b.start) / b.gap), 0);
                
                sendToServer(mySocket.LocalEndPoint.ToString() + "/ChessAdded/" + rival + "/" + numOfRow + "/" + numOfColumn );

                newChessAdded(numOfRow, numOfColumn, myColor);
                //if (e.LeftButton == MouseButtonState.Pressed)
                //myChessArray.addChessMan(numOfRow, numOfColumn, CHESSMAN.WHITE);
                //else if (e.RightButton == MouseButtonState.Pressed)
                //myChessArray.addChessMan(numOfRow, numOfColumn, CHESSMAN.BLACK);

                
            }
        }

        private void newChessAdded(int numOfRow, int numOfColumn, CHESSMAN color)
        {
            myChessArray.addChessMan(numOfRow, numOfColumn, color);
            b = drawBoard(10);
            drawChessMan();

            win = myChessArray.checkStatus(numOfRow, numOfColumn, (int)color);
            if (win)
            {
                Line vactoryLine = new Line();
                vactoryLine.Stroke = Brushes.Red;
                vactoryLine.X1 = b.start + (myChessArray.victoryLine[1])*b.gap;
                vactoryLine.Y1 = b.start + (myChessArray.victoryLine[0]) * b.gap;
                vactoryLine.X2 = b.start + (myChessArray.victoryLine[3]) * b.gap;
                vactoryLine.Y2 = b.start + (myChessArray.victoryLine[2]) * b.gap;
                GameBoard.Children.Add(vactoryLine);
                if (color == myColor)
                {
                    ShowMsg("You Win!");
                    //sendToServer(mySocket.LocalEndPoint.ToString() + "/WinOver/" + rival);
                }else
                {
                    ShowMsg("You Lose!");
                }        
            }

            
            
            
        }

       

        private void GameBoard_Loaded(object sender, RoutedEventArgs e)
        {
            

            b = drawBoard(10);
            myChessArray = new ChessArray(b.rows, b.cols);
            //myChessArray.addChessMan(1, 1, CHESSMAN.BLACK);
            //myChessArray.addChessMan(5, 1, CHESSMAN.WHITE);
            drawChessMan();
        }




        //stable drawing functions
        private BOARD drawBoard(int size)
        {
            BOARD tmp = new BOARD();
            

            GameBoard.Children.Clear();
            
            double height = GameBoard.ActualHeight - margin;
            double width = GameBoard.ActualWidth - margin;
            double ? delta = null;

            if (height > width)
            {
                delta = width / size;
            }
            else
            {
                delta = height / size;

            }

            int row = (int)(height / delta) + 1;
            int column = (int)(width / delta) + 1;

            if (row < 1 || column < 1)
            {
                return tmp;
            }

            for (int j = 0; j < row; j++)
            {
                Line lineV = new Line();
                lineV.Stroke = Brushes.Blue;
                lineV.X1 = margin;
                lineV.Y1 = margin + j * (double)delta;
                lineV.X2 = margin + (column-1) * (double)delta;
                lineV.Y2 = margin + j * (double)delta;
                GameBoard.Children.Add(lineV);
            }
            for (int i = 0; i <= column; i++)
            {
                Line lineH = new Line();
                lineH.Stroke = Brushes.Blue;
                lineH.X1 = margin + i * (double)delta;
                lineH.Y1 = margin;
                lineH.X2 = margin + i * (double)delta;
                lineH.Y2 = margin + (row - 1) * (double)delta;
                GameBoard.Children.Add(lineH);
            }

            tmp.cols = column;
            tmp.rows = row;
            tmp.gap = (double)delta;
            tmp.start = margin;
            drawPoints(tmp, 3);
            //drawChessMan();
            return tmp;

        }

        private void drawPoints(BOARD board, int boarderSize)
        {
            int dotSize = (int)b.gap/5;

            Ellipse dot_topl = new Ellipse();
            dot_topl.Fill = Brushes.Black;
            dot_topl.Width = dotSize;
            dot_topl.Height = dotSize;
            dot_topl.SetValue(Canvas.LeftProperty, (double)(board.start + board.gap * boarderSize - dotSize / 2));
            dot_topl.SetValue(Canvas.TopProperty, (double)(board.start + board.gap * boarderSize - dotSize / 2));
            GameBoard.Children.Add(dot_topl);

            Ellipse dot_topr = new Ellipse();
            dot_topr.Fill = Brushes.Black;
            dot_topr.Width = dotSize;
            dot_topr.Height = dotSize;
            dot_topr.SetValue(Canvas.LeftProperty, (double)(board.start + board.gap * (board.cols - 1 - boarderSize) - dotSize / 2));
            dot_topr.SetValue(Canvas.TopProperty, (double)(board.start + board.gap * boarderSize - dotSize / 2));
            GameBoard.Children.Add(dot_topr);

            Ellipse dot_bottoml = new Ellipse();
            dot_bottoml.Fill = Brushes.Black;
            dot_bottoml.Width = dotSize;
            dot_bottoml.Height = dotSize;
            dot_bottoml.SetValue(Canvas.LeftProperty, (double)(board.start + board.gap * boarderSize - dotSize / 2));
            dot_bottoml.SetValue(Canvas.TopProperty, (double)(board.start + board.gap * (board.rows - 1 - boarderSize) - dotSize / 2));
            GameBoard.Children.Add(dot_bottoml);

            Ellipse dot_bottomr = new Ellipse();
            dot_bottomr.Fill = Brushes.Black;
            dot_bottomr.Width = dotSize;
            dot_bottomr.Height = dotSize;
            dot_bottomr.SetValue(Canvas.LeftProperty, (double)(board.start + board.gap * (board.cols - 1 - boarderSize) - dotSize / 2));
            dot_bottomr.SetValue(Canvas.TopProperty, (double)(board.start + board.gap * (board.rows - 1 - boarderSize) - dotSize / 2));
            GameBoard.Children.Add(dot_bottomr);

        }

        public void drawChessMan()
        {
            if (myChessArray == null || myColor == CHESSMAN.NOTHING)
            {
                return;
            }

            int dotsize = (int)b.gap/2;
            int[,] array = myChessArray.getArray();
            for (int i = 0; i < b.rows; i++)
            {
                for (int j = 0; j < b.cols; j++)
                {
                    if(array[i, j] != (int)CHESSMAN.NOTHING)
                    {
                        Ellipse dot = new Ellipse();
                        dot.Width = dotsize;
                        dot.Height = dotsize;
                        dot.SetValue(Canvas.LeftProperty, (double)(b.start + j * b.gap - dotsize/2));
                        dot.SetValue(Canvas.TopProperty, (double)(b.start + i * b.gap - dotsize / 2));

                        if (array[i, j] == (int)CHESSMAN.BLACK)
                        {

                            dot.Fill = Brushes.Black;

                        }else
                        {
                            dot.Fill = Brushes.White;
                        }
                        GameBoard.Children.Add(dot);
                    }                    
                }
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            //连接到的目标IP

            IPAddress ip = IPAddress.Parse(IPAddressTextBox.Text);

            //IPAddress ip = IPAddress.Any;

            //连接到目标IP的哪个应用(端口号！)

            IPEndPoint point = new IPEndPoint(ip, int.Parse(IPPortTextBox.Text));

            try

            {

                //连接到服务器

                mySocket.Connect(point);

                ShowMsg("连接成功");

                ShowMsg("服务器" + mySocket.RemoteEndPoint.ToString());

                ShowMsg("客户端:" + mySocket.LocalEndPoint.ToString());

                //连接成功后，就可以接收服务器发送的信息了

                Thread th = new Thread(ReceiveMsg);

                //th.IsBackground = true;

                th.Start();

            }

            catch (Exception ex)

            {

                ShowMsg(ex.Message);

            }

        }

        void ShowMsg(string msg)
        {

            myDialogtextBox.Inlines.Add(msg + "\r\n");

        }

        void readAndShowMessage(string msg)
        {
            ShowMsg(msg);

            if (msg.Contains('/'))
            {
                string[] splitresult = msg.Split('/');
                if (splitresult != null && splitresult.Length >= 2)
                {
                    
                    if (splitresult[1] == "A")
                    {
                        int index = dataGrid_ListedUsers.Items.Count;
                        User user = new User { No = (index + 1).ToString(), IP = splitresult[2], Name = "Guest" + (index + 1).ToString(), Player = " " };
                        dataGrid_ListedUsers.Items.Add(user);

                    }else if (splitresult[1] == "ChatRoom")
                    {
                        myDialogtextBox.Inlines.Add(msg + "\r\n");
                    }else if (splitresult[1] == "Invite" && myColor == CHESSMAN.NOTHING)
                    {
                        foreach (User user in dataGrid_ListedUsers.Items)
                        {
                            if(user.IP == splitresult[0])
                            {
                                dataGrid_ListedUsers.Items.Remove(user);
                                user.Player = "Black";
                                dataGrid_ListedUsers.Items.Add(user);
                                myColor = CHESSMAN.WHITE;
                                rival = splitresult[0];
                                break;
                            }
                        }
                    }else if (splitresult[1] == "ChessAdded")
                    {
                        int row = int.Parse(splitresult[3]);
                        int column = int.Parse(splitresult[4]);
                        if(myColor == CHESSMAN.WHITE)
                        {
                            newChessAdded(row,column,CHESSMAN.BLACK);
                        }else
                        {
                            newChessAdded(row, column, CHESSMAN.WHITE);
                        }

                    }else if (splitresult[0] == "Remove")
                    {
                        foreach (User user in dataGrid_ListedUsers.Items)
                        {
                            if (user.IP == splitresult[1])
                            {
                                dataGrid_ListedUsers.Items.Remove(user);
                                break;
                            }
                        }
                    }
                    
                }
            }
        }

        
        void ReceiveMsg()

        {
            byte[] buffer = new byte[128 * 128];

            while (true)

            {

                try

                {
                    //me recieving from server about the new clients
                    int n = mySocket.Receive(buffer);

                    string s = Encoding.UTF8.GetString(buffer, 0, n);

                    Dispatcher.Invoke(displayMessageDelegate, s);

                    //ShowMsg(mySocket.RemoteEndPoint.ToString() + ":" + s);

                }

                catch (Exception ex)

                {
                    Dispatcher.Invoke(displayMessageDelegate, ex.Message);

                    //ShowMsg(ex.Message);

                    break;

                }

            }
        }

        private void dataGrid_ListedUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = dataGrid_ListedUsers.SelectedItem as User;
        }

        private void sendToServer(string msg)
        {
            try

            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);

                mySocket.Send(buffer);

            }

            catch (Exception ex)

            {

                ShowMsg(ex.Message);

            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMsg("Me/ChatRoom/" + DialogTextBox.Text);
            sendToServer(mySocket.LocalEndPoint.ToString() + "/ChatRoom/" + DialogTextBox.Text);
        }

        private void InviteBottom_Click(object sender, RoutedEventArgs e)
        {
            User user = dataGrid_ListedUsers.SelectedItem as User;
            if (user == null)
                return;

            sendToServer(mySocket.LocalEndPoint.ToString() + "/Invite/" + user.IP);
            dataGrid_ListedUsers.Items.Remove(user);
            user.Player = "White";
            dataGrid_ListedUsers.Items.Add(user);
            myColor = CHESSMAN.BLACK;
            rival = user.IP;
            ShowMsg("Me/Invite/" + user.IP);
        }

        //private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}
