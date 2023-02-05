using MaterialDesignColors;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
using static Azure.Core.HttpHeader;

namespace AdmUser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string connect = @"Data Source = DESKTOP-JA41I9L; Initial Catalog = Rozetka; Trusted_connection=True";

        public string str = "SELECT * FROM Category";

        public string InsertAdm = "INSERT INTO Category (Names) ";
        public string Inserts;
        int kol = 0;
        string IDD;
        public MainWindow()
        {
            InitializeComponent();
        }
        //public partial class Window1 : MainWindow
        //{
        //    public string ViewModel { get; set; }

        //    public Window1()
        //    {
        //        InitializeComponent();
        //    }

        //    public void ShowViewModel()
        //    {
        //        MessageBox.Show(ViewModel);
        //    }
        //}

        async void Inser()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                Inserts = InsertAdm + "VALUES('" + WriteBox.Text + "')";
                //MessageBox.Show(Inserts);
                //открываем подклчение
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(Inserts, connection);

                int num = await command.ExecuteNonQueryAsync();
                Console.WriteLine($"Добавлено объектов {num}");
            }
            Console.Read();
            Inserts = "";
        }

        async void Open()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                InfBox.Text = "";
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(str, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {

                        string s1 = reader.GetName(0);
                        string s2 = reader.GetName(1);



                        while (await reader.ReadAsync())
                        {
                            object id = reader.GetValue(0);
                            object id1 = reader.GetValue(1);


                            InfBox.Text += id + "\t" + id1 + "\n";


                        }
                        await reader.CloseAsync();
                    }

                }
                catch (Exception e) { Console.WriteLine(e.Message); };

            }
        }

        private async void Searchh()
        {
            kol = 0;
            using (SqlConnection connection = new SqlConnection(connect))
            {

                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(str, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {

                        string s1 = reader.GetName(0);
                        string s2 = reader.GetName(1);

                        while (await reader.ReadAsync())
                        {

                            object id = reader.GetValue(0);
                            object id1 = reader.GetValue(1);
                            
                            if (WriteBox.Text.ToLower() == id1.ToString().ToLower())
                            {
                               
                                InfSearch.Text = "";
                                
                                InfSearch.Text = id + "\t" + id1;
                                kol = 1;
                                IDD = id.ToString();
                                break;


                            }
                            else
                            {
                                if(WriteBox.Text!="")
                                {
                                    InfSearch.Text = "No inf";
                                }
                                   
                            }

                        }
                        await reader.CloseAsync();
                    }

                }
                catch (Exception b) { Console.WriteLine(b.Message); };

                if (kol == 0)
                {
                    Del.Visibility = Visibility.Hidden;
                }
                else if (kol == 1)
                {
                    Del.Visibility = Visibility.Visible;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 form = new Window1();
            form.ShowDialog();
            Adm.Visibility = Visibility.Hidden;
            User.Visibility = Visibility.Hidden;
            LabelAdm.Visibility = Visibility.Visible;
            Print.Visibility = Visibility.Visible;
            Search.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Visible;
            Del.Visibility = Visibility.Visible;
            InfBox.Visibility = Visibility.Visible;
            WriteBox.Visibility = Visibility.Visible;
            InfSearch.Visibility = Visibility.Visible;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 form = new Window1();
            form.ShowDialog();
            Adm.Visibility = Visibility.Hidden;
            User.Visibility = Visibility.Hidden;
            LabelUser.Visibility = Visibility.Visible;
            Search.Visibility = Visibility.Hidden;
            Print.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Visible;
            Del.Visibility = Visibility.Visible;
            InfBox.Visibility = Visibility.Visible;
            WriteBox.Visibility = Visibility.Visible;
            InfSearch.Visibility = Visibility.Visible;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if(LabelAdm.Visibility==Visibility)
            {
                Open();
            }
            else if(LabelUser.Visibility==Visibility)
            {
                try
                {
                    //MessageBox.Show("alo");
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(str, connection);

                        DataSet ds = new DataSet();

                        adapter.Fill(ds,"Category");

                        DataTable dt = ds.Tables[0];

                        foreach (DataColumn column in dt.Columns)
                            InfBox.Text+=($"{column.ColumnName}\t");

                        InfBox.Text += "\n";
                        foreach (DataRow row in dt.Rows)
                        {
                            var cells = row.ItemArray;
                            foreach (object cell in cells)
                                InfBox.Text += ($" {cell}\t");
                            InfBox.Text += "\n";
                        }

                    }
                    Console.Read();
                }
                catch (Exception s) { Console.WriteLine(s.Message); };
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (LabelAdm.Visibility == Visibility)
            {
                Searchh();
            }
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (LabelAdm.Visibility == Visibility)
            {
                Inser();
            }
            else if (LabelUser.Visibility == Visibility)
            {
                if (WriteBox.Text != "")
                {
                    try
                    {
                        
                        using (SqlConnection connection = new SqlConnection(connect))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(str, connection);

                            DataSet ds = new DataSet();

                            adapter.Fill(ds);


                            DataTable dt = ds.Tables[0];

                            DataRow dr = dt.NewRow();
                            //DataRow dr2 = dt.NewRow();

                            dr["Names"] = WriteBox.Text;

                            ////dr["Age"] = 212235423;
                            dt.Rows.Add(dr);
                            //MessageBox.Show(dt.ToString());
                           

                            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);

                            adapter.Update(ds);
                       

                        }
                        Console.Read();
                    }
                    catch (Exception t) { Console.WriteLine(t.Message); };
                }
                else { return; }
            }
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            if (LabelAdm.Visibility == Visibility)
            {
                if (IDD != "")
                {
                    string str2 = "DELETE FROM Category WHERE id = " + IDD;
                    using (SqlConnection connection = new SqlConnection(connect))
                    {

                        try
                        {
                            await connection.OpenAsync();
                            SqlCommand command = new SqlCommand(str2, connection);
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            InfSearch.Text = "Successfully";

                        }
                        catch (Exception c) { Console.WriteLine(c.Message); };
                    }
                }
            }
            if(LabelUser.Visibility==Visibility)
            {
                try
                {
                    //MessageBox.Show("alo");
                    using (SqlConnection connection = new SqlConnection(connect))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(str, connection);

                        DataSet ds = new DataSet();

                        adapter.Fill(ds);


                        DataTable dt = ds.Tables[0];

                        //MessageBox.Show(dt.Rows[0].ItemArray[1].ToString());
                        dt.Rows.RemoveAt(0);
                        //dt.Rows[0].Delete();
                        dt.AcceptChanges();
                        adapter.Update(ds);


                        //MessageBox.Show(dt.Rows[0].ItemArray[1].ToString());


                        SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);

                        adapter.Update(ds);


                        foreach (DataColumn column in dt.Columns)
                            InfBox.Text += ($"{column.ColumnName}\t");

                        InfBox.Text += "\n";
                        foreach (DataRow row in dt.Rows)
                        {
                            var cells = row.ItemArray;
                            foreach (object cell in cells)
                                InfBox.Text += ($" {cell}\t");
                            InfBox.Text += "\n";
                        }

                    }
                    Console.Read();
                }
                catch (Exception t) { Console.WriteLine(t.Message); };
            }

            
        }
    }
}
