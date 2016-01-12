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
    public partial class DetailsForm : Form
    {
        DateTime end_time, brk_end;
        double first_half, second_half;

        public DetailsForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker6_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           SettingAndPlan s = new SettingAndPlan();
            s.Show();
            if((textBox1.Text!=null)&&(textBox2.Text!=null))
            {
                DateTime brkstrt = dateTimePicker5.Value;
                DateTime wrkstrt = dateTimePicker3.Value;
                DateTime brk_time = dateTimePicker6.Value;
                first_half=brkstrt.Subtract(wrkstrt).TotalHours;
                if ( first_half> 0)
                {
                    brk_end = brkstrt.AddHours(brk_time.Hour);
                    brk_end = brk_end.AddMinutes(brk_time.Minute);
                    if (brk_end.Hour >= 1) brk_end.AddHours(12);
                    MessageBox.Show(brk_end.ToShortTimeString());

                }
            }
        }
    }
}
