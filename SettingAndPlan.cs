using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mr.Manager_v_1._0
{
    public partial class SettingAndPlan : Form
    {
        DateTime f;
        DateTimePicker dt;
        Rectangle oRectangle;
        int ci, ri;
        NumericUpDown nu;
        List<int> task,grp;
        int max ;
        public SettingAndPlan()
        {
            InitializeComponent();
            dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
        }

        private void SettingAndPlan_Load(object sender, EventArgs e)
        {
           
            task = new List<int>();
            grp = new List<int>();
            max = 1;

            label1.Left = (dataGridView2.Location.X + dataGridView2.Size.Width / 2 )+100;
            //(int)(Screen.PrimaryScreen.Bounds.Width - 280)/2;
            label2.Left = (dataGridView2.Location.X + dataGridView2.Size.Width / 2 )+100;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            grp.Add(1);
            dataGridView1.Scroll += cell_scroll;
            dataGridView1.CellEnter += cell_enter;
            this.Width = Screen.PrimaryScreen.Bounds.Width -280;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            dataGridView1.Width = Screen.PrimaryScreen.Bounds.Width-320;
            dataGridView2.Width = Screen.PrimaryScreen.Bounds.Width-320;           
            dataGridView1.CellValidating += cell_valid;
        DateTime start=new DateTime(2015,7,1);
        DateTime end=new DateTime(2015,8,29);
        dataGridView1.ColumnCount=10;
        dataGridView2.ColumnCount = (int)end.Subtract(start).TotalDays+1;
        dataGridView1.Columns[0].Name = "Task ID";

        dataGridView1.Columns[1].Name = "Actual Percentage Done";
       
        dataGridView1.Columns[2].Name = "Estimated Effort";
        dataGridView1.Columns[3].Name = "Actual Effort";

        dataGridView1.Columns[4].Name = "Estimated Start Date";
        dataGridView1.Columns[5].Name = "Actual Start Date";
        dataGridView1.Columns[6].Name = "Estimated End Date";
        dataGridView1.Columns[7].Name = "Actual End Date";
        dataGridView1.Columns[8].Name = "Resource Name";
        dataGridView1.Columns[9].Name = "Predesessor";
        
        int j = 0;
     
            for (DateTime i = start; i <= end; i = i.AddDays(1))
            {
                dataGridView2.Columns[j].Name = i.ToShortDateString();

                j++;

            }
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            
            dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            
        }

        private void cell_valid(object sender, DataGridViewCellValidatingEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ri = e.RowIndex;
            ci = e.ColumnIndex;
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                dt.Visible = false;
                if (e.ColumnIndex == 4)
                {


                    double k = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[2].Value) / 8;
                    double r = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[2].Value) % 8;
                    DateTime d = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
                    if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                    {
                        while (k > 0)
                        {
                            if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                                k = k + 0;
                            else
                                k--;
                            d = d.AddDays(1);
                        }
                        if (d.DayOfWeek == DayOfWeek.Saturday) d = d.AddDays(2);
                        else if (d.DayOfWeek == DayOfWeek.Sunday) d = d.AddDays(1);
                        double min = r * 60;
                        d = d.AddMinutes(min);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = d.ToShortDateString();
                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        changePredecessor();
                    }
                    else
                    {
                        e.Cancel = true;
                        MessageBox.Show("The Entered Date is not a Working Day!");
                    }
                }

                else if (e.ColumnIndex == 5)
                {
                    double k = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[3].Value) / 8;
                    double r = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[3].Value) % 8;
                    DateTime d = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
                    if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                    {
                        while (k > 0)
                        {
                            if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                                k = k + 0;
                            else
                                k--;
                            d = d.AddDays(1);
                        }
                        if (d.DayOfWeek == DayOfWeek.Saturday) d = d.AddDays(2);
                        else if (d.DayOfWeek == DayOfWeek.Sunday) d = d.AddDays(1);
                        double min = r * 60;
                        d = d.AddMinutes(min);
                        
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = d.ToShortDateString();
                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                        prepareGantt();
                        changePredecessor();
                    }
                    else
                    {
                        e.Cancel = true;
                        MessageBox.Show("The Entered Date is not a Working Day!");


                    }
                }
                
                }
            else if (e.ColumnIndex == 1)
                nu.Visible = false;
            else if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                try
                {
                    double s = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());


                    if (e.ColumnIndex == 2 && (dataGridView1.Rows[e.RowIndex].Cells[4].Value != null))
                    {
                        double k = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[2].Value) / 8;
                        double r = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[2].Value) % 8;
                        DateTime d = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
                        while (k > 0)
                        {
                            if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                                k = k + 0;
                            else
                                k--;
                            d = d.AddDays(1);
                        }
                        if (d.DayOfWeek == DayOfWeek.Saturday) d = d.AddDays(2);
                        else if (d.DayOfWeek == DayOfWeek.Sunday) d = d.AddDays(1);
                        double min = r * 60;
                        d = d.AddMinutes(min);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = d.ToShortDateString();
                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        changePredecessor();
                    }

                    if (e.ColumnIndex == 3 && (dataGridView1.Rows[e.RowIndex].Cells[5].Value != null))
                    {
                        double k = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[3].Value) / 8;
                        double r = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[3].Value) % 8;
                        DateTime d = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
                        while (k > 0)
                        {
                            if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                                k = k + 0;
                            else
                                k--;
                            d = d.AddDays(1);
                        }
                        if (d.DayOfWeek == DayOfWeek.Saturday) d = d.AddDays(2);
                        else if (d.DayOfWeek == DayOfWeek.Sunday) d = d.AddDays(1);
                        double min = r * 60;
                        d = d.AddMinutes(min);
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = d.ToShortDateString();
                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        changePredecessor();
                    }

                }
                catch (Exception e1)
                {
                    e.Cancel = true;
                    MessageBox.Show("Enter a valid Number or Decimal Input");
                }



            }


            else if (e.ColumnIndex == 0)
            {
                // dataGridView1.Rows[e.RowIndex].Cells[11].Value = e.RowIndex + 1;
                int t = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                if (!task.Contains(t))
                {
                    task.Add(t);
                }

            }

            else if (e.ColumnIndex == 9)
            {

                int s = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
                if (e.RowIndex != 0)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
                    {


                        if (!s.Equals(null))
                        {
                            if (!task.Contains(s))
                            {
                                MessageBox.Show("Enter a valid taskid");
                                e.Cancel = true;
                                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                                dataGridView1.CurrentCell.Selected = true;
                                dataGridView1.BeginEdit(true);
                            }
                            else if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Equals(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString()))
                            {
                                MessageBox.Show("Task id and Predecessor of a task cannot be same ");
                                e.Cancel = true;
                                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                                dataGridView1.CurrentCell.Selected = true;
                                dataGridView1.BeginEdit(true);
                            }
                            else if (Convert.ToInt32(dataGridView1.Rows[s - 1].Cells[9].Value) == (e.RowIndex + 1))
                            {
                                MessageBox.Show("Circle of reference");
                                e.Cancel = true;
                                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                                dataGridView1.CurrentCell.Selected = true;

                                dataGridView1.BeginEdit(true);

                            }
                            else
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[5].Value = dataGridView1.Rows[s - 1].Cells[7].Value;
                                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                ConvertToEndDate(e.RowIndex, e.ColumnIndex);
                            }
                        }
                    }
                    else
                        MessageBox.Show("First enter task id");



                }
                changePredecessor();
                prepareGantt();
                HorizontalScroll.Value = HorizontalScroll.Minimum;
            }

                
        }

        private void changePredecessor()
        {
            for(int h=0;h<dataGridView1.RowCount;h++)
            {
                if(dataGridView1.Rows[h].Cells[9].Value!=null)
                {
                    int p = Convert.ToInt32(dataGridView1.Rows[h].Cells[9].Value.ToString());
                    dataGridView1.Rows[h].Cells[5].Value = Convert.ToDateTime( dataGridView1.Rows[p - 1].Cells[7].Value).ToShortDateString();
                    ConvertToEndDate(h,7);
                }
            }
            prepareGantt();
        }

    

        
        private void ConvertToEndDate(int p1, int p2)
        {
            double k = Convert.ToDouble(dataGridView1.Rows[p1].Cells[3].Value) / 8;
            double r = Convert.ToDouble(dataGridView1.Rows[p1].Cells[3].Value) % 8;
            DateTime d = Convert.ToDateTime(dataGridView1.Rows[p1].Cells[5].Value);
            while (k > 0)
            {
                if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                    k = k + 0;
                else
                    k--;
                d = d.AddDays(1);
            }
            if (d.DayOfWeek == DayOfWeek.Saturday) d = d.AddDays(2);
            else if (d.DayOfWeek == DayOfWeek.Sunday) d = d.AddDays(1);
            double min = r * 60;
            d = d.AddMinutes(min);
            dataGridView1.Rows[p1].Cells[7].Value = d.ToShortDateString();
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    
        }

        

        private void prepareGantt()
        {
            for (int h = 0; h < dataGridView2.RowCount;h++ )
            {
                for(int k=0;k<dataGridView2.ColumnCount;k++)
                {
                    
                    dataGridView2.Rows[h].Cells[k].Style.BackColor = Color.White;
                }
            }

                try
                {
                    for (int j = dataGridView2.RowCount; j <= dataGridView1.RowCount; j++)
                        dataGridView2.Rows.Add("");
                    int i = 0;

                    for (int h = 0; h < dataGridView1.RowCount; h++)
                    {
                        if (dataGridView1.Rows[h].Cells[5].Value != null)
                        {
                            DateTime start = Convert.ToDateTime(dataGridView1.Rows[h].Cells[5].Value.ToString());
                            DateTime end = Convert.ToDateTime(dataGridView1.Rows[h].Cells[7].Value.ToString());
                            DateTime dt = new DateTime(2015, 7, 1);

                            if (dt.Date.Equals(start.Date)) i = 0;
                            else i=(int)start.Subtract(dt).TotalDays;
                            for (; i <((int)end.Subtract(dt).TotalDays +1); i++)
                            {


                                if (start.AddDays(i) < DateTime.Now || start.AddDays(i).Equals(DateTime.Now))
                                dataGridView2.Rows[h].Cells[i].Style.BackColor = Color.Green;
                                 
                                else
                        dataGridView2.Rows[h].Cells[i].Style.BackColor = Color.Black;

                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("Enter Actual Start Date and Actual Effort "+er.Message);
                }
                dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Selected = true;
        }

        private void cell_enter(object sender, DataGridViewCellEventArgs e)
        {
            ci = e.ColumnIndex;
            ri = e.RowIndex;
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5  )
            {
                dataGridView1.Rows[e.RowIndex].Cells[4].ReadOnly = true;
                dataGridView1.Rows[e.RowIndex].Cells[5].ReadOnly = true;
                //dataGridView1.Rows[e.RowIndex].Cells[4].Value = DateTime.Now.ToShortDateString();
                //dataGridView1.Rows[e.RowIndex].Cells[5].Value = DateTime.Now.ToShortDateString(); 
                
                dt = new DateTimePicker();
                dataGridView1.Controls.Add(dt);
                dt.Format = DateTimePickerFormat.Short;
                dt.CloseUp += new EventHandler(oDateTimePicker_CloseUp);
                dt.TextChanged += new EventHandler(dateTimePicker_OnTextChange);
                
                dt.Visible = true;
                oRectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dt.Size = new Size(oRectangle.Width, oRectangle.Height);
                dt.Location = new Point(oRectangle.X,oRectangle.Y);
                
          
               
            }
            else  if(e.ColumnIndex==0 )
            {
                dataGridView1.Rows[e.RowIndex].Cells[0].ReadOnly = true;
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = e.RowIndex + 1;
                
            }

            else if (e.ColumnIndex == 6 || e.ColumnIndex == 7)
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            
            else if(e.ColumnIndex==1)
            {
                dataGridView1.Rows[e.RowIndex].Cells[1].ReadOnly = true;
                dataGridView1.Rows[e.RowIndex].Cells[1].Value ="0%";
                nu = new NumericUpDown();
                dataGridView1.Controls.Add(nu);
                nu.Maximum=100;
                nu.Minimum = 0;
                               
               
                nu.Visible = true;
                nu.Leave += new EventHandler(nu_OnTextChange);
               if (dataGridView1.Rows[ri].Cells[ci].Value != null) nu.Value = Convert.ToInt32(dataGridView1.Rows[ri].Cells[ci].Value.ToString().Remove(dataGridView1.Rows[ri].Cells[ci].Value.ToString().Length-1));
                oRectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                nu.Size = new Size(oRectangle.Width, oRectangle.Height);
                nu.Location = new Point(oRectangle.X, oRectangle.Y);
                //nu.l += new EventHandler(oDateTimePicker_CloseUp);
 
            }
            else if(e.ColumnIndex==9)
                dataGridView1.Rows[0].Cells[9].ReadOnly = true;
        }

        private void nu_OnTextChange(object sender, EventArgs e)
        {
            if (Convert.ToInt32(nu.Value) < 100)
            {
                dataGridView1.Rows[ri].Cells[ci].Value = nu.Value.ToString() + "%";
                nu.Visible = false;
                
            }
            else
            {
                MessageBox.Show("Percentage can be betwenn 0 to 100 only !");
                dataGridView1.CurrentCell.Selected = true;
                nu.Visible = false;
            }
        }

        private void cell_scroll(object sender, ScrollEventArgs e)
        {
            if (ci == 4 || ci == 5 || ci == 6 || ci == 7)
            {
                oRectangle = dataGridView1.GetCellDisplayRectangle(ci, ri, true);
                dt.Size = new Size(oRectangle.Width, oRectangle.Height);
                dt.Location = new Point(oRectangle.X, oRectangle.Y);
                //dt.CloseUp += new EventHandler(oDateTimePicker_CloseUp);
            }
            else if(ci==1)
            {
                oRectangle = dataGridView1.GetCellDisplayRectangle(ci, ri, true);
                nu.Size = new Size(oRectangle.Width, oRectangle.Height);
                nu.Location = new Point(oRectangle.X, oRectangle.Y);
            }

            }

        private void oDateTimePicker_CloseUp(object sender, EventArgs e)
        {
            dt.Visible = false;
        }

        private void dateTimePicker_OnTextChange(object sender, EventArgs e)
        {
            dataGridView1.Rows[ri].Cells[ci].ReadOnly = false;
            dataGridView1.Rows[ri].Cells[ci].Value = dt.Text;
            dataGridView1.Rows[ri].Cells[ci].ReadOnly = true;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            
            f = dt.Value;
            dt.Visible = false;
        }

        
            
            
        }
    }

